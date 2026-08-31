using MediatR;
using nue.protocol.exvs;
using ServerOver.Caches;

namespace ServerOver.Handlers.Game;

public record SaveInsideDataCommand(Request Request) : IRequest<Response>;

public class SaveInsideDataCommandHandler(ICardIdPcbSerialCache cardIdPcbSerialCache)
    : IRequestHandler<SaveInsideDataCommand, Response>
{
    public async Task<Response> Handle(SaveInsideDataCommand request, CancellationToken cancellationToken)
    {
        await cardIdPcbSerialCache.TryAddOrUpdateByGameRequest(request.Request);

        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_inside_data = new()
        };
    }
}