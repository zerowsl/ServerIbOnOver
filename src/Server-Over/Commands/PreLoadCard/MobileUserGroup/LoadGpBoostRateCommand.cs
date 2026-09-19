using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class LoadGpBoostRateCommand(ServerDbContext context, CardServerConfig config) 
    : BasePreLoadCard2Command
{
    public override void Fill(CardProfile cardProfile, Response2.PreLoadCard.MobileUserGroup mobileUserGroup) 
    {
        var cardId = (uint)cardProfile.Id;

        if (mobileUserGroup.customize_group != null && config.CustomConfigs != null)
        {
            mobileUserGroup.customize_group.GpBoostRate = config.CustomConfigs.GpBoostRate;
        }
    }
}