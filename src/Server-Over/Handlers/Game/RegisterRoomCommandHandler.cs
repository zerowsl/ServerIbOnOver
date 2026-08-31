using MediatR;
using nue.protocol.exvs;

namespace ServerOver.Handlers.Game;

public record RegisterRoomCommand(Request Request) : IRequest<Response>;

public class RegisterRoomCommandHandler : IRequestHandler<RegisterRoomCommand, Response>
{
    public Task<Response> Handle(RegisterRoomCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            register_room = new() { Status = 0 }
        });
    }
}