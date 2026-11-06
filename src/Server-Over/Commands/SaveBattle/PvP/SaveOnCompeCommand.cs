using nue.protocol.exvs;
using ServerOver.Context.Battle;
using ServerOver.Handlers.Game;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.PvP;

public class SaveOnCompeCommand(ServerDbContext context, CardServerConfig config, ILogger? logger) 
    : ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var onlineDomain = battleResultContext.OnlineBattleDomain;
        var result = onlineDomain.on_compe_result;
        if (onlineDomain.OnlineMatchingMode != OnlineMatchingMode.OmmEvent || result == null)
        {
            return;
        }
        if (config.CustomConfigs?.Room?.Enable != true || config.CustomConfigs?.Room?.OnCompe?.Enable != true || config.CustomConfigs?.Room?.OnCompe?.EnableSettlement != true)
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
        var result = onlineDomain.on_compe_result!;
        
        logger?.LogWarning("vsm_on solo user='{cardId}', before_oncompe_point=''{beforeOncompePoint}', after_oncompe_point='{afterOncompePoint}'.", cardProfile.Id, result.BeforeOncompePoint, result.AfterOncompePoint);
        
        var m = context.SoloOnCompeRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id && !x.IsOutdated);
        m ??= SoloOnCompeRecord.New(cardProfile.Id);
        m.Point = result.AfterOncompePoint;
        //m.Rank = 
        
        if (m.Id <= 0) context.SoloOnCompeRecordDbSet.Add(m);
        else context.SoloOnCompeRecordDbSet.Update(m);
        context.SaveChanges();
    }

    private void SaveTeam(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var onlineDomain = battleResultContext.OnlineBattleDomain;
        var result = onlineDomain.on_compe_result!;
        
        logger?.LogWarning("vsm_on team user='{cardId}', before_oncompe_point=''{beforeOncompePoint}', after_oncompe_point='{afterOncompePoint}'.", cardProfile.Id, result.BeforeOncompePoint, result.AfterOncompePoint);
        
        var m = context.TeamOnCompeRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id && !x.IsOutdated);
        m ??= TeamOnCompeRecord.New(cardProfile.Id);
        m.Point = result.AfterOncompePoint;
        //m.Rank = 
        
        if (m.Id <= 0) context.TeamOnCompeRecordDbSet.Add(m);
        else context.TeamOnCompeRecordDbSet.Update(m);
        context.SaveChanges();
    }
    

}