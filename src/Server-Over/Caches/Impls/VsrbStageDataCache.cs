using Microsoft.Extensions.Caching.Memory;

namespace ServerOver.Caches;

public class VsrbStageDataCache(IMemoryCache memoryCache) : IVsrbStageDataCache
{
    const string _key = "ib:vsrb:cardid:"; // :{cardId}
    const int ttl = 60;

    public Task<VsrbStageDataCacheItem?> Get(uint cardId)
    {
        var k = $"{_key}{cardId}";
        var r = memoryCache.Get<VsrbStageDataCacheItem>(k);
        return Task.FromResult(r);
    }

    public Task Add(VsrbStageDataCacheItem item)
    {
        if (item != default)
        {
            var k = $"{_key}{item.CardId}";
            memoryCache.Set(k, item, TimeSpan.FromSeconds(ttl));
        }
        return Task.CompletedTask;
    }

    public Task Remove(uint cardId)
    {
        var k = $"{_key}{cardId}";
        memoryCache.Remove(k);
        return Task.CompletedTask;
    }
}
