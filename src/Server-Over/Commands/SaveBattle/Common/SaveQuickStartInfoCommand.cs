using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Profile;
using ServerOver.Persistence;
using QuickStartInfo = nue.protocol.exvs.QuickStartInfo;

namespace ServerOver.Commands.SaveBattle.Common;

public class SaveQuickStartInfoCommand(ServerDbContext context, QuickStartInfo quickStartInfo = default!)
    : ISaveBattleDataCommand
{
    private readonly QuickStartInfo _quickStartInfo = quickStartInfo;
    
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var cardId = (uint)cardProfile.Id;
        var quickStartInfo = _quickStartInfo; //
        
        if (quickStartInfo == null || quickStartInfo.MstMobileSuitId <= 0u)
        {
            // 街机小关卡第一关之后全是0 ...
            return;
        }
        
        var m = context.QuickStartInfoDbSet.FirstOrDefault(x => x.CardId == cardId);
        var isChanged = false;
        switch (m?.SaveMode)
        {
            // save nomarl
            case null:
            case 0:
                m ??= new() { CardId = cardId, SaveMode = 0 };
                isChanged = Set(m, quickStartInfo);
                break;
            
            // save on same GameMode
            case 1:
                if (m.GameMode == quickStartInfo.GameMode)
                {
                    isChanged = Set(m, quickStartInfo);
                }
                break;
            
            // -1 = not enable (close)
            //  2 = not save, set only
            default:
                break;
        }
        if (isChanged)
        {
            if (m.Id <= 0) context.QuickStartInfoDbSet.Add(m);
            else context.QuickStartInfoDbSet.Update(m);
            context.SaveChanges();
        }
    }
    
    static bool Set(QuickStartInfoProfile m, QuickStartInfo quickStartInfo)
    {
        m.GameMode = quickStartInfo.GameMode;
        m.RuleType = quickStartInfo.RuleType;
        m.TeamType = quickStartInfo.TeamType;
        m.MstMobileSuitId = quickStartInfo.MstMobileSuitId;
        m.BurstType = quickStartInfo.BurstType;
        m.PartnerMobileSuitId = quickStartInfo.PartnerMobileSuitId;
        m.PartnerBurstType = quickStartInfo.PartnerBurstType;
        m.BattleStageId = quickStartInfo.BattleStageId;
        return true;
    }
}