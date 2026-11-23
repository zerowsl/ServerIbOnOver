using MediatR;
using nue.protocol.exvs;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Common;
using ServerOver.Context.Battle;
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

        var sessionId = request.Request.save_practice_result.SessionId;
        var cardId = request.Request.save_practice_result.PilotId;
        var practiceResult = request.Request.save_practice_result.Result;

        var cardProfile = context.CardProfiles.FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);
        if (cardProfile == null)
        {
            return Task.FromResult(response);
        }

        var battleResultContext = new BattleResultContext();

        var cmds = new List<ISaveBattleDataCommand>()
        {
            new SaveQuickStartInfoCommand(context, practiceResult.QuickStartInfo)
        };
        cmds.ForEach(command => command.Save(cardProfile, battleResultContext));

        context.SaveChanges();

        return Task.FromResult(response);
    }
}