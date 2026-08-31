using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class LoadPlayerBadgeCommand(ServerDbContext context) : IPreLoadCard2Command
{
    public void Fill(CardProfile cardProfile, Response2.PreLoadCard preLoadCard)
    {
        var cardId = (uint)cardProfile.Id;
        var mobileUserGroup = preLoadCard.User;
        if (mobileUserGroup == null) return;

        //preLoadCard.User.PlayerBadgeId = preLoadCard?.load_player?.PrestigeId ?? 0u;

        var pb = context.PlayerBadgeDbSet.FirstOrDefault(x => x.CardId == cardId);
        pb ??= new() { CardId = cardId };

        mobileUserGroup.PlayerBadgeId = pb.BadgeId;
        mobileUserGroup.PlayerBadgeExp = (uint)pb.BadgeExp;
    }
}