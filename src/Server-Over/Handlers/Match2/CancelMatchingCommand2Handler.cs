using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.mms;
using ServerOver.Caches;
using ServerOver.Models.Config;

namespace ServerOver.Handlers.Match;

public record CancelMatchingCommand2(Request Request) : IRequest<Response>;

public class CancelMatchingCommand2Handler(IMatchingCache matchCache, 
    IOptions<CardServerConfig> options)
    : IRequestHandler<CancelMatchingCommand2, Response>
{
    readonly CardServerConfig _cardServerConfig = options.Value;
    
    public async Task<Response> Handle(CancelMatchingCommand2 cmd, CancellationToken cancellationToken)
    {
        var request = cmd.Request;
        var matching = request.cancel_matching;
        
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
        
        await matchCache.RemoveClientByNodeId(matching.NodeId, matching.ApplyId);
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Code = ErrorCode.Success,
            cancel_matching = new()
        };
        return response;
    }
}