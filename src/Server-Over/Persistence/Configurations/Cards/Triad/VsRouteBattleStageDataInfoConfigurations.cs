using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Triad;

namespace ServerOver.Persistence.Configurations.Cards.Triad;

public class VsRouteBattleStageDataInfoConfigurations : IEntityTypeConfiguration<VsRouteBattleStageDataInfo>
{
    public void Configure(EntityTypeBuilder<VsRouteBattleStageDataInfo> builder)
    {
        builder.HasKey(x => x.Id);
        var i = 0;
        builder.Property(x => x.Id).HasColumnOrder(++i);
        builder.Property(x => x.CardId).HasColumnOrder(++i);
        builder.Property(x => x.PatternId).HasColumnOrder(++i);
        builder.Property(x => x.Difficulty).HasColumnOrder(++i);
        builder.Property(x => x.StageId).HasColumnOrder(++i);
        builder.Property(x => x.TeamCostMax).HasColumnOrder(++i);
        builder.Property(x => x.TeamCostCurr).HasColumnOrder(++i);
        builder.Property(x => x.MobileSuitId).HasColumnOrder(++i);
        builder.Property(x => x.Chips).HasColumnOrder(++i);
        builder.Property(x => x.BossIds).HasColumnOrder(++i);
        builder.Property(x => x.CreateTime).HasColumnOrder(++i);
        builder.Property(x => x.UpdateTime).HasColumnOrder(++i);
    }
}