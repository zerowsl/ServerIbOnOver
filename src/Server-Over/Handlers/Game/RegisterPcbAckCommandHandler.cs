using MediatR;
using nue.protocol.exvs;
using ServerOver.Caches;

namespace ServerOver.Handlers.Game;

public record RegisterPcbAckCommand(Request Request) : IRequest<Response>;

public class RegisterPcbAckCommandHandler(ICardIdPcbSerialCache cardIdPcbSerialCache)
	: IRequestHandler<RegisterPcbAckCommand, Response>
{
    public async Task<Response> Handle(RegisterPcbAckCommand request, CancellationToken cancellationToken)
    {
        await cardIdPcbSerialCache.TryAddOrUpdateByGameRequest(request.Request);

        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            register_pcb_ack = new()
        };
    }
}