using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Caches;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Common;
using ServerOver.Commands.SaveBattle.PvP;
using ServerOver.Context.Battle;
using ServerOver.Mapper.Context;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using ServerOver.Utils;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Handlers.Game.Online;

public record SaveVsmOnResultCommand(Request Request) : IRequest<Response>;

public class SaveVsmOnResultCommandHandler(ILogger<SaveVsmOnResultCommandHandler> logger, ServerDbContext _context,
    ICardIdPcbSerialCache cardIdPcbSerialCache,
    IOptions<CardServerConfig> options) 
    : IRequestHandler<SaveVsmOnResultCommand, Response>
{
    private readonly CardServerConfig _config = options.Value;

    public async Task<Response> Handle(SaveVsmOnResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vsm_on_result.SessionId;
        var cardId = request.Request.save_vsm_on_result.PilotId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsm_on_result = new()
            };
        }

        var playResultGroup = request.Request.save_vsm_on_result.Result;
        var battleResultContext = playResultGroup.ToBattleResultContext();
        SetContext(battleResultContext, request.Request.save_vsm_on_result);

        var ally = playResultGroup.Partner;
        if (ally is not null)
        {
            if (ally.CpuFlag == 0 || _config.CustomConfigs?.RecordCpusToBattleHistory == true)
            {
                battleResultContext.BattleHistoryDomain.Ally = ally;
            }
        }

        battleResultContext.BattleHistoryDomain.PlayerActions = playResultGroup.PlayerBattleLogs;

        battleResultContext.BattleHistoryDomain.FilteredTargets = playResultGroup.Foes
            .Where(foe => foe.CpuFlag == 0 || _config.CustomConfigs?.RecordCpusToBattleHistory == true)
            .ToList();

        battleResultContext.BattleHistoryDomain.FilteredTargets
            .ForEach(foe =>
            {
                battleResultContext.BattleStatisticDomain.TotalEnemyDefeatedCount += foe.DownNum;
            });

        var saveBattleDataCommands = new List<ISaveBattleDataCommand>()
        {
            // Common Commands
            new SaveGpCommand(_context), // ob
            new SavePlayerLevelCommand(_context, logger),
            new SaveNaviCommand(_context),
            new SaveMobileSuitMasteryCommand(_context),
            new SaveTeamCommand(_context),
            // PvP Commands
            new SaveWinLossRecordCommand(_context),
            new SavePlayerStatisticCommand(_context),
            new SaveChallengeMissionDataCommand(_context),
            new SaveMobileSuitTrackerStatCommand(_context),
            new SaveBurstTypeCommand(_context),
            new SaveBattleHistoryCommand(_context, cardIdPcbSerialCache),
            new RemovePreBattleHistoryCommand(_context),
            //
            // ib
            new SaveQuickStartInfoCommand(_context, playResultGroup.QuickStartInfo),
            new SaveClassMatchCommand(_context, _config, logger),
            new SavePlayerBadgeCommand(_context, logger),
        };

        saveBattleDataCommands.ForEach(command => command.Save(cardProfile, battleResultContext));

        _context.SaveChanges();
        
        await OnBattled(request.Request.save_vsm_on_result);

        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_on_result = new()
        };
    }

    private async Task OnBattled(Request.SaveVsmOnResult result)
    {
        var playResultGroup = result.Result;
        var roomId = playResultGroup.RoomId;
        await default(ValueTask);

        MyUtils.ShowTipAfterSaveBattleResult(logger, "店外结算已经成功保存了!!!");

        //...
    }

    static void SetContext(BattleResultContext battleResultContext, Request.SaveVsmOnResult vsmOn)
    {
        var playResultGroup = vsmOn.Result;
        var isShuffle = vsmOn.ShuffleFlag;

        battleResultContext.CommonDomain.BattleMode = playResultGroup.OnlineMatchingMode switch
        {
            OnlineMatchingMode.OmmClass => isShuffle ? BattleModeConstant.ClassMatchSolo : BattleModeConstant.ClassMatchTeam,
            OnlineMatchingMode.OmmFree => isShuffle ? BattleModeConstant.FreeSolo : BattleModeConstant.FreeTeam,
            OnlineMatchingMode.OmmFes => isShuffle ? BattleModeConstant.FesSolo : BattleModeConstant.FesTeam,
            _ => isShuffle ? BattleModeConstant.FreeSolo : BattleModeConstant.FreeTeam,
        };

        battleResultContext.PlayerBadgeDomain ??= new();
        battleResultContext.PlayerBadgeDomain.IsWin = playResultGroup.WinFlag;
        battleResultContext.PlayerBadgeDomain.BadgeIdBefore = playResultGroup.PrestigeId; //
        battleResultContext.PlayerBadgeDomain.BadgeIdAfter = playResultGroup.PrestigeId;
        battleResultContext.PlayerBadgeDomain.ExpIncrement = playResultGroup.ObtainedScore;
        battleResultContext.PlayerBadgeDomain.PlayerLevelId = playResultGroup.PlayerLevelId;

        battleResultContext.NetworkDataDomain ??= new();
        battleResultContext.NetworkDataDomain.PcbDatas = vsmOn.NetworkReport?.PcbDatas ?? [];

        battleResultContext.OnlineBattleDomain ??= new();
        battleResultContext.OnlineBattleDomain.OnlineMatchingMode = playResultGroup.OnlineMatchingMode;
        battleResultContext.OnlineBattleDomain.class_match_result = playResultGroup.class_match_result;
        battleResultContext.OnlineBattleDomain.TeamType = playResultGroup.QuickStartInfo.TeamType;
        battleResultContext.OnlineBattleDomain.IsShuffle = vsmOn.ShuffleFlag;
    }
}
