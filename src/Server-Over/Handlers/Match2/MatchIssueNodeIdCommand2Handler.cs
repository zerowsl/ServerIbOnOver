using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.mms;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Match;

public record MatchIssueNodeIdCommand2(Request Request) : IRequest<Response>;

public class MatchIssueNodeIdCommand2Handler(ServerDbContext _context,
    IOptions<CardServerConfig> options) 
	: IRequestHandler<MatchIssueNodeIdCommand2, Response>
{
	readonly CardServerConfig _config = options.Value; 
	
    public Task<Response> Handle(MatchIssueNodeIdCommand2 request, CancellationToken cancellationToken)
    {
        uint[] nodeList = _config.CustomConfigs.RemoteCardServerConfigs.Enable ? [1]
            : _config.CustomConfigs.LocalMatchingConfigs.Enable ? [48602, 48603, 48604, 48605, 48606, 48607, 48608, 48609]
			: [1];
        
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Code = ErrorCode.Success,
            issue_node_id = new Response.IssueNodeId
            {
                NodeIds = nodeList
            }
        };

        return Task.FromResult(response);
    }
}