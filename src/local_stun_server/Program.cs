using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StunService.Models;
using StunService.Services;

var builder = Host.CreateApplicationBuilder(args);
Console.Title = GlobalVars.Title;
GlobalVars.Init(args);

// 注册
builder.Services.AddSingleton<StunProtocol>();
builder.Services.AddHostedService<StunServer>();

// 配置日志
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
});

var host = builder.Build();

Console.WriteLine("STUN Server (RFC 5389)");
Console.WriteLine("=====================");
Console.WriteLine("Listening on UDP ports:");
Console.WriteLine($"  Bind IP:   {GlobalVars.IPAddress}");
Console.WriteLine($"  Primary Port:   {GlobalVars.PrimaryPort}");
Console.WriteLine("Press Ctrl+C to exit");
Console.WriteLine();

await host.RunAsync();