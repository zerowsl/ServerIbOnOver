using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServerOver.Handlers.UI.QuickStarts;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Controllers.UI;

[ApiController]
[Route("ui/QuickStart")]
public class QuickStartController(IMediator mediator) : BaseController<MessageController>
{
    [HttpGet("get/{accessCode}/{chipId}")]
    [Produces("application/json")]
    public async Task<ActionResult<QuickStartProfile>> GetTrainingProfile(string accessCode, string chipId)
    {
        var response = await mediator.Send(new GetQuickStartProfileCommand(accessCode, chipId));
        return response;
    }
    
    [HttpPost("save")]
    [Produces("application/json")]
    public async Task<ActionResult<BasicResponse>> UpsertGamepadConfig([FromBody] UpdateQuickStartProfileRequest request)
    {
        var response = await mediator.Send(new UpsertQuickStartProfileCommand(request));
        return response;
    }
}