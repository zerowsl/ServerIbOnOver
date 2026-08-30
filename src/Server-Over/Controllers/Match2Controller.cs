using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using nue.protocol.mms;
using ProtoBuf;
using ServerOver.Handlers.Match;
using ServerOver.Utils;
using Swan.Formatters;

namespace ServerOver.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class Match2Controller(IMediator _mediator)  : BaseController<Match2Controller>
{
    [Route("match2")]
    [HttpPost]
    [Produces("application/protobuf")]
    public async Task<IActionResult> Match2([FromBody] Request request)
    {
        Logger.LogInformation("match2 Request is {Request}", request.Stringify());
        
        object response = request.Type switch
        {
            MethodType.Ping => await _mediator.Send(new MatchPingCommand(request)),
            MethodType.MatchingSetting => await _mediator.Send(new MatchingSettingCommand(request)),
			MethodType.IssueNodeId => await _mediator.Send(new MatchIssueNodeIdCommand2(request)),
			MethodType.EntryMatching => await _mediator.Send(new EntryMatchingCommand2(request)),
            MethodType.CheckMatching => await _mediator.Send(new CheckMatchingCommand2(request)),
            MethodType.CancelMatching => await _mediator.Send(new CancelMatchingCommand2(request)),
            _ => UnhandledResponse(request)
        };
        
        
        return Ok(response);
    }
    
    private Response UnhandledResponse(Request request)
    {
        Logger.LogWarning("match2 Unhandled case: {Type}", request.Type);
        return new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Code = ErrorCode.ErrServer
        };
    }
    
}