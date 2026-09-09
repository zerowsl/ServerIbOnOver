using nue.protocol.exvs;
using ServerOver.Context.Battle;
using ServerOver.Handlers.Game;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.PvP;

public class SaveClassMatchCommand(ServerDbContext context, CardServerConfig config, ILogger? logger) 
    : ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var onlineDomain = battleResultContext.OnlineBattleDomain;
        var result = onlineDomain.class_match_result;
        if (onlineDomain.OnlineMatchingMode != OnlineMatchingMode.OmmClass || result == null)
        {
            return;
        }
        if (config.CustomConfigs?.ClassMatchG?.EnableRate != true)
        {
            return;
        }
        if (onlineDomain.TeamType == 0)
        {
            SaveSolo(cardProfile, battleResultContext);
        }
        else
        {
            SaveTeam(cardProfile, battleResultContext);
        }
    }

    private void SaveSolo(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var onlineDomain = battleResultContext.OnlineBattleDomain;
        var result = onlineDomain.class_match_result!;
        
        var (afterRate, afterClassId, afterGradeId) = (result.AfterRate, result.ClassIdSolo, result.GradeId);
        var isChanged = ComputeAfter(afterRate, ref afterClassId, ref afterGradeId, 0, result.ObtainedRate);
        logger?.LogWarning("vsm_on solo after \"classId={afterClassId}, gradeId={afterGradeId}, rate={afterRate}\" obtained='{obtainedRate}'.", afterClassId, afterGradeId, afterRate, result.ObtainedRate);
        if (!isChanged)
        {
            return;
        }
        
        var m = context.SoloClassMatchGRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id);
        m ??= SoloClassMatchGRecord.New(cardProfile.Id);
        m.ClassId = afterClassId;
        m.GradeId = afterGradeId;
        m.Rate = afterRate;
        //m.WeeklyTotalBattleCount = 
        
        if (m.Id <= 0) context.SoloClassMatchGRecordDbSet.Add(m);
        else context.SoloClassMatchGRecordDbSet.Update(m);
        context.SaveChanges();
    }

    private void SaveTeam(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var onlineDomain = battleResultContext.OnlineBattleDomain;
        var result = onlineDomain.class_match_result!;
        
        var (afterRate, afterClassId, afterGradeId) = (result.AfterRate, result.ClassIdTeam, result.GradeId);
        var isChanged = ComputeAfter(afterRate, ref afterClassId, ref afterGradeId, 1, result.ObtainedRate);
        logger?.LogWarning("vsm_on team after \"classId={afterClassId}, gradeId={afterGradeId}, rate={afterRate}\" obtained='{obtainedRate}'.", afterClassId, afterGradeId, afterRate, result.ObtainedRate);
        if (!isChanged)
        {
            return;
        }

        var m = context.TeamClassMatchGRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id);
        m ??= TeamClassMatchGRecord.New(cardProfile.Id);
        m.ClassId = afterClassId;
        m.GradeId = afterGradeId;
        m.Rate = afterRate;
        //m.WeeklyTotalBattleCount = 
        
        if (m.Id <= 0) context.TeamClassMatchGRecordDbSet.Add(m);
        else context.TeamClassMatchGRecordDbSet.Update(m);
        context.SaveChanges();
    }
    
    static Response2.LoadClassMatch GetLoadClassMatch()
    {
        var loadClassMatch = new Response2.LoadClassMatch();
        LoadClassMatchCommand2Handler.LoadGradeThresholds(loadClassMatch);
        return loadClassMatch;
    }
    
    static uint? FindMinRate(Response2.LoadClassMatch loadClassMatch, uint teamType, uint classId, uint gradeId)
    {
        return loadClassMatch.GradeThresholds.FirstOrDefault(x => x.TeamType == teamType && x.ClassId == classId && x.GradeId == gradeId)?.MinRate;
    }
    
    static uint? FindMaxRate(Response2.LoadClassMatch loadClassMatch, uint teamType, uint classId, uint gradeId)
    {
        return loadClassMatch.GradeThresholds.FirstOrDefault(x => x.TeamType == teamType && x.ClassId == classId && x.GradeId == gradeId)?.MaxRate;
    }
    
    internal bool ComputeAfter(float afterRate, ref uint afterClassId, ref uint afterGradeId, uint teamType, float obtainedRate)
    {
        var loadClassMatch = GetLoadClassMatch();
        
        var maxRate = FindMaxRate(loadClassMatch, teamType, afterClassId, afterGradeId);
        if (maxRate != null && afterRate >= maxRate.Value)
        {
            afterGradeId += 1;
            if (afterGradeId >= 11)
            {
                if (afterClassId >= 4) return true;
                afterClassId += 1;
                afterGradeId = 1;
            }
            return true;
        }
        
        var minRate = FindMinRate(loadClassMatch, teamType, afterClassId, afterGradeId);
        if (minRate != null && afterRate <= minRate.Value)
        {
            afterGradeId -= 1;
            if (afterGradeId == 0)
            {
                afterClassId -= 1;
                afterGradeId = 10;
                if (teamType == 0 && afterClassId < 1)
                {
                    afterClassId = 1;
                    afterGradeId = 1;
                }
                else if (teamType == 1 && afterClassId < 2)
                {
                    afterClassId = 2;
                    afterGradeId = 1;
                }
            }
            return true;
        }
        
        // no changed
        return obtainedRate != 0 && maxRate != null && minRate != null;
    }
}