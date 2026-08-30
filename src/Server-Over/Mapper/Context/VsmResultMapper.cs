using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Context.Battle;

namespace ServerOver.Mapper.Context;

[Mapper]
public static partial class VsmResultMapper
{
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.Gp)],
        [nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.GpIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.WinFlag)],
        [nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.IsWin)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.LevelId)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdBefore)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.LevelId)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdBefore)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.PlayerLevelId)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdAfter)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.LevelExp)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.ExpIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.PrestigeId)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.PrestigeId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.GuestNavId)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.GuestNavFamiliarity)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavFamiliarityIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.BattleNavId)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.BattleNavFamiliarity)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavFamiliarityIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.SkillPointMobileSuitId)],
        [nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.MasteryMobileSuitId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.MstMobileSuitId)],
        [nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.ActualMobileSuitId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.TagTeamId)],
        [nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.TagSkillPoint)],
        [nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamExp)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.PlayedAt)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.PlayedAt)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.VsElapsedTime)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ElapsedSeconds)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.StageId)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.StageId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.ResultScore)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.Score)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.ResultOrder)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ScoreRank)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.GivenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalGivenDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.TakenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalTakenDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.ConsecutiveWin)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ConsecutiveWinCount)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.NoDamageFlag)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.NoDamageFlag)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.BurstGivenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalExBurstDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.BurstType)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.BurstType)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.Burst)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.BurstCount)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.Overheat)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.OverheatCount)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.ComboGivenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ComboGivenDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmResult.PlayResultGroup.SkinId)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.SkinId)]
    )]
    public static partial BattleResultContext ToBattleResultContext(this Request.SaveVsmResult.PlayResultGroup resultGroup);
}