using ServerOver.Caches;
using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad;

public class SaveIbVsRouteBattleStageUiDataCommand(ServerDbContext context, IVsrbStageDataCache vsrbStageDataCache) 
    : ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var cardId = (uint)cardProfile.Id;
        var domain = battleResultContext.StageResultInfoDomain;

        if (domain?.IsCompleted != true || domain.CompletedResult == null)
        {
            return;
        }

        var m = context.VsRouteBattleStageDataInfoDbSet.FirstOrDefault(x => x.CardId == cardId);
        m ??= new();
        m.CreateTime = DateTime.Now;
        m.UpdateTime = DateTime.Now;
        m.CardId = cardId;
        m.PatternId = domain.CompletedResult.PatternId;
        m.Difficulty = domain.Difficulty;
        m.StageId = domain.StageId;
        m.BossIds = string.Join(',', [domain.CompletedResult.Boss1, domain.CompletedResult.Boss2]);
        m.TeamCostMax = domain.PlayResult.TeamCostOnStart;
        m.TeamCostCurr = domain.PlayResult.TeamCostCurr;
        m.MobileSuitId = domain.PlayResult.MsId;
        m.Chips = string.Join(',', domain.CompletedResult.Chips ?? []);
        //
        if (m.Id <= 0) context.VsRouteBattleStageDataInfoDbSet.Add(m);
        else context.VsRouteBattleStageDataInfoDbSet.Update(m);
        context.SaveChanges();

        vsrbStageDataCache.Add(new(cardId, domain.StageId)).GetAwaiter().GetResult();
    }
}