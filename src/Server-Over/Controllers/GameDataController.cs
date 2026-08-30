using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using ServerOver.Caches;
using ServerOver.Handlers.Game;
using ServerOver.Mapper.Usage;
using ServerOver.Models.Config;
using ServerOver.Utils;
using System.Diagnostics;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Controllers;

[ApiController]
[Route("gamedata")]
public class GameDataController(IMediator mediator, IVsrbStageDataCache vsrbStageDataCache,
    ILogger<GameDataController> logger) 
    : BaseController<GameDataController>
{
    [HttpGet("configs")]
    [Produces("application/json")]
    public ConfigsResponse GetConfigs([FromServices] IOptions<CardServerConfig> opt)
    {
        var config = opt.Value;
        var r = config.ToConfigsResponse();
        return r;
    }

    [HttpPost, HttpPut]
    [Route("vsrb/w")]
    [Produces("application/octet-stream")]
    public async Task<object> WriteByVsRouteBattle([FromQuery] uint cardId, [FromQuery] uint stageId)
    {
        logger.LogInformation("call '/gamedata/vsrb/w' ok. content-type='{ct}'", HttpContext.Request.ContentType); // null

        // to cache step 2
        if (cardId > 0)
        {
            using var readStream = new FileBufferingReadStream(HttpContext.Request.Body, 50);
            await readStream.DrainAsync(HttpContext.RequestAborted);
            readStream.Position = 0;

            var item = await vsrbStageDataCache.Get(cardId);
            if (item != null)
            {
                Debug.Assert(item.StageId == stageId);
                using var s = new MemoryStream();
                await readStream.CopyToAsync(s);
                s.Seek(0, SeekOrigin.Begin);

                item.Data = s.ToArray();
                await vsrbStageDataCache.Add(item);
            }
        }

        return new 
        {
            Success = true
        };
    }
    
    
    [HttpGet("vsrb/r")]
    [Produces("application/octet-stream")]
    public async Task<IActionResult> ReadByVsRouteBattle([FromQuery] uint cardId)
    {
        logger.LogInformation("call '/gamedata/vsrb/r' ok");
        var r = await mediator.Send(new LoadVsRouteBattleDataByUrlQuery(cardId));
        return File(r, "application/octet-stream");
    }

}