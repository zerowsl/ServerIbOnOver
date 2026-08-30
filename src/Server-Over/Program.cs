using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Serilog;
using ServerOver;
using ServerOver.Caches;
using ServerOver.Common.Validation;
using ServerOver.Middlewares;
using ServerOver.Models.Config;
using ServerOver.Persistence;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("+=====================================+");
Log.Information("+              EXVS2-POC              +");
Log.Information("+                                     +");
Log.Information("+ Disclaimer:                         +");
Log.Information("+ FREE SOFTWARE, BEWARE OF SCAMMERS!  +");
Log.Information("+ IF YOU PAID, YOU ARE BEING SCAMMED! +");
Log.Information("+=====================================+");

Log.Information("Server starting...");

try
{
    SQLitePCL.Batteries_V2.Init();
    
    var builder = WebApplication.CreateBuilder(args);
    Console.Title = GlobalVars.Title;
    ProgramArgs.From(args);
    
    const string configurationsDirectory = "Configurations";
    builder.Configuration.AddJsonFile($"{configurationsDirectory}/kestrel.json", optional: false)
        .AddJsonFile($"{configurationsDirectory}/log.json", optional: false)
        .AddJsonFile($"{configurationsDirectory}/server.json", optional: false);

    builder.Services.AddOptions<CardServerConfig>()
        .Bind(builder.Configuration.GetSection(CardServerConfig.CARD_SERVER_SECTION))
        .ValidateMiniValidation()
        .ValidateOnStart();

    // Add services to the container.
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.WriteTo.Console().ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddMediatR(cfg => {
        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    });
    builder.Services.AddControllers().AddProtoBufNet();

    builder.Services.AddDbContext<ServerDbContext>(options => 
        options.UseSqlite(
            new SqliteConnectionStringBuilder() { DataSource = (builder.Configuration["ConnectionStrings:CardServer"]?.ToString() is string db && !string.IsNullOrEmpty(db) ? db : "ServerOver.db" ) }.ConnectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        )
    );
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c => 
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        c.DocInclusionPredicate((docName, description) => true);
    });

    builder.Services.AddMemoryCache();
	builder.Services.AddSingleton<IMatchingCache, MatchingCache>();
    builder.Services.AddSingleton<ICardIdPcbSerialCache, CardIdPcbSerialCache>();
    builder.Services.AddSingleton<IVsrbStageDataCache, VsrbStageDataCache>();

    builder.Services.AddHttpContextAccessor();
    
    builder.Services.AddHttpClient(string.Empty, (client) =>
    {
        client.Timeout = new TimeSpan(0, 0, 10);
    })
    .ConfigurePrimaryHttpMessageHandler((sp) => new SocketsHttpHandler() 
    {
        UseProxy = false,
        SslOptions = new()
        {
            RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
        }
    });
    
    var app = builder.Build();
    
    var config = app.Services.GetRequiredService<IOptions<CardServerConfig>>().Value;
    GlobalVars.Init(config);
    Log.Information("Card server config: {@Config}", config.ToString());
    
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms, " +
                                  "request host: {RequestHost}";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        };
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(c => 
        {
            c.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
        });
        app.UseSwaggerUI(c => 
        {
            c.ShowExtensions();
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        });
    }

    // auto start migration
	using (var scope = app.Services.CreateScope())
	{
        if (!string.IsNullOrEmpty(ProgramArgs.DbGoBackTo))
        {
            Log.Information("确定数据库文件要回退到ob吗？输入'y'键并按回车继续,其他键退出：");
            if (Console.ReadLine()?.Trim() == "y")
            {
                Log.Information("正在回退到ob中...");
                var db = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
                await db.Database.MigrateAsync(ProgramArgs.DbGoBackTo);
                Log.Information("数据库文件已回退到ob !!");
            }
            return;
        }
        else
        {
            var db = scope.ServiceProvider.GetRequiredService<ServerDbContext>();
            await db.Database.MigrateAsync();
        }
	}

    app.UseBlazorFrameworkFiles();
	app.MapControllers();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");
    app.UseWhen(
        context => context.Request.Path.StartsWithSegments("/sys/servlet/PowerOn", StringComparison.InvariantCulture),
        applicationBuilder => applicationBuilder.UseAllNetRequestMiddleware());
    app.UseStatusCodePages(context =>
    {
        var code = context.HttpContext.Response.StatusCode;
        if (code == 404)
        {
            app.Logger.LogWarning("Request to {Path} returned 404, type is {Type}", 
                context.HttpContext.Request.Path,
                context.HttpContext.Request.Method);
        }

        return Task.CompletedTask;
    });
    app.UseForwardedHeaders();
    app.Run();
}
catch (Exception ex) when (
    // https://github.com/dotnet/runtime/issues/60600
    ex.GetType().Name is not "StopTheHostException"
    // HostAbortedException was added in .NET 7
    // need to do it this way until we target .NET 8
    && ex.GetType().Name is not "HostAbortedException")
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}