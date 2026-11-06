using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerOver.Models.Cards.Battle;

namespace ServerOver.Persistence.Configurations.Cards.Battle;

public class SoloOnCompeRecordConfigurations : IEntityTypeConfiguration<SoloOnCompeRecord>
{
    public void Configure(EntityTypeBuilder<SoloOnCompeRecord> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class TeamOnCompeRecordConfigurations : IEntityTypeConfiguration<TeamOnCompeRecord>
{
    public void Configure(EntityTypeBuilder<TeamOnCompeRecord> builder)
    {
        builder.HasKey(x => x.Id);
    }
}