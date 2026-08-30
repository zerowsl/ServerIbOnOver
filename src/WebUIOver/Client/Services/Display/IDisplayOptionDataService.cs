using WebUIOver.Shared.Dto.Common;

namespace WebUIOver.Client.Services.Display;

public interface IDisplayOptionDataService
{
    public Task InitializeAsync();
    public IReadOnlyList<IdValuePair> GetDisplayOptionsSortedById();
    public IReadOnlyList<IdValuePair> GetPlayerLevelDisplayOptionsSortedById();
    public IReadOnlyList<IdValuePair1> GetPlayerBadgeDisplayOptionsSortedById();
}