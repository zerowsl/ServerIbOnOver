using nue.protocol.exvs;

namespace ServerOver.Context.Battle.Domain.PvP;

public class OnlineBattleDomain
{
    //public OnlineMatchingMode OnlineMatchingMode { get; set; } = new();
    public OnlineMatchingMode OnlineMatchingMode { get; set; } = OnlineMatchingMode.OmmClass;

    public bool IsShuffle { get; set; } = false;
    public int LicenseScoreChange { get; set; } = 0;
	
	public Request.SaveVsmOnResult.PlayResultGroup.ClassMatchResult? class_match_result { get; set; }
	public uint TeamType { get; set; }
}