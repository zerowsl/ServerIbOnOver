using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Profile;

namespace ServerOver.Persistence.Configurations.Cards.Profile;

public class TrainingProfileConfigurations : IEntityTypeConfiguration<TrainingProfile>
{
    public void Configure(EntityTypeBuilder<TrainingProfile> builder)
    {
        builder.HasKey(x => x.Id);
        var i = 0;
        builder.Property(x => x.Id).HasColumnOrder(++i);
        builder.Property(x => x.CardId).HasColumnOrder(++i);
        builder.Property(x => x.MstMobileSuitId).HasColumnOrder(++i);
        builder.Property(x => x.BurstType).HasColumnOrder(++i);
        builder.Property(x => x.CpuLevel).HasColumnOrder(++i);
        builder.Property(x => x.ExBurstGauge).HasColumnOrder(++i);
        builder.Property(x => x.DamageDisplay).HasColumnOrder(++i);
        builder.Property(x => x.CpuAutoGuard).HasColumnOrder(++i);
        builder.Property(x => x.CommandGuideDisplay).HasColumnOrder(++i);
        builder.Property(x => x.CreateTime).HasColumnOrder(++i);
        builder.Property(x => x.UpdateTime).HasColumnOrder(++i);
        builder.Property(x => x.Player1HpMode).HasColumnOrder(++i);
        builder.Property(x => x.CpuHpAutoRegen).HasColumnOrder(++i);
        builder.Property(x => x.ExOverLimit).HasColumnOrder(++i);
        builder.Property(x => x.QuickFill).HasColumnOrder(++i);
    }
}