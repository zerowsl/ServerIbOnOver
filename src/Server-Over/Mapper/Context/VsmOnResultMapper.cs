using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Context.Battle;

namespace ServerOver.Mapper.Context;

[Mapper]
public static partial class VsmOnResultMapper
{
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.Gp)],
        [nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.GpIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.WinFlag)],
        [nameof(BattleResultContext.CommonDomain), nameof(BattleResultContext.CommonDomain.IsWin)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.LevelId)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdBefore)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.PlayerLevelId)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.LevelIdAfter)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.LevelExp)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.ExpIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.PrestigeId)],
        [nameof(BattleResultContext.PlayerLevelDomain), nameof(BattleResultContext.PlayerLevelDomain.PrestigeId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.GuestNavId)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.GuestNavFamiliarity)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.GuestNavFamiliarityIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.BattleNavId)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.BattleNavFamiliarity)],
        [nameof(BattleResultContext.NaviDomain), nameof(BattleResultContext.NaviDomain.BattleNavFamiliarityIncrement)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.SkillPointMobileSuitId)],
        [nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.MasteryMobileSuitId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.MstMobileSuitId)],
        [nameof(BattleResultContext.MobileSuitMasteryDomain), nameof(BattleResultContext.MobileSuitMasteryDomain.ActualMobileSuitId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.TagTeamId)],
        [nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.TagSkillPoint)],
        [nameof(BattleResultContext.TeamDomain), nameof(BattleResultContext.TeamDomain.TeamExp)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.PlayedAt)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.PlayedAt)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.VsElapsedTime)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ElapsedSeconds)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.StageId)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.StageId)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.ResultScore)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.Score)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.ResultOrder)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ScoreRank)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.GivenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalGivenDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.TakenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalTakenDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.ConsecutiveWin)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ConsecutiveWinCount)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.NoDamageFlag)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.NoDamageFlag)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.BurstGivenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.TotalExBurstDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.BurstType)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.BurstType)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.Burst)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.BurstCount)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.Overheat)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.OverheatCount)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.ComboGivenDamage)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.ComboGivenDamage)]
    )]
    [MapProperty(
        [nameof(Request.SaveVsmOnResult.PlayResultGroup.SkinId)],
        [nameof(BattleResultContext.BattleStatisticDomain), nameof(BattleResultContext.BattleStatisticDomain.SkinId)]
    )]
    public static partial BattleResultContext ToBattleResultContext(this Request.SaveVsmOnResult.PlayResultGroup resultGroup);
}