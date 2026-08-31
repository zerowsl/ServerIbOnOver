using MediatR;
using nue.protocol.exvs;

namespace ServerOver.Handlers.Game.Online;

public record SaveFailedBattleLogOnCommand(Request Request) : IRequest<Response>;

public class SaveFailedBattleLogOnCommandHandler : IRequestHandler<SaveFailedBattleLogOnCommand, Response>
{
    public Task<Response> Handle(SaveFailedBattleLogOnCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_failed_battle_log_on = new()
        });
    }
}