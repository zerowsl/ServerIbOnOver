using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Commands.SaveBattle;
using ServerOver.Commands.SaveBattle.Common;
using ServerOver.Context.Battle;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record SaveVstResultCommand(Request Request) : IRequest<Response>;

public class SaveVstResultCommandHandler(ILogger<SaveVstResultCommandHandler> logger, ServerDbContext context, 
    IOptions<CardServerConfig> options)
    : IRequestHandler<SaveVstResultCommand, Response>
{
    private readonly CardServerConfig _config = options.Value;
    
    public Task<Response> Handle(SaveVstResultCommand request, CancellationToken cancellationToken)
    {
        var sessionId = request.Request.save_vst_result.SessionId;
        var cardId = request.Request.save_vst_result.PilotId;

        var cardProfile = context.CardProfiles
            .FirstOrDefault(x => x.SessionId == sessionId && x.Id == cardId);

        if (cardProfile == null)
        {
            return Task.FromResult(CreateStandardResponse(request));
        }

        var vstResult = request.Request.save_vst_result;

        //if (vstResult.TutorialFlag) // ob
        //{
        //    return Task.FromResult(CreateStandardResponse(request));
        //}

        //if (vstResult.TrainingType is not 0) // ob
        //{
        //    return Task.FromResult(CreateStandardResponse(request));
        //}
        
        var quickStartInfo = vstResult.QuickStartInfo;
        if (quickStartInfo?.GameMode == 7u && _config.CustomConfigs?.EnableForceToSoloWhenSaveQuickStartInfoIfGameModeIs7 == true)
        {
            quickStartInfo.TeamType = 0u;
        }
        
        var battleResultContext = new BattleResultContext();
        
        var trainingSetting = context.TrainingProfileDbSet
            .First(x => x.CardProfile == cardProfile);

        trainingSetting.CpuLevel = vstResult.CpuLevel;
        trainingSetting.ExBurstGauge = vstResult.ExBurstGauge;
        trainingSetting.DamageDisplay = vstResult.DamageDisplay;
        trainingSetting.CpuAutoGuard = vstResult.CpuAutoGuard;
        trainingSetting.CommandGuideDisplay = vstResult.CommandGuideDisplay;
        trainingSetting.MstMobileSuitId = vstResult.LastMobileSuitId;
        trainingSetting.BurstType = vstResult.LastBurstType;
        trainingSetting.ExOverLimit = vstResult.ExOverLimit;
        trainingSetting.Player1HpMode = vstResult.Player1HpMode;
        trainingSetting.CpuHpAutoRegen = vstResult.CpuHpAutoRegen;
        trainingSetting.QuickFill = vstResult.QuickFill;
        
        var cmds = new List<ISaveBattleDataCommand>()
        {
            new SaveQuickStartInfoCommand(context, quickStartInfo)
        };
        cmds.ForEach(command => command.Save(cardProfile, battleResultContext));
        
        context.SaveChanges();
        
        return Task.FromResult(new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vst_result = new()
        });
    }

    static Response CreateStandardResponse(SaveVstResultCommand request)
    {
        return new Response
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            save_vst_result = new()
        };
    }
}
