using MediatR;
using nue.protocol.exvs;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SavePracticeResultCommand(Request Request) : IRequest<Response>;

public class SavePracticeResultCommandHandler(ServerDbContext context) : IRequestHandler<SavePracticeResultCommand, Response>
{
    public Task<Response> Handle(SavePracticeResultCommand request, CancellationToken cancellationToken)
    {
        var response = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_practice_result = new()
        };
        
        return Task.FromResult(response);
    }
}