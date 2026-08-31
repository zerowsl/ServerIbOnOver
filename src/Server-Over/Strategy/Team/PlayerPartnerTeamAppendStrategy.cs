using nue.protocol.exvs;
using ServerOver.Constants;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Strategy.Team;

public class PlayerPartnerTeamAppendStrategy : ITagTeamAppendStrategy
{
    private readonly ServerDbContext _context;

    public PlayerPartnerTeamAppendStrategy(ServerDbContext context)
    {
        _context = context;
    }

    public void Append(CardProfile cardProfile, List<TagTeamGroup> tagTeams)
    {
        _context.TagTeamDataDbSet
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
                    BoostRemains = TeamConstants.BoostRemains
                });
            });

        var partnerIds = tagTeams.Select(x => (int)x.PartnerId).Distinct().ToArray();
        var partners = partnerIds.Length == 0 ? [] : _context.CardProfiles.Where(x => partnerIds.Contains(x.Id)).ToArray();
        tagTeams.ForEach(tagTeam =>
        {
            var p = partners.FirstOrDefault(x => x.Id == tagTeam.PartnerId);
            if (p != null) tagTeam.PartnerName = p.UserName;
        });
    }
}