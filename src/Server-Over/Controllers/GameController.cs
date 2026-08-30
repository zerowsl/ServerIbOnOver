using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using nue.protocol.exvs;
using ProtoBuf;
using ServerOver.Handlers.Game;
using ServerOver.Handlers.Game.Online;
using ServerOver.Handlers.Game.Tournament;
using ServerOver.Utils;
using Swan.Formatters;

namespace ServerOver.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class GameController(IMediator mediator) 
    : BaseController<GameController>
{
    [Route("")]
    [HttpPost]
    [Produces("application/protobuf")]
    public async Task<IActionResult> Game([FromBody] Request request)
    {
        var baseAddress = Request.Host.ToString();
        var baseAddress2 = GetBaseAddressWithScheme();
        
        Logger.LogInformation("Request is {Request}", request.Stringify());

        object response = request.Type switch
        {
            MethodType.MthdRegisterPcb => await mediator.Send(new RegisterPcbCommand(request, baseAddress)),
            MethodType.MthdRegisterPcbAck => await mediator.Send(new RegisterPcbAckCommand(request)),
            MethodType.MthdSaveInsideData => await mediator.Send(new SaveInsideDataCommand(request)),
			
		    //MethodType.MthdLoadGameData => await mediator.Send(new LoadGameDataQuery(request)), // ob
            MethodType.MthdLoadGameData => await mediator.Send(new LoadGameDataQuery2(request)),
            MethodType.MthdLoadRankMatch => await mediator.Send(new LoadRankMatchQuery(request)),
            MethodType.MthdLoadClassMatch => await mediator.Send(new LoadClassMatchCommand2(request)), // ib
            
            MethodType.MthdPreLoadCard => await mediator.Send(new PreLoadCardQuery2(request)),
            MethodType.MthdLoadCard => await mediator.Send(new LoadCardQuery2(request, baseAddress2)),
            MethodType.MthdRegisterCard => await mediator.Send(new RegisterCardCommand(request)),
            
            MethodType.MthdSaveVsmResult => await mediator.Send(new SaveVsmResultCommand(request)),
            MethodType.MthdSaveVsmOnResult => await mediator.Send(new SaveVsmOnResultCommand(request)),
            MethodType.MthdSaveVscResult => await mediator.Send(new SaveVscResultCommand(request)), // ib
            MethodType.MthdSaveStageResult => await mediator.Send(new SaveStageResultCommand(request, baseAddress2)), // ib
            MethodType.MthdSavePracticeResult => await mediator.Send(new SavePracticeResultCommand(request)),
            MethodType.MthdSaveVstResult => await mediator.Send(new SaveVstResultCommand(request)),
            MethodType.MthdSaveCharge => await mediator.Send(new SaveChargeCommand(request)),
                
            MethodType.MthdSaveBattleLog => await mediator.Send(new SaveBattleLogCommand(request)),
            MethodType.MthdSaveBattleLogOn => await mediator.Send(new SaveBattleLogOnCommand(request)),
            MethodType.MthdSaveFailedBattleLogOn => await mediator.Send(new SaveFailedBattleLogOnCommand(request)),
            MethodType.MthdSaveUserPlayResearchData => await mediator.Send(new SaveUserPlayResearchDataCommand(request)),
            MethodType.MthdSaveResultCapture => await mediator.Send(new SaveResultCaptureCommand(request, baseAddress)),
            
            MethodType.MthdCheckCommunication => await mediator.Send(new CheckCommunicationQuery(request)),
            
            MethodType.MthdLoadSpotInfo => await mediator.Send(new LoadSpotInfoCommand(request)),
            //MethodType.MthdLoadRanking => await mediator.Send(new LoadRankingQuery(request)), // ob
			MethodType.MthdLoadRanking => await mediator.Send(new LoadRankingQuery2(request)), // ib
            MethodType.MthdCheckTelop => await mediator.Send(new CheckTelopQuery(request)),
            MethodType.MthdLoadTelop => await mediator.Send(new LoadTelopQuery(request)),
            MethodType.MthdCheckMovieRelease => await mediator.Send(new CheckMovieReleaseQuery(request)),
            MethodType.MthdLoadSpotUrl => await mediator.Send(new LoadSpotUrlQuery(request)),
            //MethodType.MthdLoadReplayCard => await mediator.Send(new LoadReplayCardCommand(request, baseAddress)),
            //MethodType.MthdPreSaveReplay => await mediator.Send(new PreSaveReplayCommand(request, baseAddress)),
            MethodType.MthdLoadMeetingCard => await mediator.Send(new LoadMeetingCardCommand(request)),
            MethodType.MthdSaveTournamentResult => await mediator.Send(new SaveTournamentResultCommand(request)),
            MethodType.MthdLoadBlackList => await mediator.Send(new LoadBlackListQuery(request)),
            MethodType.MthdStartTournament => await mediator.Send(new StartTournamentCommand(request)),
            MethodType.MthdPing => await mediator.Send(new PingCommand(request)),
            MethodType.MthdCheckTime => await mediator.Send(new CheckTimeQuery(request)),
            MethodType.MthdSaveLog => await mediator.Send(new SaveLogCommand(request)),
            MethodType.MthdCheckResourceData => await mediator.Send(new CheckResourceDataQuery(request)),
            
            MethodType.MthdLoadRoom => await mediator.Send(new LoadRoomQuery(request)),
            MethodType.MthdRegisterRoom => await mediator.Send(new RegisterRoomCommand(request)),

            _ => UnhandledResponse(request)
        };

        return Ok(response);
    }
    
    private Response UnhandledResponse(Request request)
    {
        Logger.LogWarning("Unhandled case: {Type}", request.Type);
        return new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.ErrServer,
            ErrorMsg = "Unhandled case"
        };
    }

}