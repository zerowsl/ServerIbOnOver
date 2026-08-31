using nue.protocol.exvs;
using ServerOver.Caches;
using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle.History;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Enum;

namespace ServerOver.Commands.SaveBattle.PvP;

public class SaveBattleHistoryCommand(ServerDbContext _context, ICardIdPcbSerialCache cardIdPcbSerialCache) 
    : ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var commonDomain = battleResultContext.CommonDomain;
        var battleStatisticsDomain = battleResultContext.BattleStatisticDomain;
        var teamDomain = battleResultContext.TeamDomain;
        var playerLevelDomain = battleResultContext.PlayerLevelDomain;
        var masteryDomain = battleResultContext.MobileSuitMasteryDomain;
        var battleHistoryDomain = battleResultContext.BattleHistoryDomain;
        
        var battleHistory = new BattleHistory()
        {
            BattleMode = commonDomain.BattleMode,
            IsWin = commonDomain.IsWin,
            PlayedAt = battleStatisticsDomain.PlayedAt,
            ElapsedSeconds = battleStatisticsDomain.ElapsedSeconds,
            TeamId = teamDomain.TeamId,
            StageId = battleStatisticsDomain.StageId,
            Score = battleStatisticsDomain.Score,
            ScoreRank = battleStatisticsDomain.ScoreRank,
            BurstType = battleStatisticsDomain.BurstType,
            BurstCount = battleStatisticsDomain.BurstCount,
            TotalExBurstDamage = battleStatisticsDomain.TotalExBurstDamage,
            GivenDamage = battleStatisticsDomain.TotalGivenDamage,
            TakenDamage = battleStatisticsDomain.TotalTakenDamage,
            OverheatCount = battleStatisticsDomain.OverheatCount,
            ComboGivenDamage = battleStatisticsDomain.ComboGivenDamage,
            ConsecutiveWinCount = battleStatisticsDomain.ConsecutiveWinCount >= 1 ? battleStatisticsDomain.ConsecutiveWinCount - 1 : 0,
            CardProfile = cardProfile
        };

        var g1 = FindSelfClassG(cardProfile, commonDomain.BattleMode);
        battleHistory.BattleSelf = new BattleSelf()
        {
            BattleHistory = battleHistory,
            CardId = (uint) cardProfile.Id,
            PlayerName = cardProfile.UserName,
            //ClassId = FindSelfClassId(cardProfile, commonDomain.BattleMode), // ob
            ClassId = g1.ClassId,
            GradeId = g1.GradeId,
            PrestigeId = playerLevelDomain.PrestigeId,
            LevelId = playerLevelDomain.LevelIdBefore,
            MobileSuitId = masteryDomain.ActualMobileSuitId,
            SkinId = battleStatisticsDomain.SkinId,
            Mastery = FindSelfMasteryId(cardProfile, masteryDomain.ActualMobileSuitId),
            BurstType = battleStatisticsDomain.BurstType
        };

        battleHistoryDomain.PlayerActions.ForEach(playerAction =>
        {
            battleHistory.ActionLogs.Add(new BattleActionLog()
            {
                BattleHistory = battleHistory,
                FrameTime = playerAction.Frame,
                PlayerName = cardProfile.UserName,
                CardId = cardProfile.Id,
                ActionType = playerAction.Status
            });
        });
        // 发现游戏提交的觉醒数是只算了半觉，不包含全觉的，
        // 临时用log来算出总的觉醒数.. 取最大的,自定义对战可能没时间线...
        battleHistory.BurstCount = Math.Max(battleHistory.BurstCount, (uint)battleHistory.ActionLogs.Count(x => x.ActionType == 3));

        uint playerCount = 1;

        if (battleHistoryDomain.Ally is not null)
        {
            playerCount++;
            battleHistory.Ally = ConstructAllyWithActionLogAppend(battleResultContext, battleHistory, battleHistoryDomain.Ally, playerCount);
        }
        else
        {
            battleHistory.Ally = null;
        }
        
        battleHistoryDomain.FilteredTargets
            .ForEach(target =>
            {
                playerCount++;
                battleHistory.Targets.Add(ConstructTargetWithActionLogAppend(battleResultContext, battleHistory, target, playerCount));
            });
        
        if (commonDomain.IsWin == false)
        {
            UpdateConsecutiveWinsWhenLose(battleHistory, cardProfile);
        }

        _context.BattleHistoryDbSet.Add(battleHistory);
        _context.SaveChanges();
    }

    [Obsolete("ob")]
    uint FindSelfClassId(CardProfile cardProfile, string battleMode)
    {
        if (battleMode == BattleModeConstant.OfflineTeam || battleMode == BattleModeConstant.ClassMatchTeam
            || battleMode == BattleModeConstant.FesTeam || battleMode == BattleModeConstant.FreeTeam){
            return _context.TeamClassMatchRecordDbSet
                .Where(x => x.CardProfile == cardProfile)
                .Select(x => x.ClassId)
                .FirstOrDefault();
        }
            
        return _context.SoloClassMatchRecordDbSet
            .Where(x => x.CardProfile == cardProfile)
            .Select(x => x.ClassId)
            .FirstOrDefault();
    }

    (uint ClassId, uint GradeId) FindSelfClassG(CardProfile cardProfile, string battleMode)
    {
        if (battleMode == BattleModeConstant.ClassMatchSolo)
        {
            var g = _context.SoloClassMatchGRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id);
            return (g?.ClassId ?? 0u, g?.GradeId ?? 0u);
        }
        if (battleMode == BattleModeConstant.ClassMatchTeam)
        {
            var g = _context.TeamClassMatchGRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id);
            return (g?.ClassId ?? 0u, g?.GradeId ?? 0u);
        }
        return default;
    }

    uint FindSelfMasteryId(CardProfile cardProfile, uint mobileSuitId)
    {
        var usage = _context.MobileSuitUsageDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile && x.MstMobileSuitId == mobileSuitId);

        if (usage is null)
        {
            return 0;
        }

        int actualUsage = (int) usage.MsUsedNum - 1;

        return actualUsage >= 0 ? (uint) actualUsage : 0;
    }
    
    BattleAlly ConstructAllyWithActionLogAppend(BattleResultContext battleResultContext, BattleHistory battleHistory, AdversaryManGroup adversaryMan, uint playerCount)
    {
        string playerName = default!;
        if (adversaryMan.CpuFlag != 0)
        {
            playerName = "CPU " + playerCount;
        }
        else
        {
            var player = _context.CardProfiles.FirstOrDefault(x => x.Id == adversaryMan.PilotId);
            playerName = player != null ? player.UserName : ("Player " + playerCount);
        
            // 进一步判断是否外部玩家(同一个cardId) 
            if (player != null)
            {
                if (battleHistory.BattleSelf.CardId == player.Id)
                {
                    playerName = ("Player " + playerCount);
                }
                else if (battleResultContext.NetworkDataDomain.PcbDatas.Count > 0)
                {
                    var pcbSerial = cardIdPcbSerialCache.GetByCardId((uint)player.Id).GetAwaiter().GetResult()?.PcbSerial;
                    if (!battleResultContext.NetworkDataDomain.PcbDatas.Any(x => x.PcbSerial == pcbSerial))
                    {
                        playerName = ("Player " + playerCount);
                    }
                }
            }
        }

        var battlePerson = new BattleAlly()
        {
            BattleHistory = battleHistory,
            CardId = adversaryMan.PilotId,
            PlayerName = playerName,
            ClassId = adversaryMan.ClassId,
            GradeId = adversaryMan.GradeId,
            PrestigeId = adversaryMan.PrestigeId,
            LevelId = adversaryMan.PlayerLevelId,
            MobileSuitId = adversaryMan.MstMobileSuitId,
            SkinId = adversaryMan.SkinId,
            Mastery = adversaryMan.SkillPoint,
            BurstType = adversaryMan.BurstType
        };
        
        AppendBattleLog(battleHistory, adversaryMan, playerName);
        
        return battlePerson;
    }
    
    BattleTarget ConstructTargetWithActionLogAppend(BattleResultContext battleResultContext, BattleHistory battleHistory, AdversaryManGroup adversaryMan, uint playerCount)
    {
        string playerName = default!;
        if (adversaryMan.CpuFlag != 0)
        {
            playerName = "CPU " + playerCount;
        }
        else
        {
            var player = _context.CardProfiles.FirstOrDefault(x => x.Id == adversaryMan.PilotId);
            playerName = player is not null ? player.UserName : ("Player " + playerCount);

            // 进一步判断是否外部玩家(同一个cardId) 
            if (player != null)
            {
                if (battleHistory.BattleSelf.CardId == player.Id)
                {
                    playerName = ("Player " + playerCount);
                }
                else if (battleResultContext.NetworkDataDomain.PcbDatas.Count > 0)
                {
                    var pcbSerial = cardIdPcbSerialCache.GetByCardId((uint)player.Id).GetAwaiter().GetResult()?.PcbSerial;
                    if (!battleResultContext.NetworkDataDomain.PcbDatas.Any(x => x.PcbSerial == pcbSerial))
                    {
                        playerName = ("Player " + playerCount);
                    }
                }
            }
        }

        var battlePerson = new BattleTarget()
        {
            BattleHistory = battleHistory,
            CardId = adversaryMan.PilotId,
            PlayerName = playerName,
            ClassId = adversaryMan.ClassId,
            GradeId = adversaryMan.GradeId,
            PrestigeId = adversaryMan.PrestigeId,
            LevelId = adversaryMan.PlayerLevelId,
            MobileSuitId = adversaryMan.MstMobileSuitId,
            SkinId = adversaryMan.SkinId,
            Mastery = adversaryMan.SkillPoint,
            BurstType = adversaryMan.BurstType
        };
        
        AppendBattleLog(battleHistory, adversaryMan, playerName);
        
        return battlePerson;
    }

    private void AppendBattleLog(BattleHistory battleHistory, AdversaryManGroup adversaryMan, string playerName)
    {
        adversaryMan.BattleLogs.ForEach(battleLog =>
        {
            battleHistory.ActionLogs.Add(new BattleActionLog()
            {
                BattleHistory = battleHistory,
                FrameTime = battleLog.Frame,
                CardId = (int) adversaryMan.PilotId,
                PlayerName = playerName,
                ActionType = battleLog.Status
            });
        });
    }

    private void UpdateConsecutiveWinsWhenLose(BattleHistory battleHistory, CardProfile cardProfile)
    {
        var preBattleHistory = _context.PreBattleHistoryDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile);

        if (preBattleHistory is null)
        {
            return;
        }

        battleHistory.ConsecutiveWinCount = preBattleHistory.CurrentConsecutiveWins;
    }
}