using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.PreLoadCard.MobileUserGroup;

public class DisplaySettingCommand(ServerDbContext context) : IPreLoadMobileUserGroupCommand
{
    private readonly ServerDbContext _context = context;

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.MobileUserGroup mobileUserGroup)
    {
        var playerProfile = _context.PlayerProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.OpenRecord = playerProfile.OpenRecord;
        mobileUserGroup.OpenEchelon = playerProfile.OpenEchelon;
        mobileUserGroup.OpenSkillpoint = playerProfile.OpenSkillpoint;

        // 对战和街机的按键显示方式,这里跟训练场一样
        var trainingProfile = _context.TrainingProfileDbSet.FirstOrDefault(x => x.CardProfile == cardProfile);
        mobileUserGroup.customize_group.CommandGuideDisplay = trainingProfile?.CommandGuideDisplay ?? 0;
    }
}