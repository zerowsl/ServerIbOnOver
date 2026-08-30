using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Shared.Dto.Request;

public class UpdateQuickStartProfileRequest : BasicCardRequest
{
    public QuickStartProfile QuickStartProfile { get; set; } = default!;
}