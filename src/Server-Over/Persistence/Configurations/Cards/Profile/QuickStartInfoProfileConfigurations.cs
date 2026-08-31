using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Profile;

namespace ServerOver.Persistence.Configurations.Cards.Profile;

public class QuickStartInfoProfileConfigurations : IEntityTypeConfiguration<QuickStartInfoProfile>
{
    public void Configure(EntityTypeBuilder<QuickStartInfoProfile> builder)
    {
        builder.HasKey(x => x.Id);
        var i = 0;
        builder.Property(x => x.Id).HasColumnOrder(++i);
        builder.Property(x => x.CardId).HasColumnOrder(++i);
        builder.Property(x => x.SaveMode).HasColumnOrder(++i);
        builder.Property(x => x.GameMode).HasColumnOrder(++i);
        builder.Property(x => x.RuleType).HasColumnOrder(++i);
        builder.Property(x => x.TeamType).HasColumnOrder(++i);
        builder.Property(x => x.MstMobileSuitId).HasColumnOrder(++i);
        builder.Property(x => x.BurstType).HasColumnOrder(++i);
        builder.Property(x => x.PartnerMobileSuitId).HasColumnOrder(++i);
        builder.Property(x => x.PartnerBurstType).HasColumnOrder(++i);
		builder.Property(x => x.BattleStageId).HasColumnOrder(++i);
        builder.Property(x => x.CreateTime).HasColumnOrder(++i);
        builder.Property(x => x.UpdateTime).HasColumnOrder(++i);
    }
}