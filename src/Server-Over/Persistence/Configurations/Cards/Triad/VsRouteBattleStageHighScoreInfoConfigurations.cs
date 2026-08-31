using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Triad;

namespace ServerOver.Persistence.Configurations.Cards.Triad;

public class VsRouteBattleStageHighScoreInfoConfigurations : IEntityTypeConfiguration<VsRouteBattleStageHighScoreInfo>
{
    public void Configure(EntityTypeBuilder<VsRouteBattleStageHighScoreInfo> builder)
    {
        builder.HasKey(x => x.Id);
        var i = 0;
        builder.Property(x => x.Id).HasColumnOrder(++i);
        builder.Property(x => x.CardId).HasColumnOrder(++i);
        builder.Property(x => x.Difficulty).HasColumnOrder(++i);
        builder.Property(x => x.ClearTime1).HasColumnOrder(++i);
        builder.Property(x => x.ClearScore1).HasColumnOrder(++i);
        builder.Property(x => x.ClearTime2).HasColumnOrder(++i);
        builder.Property(x => x.ClearScore2).HasColumnOrder(++i);
        builder.Property(x => x.ClearTime3).HasColumnOrder(++i);
        builder.Property(x => x.ClearScore3).HasColumnOrder(++i);
        builder.Property(x => x.TotalClearTime).HasColumnOrder(++i);
        builder.Property(x => x.TotalClearScore).HasColumnOrder(++i);
        builder.Property(x => x.IsRecord).HasColumnOrder(++i);
        builder.Property(x => x.CreateTime).HasColumnOrder(++i);
        builder.Property(x => x.UpdateTime).HasColumnOrder(++i);
    }
}