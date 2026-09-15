using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.mms;
using ServerOver.Caches;
using ServerOver.Models.Config;

namespace ServerOver.Handlers.Match;

public record EntryMatchingCommand2(Request Request) : IRequest<Response>;

public class EntryMatchingCommand2Handler(IMatchingCache matchingCache, ICardIdPcbSerialCache cardIdPcbSerialCache,
	IOptions<CardServerConfig> options, 
	ILogger<EntryMatchingCommand2Handler> logger)
	: IRequestHandler<EntryMatchingCommand2, Response>
{
    readonly CardServerConfig _cardServerConfig = options.Value;

    public async Task<Response> Handle(EntryMatchingCommand2 cmd, CancellationToken cancellationToken)
    {
		var request = cmd.Request;
		
		if (( _cardServerConfig.CustomConfigs.RemoteCardServerConfigs?.Enable == true)
            || ( _cardServerConfig.CustomConfigs.LocalMatchingConfigs?.Enable != true))
        {
            return new Response
            {
                Type = request.Type,
                RequestId = request.RequestId,
                Code = ErrorCode.ErrServer
            };
        }
		
		var result = new Response.EntryMatching();
		result.PolingInterval = _cardServerConfig.CustomConfigs.LocalMatchingConfigs.Match2.CheckingInterval;
		result.ApplyId = request.entry_matching.ApplyId;
		
		await Handle(request.entry_matching);

        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Code = ErrorCode.Success,
            entry_matching = result
        };
        return response;
    }
    
    async Task Handle(Request.EntryMatching matching)
    {
        if (matching.Myclient == null) return;
        var pcbSerial = matching.Myclient.PcbSerial;
        if (string.IsNullOrEmpty(pcbSerial)) return;

        var cardInfo = await cardIdPcbSerialCache.GetByPcbSerial(pcbSerial);
        if (cardInfo == null) logger.LogWarning("EntryMatching2 fail by missing cardId-PcbSerial Cache !!");
        if (cardInfo == null && GlobalVars.IsGameLocal) return;

        await matchingCache.AddClient(new(matching, cardInfo?.CardId ?? 0u));
    }
}