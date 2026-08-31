using MediatR;
using nue.protocol.exvs;
using ServerOver.Caches;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Triad;
using ServerOver.Mapper.Context;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveStageResultCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class SaveStageResultCommandHandler(ServerDbContext context, IVsrbStageDataCache vsrbStageDataCache) 
    : IRequestHandler<SaveStageResultCommand, Response>
{
    public async Task<Response> Handle(SaveStageResultCommand command, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(command.BaseAddress))
        {
            return await Handle1(command);
        }
        else
        {
            //await HandleWhenIsGameOver(command);
            return new Response
            {
                Error = Error.Success,
                save_stage_result = new()
            };
        }
    }

    // to cache step 1
    // to cache step 2 '/gamedata/vsrb/w'
    private async Task<Response> Handle1(SaveStageResultCommand command)
    {
        var request = command.Request;
        var stageResult = request.save_stage_result;
        var cardId = stageResult?.PilotId ?? 0;
        if (cardId == 0) cardId = request.AmId;

        if (cardId <= 0 || stageResult?.PlayResult == null)
        {
            return new()
            {
                Type = request.Type,
                RequestId = request.RequestId,
                Error = Error.Success,
                save_stage_result = new()
            };
        }

#if DEBUG
        var cardProfile = context.CardProfiles.FirstOrDefault(x => x.Id == cardId); // for test
#else
        var cardProfile = context.CardProfiles.FirstOrDefault(x => x.SessionId == stageResult.SessionId && x.Id == cardId);
#endif
        if (cardProfile == null)
        {
            return new()
            {
                Type = request.Type,
                RequestId = request.RequestId,
                Error = Error.Success,
                save_stage_result = new()
            };
        }

        var battleResultContext = stageResult.PlayResult.ToBattleResultContext();
        List<ISaveBattleDataCommand> cmds = [
            // 记录stage分数
            new SaveIbVsRouteBattleStageHighScoreCommand(context),

            // try save uidata
            new SaveIbVsRouteBattleStageUiDataCommand(context, vsrbStageDataCache), 
        ];
        foreach (var cmd in cmds)
        {
            cmd.Save(cardProfile, battleResultContext);
        }
        await context.SaveChangesAsync();

        // 其实此时, 游戏中用户还未决定是连战还是中断..

        string? url = null;
        if (stageResult.PlayResult.CompletedResult != null)
        {
            var baseAddress = command.BaseAddress;
            url = $"{baseAddress}/gamedata/vsrb/w?cardid={cardId}&stageId={stageResult.PlayResult.StageId}";
        }
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            save_stage_result = new() { Url = url }
        };
        return response;
    }
}