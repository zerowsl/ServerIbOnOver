using MediatR;
using nue.protocol.exvs;
using ServerOver.Caches;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game.Online;

public record SaveBattleLogOnCommand(Request Request) : IRequest<Response>;

public class SaveBattleLogOnCommandHandler(ServerDbContext context, 
    ICardIdPcbSerialCache cardIdPcbSerialCache,
    ILogger<SaveBattleLogOnCommandHandler> logger) 
    : IRequestHandler<SaveBattleLogOnCommand, Response>
{
    public async Task<Response> Handle(SaveBattleLogOnCommand request, CancellationToken cancellationToken)
    {
        await cardIdPcbSerialCache.TryAddOrUpdateByGameRequest(request.Request);

        var successResponse = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_battle_log_on = new()
        };
        
        return successResponse;
    }
}
