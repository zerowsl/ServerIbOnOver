using MediatR;
using ServerOver.Mapper.QuickStarts;
using ServerOver.Persistence;
using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Exception;

namespace ServerOver.Handlers.UI.QuickStarts;

public record GetQuickStartProfileCommand(string AccessCode, string ChipId) : IRequest<QuickStartProfile>;

public class GetQuickStartProfileCommandHandler(ServerDbContext context) : IRequestHandler<GetQuickStartProfileCommand, QuickStartProfile>
{
    public Task<QuickStartProfile> Handle(GetQuickStartProfileCommand request, CancellationToken cancellationToken)
    {
        var cardProfile = context.CardProfiles
            .FirstOrDefault(x => x.AccessCode == request.AccessCode && x.ChipId == request.ChipId);

        if (cardProfile is null)
        {
            throw new InvalidCardDataException("Card Profile is invalid");
        }

        var profile = context.QuickStartInfoDbSet
            .FirstOrDefault(x => x.CardId == cardProfile.Id);

        profile ??= new()
        {
            CardId = (uint)cardProfile.Id,
            MstMobileSuitId = 65534,
        };

        var r = profile.ToQuickStartProfile();
        return Task.FromResult(r);
    }
}