using nue.protocol.exvs;

namespace ServerOver.Context.Battle.Domain.Triad;

public class StageResultInfoDomain
{
    public uint StageId { get; set; }
    public uint Difficulty { get; set; }

    public bool IsCompleted { get; set; }

    public uint TotalClearTime { get; set; }
    public uint TotalClearScore { get; set; }

    public Request.SaveStageResult.PlayResultGroup PlayResult { get; set; } = default!;
    public Request.SaveStageResult.PlayResultGroup.CompletedResultGroup? CompletedResult => PlayResult?.CompletedResult;
}