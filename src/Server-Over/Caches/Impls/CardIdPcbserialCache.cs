using nue.protocol.exvs;

namespace ServerOver.Caches;

public sealed class CardIdPcbSerialCache(ILogger<CardIdPcbSerialCache> logger) : ICardIdPcbSerialCache
{
    readonly List<CardIdPcbSerialInfo> _items = new(100);

    public Task<CardIdPcbSerialInfo?> GetByCardId(uint cardId)
    {
        return Task.FromResult(PrivateGetBy(cardId, default!));
    }

    public Task<CardIdPcbSerialInfo?> GetByPcbSerial(string pcbSerial)
    {
        return Task.FromResult(PrivateGetBy(default, pcbSerial));
    }

    CardIdPcbSerialInfo? PrivateGetBy(uint cardId, string pcbSerial)
    {
        lock (_items)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var i = pcbSerial == null ? _items.FindIndex(x => x.CardId == cardId)
                : _items.FindIndex(x => x.PcbSerial == pcbSerial);
            if (i == -1) return null;

            var item = _items[i];
            if ((now - item.AddAt) < GlobalVars.Ttl4CardIdPcbSerial)
            {
                if (GlobalVars.IsTtlSlide1) _items[i] = item = new(item.CardId, item.PcbSerial, now);
                return item; // 未超时
            }
            _items.RemoveAt(i); // 超时
            return null;
        }
    }

    public Task AddOrUpdate(uint cardId, string pcbSerial)
    {
        if (!string.IsNullOrEmpty(pcbSerial) && cardId > 0)
            PrivateAddOrUpdate(cardId, pcbSerial);
        return Task.CompletedTask;
    }

    void PrivateAddOrUpdate(uint cardId, string pcbSerial)
    {
        lock (_items)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            var i = _items.FindIndex(x => x.CardId != cardId && x.PcbSerial == pcbSerial);
            if (i > -1)
            {
                var item0 = _items[i];
                if ((now - item0.AddAt) < GlobalVars.Ttl4CardIdPcbSerial) // 未超时
                {
                    logger.LogError("Found same pcbserial with 2 diff cards.");
                    throw new Exception("Found same pcbserial with 2 diff cards.");
                    //return;
                }
                else // 超时
                {
                    _items.RemoveAt(i);
                }
            }

            _items.RemoveAll(x => x.CardId == cardId);
            _items.Add(new(cardId, pcbSerial, now));
        }
    }

    public async Task TryAddOrUpdateByGameRequest(Request request)
    {
        var cardId = request.AmId;
        var pcbSerial = request.PcbSerial;
        if (cardId > 0 && !string.IsNullOrEmpty(pcbSerial))
        {
            await AddOrUpdate(cardId, pcbSerial);
        }
    }

    public Task CleanUp()
    {
        lock (_items)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _items.RemoveAll(x => (now - x.AddAt) >= GlobalVars.Ttl4CardIdPcbSerial);
        }
        return Task.CompletedTask;
    }

    public Task<CardIdPcbSerialInfo[]> GetAll()
    {
        CleanUp().Wait();
        lock (_items)
        {
            return Task.FromResult(_items.ToArray());
        }
    }
}
