using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Caches;
using ServerOver.Commands.PreLoadCard;
using ServerOver.Commands.PreLoadCard.LoadPlayer;
using ServerOver.Commands.PreLoadCard.MobileUserGroup;
using ServerOver.Mapper.Usage;
using ServerOver.Models.Cards;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record PreLoadCardQuery2(Request Request) : IRequest<Response2>;

public partial class PreLoadCardQuery2Handler
{
    private async Task<(Response, CardProfile)> HandleOb(Request request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success
        };

        var preLoadCardRequest = request.pre_load_card;
        
        var cardProfile = await _context.CardProfiles
            .FirstOrDefaultAsync(x => x.AccessCode == preLoadCardRequest.AccessCode && x.ChipId == preLoadCardRequest.ChipId, cancellationToken);
        
        var sessionId = preLoadCardRequest.AccessCode + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        if (cardProfile != null)
        {
            return await ReadAndReturn(preLoadCardRequest, cardProfile, sessionId, response);
        }
        
        _logger.LogInformation("Card not exist for ChipId = {ChipId}, Now creating...", preLoadCardRequest.ChipId);

        var newCardProfile = new CardProfile()
        {
            AccessCode = preLoadCardRequest.AccessCode,
            ChipId = preLoadCardRequest.ChipId,
            SessionId = sessionId,
            UserName = "EXVS2-" + GetRandomAlphaNumeric(6),
            IsNewCard = true,
            DistinctTeamFormationToken = Guid.NewGuid().ToString("n").Substring(0, 16),
            LastPlayedAt = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds()
        };

        newCardProfile.DefaultStickerProfile.Tracker1 = 1;
        newCardProfile.DefaultStickerProfile.Tracker1 = 2;
        newCardProfile.DefaultStickerProfile.Tracker1 = 3;
        
        _context.CardProfiles.Add(newCardProfile);
        await _context.SaveChangesAsync(cancellationToken);
        
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            IsNewCard = true
        };
        
        return (response, newCardProfile);
    }

    private async Task<(Response, CardProfile)> ReadAndReturn(Request.PreLoadCard preLoadCardRequest, CardProfile cardProfile,
        string sessionId, Response response)
    {
        _logger.LogInformation("Card exists for ChipId = {ChipId}, Now reading from Database",
            preLoadCardRequest.ChipId);
        
        cardProfile.SessionId = sessionId;
        await _context.SaveChangesAsync();
        
        if (cardProfile.IsNewCard)
        {
            _logger.LogInformation("ChipId = {} is still a new card, will go to RegisterCard", preLoadCardRequest.ChipId);
            response.pre_load_card = new Response.PreLoadCard
            {
                SessionId = sessionId,
                AcidResponse = null,
                AcidError = AcidError.AcidSuccess,
                IsNewCard = true
            };
            return (response, cardProfile);
        }
        
        var mobileUserGroup = new Response.PreLoadCard.MobileUserGroup()
        {
            customize_group = new Response.PreLoadCard.MobileUserGroup.CustomizeGroup()
            {
                GpBoostRemains = 9999,
                NaviBoostRemains = GlobalVars.NaviBoostRemains,
                TagSkillPointBoostFlag = true,
                GuestNavAnnivFlag = false
            }
        };
        
        var mobileUserGroupCommands = new List<IPreLoadMobileUserGroupCommand>()
        {
            new PreLoadMobileUserBasicInformationCommand(_context),
            new DisplaySettingCommand(_context),
            new PreLoadTitleCommand(_context),
            new PreLoadTriadPartnerCommand(_context),
            new NaviSettingCommand(_context),
            new BoostSettingCommand(_context),
            new NaviCommand(_context),
            new MobileSuitCommand(_context),
            new ChallengeMissionCommand(_context), // 
        };
        
        mobileUserGroupCommands.ForEach(command => command.Fill(cardProfile, mobileUserGroup));

        var loadPlayer = new Response.PreLoadCard.LoadPlayer();

        var preLoadPlayerCommands = new List<IPreLoadPlayerCommand>()
        {
            new PreLoadPlayerBasicInformationCommand(_context),
            new PlayerLevelCommand(_context),
            //new SoloClassInformationCommand(_context), // ob
            //new TeamClassInformationCommand(_context), // ob
            new WinLossDataCommand(_context)
        };
        
        preLoadPlayerCommands.ForEach(command => command.Fill(cardProfile, loadPlayer));
        
        response.pre_load_card = new Response.PreLoadCard
        {
            SessionId = sessionId,
            AcidResponse = null,
            AcidError = AcidError.AcidSuccess,
            AmId = (uint) cardProfile.Id,
            IsNewCard = false,
            User = mobileUserGroup,
            load_player = loadPlayer
        };
        
        var preLoadCardCommands = new List<IPreLoadCardCommand>()
        {
            new PrivateRoomCommand(_context, _config)
        };
        
        preLoadCardCommands.ForEach(command => command.Fill(cardProfile, response.pre_load_card));
        
        return (response, cardProfile);
    }

    static string GetRandomAlphaNumeric(int length)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = Random.Shared;
        var list = Enumerable.Repeat(0, length).Select(x => chars[random.Next(chars.Length)]);
        return string.Join("", list);
    }
}

public partial class PreLoadCardQuery2Handler(ILogger<PreLoadCardQuery2Handler> _logger, ServerDbContext _context,
    IOptions<CardServerConfig> options,
    ICardIdPcbSerialCache cardIdPcbSerialCache)
    : IRequestHandler<PreLoadCardQuery2, Response2>
{
    private readonly CardServerConfig _config = options.Value;

    public async Task<Response2> Handle(PreLoadCardQuery2 request, CancellationToken cancellationToken)
    {
        var (r, cardProfile) = await HandleOb(request.Request, cancellationToken);
        var r2 = r.ToResponse2();
        
        if (r2.pre_load_card != null)
        {
            List<IPreLoadCard2Command> cmds = [
                // .pre_load_card.load_player
                new IbSoloClassInformationCommand(_context, _config.CustomConfigs),
                new IbTeamClassInformationCommand(_context, _config.CustomConfigs),

                // .pre_load_card.User
                new LoadGpBoostRateCommand(_context, _config), 
                new LoadPlayerBadgeCommand(_context),
            ];
            foreach (var cmd in cmds)
            {
                cmd.Fill(cardProfile, r2.pre_load_card);
            }
        }

		var cardId = (uint)cardProfile.Id;
		if (cardId <= 0u) cardId = r2.pre_load_card?.AmId ?? 0u;
		var pcbSerial = request.Request.PcbSerial;
		if (cardId > 0u && !string.IsNullOrEmpty(pcbSerial))
		{
			await cardIdPcbSerialCache.AddOrUpdate(cardId, pcbSerial);
		}
		
		return r2;
    }
}
