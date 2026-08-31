using MediatR;
using nue.protocol.exvs;
using ServerOver.Caches;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Common;
using ServerOver.Commands.SaveBattle.Triad;
using ServerOver.Context.Battle;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveUserPlayResearchDataCommand(Request Request) : IRequest<Response>;

// GameOver 时才触发
public class SaveUserPlayResearchDataCommandHandler(ServerDbContext context, IVsrbStageDataCache vsrbStageDataCache,
    ILogger<SaveUserPlayResearchDataCommandHandler> logger) 
    : IRequestHandler<SaveUserPlayResearchDataCommand, Response>
{
    public async Task<Response> Handle(SaveUserPlayResearchDataCommand request, CancellationToken cancellationToken)
    {
        var research = request.Request.save_user_play_research_data!;
        var sessionId = research.SessionId;
        var cardId = research.PilotId;

        var cardProfile = context.CardProfiles.FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);
        if (cardProfile == null)
        {
            return new Response
            {
                Type = request.Request.Type,
                RequestId = request.Request.RequestId,
                Error = Error.Success,
                save_user_play_research_data = new()
            };
        }

        var battleResultContext = new BattleResultContext();
        SetContext4PlayerBadgeDomain(battleResultContext, research);

        List<ISaveBattleDataCommand> cmds = [
            // 真正保存 Vs.RouteBattle Data
            new SaveIbVsRouteBattleStageTrueDataCommand(vsrbStageDataCache),

            // 徽章升级会延迟保存..
            // 连续游戏第2次保存或者gameover时..
            new SavePlayerBadgeCommand(context, logger),
        ];
        foreach (var cmd in cmds) 
        {
            cmd.Save(cardProfile, battleResultContext);
        }

        await context.SaveChangesAsync();

        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_user_play_research_data = new()
        };
    }

    static void SetContext4PlayerBadgeDomain(BattleResultContext battleResultContext, Request.SaveUserPlayResearchData data)
    {
        battleResultContext.PlayerBadgeDomain ??= new();
        battleResultContext.PlayerBadgeDomain.BadgeIdBefore = null;
        battleResultContext.PlayerBadgeDomain.BadgeIdAfter = data.BadgeId;
        battleResultContext.PlayerBadgeDomain.ExpIncrement = 0;
        battleResultContext.PlayerBadgeDomain.IsGameOver = true;
        battleResultContext.PlayerBadgeDomain.PlayerLevelId = data.PlayerLevelId;
    }
}
