using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Models.Cards.Triad;
using ServerOver.Utils;

namespace ServerOver.Mapper.Card.Triad;

[Mapper]
public static partial class VsRouteBattleDataMapper
{
    [MapperIgnoreTarget(nameof(Response2.LoadCard.MobileUserGroup.VsRouteBattleDataGroup.Boss1))]
    [MapperIgnoreTarget(nameof(Response2.LoadCard.MobileUserGroup.VsRouteBattleDataGroup.Boss2))]
    [MapperIgnoreTarget(nameof(Response2.LoadCard.MobileUserGroup.VsRouteBattleDataGroup.Chips))]
    [MapperIgnoreTarget(nameof(Response2.LoadCard.MobileUserGroup.VsRouteBattleDataGroup.Url))]
    private static partial Response2.LoadCard.MobileUserGroup.VsRouteBattleDataGroup ToVsRouteBattleData0(VsRouteBattleStageDataInfo dto);

    [UserMapping(Default = true)]
    public static Response2.LoadCard.MobileUserGroup.VsRouteBattleDataGroup ToVsRouteBattleData(this VsRouteBattleStageDataInfo dto)
    {
        // before mapping
        var r = ToVsRouteBattleData0(dto);
        // after mapping 
        if (dto.BossIds != null)
        {
            var ids = ArrayUtil.FromString(dto.BossIds);
            r.Boss1 = ids.ElementAtOrDefault(0);
            r.Boss2 = ids.ElementAtOrDefault(1);
        }
        if (dto.Chips != null)
        {
            r.Chips = ArrayUtil.FromString(dto.Chips);
        }
        return r;
    }
}