using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Serilog;
using ServerOver;
using ServerOver.BackgroundServices;
using ServerOver.Caches;
using ServerOver.Models.Config;

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

Log.Information("Match Server starting...");

try
{
    var builder = WebApplication.CreateBuilder(args);
    Console.Title = GlobalVars.Title;
    
    const string configurationsDirectory = "Configurations";
    builder.Configuration.AddJsonFile($"{configurationsDirectory}/kestrel.json", optional: false)
        .AddJsonFile($"{configurationsDirectory}/log.json", optional: false)
        .AddJsonFile($"{configurationsDirectory}/server.json", optional: false);

    builder.Services.AddOptions<CardServerConfig>()
        .Bind(builder.Configuration.GetSection(CardServerConfig.CARD_SERVER_SECTION))
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

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c => 
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
        c.DocInclusionPredicate((docName, description) => true);
    });

    if (bool.TryParse(builder.Configuration["CardServerConfig:CustomConfigs:LocalMatchingConfigs:Enable"], out var b1) && b1 
        && bool.TryParse(builder.Configuration["CardServerConfig:CustomConfigs:LocalMatchingConfigs:Match3:Enable"], out var b2) && b2)
    {
        builder.Services.AddHostedService<Match3BgService>();
    }

    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<IIdCache, IdCache>();
    builder.Services.AddSingleton<IMatchingCache, MatchingCache>();
    builder.Services.AddSingleton<IMatchedCache, MatchedCache>();
    builder.Services.AddSingleton<IBattlingCache, BattlingCache>();

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


	app.MapControllers();
    app.UseStaticFiles();
    //app.MapFallbackToFile("index.html");
    //app.UseWhen(
    //    context => context.Request.Path.StartsWithSegments("/sys/servlet/PowerOn", StringComparison.InvariantCulture),
    //    applicationBuilder => applicationBuilder.UseAllNetRequestMiddleware());
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