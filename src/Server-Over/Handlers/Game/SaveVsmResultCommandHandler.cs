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

namespace ServerOver.Handlers.Game;

public record SaveVsmResultCommand(Request Request) : IRequest<Response>;

public class SaveVsmResultCommandHandler(ILogger<SaveVsmResultCommandHandler> logger, ServerDbContext _context,
    ICardIdPcbSerialCache cardIdPcbSerialCache,
    IOptions<CardServerConfig> options)
    : IRequestHandler<SaveVsmResultCommand, Response>
{
    private readonly CardServerConfig _config = options.Value;

    public async Task<Response> Handle(SaveVsmResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vsm_result.SessionId;
        var cardId = request.Request.save_vsm_result.PilotId;

        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_vsm_result = new()
            };
        }

        var isShuffle = request.Request.save_vsm_result.ShuffleFlag;
        var playResultGroup = request.Request.save_vsm_result.Result;
        var battleResultContext = playResultGroup.ToBattleResultContext();
        SetContext(battleResultContext, playResultGroup);
        battleResultContext.CommonDomain.BattleMode = isShuffle ? BattleModeConstant.OfflineSolo : BattleModeConstant.OfflineTeam;

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
            new SavePlayerLevelCommand(_context),
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
            new SavePlayerBadgeCommand(_context, logger),
        };
        
        saveBattleDataCommands.ForEach(command => command.Save(cardProfile, battleResultContext));
        
        _context.SaveChanges();
        
        await OnBattled(request.Request.save_vsm_result);
        
        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vsm_result = new()
        };
    }
    
    private async Task OnBattled(Request.SaveVsmResult result)
    {
        var playResultGroup = result.Result;
        await default(ValueTask);

        MyUtils.ShowTipAfterSaveBattleResult(logger, "店内结算已经成功保存了!!!");
        
    }

    static void SetContext(BattleResultContext battleResultContext, Request.SaveVsmResult.PlayResultGroup playResultGroup)
    {
        battleResultContext.PlayerBadgeDomain ??= new();
        battleResultContext.PlayerBadgeDomain.IsWin = playResultGroup.WinFlag;
        battleResultContext.PlayerBadgeDomain.ExpIncrement = 0;
        battleResultContext.PlayerBadgeDomain.PlayerLevelId = playResultGroup.PlayerLevelId;
    }
}
