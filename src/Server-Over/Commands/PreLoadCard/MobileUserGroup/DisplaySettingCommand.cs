using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class DisplaySettingCommand(ServerDbContext context) : BasePreLoadCard2Command
{
    public override void Fill(CardProfile cardProfile, Response2.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var playerProfile = context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.OpenRecord = playerProfile.OpenRecord;
        mobileUserGroup.OpenEchelon = playerProfile.OpenEchelon;
        mobileUserGroup.OpenSkillpoint = playerProfile.OpenSkillpoint;

        // 不用再查数据库了
        mobileUserGroup.GamePadStyle = playerProfile?.GamePadStyle ?? 0;
        mobileUserGroup.customize_group ??= new();
        mobileUserGroup.customize_group.CommandDispConfig = playerProfile?.CommandDispConfig ?? 0;
    }
}