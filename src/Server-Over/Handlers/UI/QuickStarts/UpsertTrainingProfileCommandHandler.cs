using MediatR;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Request;
using WebUIOver.Shared.Dto.Response;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.QuickStarts;

public record UpsertQuickStartProfileCommand(UpdateQuickStartProfileRequest Request) : IRequest<BasicResponse>;

public class UpsertQuickStartProfileCommandHandler(ServerDbContext context) : IRequestHandler<UpsertQuickStartProfileCommand, BasicResponse>
{
    public Task<BasicResponse> Handle(UpsertQuickStartProfileCommand request, CancellationToken cancellationToken)
    {
        var updateRequest = request.Request;

        var cardProfile = context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == updateRequest.AccessCode && x.ChipId == updateRequest.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var profile = context.QuickStartInfoDbSet.FirstOrDefault(x => x.CardId == cardProfile.Id);
        profile ??= new() { CardId = (uint)cardProfile.Id };
        profile.SaveMode = updateRequest.QuickStartProfile.SaveMode;
        profile.GameMode = updateRequest.QuickStartProfile.GameMode;
        profile.RuleType = updateRequest.QuickStartProfile.RuleType;
        profile.TeamType = updateRequest.QuickStartProfile.TeamType;
        profile.MstMobileSuitId = updateRequest.QuickStartProfile.MstMobileSuitId;
        profile.BurstType = updateRequest.QuickStartProfile.BurstType;
        profile.PartnerMobileSuitId = updateRequest.QuickStartProfile.PartnerMobileSuitId;
        profile.PartnerBurstType = updateRequest.QuickStartProfile.PartnerBurstType;
        profile.BattleStageId = updateRequest.QuickStartProfile.BattleStageId;

        if (profile.Id <= 0) context.QuickStartInfoDbSet.Add(profile);
        else context.QuickStartInfoDbSet.Update(profile);
        context.SaveChanges();

        return Task.FromResult(new BasicResponse
        {
            Success = true
        });
    }
}