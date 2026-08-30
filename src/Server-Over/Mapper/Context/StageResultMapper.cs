using nue.protocol.exvs;
using Riok.Mapperly.Abstractions;
using ServerOver.Context.Battle;

namespace ServerOver.Mapper.Context;

[Mapper]
public static partial class StageResultMapper
{
    [MapProperty(
        [nameof(Request.SaveStageResult.PlayResultGroup.StageId)],
        [nameof(BattleResultContext.StageResultInfoDomain), nameof(BattleResultContext.StageResultInfoDomain.StageId)]
    )]
    [MapProperty(
        [nameof(Request.SaveStageResult.PlayResultGroup.Difficulty)],
        [nameof(BattleResultContext.StageResultInfoDomain), nameof(BattleResultContext.StageResultInfoDomain.Difficulty)]
    )]
    [MapProperty(
        [nameof(Request.SaveStageResult.PlayResultGroup.IsCompleted)],
        [nameof(BattleResultContext.StageResultInfoDomain), nameof(BattleResultContext.StageResultInfoDomain.IsCompleted)]
    )]
    [MapProperty(
        [nameof(Request.SaveStageResult.PlayResultGroup.CompletedTime)],
        [nameof(BattleResultContext.StageResultInfoDomain), nameof(BattleResultContext.StageResultInfoDomain.TotalClearTime)]
    )]
    [MapProperty(
        [nameof(Request.SaveStageResult.PlayResultGroup.StageScore)],
        [nameof(BattleResultContext.StageResultInfoDomain), nameof(BattleResultContext.StageResultInfoDomain.TotalClearScore)]
    )]
    private static partial BattleResultContext ToBattleResultContext0(Request.SaveStageResult.PlayResultGroup resultGroup);

    public static BattleResultContext ToBattleResultContext(this Request.SaveStageResult.PlayResultGroup resultGroup)
    {
        var r = ToBattleResultContext0(resultGroup);
        r.StageResultInfoDomain.PlayResult = resultGroup;
        return r;
    }
}