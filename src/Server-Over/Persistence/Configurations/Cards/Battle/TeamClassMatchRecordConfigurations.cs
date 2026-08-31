using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class TeamClassMatchRecordConfigurations : IEntityTypeConfiguration<TeamClassMatchRecord>
{
    public void Configure(EntityTypeBuilder<TeamClassMatchRecord> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class TeamClassMatchGRecordConfigurations : IEntityTypeConfiguration<TeamClassMatchGRecord>
{
    public void Configure(EntityTypeBuilder<TeamClassMatchGRecord> builder)
    {
        builder.HasKey(x => x.Id);
    }
}