namespace WebUIOver.Shared.Dto.Enum;

public enum BurstType : uint
{
    //Covering = 0, // ob c
	Extend = 0, // ib e
    Fighting = 1, // ib f
    Shooting = 2, // ib s
    //Vertical = 3, // ob v
}

public static class BurstTypeUtils
{
    public static IEnumerable<BurstType> GetAllInOrder()
    {
        yield return BurstType.Shooting;
        yield return BurstType.Extend;
        yield return BurstType.Fighting;
    }

    public static int GetIndexInGame(BurstType b)
    {
        return b switch
        {
            BurstType.Shooting => 0,
            BurstType.Extend => 1,
            BurstType.Fighting => 2,
            _ => 3,
        };
    }
}