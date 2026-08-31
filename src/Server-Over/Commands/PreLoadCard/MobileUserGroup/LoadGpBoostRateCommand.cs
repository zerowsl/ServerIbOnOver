using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class LoadGpBoostRateCommand(ServerDbContext context, CardServerConfig config) 
    : IPreLoadCard2Command
{
    public void Fill(CardProfile cardProfile, Response2.PreLoadCard preLoadCard)
    {
        var cardId = (uint)cardProfile.Id;
        var mobileUserGroup = preLoadCard.User;
        if (mobileUserGroup == null) return;

        if (mobileUserGroup.customize_group != null && config.CustomConfigs != null)
        {
            mobileUserGroup.customize_group.GpBoostRate = config.CustomConfigs.GpBoostRate;
        }
    }
}