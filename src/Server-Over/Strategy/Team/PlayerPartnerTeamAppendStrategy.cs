using nue.protocol.exvs;
using ServerOver.Constants;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Strategy.Team;

public class PlayerPartnerTeamAppendStrategy(ServerDbContext context) : ITagTeamAppendStrategy
{
    public void Append(CardProfile cardProfile, List<TagTeamGroup> tagTeams)
    {
        context.TagTeamDataDbSet
            .Where(x => x.TeammateCardId == cardProfile.Id)
            .ToList()
            .ForEach(tagTeam =>
            {
                tagTeams.Add(new TagTeamGroup()
                {
                    Id = (uint)tagTeam.Id,
                    Name = tagTeam.TeamName,
                    PartnerId = (uint)tagTeam.CardId,
                    SkillPoint = tagTeam.SkillPoint,
                    SkillPointBoost = TeamConstants.TagSkillPointBoost,
                    BackgroundPartsId = tagTeam.BackgroundPartsId,
                    EffectId = tagTeam.EffectId,
                    EmblemId = tagTeam.EmblemId,
                    BgmId = tagTeam.BgmId,
                    NameColorId = tagTeam.NameColorId,
                    BoostRemains = TeamConstants.BoostRemains,
                    //GroupTagPoint = (uint)tagTeam.OnlineRankPoint,
                });
            });

        var partnerIds = tagTeams.Select(x => (int)x.PartnerId).Distinct().ToArray();
        var partners = partnerIds.Length == 0 ? [] : context.CardProfiles.Where(x => partnerIds.Contains(x.Id)).ToArray();
        var partnerLvs = partnerIds.Length == 0 ? [] : context.PlayerLevelDbSet.Where(x => partnerIds.Contains(x.CardId)).ToArray();
        var partnerBadges = partnerIds.Length == 0 ? [] : context.PlayerBadgeDbSet.Where(x => partnerIds.Contains((int)x.CardId)).ToArray();
        var partnerTeamClasses = partnerIds.Length == 0 ? [] : context.TeamClassMatchGRecordDbSet.Where(x => partnerIds.Contains(x.CardId)).ToArray();
        var onlinePairs = partnerIds.Length == 0 ? [] : context.OnlinePairDbSet.Where(x => x.CardId == x.CardId).ToArray();

        tagTeams.ForEach(tagTeam =>
        {
            var p = partners.FirstOrDefault(x => x.Id == tagTeam.PartnerId);
            if (p != null) tagTeam.PartnerName = p.UserName;

            var lv = partnerLvs.FirstOrDefault(x => x.CardId == tagTeam.PartnerId);
            if (lv != null) tagTeam.PartnerPlayerLevelId = lv.PlayerLevelId;

            var badge = partnerBadges.FirstOrDefault(x => x.CardId == tagTeam.PartnerId);
            if (badge != null) tagTeam.PartnerBadgeId = badge.BadgeId;

            var g = partnerTeamClasses.FirstOrDefault(x => x.CardId == tagTeam.PartnerId);
            if (g != null)
            {
                tagTeam.PartnerClassId = g.ClassId;
                tagTeam.PartnerGradeId = g.GradeId;
            }

            tagTeam.TagStatus = onlinePairs.Any(x => x.TeamId == tagTeam.Id) ? 1u : 0u;
        });
    }
}