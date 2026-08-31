using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Commands.LoadGameData;
using ServerOver.Mapper.Usage;
using ServerOver.Models.Config;

namespace ServerOver.Handlers.Game;

public record LoadGameDataQuery(Request Request) : IRequest<Response>;

public class LoadGameDataQueryHandler : IRequestHandler<LoadGameDataQuery, Response>
{
    private readonly CardServerConfig _config;
    
    public LoadGameDataQueryHandler(IOptions<CardServerConfig> options)
    {
        _config = options.Value;
    }
    
    public Task<Response> Handle(LoadGameDataQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        
        var loadGameData = new Response.LoadGameData
        {
            ReleaseCpuScenes = [1u, 2u, 3u, 4u, 5u, 6u, 7u, 8u, 9u, 10u, 11u, 12u, 13u, 14u, 15u, 16u, 17u],
            TrainingTimeLimit = _config.GameConfigurations.TrainingMinutes,
            NewcardCampaignFlag = true,
            BaseWinPoint = 20,
            BaseLosePoint = 20,
            WinBonus = 20,
            WinResultTopBonus = 20,
            LoseResultTopBonus = 20,
            ObPassBonusGp = 100,
            LoadGameDataVer = GlobalVars.LoadGameDataVer,
        };
        
        var loadGameDataCommands = new List<ILoadGameDataCommand>{
            new ReleasedContentCommand(_config),
            new PlayerLevelSettingCommand(),
            new VersusInfoCommand(),
            //new TriadCourseCommand(), // ob
            new TriadWantedDataCommand(_config),
            new FesDataCommand(_config),
            new FreeMatchDataCommand()
        };
        
        loadGameDataCommands.ForEach(command => command.Fill(loadGameData));
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            load_game_data = loadGameData
        };
        
        return Task.FromResult(response);
    }
}

public record LoadGameDataQuery2(Request Request) : IRequest<Response2>;

public class LoadGameDataQuery2Handler(IMediator mediator, IOptions<CardServerConfig> options)
    : IRequestHandler<LoadGameDataQuery2, Response2>
{
	private readonly CardServerConfig _config = options.Value;

    public async Task<Response2> Handle(LoadGameDataQuery2 request, CancellationToken cancellationToken)
    {
        var r = await mediator.Send(new LoadGameDataQuery(request.Request), cancellationToken);
        var r2 = r.ToResponse2();
		
		r2.load_game_data.on_vs_info ??= new(); //!? 也会影响stages
		r2.load_game_data.Stages = _config.CustomConfigs.p36_stages ?? [1u, 0u];

        
        if (_config.CustomConfigs != null && _config.CustomConfigs.PLvBoostPointBonus > 0)
        {
            r2.load_game_data._PlayerPointBonusSetting = new()
            {
                StartDate = (ulong)DateTimeOffset.Parse("2026-05-01").ToUnixTimeSeconds(),
                EndDate = (ulong)DateTimeOffset.Parse("2099-12-31 23:59:59").ToUnixTimeSeconds(),
                PointBonus = 1
            };
        }

        return r2;
    }
}
