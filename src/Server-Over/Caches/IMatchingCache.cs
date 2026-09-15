using nue.protocol.mms;

namespace ServerOver.Caches;

public interface IMatchingCache
{
    Task<MatchingClientInfo?> GetClientByNodeId(uint nodeId);
    Task<MatchingClientInfo?> GetClientByPcbSerial(string pcbSerial);
    
    Task AddClient(MatchingClientInfo clientInfo);
    
    Task RemoveClientByNodeId(uint nodeId, uint? applyId = null);
    Task RemoveClientByPcbSerial(string pcbSerial, uint? applyId = null);
}

public sealed class MatchingClientInfo(Request.EntryMatching matching, uint cardId = 0)
{
	public Client Client => matching.Myclient;
	public uint ApplyId => matching.ApplyId;
	public World World => matching.World;
	public ulong EntryAt => matching.EntryAt;
	public uint TtlSec => matching.MatchingTimeout;
	
	public string PcbSerial => matching.Myclient.PcbSerial;
	public uint NodeId => matching.Myclient.NodeId;

    public uint CardId { get; set; } = cardId; // 可选
}
