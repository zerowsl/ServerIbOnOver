using nue.protocol.exvs;

namespace ServerOver.Caches;

public interface ICardIdPcbSerialCache
{
    // List<CardIdPcbSerialInfo> _items = []; // ttl = 600s

    Task<CardIdPcbSerialInfo?> GetByCardId(uint cardId);
    Task<CardIdPcbSerialInfo?> GetByPcbSerial(string pcbSerial);
    
    Task AddOrUpdate(uint cardId, string pcbSerial);
    Task TryAddOrUpdateByGameRequest(Request request);

    //Task RemoveByNodeId(uint cardId);
    //Task RemoveByPcbSerial(string pcbSerial);

    Task CleanUp();
    Task<CardIdPcbSerialInfo[]> GetAll();
}

public readonly record struct CardIdPcbSerialInfo(uint CardId, string PcbSerial, long AddAt);
