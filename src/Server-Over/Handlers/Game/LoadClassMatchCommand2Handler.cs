using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ServerOver.Models.Config;
using ServerOver.Persistence;

namespace ServerOver.Handlers.Game;

public record LoadClassMatchCommand2(Request Request) : IRequest<object>;

public partial class LoadClassMatchCommand2Handler(ServerDbContext _context, IOptions<CardServerConfig> options,
    ILogger<LoadClassMatchCommand2Handler> _logger)
    : IRequestHandler<LoadClassMatchCommand2, object>
{
    private readonly CardServerConfig _config = options.Value;

    public async Task<object> Handle(LoadClassMatchCommand2 command, CancellationToken cancellationToken)
    {
        if (_config.CustomConfigs?.ClassMatchG?.EnableRate == true)
        {
            return await HandleIb(command, cancellationToken);
        }
        else
        {
            return await HandleOb(command, cancellationToken);
        }
    }

    internal Task<Response2> HandleIb(LoadClassMatchCommand2 command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        // Approximately: (ob)
        // Rate obtained in Win = ConstantRate * [1 + (WinScore / WinScoreCoefficient)], 
        // Rate deducted in Loss = ConstantRate
		//
        var r = new Response2.LoadClassMatch()
        {
            ConstantRate = _config.CustomConfigs.ClassMatchG.ConstantRate, // Mainly relies on this Rate for both Win and Loss
            WinScoreCoefficient = _config.CustomConfigs.ClassMatchG.WinScoreCoefficient, // During Class Match Pt calculation, Player Battle Score will be divided by this factor
            SearchRatios = new(),
            InitialRate = _config.CustomConfigs.ClassMatchG.InitialRate
        };

        LoadChangeInitRates(r);
        LoadGradeThresholds(r);
        LoadConsecutives(r);
        LoadSearchRatios(r);
        LoadMatching(r);
        
        var response = new Response2
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            load_class_match = r
        };
        return Task.FromResult(response);
    }
    
    private void LoadMatching(Response2.LoadClassMatch r)
    {
        // 0-17
        r.ClassMatchingTimeSchedules.Add(new()
        {
            StartAt = (ulong)(DateTimeOffset.Now - TimeSpan.FromDays(10)).ToUnixTimeSeconds(),
            EndAt = (ulong)(DateTimeOffset.Now + TimeSpan.FromDays(365)).ToUnixTimeSeconds(),
            PatternId = GlobalVars.PatternId
        });

        r.ClassMatchingPatternTables.Add(new()
        {
            PatternId = GlobalVars.PatternId
            //...
        });

    }
    
    internal static void LoadChangeInitRates(Response2.LoadClassMatch r)
    {
        var rate = 1000u;
        if (true)
        {
            r.ClassChangeInitialRates.Add(new()
            {
                TeamType = 0, 
                ClassId = 0, 
                UpDefaultRate = 5,
                DownDefaultRate = 0
            });
        }
        for (var iClassId = 1u; iClassId <= 53u; iClassId++)
        {
            r.ClassChangeInitialRates.Add(new()
            {
                TeamType = 0, 
                ClassId = iClassId, 
                UpDefaultRate = rate, 
                DownDefaultRate = 0
            });
        }
        if (true)
        {
            r.ClassChangeInitialRates.Add(new()
            {
                TeamType = 1, 
                ClassId = 0, 
                UpDefaultRate = 5,
                DownDefaultRate = 0
            });
        }
        for (var iClassId = 1u; iClassId <= 53u; iClassId++)
        {
            r.ClassChangeInitialRates.Add(new()
            {
                TeamType = 1, 
                ClassId = iClassId, 
                UpDefaultRate = rate, 
                DownDefaultRate = 0
            });
        }
    }
    
    internal static void LoadGradeThresholds(Response2.LoadClassMatch r)
    {
        var (diff, iRate) = (GlobalVars.RateDiff, GlobalVars.DefaultRateRange);
        var (v1, v2) = (1u, iRate);
        for (var iClassId = 1u; iClassId <= 4u; iClassId++)
        {
            for (var iGradeId = 1u; iGradeId <= 10u; iGradeId++)
            {
                r.GradeThresholds.Add(new()
                {
                    TeamType = 0,
                    ClassId = iClassId,
                    GradeId = iGradeId,
                    MaxRate = !(iClassId == 4u && iGradeId == 10u) ? (v2 + diff) : (v2 += iRate),
                    MinRate = v1
                });
                (v1, v2) = (v2, v2 + iRate);
            }
        }
        (v1, v2) = (iRate * 10 + 1u, iRate * 11);
        for (var iClassId = 2u; iClassId <= 4u; iClassId++)
        {
            for (var iGradeId = 1u; iGradeId <= 10u; iGradeId++)
            {
                r.GradeThresholds.Add(new()
                {
                    TeamType = 1,
                    ClassId = iClassId,
                    GradeId = iGradeId,
                    MaxRate = !(iClassId == 4u && iGradeId == 10u) ? (v2 + diff) : (v2 += iRate),
                    MinRate = v1
                });
                (v1, v2) = (v2, v2 + iRate);
            }
        }
    }
    
    private void LoadConsecutives(Response2.LoadClassMatch r)
    {
        r.ConsecutiveWinGrades.Add(new()
        {
            ConsecutiveNumStart = 1, 
            ConsecutiveNumEnd = 1, 
            GradeNum = 1
        });
        r.ConsecutiveWinGrades.Add(new()
        {
            ConsecutiveNumStart = 2, 
            ConsecutiveNumEnd = 3, 
            GradeNum = 2
        });
        r.ConsecutiveWinGrades.Add(new()
        {
            ConsecutiveNumStart = 4, 
            ConsecutiveNumEnd = 5, 
            GradeNum = 3
        });
        r.ConsecutiveWinGrades.Add(new()
        {
            ConsecutiveNumStart = 6, 
            ConsecutiveNumEnd = uint.MaxValue,
            GradeNum = 4
        });
        
        r.ConsecutiveLoseGrades.Add(new()
        {
            ConsecutiveNumStart = 2, 
            ConsecutiveNumEnd = uint.MaxValue, 
            GradeNum = 1
        });
    }
    
    private void LoadSearchRatios(Response2.LoadClassMatch r)
    {
        // r.SearchRatios.SearchRatioSolo.Add(new()
        // {
        // });
    }
}