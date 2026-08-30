using ServerOver.Common.Enum;

namespace ServerOver.Models.Config;

public class FesConfigurations
{
    public FesType MondayType { get; set; } = FesType.AttackBoost;
    public FesType TuesdayType { get; set; } = FesType.AttackBoost;
    public FesType WednesdayType { get; set; } = FesType.AttackBoost;
    public FesType ThursdayType { get; set; } = FesType.AttackBoost;
    public FesType FridayType { get; set; } = FesType.AttackBoost;
    public FesType SaturdayType { get; set; } = FesType.AttackBoost;
    public FesType SundayType { get; set; } = FesType.AttackBoost;

    public FesType GetTodayFesType()
    {
        var i = (int)DateTime.Now.DayOfWeek;
        var type = GetFesTypes().ElementAt(i);
        return type;
    }

    IEnumerable<FesType> GetFesTypes()
    {
        yield return SundayType;
        yield return MondayType;
        yield return TuesdayType;
        yield return WednesdayType;
        yield return ThursdayType;
        yield return FridayType;
        yield return SaturdayType;
    }
}