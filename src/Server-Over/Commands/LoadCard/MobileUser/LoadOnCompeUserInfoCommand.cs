using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadOnCompeUserInfoCommand(ServerDbContext context, CustomConfigs? customConfigs) 
    : BaseLoadCard2Command
{
    public override void Fill(CardProfile cardProfile, Response2.LoadCard.MobileUserGroup mobileUserGroup)
    {
        if (customConfigs?.Room?.Enable != true || customConfigs?.Room?.OnCompe?.Enable != true)
        {
            return;
        }
        
        BaseOnCompeRecord dto = default!;
        var mt = customConfigs!.Room!.MatchingType;
        switch (mt)
        {
            case 1u:
                var dto1 = context.SoloOnCompeRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id && !x.IsOutdated);
                dto = dto1 ?? SoloOnCompeRecord.New(cardProfile.Id);
                break;
            case 2u:
                var dto2 = context.TeamOnCompeRecordDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id && !x.IsOutdated);
                dto = dto2 ?? TeamOnCompeRecord.New(cardProfile.Id);
                break;
        }
        if (dto == default) return;

        mobileUserGroup.OnCompeUserInfo = new()
        {
            OnCompePoint = dto.Point, 
            CurrentRank = dto.Rank
        };
    }
}