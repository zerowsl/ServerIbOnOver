using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Commands.LoadCard;
using ServerOver.Commands.LoadCard.MobileUser;
using ServerOver.Commands.LoadCard.PilotData;
using ServerOver.Commands.LoadCard.PostProcess;
using ServerOver.Constants;
using ServerOver.Context.Tracker;
using ServerOver.Mapper.Usage;
using ServerOver.Models.Cards;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using ServerOver.Processor.Tracker;
using ServerOver.Processor.Tracker.Rarity;
using ServerOver.Strategy.Team;

namespace ServerOver.Handlers.Game;

public record LoadCardQuery2(Request Request, string BaseAddress) : IRequest<Response2>;

public class LoadCardQuery2Handler(ILogger<LoadCardQuery2Handler> _logger, ServerDbContext _context,
    IOptions<CardServerConfig> options)
    : IRequestHandler<LoadCardQuery2, Response2>
{
    private readonly CardServerConfig _configs = options.Value;

    private Task<(Response, CardProfile)> HandleOb(Request request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
        };

        var sessionId = request.load_card.SessionId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            response.Error = Error.ErrServer;
            return Task.FromResult((response, cardProfile!));
        }

        var pilotDataGroup = new Response.LoadCard.PilotDataGroup()
        {
            RewardMsIds = null,
            pilot_class_match = new Response.LoadCard.PilotDataGroup.PilotClassMatch()
        };

        var playerLeaderTeamAppendStrategy = new PlayerLeaderTeamAppendStrategy(_context);
        var playerPartnerTeamAppendStrategy = new PlayerPartnerTeamAppendStrategy(_context);

        var pilotDataCommands = new List<IPilotDataCommand>()
        {
            new PilotMobileSuitUsageCommand(_context),
            new TriadMiscInfoCommand(_context),
            //new TriadCourseDataCommand(_context), // ob
            //new TriadTimeAttackRankRibbonCommand(_context), // ob
            //new TriadHighScoreRankRibbonCommand(_context), // ob
            //new TriadTargetDestroyRankRibbonCommand(_context), // ob
            //new TriadWantedRankRibbonCommand(_context), // ob
            new TrainingDataCommand(_context),
            //new SoloClassMatchCommand(_context), // ob
            //new TeamClassMatchCommand(_context), // ob
            new LicenseScoreCommand(_context),
            new AppendTeamCommand(playerLeaderTeamAppendStrategy),
            new AppendTeamCommand(playerPartnerTeamAppendStrategy)
        };

        pilotDataCommands.ForEach(command => command.Fill(cardProfile, pilotDataGroup));

        var mobileUserGroup = new Response.LoadCard.MobileUserGroup()
        {
            MobileUserId = (uint)cardProfile.Id,
            PaidFlag = LoadCardMobileUserConstants.PaidFlag,
            PlayingStampFlag = LoadCardMobileUserConstants.PlayingStampFlag,
            SupportTicketRemains = LoadCardMobileUserConstants.SupportTicketRemains,
            online_tag_info = new Response.LoadCard.MobileUserGroup.OnlineTagInfo()
        };

        var pilotTrackerContextGenerator = new PilotTrackerContextGenerator(_context);
        var pilotTrackerContext = pilotTrackerContextGenerator.Generate(cardProfile);
        var rarityProcessor = new OverboostRarityProcessor();
        var pilotTrackerProcessor = new PilotTrackerProcessor(pilotTrackerContext, rarityProcessor);
        var msTrackerProcessor = new MobileSuitTrackerProcessor(pilotTrackerProcessor);

        var mobileUserCommands = new List<ILoadCardMobileUserCommand>()
        {
            new LoadCustomizeGroupCommand(_context),
            new LoadTriadPartnerCommand(_context),
            new RadarSettingCommand(_context),
            new LoadGamepadCommand(_context),
            new LoadTitleCommand(_context),
            new LoadMessageCommand(_context),
            new OnlinePairCommand(_context),
            new DefaultStickerCommand(_context, pilotTrackerProcessor),
            new MobileSuitStickerCommand(_context, pilotTrackerProcessor, msTrackerProcessor),
            new QuickTagCommand(_context)
        };

        mobileUserCommands.ForEach(command => command.Fill(cardProfile, mobileUserGroup));

        var postProcessCommands = new List<ILoadCardPostProcessCommand>()
        {
            new ExTutorialCommand(_context),
            new LatestPlayTimeUpdateCommand(_context)
        };

        postProcessCommands.ForEach(command => command.PostProcess(cardProfile, request.load_card));

        response.load_card = new Response.LoadCard
        {
            pilot_data_group = pilotDataGroup,
            mobile_user_group = mobileUserGroup,
        };

        return Task.FromResult((response, cardProfile));
    }

    public async Task<Response2> Handle(LoadCardQuery2 request, CancellationToken cancellationToken)
    {
        var (r, cardProfile) = await HandleOb(request.Request, cancellationToken);
        var r2 = r.ToResponse2();
        if (cardProfile == null) return r2;

        var baseAddress = request.BaseAddress;

        if (r.load_card.pilot_data_group.TagTeams.Count > 0)
        {
            r2.load_card.mobile_user_group.TagTeams.AddRange(r.load_card.pilot_data_group.TagTeams);
        }
        
        List<ILoadCard2Command> cmds2 = [
            // pilot_data_group
            new LoadIbHighScoreInfosCommand(_context), // 刷卡后，开启stage高难度

            // mobile_user_group
            new LoadIbVsRouteBattleDataCommand(baseAddress, _context), // 加载ib街机模式中断再开的(ui)数据
            new LoadQuickStartInfoCommand(_context), // load快速启动
        ];
        foreach (var command in cmds2)
        {
            command.Fill(cardProfile, r2.load_card);
        }

        return r2;
    }
}
