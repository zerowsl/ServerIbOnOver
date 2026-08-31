using MediatR;
using nue.protocol.exvs;
using ServerOver.Caches;
using ServerOver.Models.Cards.Battle.History;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveBattleLogCommand(Request Request) : IRequest<Response>;

public class SaveBattleLogCommandHandler(ServerDbContext _context, ICardIdPcbSerialCache cardIdPcbSerialCache,
    ILogger<SaveBattleLogCommandHandler> _logger)
    : IRequestHandler<SaveBattleLogCommand, Response>
{

    public async Task<Response> Handle(SaveBattleLogCommand request, CancellationToken cancellationToken)
    {
        await cardIdPcbSerialCache.TryAddOrUpdateByGameRequest(request.Request);

        var successResponse = new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_battle_log = new Response.SaveBattleLog()
        };

        var saveBattleLogRequest = request.Request.save_battle_log;
        var sessionId = saveBattleLogRequest.SessionId;
        
        var cardProfile = _context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId);

        if (cardProfile == null)
        {
            return (successResponse);
        }

        var oldPreBattleHistory = _context.PreBattleHistoryDbSet
            .FirstOrDefault(x => x.CardProfile == cardProfile);

        if (oldPreBattleHistory is not null)
        {
            _context.PreBattleHistoryDbSet.Remove(oldPreBattleHistory);
            _context.SaveChanges();
        }

        if (saveBattleLogRequest.BattleLog.GameMode != GameMode.GmodeNone)
        {
            return (successResponse);
        }

        var ownPlayer = saveBattleLogRequest.BattleLog.Pilots
            .FirstOrDefault(x => x.IsHuman && x.PilotId == cardProfile.Id);

        if (ownPlayer is null)
        {
            return (successResponse);
        }

        var newPreBattleHistory = new PreBattleHistory()
        {
            CurrentConsecutiveWins = ownPlayer.ConsecutiveWin,
            CardId = cardProfile.Id,
            CardProfile = cardProfile
        };

        _context.PreBattleHistoryDbSet.Add(newPreBattleHistory);
        _context.SaveChanges();
        
        return (successResponse);
    }
}
