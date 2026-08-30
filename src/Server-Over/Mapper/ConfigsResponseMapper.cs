using Riok.Mapperly.Abstractions;
using ServerOver.Models.Config;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Mapper.Usage;

[Mapper]
public static partial class ConfigsResponseMapper
{
    [MapProperty(
		nameof(CardServerConfig.CustomConfigs.MaxFavouriteMs),
        nameof(ConfigsResponse.MaxFavouriteMs)
    )]
    public static partial ConfigsResponse ToConfigsResponse(this CardServerConfig c);
}