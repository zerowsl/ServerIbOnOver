namespace ServerOver.Caches;

public interface IVsrbStageDataCache
{
    // ttl = 60s
    Task<VsrbStageDataCacheItem?> Get(uint cardId);
    
    Task Add(VsrbStageDataCacheItem item);
    Task Remove(uint cardId);
}

public class VsrbStageDataCacheItem(uint cardId, uint stageId, bool isTimeRental)
{
    public uint CardId { get; } = cardId;
    public uint StageId { get; } = stageId;
    public bool IsTimeRental { get; } = isTimeRental;
    public byte[]? Data { get; set; }
}
