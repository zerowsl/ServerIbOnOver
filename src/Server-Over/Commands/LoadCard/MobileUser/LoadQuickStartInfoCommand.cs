using ServerOver.Mapper.QuickStarts;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using Response2 = nue.protocol.exvs.Response2;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadQuickStartInfoCommand(ServerDbContext context)
    : ILoadCard2Command
{
    public void Fill(CardProfile cardProfile, Response2.LoadCard loadCard)
    {
        var cardId = cardProfile.Id;
        
        var m = context.QuickStartInfoDbSet.FirstOrDefault(x => x.CardId == cardId);
        
        // null or close
        if (m?.SaveMode is null or -1)
        {
            return;
        }
        
        loadCard.mobile_user_group.QuickStart = m.ToResponse2QuickStartGroup();
    }
}