namespace ServerOver.Context.Battle.Domain;

public class PlayerBadgeDomain
{
    public uint? BadgeIdBefore { get; set; }
    public uint? BadgeIdAfter { get; set; }
    public int ExpIncrement { get; set; } = 0;

    public bool IsGameOver { get; set; } = false;
    public bool IsWin { get; set; }
    public uint PlayerLevelId { get; set; }
}