using Common;
using Microsoft.Extensions.Caching.Memory;
using nue.protocol.mms;

namespace ServerOver.Caches;

public sealed class MatchingCache(IMemoryCache memoryCache) 
    : IMatchingCache
{
    internal const string _key4Client = "match:client:"; // :{PcbSerial}
    internal const string _key4Pcb = "match:client_pcb:node:"; // :{nodeid}

    readonly SemaphoreSlim _locker = new(1, 1);

    async Task<Releaser> LockAsync()
    {
        await _locker.WaitAsync();
        return new(action: UnLock);
    }
    
    void UnLock()
    {
        try { _locker.Release(); } catch { }
    }

    public async Task<MatchingClientInfo?> GetClientByNodeId(uint nodeId)
    {
        await using var _d = await LockAsync();
        var r = PrivateGetClient(nodeId, null);
        return r;
    }
    
    public async Task<MatchingClientInfo?> GetClientByPcbSerial(string pcbSerial) 
    {
        await using var _d = await LockAsync();
        var r = PrivateGetClient(default, pcbSerial);
        return r;
    }

    private MatchingClientInfo? PrivateGetClient(uint nodeId, string? pcbSerial)
    {
        if (pcbSerial == null)
        {
            var key2 = _key4Pcb + nodeId;
            pcbSerial = memoryCache.Get<string>(key2);
        }
        if (string.IsNullOrEmpty(pcbSerial)) return null;

        var key1 = _key4Client + pcbSerial;
        var clientInfo = memoryCache.Get<MatchingClientInfo>(key1);
        return clientInfo;
    }

    public async Task AddClient(MatchingClientInfo clientInfo)
    {
        if (clientInfo?.Client == default) return;

        var key1 = _key4Client + clientInfo.Client.PcbSerial;
        var key2 = _key4Pcb + clientInfo.Client.NodeId;
        await using var _d = await LockAsync();

        var info0 = memoryCache.Get<MatchingClientInfo>(key1);
        if (info0?.Client != null)
        {
            if (info0.Client.NodeId != clientInfo.Client.NodeId)
                return;
            if (info0.ApplyId >= clientInfo.ApplyId)
                return;
        }

        var expiredTime = DateTimeOffset.FromUnixTimeSeconds((long)(clientInfo.EntryAt + clientInfo.TtlSec));
        memoryCache.Set(key1, clientInfo, expiredTime);
        memoryCache.Set(key2, clientInfo.Client.PcbSerial, expiredTime);
    }
    
    public async Task RemoveClientByNodeId(uint nodeId, uint? applyId = null)
    {
        await using var _d = await LockAsync();
        PrivateRemoveClient(nodeId, null, applyId);
    }
    
    public async Task RemoveClientByPcbSerial(string pcbSerial, uint? applyId = null)
    {
        await using var _d = await LockAsync();
        PrivateRemoveClient(default, pcbSerial, applyId);
    }

    private void PrivateRemoveClient(uint nodeId, string? pcbSerial, uint? applyId)
    {
        var key2 = _key4Pcb + nodeId;
        if (pcbSerial == null)
        {
            pcbSerial = memoryCache.Get<string>(key2);
        }
        if (string.IsNullOrEmpty(pcbSerial)) return;

        var key1 = _key4Client + pcbSerial;
        var clientInfo = memoryCache.Get<MatchingClientInfo>(key1);
        if (clientInfo == null) return;
        key2 = _key4Pcb + clientInfo.NodeId;
        if (clientInfo.ApplyId == applyId || applyId == null)
        {
            memoryCache.Remove(key1);
            memoryCache.Remove(key2);
        }
    }
}
