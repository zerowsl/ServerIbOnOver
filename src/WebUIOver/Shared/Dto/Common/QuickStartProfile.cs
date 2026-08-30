namespace WebUIOver.Shared.Dto.Common;

public class QuickStartProfile
{
    // -1=Close 0=Normal 1=SameGameMode 2=SetOnly
    public int SaveMode { get; set; } = 0;

    public uint GameMode { get; set; }
    public uint RuleType { get; set; }
    public uint TeamType { get; set; }

    public uint MstMobileSuitId { get; set; }
    public uint BurstType { get; set; }

    public uint PartnerMobileSuitId { get; set; } = 0;
    public uint PartnerBurstType { get; set; } = 0;

    public uint BattleStageId { get; set; }
}