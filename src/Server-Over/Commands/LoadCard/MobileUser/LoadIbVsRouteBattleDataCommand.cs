using nue.protocol.exvs;
using ServerOver.Mapper.Card.Triad;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

// 加载ib街机模式中断再开的(ui)数据
public class LoadIbVsRouteBattleDataCommand(string baseAddress,
    ServerDbContext context) 
    : BaseLoadCard2Command
{
    public override void Fill(CardProfile cardProfile, Response2.LoadCard.MobileUserGroup mobileUserGroup)
    {
        if (!HasData(cardProfile.Id)) return;
        
        var dto = context.VsRouteBattleStageDataInfoDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id);
        if (dto == null) return;

        mobileUserGroup.VsRouteBattleData = dto.ToVsRouteBattleData();
        mobileUserGroup.VsRouteBattleData.Url = $"{baseAddress}/gamedata/vsrb/r?cardid={cardProfile.Id}";
    }

    static bool HasData(int cardId)
    {
        return File.Exists(Path.Combine(GlobalVars.VsRouteBattleSaveDataDir, $"{cardId}.bin"));
    }
}