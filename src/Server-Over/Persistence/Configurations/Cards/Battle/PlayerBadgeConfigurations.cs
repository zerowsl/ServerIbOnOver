using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class PlayerBadgeConfigurations : IEntityTypeConfiguration<PlayerBadge>
{
    public void Configure(EntityTypeBuilder<PlayerBadge> builder)
    {
        builder.HasKey(x => x.Id);
        var i = 0;
        builder.Property(x => x.Id).HasColumnOrder(++i);
        builder.Property(x => x.CardId).HasColumnOrder(++i);
        builder.Property(x => x.BadgeId).HasColumnOrder(++i);
        builder.Property(x => x.BadgeExp).HasColumnOrder(++i);
        builder.Property(x => x.CreateTime).HasColumnOrder(++i);
        builder.Property(x => x.UpdateTime).HasColumnOrder(++i);
    }
}