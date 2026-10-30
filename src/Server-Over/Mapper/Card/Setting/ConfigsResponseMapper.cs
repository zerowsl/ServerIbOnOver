using Riok.Mapperly.Abstractions;
using ServerOver.Models.Config;
using WebUIOver.Shared.Dto.Response;

namespace ServerOver.Mapper.Card.Setting;

[Mapper]
public static partial class ConfigsResponseMapper
{
    [MapProperty(
		nameof(CardServerConfig.CustomConfigs.MaxFavouriteMs),
        nameof(ConfigsResponse.MaxFavouriteMs)
    )]
	[MapProperty(
		nameof(CardServerConfig.CustomConfigs.EabTriadGroup),
        nameof(ConfigsResponse.EnableTriadGroup)
    )]
    public static partial ConfigsResponse ToConfigsResponse(this CardServerConfig c);
}