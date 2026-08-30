using WebUIOver.Shared.Dto.Common;
using WebUIOver.Shared.Dto.Response;

namespace WebUIOver.Client.Services.Common;

public interface ICommonDataService
{
    public Task InitializeAsync();
    public IReadOnlyList<Bgm> GetBgmSortedById();
    public Bgm? GetBgmById(uint id);
    public GeneralPreview? GetGaugeById(uint id);
    public IReadOnlyList<GeneralPreview> GetGaugesSortedById();
    public GeneralPreview? GetStageById(uint id);
    public IReadOnlyList<GeneralPreview> GetStagesSortedById();

    public ConfigsResponse GetConfigs();
}