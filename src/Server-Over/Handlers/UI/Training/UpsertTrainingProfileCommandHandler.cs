using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Dto.Training;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.Training;

public record UpsertTrainingProfileCommand(UpdateTrainingProfileRequest Request) : IRequest<BasicResponse>;

public class UpsertGamepadConfigCommandHandler(ServerDbContext context) : IRequestHandler<UpsertTrainingProfileCommand, BasicResponse>
{
    public Task<BasicResponse> Handle(UpsertTrainingProfileCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        var cardProfile = context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var trainingProfile = context.TrainingProfileDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id);
        if (trainingProfile == null)
        {
            trainingProfile = new();
            context.TrainingProfileDbSet.Add(trainingProfile);
        }
        trainingProfile.CardId = cardProfile.Id;
        trainingProfile.MstMobileSuitId = updateRequest.TrainingProfile.MstMobileSuitId;
        trainingProfile.BurstType = (uint)updateRequest.TrainingProfile.BurstType;
        trainingProfile.CpuLevel = updateRequest.TrainingProfile.CpuLevel;
        trainingProfile.ExBurstGauge = updateRequest.TrainingProfile.ExBurstGauge;
        trainingProfile.DamageDisplay = updateRequest.TrainingProfile.DamageDisplay;
        trainingProfile.CpuAutoGuard = updateRequest.TrainingProfile.CpuAutoGuard;
        trainingProfile.CommandGuideDisplay = updateRequest.TrainingProfile.CommandGuideDisplay;
        trainingProfile.Player1HpMode = updateRequest.TrainingProfile.Player1HpMode;
        trainingProfile.CpuHpAutoRegen = updateRequest.TrainingProfile.CpuHpAutoRegen ? 1u : 0u;
        trainingProfile.ExOverLimit = updateRequest.TrainingProfile.ExOverLimit ? 1u : 0u;
        trainingProfile.QuickFill = updateRequest.TrainingProfile.QuickFill ? 1u : 0u;

        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }

    //public Task<BasicResponse> Handle(UpsertTrainingProfileCommand request, CancellationToken cancellationToken)
    //{
    //    var updateRequest = request.Request;
        
    //    var cardProfile = context.CardProfiles
    //        .Include(x => x.TrainingProfile)
    //        .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

    //    if (cardProfile is null)
    //    {
    //        throw new InvalidCardDataException("Card Profile is invalid");
    //    }

    //    cardProfile.TrainingProfile.CardId = cardProfile.Id;
    //    cardProfile.TrainingProfile.MstMobileSuitId = updateRequest.TrainingProfile.MstMobileSuitId;
    //    cardProfile.TrainingProfile.BurstType = (uint) updateRequest.TrainingProfile.BurstType;
    //    cardProfile.TrainingProfile.CpuLevel = updateRequest.TrainingProfile.CpuLevel;
    //    cardProfile.TrainingProfile.ExBurstGauge = updateRequest.TrainingProfile.ExBurstGauge;
    //    cardProfile.TrainingProfile.DamageDisplay = updateRequest.TrainingProfile.DamageDisplay;
    //    cardProfile.TrainingProfile.CpuAutoGuard = updateRequest.TrainingProfile.CpuAutoGuard;
    //    cardProfile.TrainingProfile.CommandGuideDisplay = updateRequest.TrainingProfile.CommandGuideDisplay;
    //    cardProfile.TrainingProfile.Player1HpMode = updateRequest.TrainingProfile.Player1HpMode;
    //    cardProfile.TrainingProfile.CpuHpAutoRegen = updateRequest.TrainingProfile.CpuHpAutoRegen ? 1u : 0u;
    //    cardProfile.TrainingProfile.ExOverLimit = updateRequest.TrainingProfile.ExOverLimit ? 1u : 0u;
    //    cardProfile.TrainingProfile.QuickFill = updateRequest.TrainingProfile.QuickFill ? 1u : 0u;

    //    context.SaveChanges();

    //    return Task.FromResult(new BasicResponse
    //    {
    //        Success = true
    //    });
    //}
}