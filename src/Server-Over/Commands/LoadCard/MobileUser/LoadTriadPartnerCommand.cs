using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.MobileUser;

public class LoadTriadPartnerCommand(ServerDbContext context) : ILoadCardMobileUserCommand
{
    public void Fill(CardProfile cardProfile, Response.LoadCard.MobileUserGroup mobileUserGroup)
    {
        var triadPartner = context.TriadPartnerDbSet
            .First(x => x.CardProfile == cardProfile);

        mobileUserGroup.MstMobileSuitId = triadPartner.MstMobileSuitId;
        mobileUserGroup.ArmorLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.ArmorLevel : 0;
        mobileUserGroup.ShootAttackLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.ShootAttackLevel : 0;
        mobileUserGroup.InfightAttackLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.InfightAttackLevel : 0;
        mobileUserGroup.BoosterLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.BoosterLevel : 0;
        mobileUserGroup.ExGaugeLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.ExGaugeLevel : 0;
        mobileUserGroup.AiLevel = triadPartner.MstMobileSuitId > 0 ? triadPartner.AiLevel : 0;
        mobileUserGroup.BurstType = triadPartner.MstMobileSuitId > 0 ? triadPartner.BurstType : 0;
        mobileUserGroup.TriadTeamName = triadPartner.TriadTeamName;
        mobileUserGroup.TriadBackgroundPartsId = triadPartner.MstMobileSuitId > 0 ? triadPartner.TriadBackgroundPartsId : 0;

        // ib
        mobileUserGroup.BattleOption1 = triadPartner.MstMobileSuitId > 0 ? triadPartner.MsSkill1 : 0;
        mobileUserGroup.BattleOption2 = triadPartner.MstMobileSuitId > 0 ? triadPartner.MsSkill2 : 0;
        mobileUserGroup.BattleOption3 = triadPartner.MstMobileSuitId > 0 ? triadPartner.MsSkill3 : 0;
        mobileUserGroup.TriadGroupPlateId = triadPartner.MstMobileSuitId > 0 ? triadPartner.GroupPlateId : 0;
        mobileUserGroup.TriadGroupEmblemBaseId = triadPartner.MstMobileSuitId > 0 ? triadPartner.GroupEmblemBaseId : 0;
        mobileUserGroup.TriadGroupEmblemPartsId = triadPartner.MstMobileSuitId > 0 ? triadPartner.GroupEmblemPartsId : 0;
        mobileUserGroup.TriadGroupEmblemEffectId = triadPartner.MstMobileSuitId > 0 ? triadPartner.GroupEmblemEffectId : 0;
    }
}