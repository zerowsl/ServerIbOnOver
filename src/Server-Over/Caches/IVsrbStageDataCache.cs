namespace ServerOver.Caches;

public interface IVsrbStageDataCache
{
    // ttl = 60s
    Task<VsrbStageDataCacheItem?> Get(uint cardId);
    
    Task Add(VsrbStageDataCacheItem item);
    Task Remove(uint cardId);
}

public class VsrbStageDataCacheItem(uint cardId, uint stageId)
{
    public uint CardId { get; } = cardId;
    public uint StageId { get; } = stageId;
    
    public byte[]? Data { get; set; }
}
