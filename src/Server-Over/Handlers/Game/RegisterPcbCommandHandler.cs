using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.exvs;
using ProtoBuf;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using System.Net.Http.Headers;

namespace ServerOver.Handlers.Game;

public record RegisterPcbCommand(Request Request, string BaseAddress) : IRequest<Response>;

public class RegisterPcbCommandHandler(IOptions<CardServerConfig> _options, ServerDbContext _context,
    IHttpClientFactory httpClientFactory, 
    ILogger<RegisterPcbCommandHandler> logger) 
    : IRequestHandler<RegisterPcbCommand, Response>
{
    private readonly CardServerConfig _config = _options.Value;
    
    public async Task<Response> Handle(RegisterPcbCommand command, CancellationToken cancellationToken)
    {
        if (_config.CustomConfigs.RemoteCardServerConfigs?.Enable == true)
        {
            return await Handle1(command, cancellationToken);
        }
        
        if (_config.CustomConfigs.LocalMatchingConfigs?.Enable == true)
        {
            return await Handle2(command, cancellationToken);
        }
        
        return await Handle(command);
    }
    
    // 原ob代码
    private Task<Response> Handle(RegisterPcbCommand command)
    {
        var request = command.Request;
        
        var response = new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            register_pcb = new Response.RegisterPcb
            {
                NextMaintenanceStartAt = 2005364002,
                NextMaintenanceEndAt = 2005364004,
                SramClear = true,
                // LmIpAddresses = {"192.168.50.239"},
                Ipv4Flag = true,
                ServerInfoes =
                {
                    new Response.RegisterPcb.ServerInfo
                    {
                        ServerType = ServerType.SrvMatch,
                        Uri = $"{command.BaseAddress}/match",
                        Port = 12345
                    }
                }
            }
        };

        return Task.FromResult(response);
    }
    
    // remote
    private async Task<Response> Handle1(RegisterPcbCommand command, CancellationToken cancellationToken)
    {
        var req = new Request
        {
            Type = command.Request.Type,
            RequestId = command.Request.RequestId,
            PcbSerial = command.Request.PcbSerial,
            LocId = command.Request.LocId,
            AmId = command.Request.AmId,
            register_pcb = command.Request.register_pcb
        };
        var url = _config.CustomConfigs.RemoteCardServerConfigs.Address;

        using var requestStream = new MemoryStream();
        Serializer.Serialize(requestStream, req);
        requestStream.Seek(0, SeekOrigin.Begin);

        var client = httpClientFactory.CreateClient(string.Empty);
        client.Timeout = TimeSpan.FromSeconds(5);
        using var httpContent = new StreamContent(requestStream);
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
        var response = await client.PostAsync(url, httpContent);
        response.EnsureSuccessStatusCode();
        using var responseStream = await response.Content.ReadAsStreamAsync();
        using var s = new MemoryStream();
        responseStream.CopyTo(s);
        s.Seek(0, SeekOrigin.Begin);
        var r = Serializer.Deserialize<Response>(s);
        return r;
    }
    
    // local
    private async Task<Response> Handle2(RegisterPcbCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var localServerInfos = _config.CustomConfigs.LocalMatchingConfigs.ServerInfos?.ToList() ?? new();
        localServerInfos.RemoveAll(x => string.IsNullOrEmpty(x?.Uri));

        logger.LogInformation("reg pcb address='{address}'", command.BaseAddress);
        
        if (_config.CustomConfigs.LocalMatchingConfigs.UseFromRemoteCardServer)
        {
            try 
            {
                var r1 = await Handle1(command, cancellationToken);
                var serverInfos = r1?.register_pcb?.ServerInfoes?.ToList() ?? [];
                serverInfos.RemoveAll(x => string.IsNullOrEmpty(x?.Uri));
                if (serverInfos.Count > 0)
                {
                    serverInfos.RemoveAll(x => x.ServerType == ServerType.SrvMatch);
                    
                    var m = localServerInfos.FirstOrDefault(x => x.ServerType == ServerType.SrvMatch) ?? GetDefaultMatchingInfo(command.BaseAddress);
                    serverInfos.Insert(0, m);
                }
                var r2 = CreateResponse(request);
                r2.register_pcb.ServerInfoes.AddRange(serverInfos);
                return r2;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        if (!localServerInfos.Any(x => x.ServerType == ServerType.SrvMatch))
        {
            localServerInfos.Insert(0, GetDefaultMatchingInfo(command.BaseAddress));
        }

        var response = CreateResponse(request);
		response.register_pcb.ServerInfoes.AddRange(localServerInfos);
        return response;
    }
    
    static Response CreateResponse(Request request)
    {
        return new Response
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Error = Error.Success,
            register_pcb = new Response.RegisterPcb
            {
                NextMaintenanceStartAt = 2005364002,
                NextMaintenanceEndAt = 2005364004,
                SramClear = true,
                // LmIpAddresses = {"192.168.50.239"},
                Ipv4Flag = true,
            }
        };
    }
    
    static Response.RegisterPcb.ServerInfo GetDefaultMatchingInfo(string baseAddress)
    {
        var str = baseAddress.Replace("http://", "").Replace("https://", "").TrimEnd('/');
        uint port = 80;
        var i = str.IndexOf(':');
        if (i != -1 && (i + 1) < str.Length)
        {
            port = uint.Parse(str[(i + 1)..]);
        }
        str += "/match2";
        return new()
        {
            ServerType = ServerType.SrvMatch,
            Uri = str, 
            Port = port
        };
    }
}