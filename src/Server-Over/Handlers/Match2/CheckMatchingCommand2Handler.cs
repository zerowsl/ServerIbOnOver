using MediatR;
using Microsoft.Extensions.Options;
using nue.protocol.mms;
using ServerOver.Caches;
using ServerOver.Common.Enum;
using ServerOver.Models.Config;

namespace ServerOver.Handlers.Match;

public record CheckMatchingCommand2(Request Request) : IRequest<Response>;

public class CheckMatchingCommand2Handler(IMatchingCache matchingCache, 
    IOptions<CardServerConfig> options,
    ILogger<CheckMatchingCommand2Handler> logger) 
	: IRequestHandler<CheckMatchingCommand2, Response>
{
    readonly CardServerConfig _cardServerConfig = options.Value;

    public async Task<Response> Handle(CheckMatchingCommand2 cmd, CancellationToken cancellationToken)
    {
		var request = cmd.Request;
		
		if (( _cardServerConfig.CustomConfigs.RemoteCardServerConfigs?.Enable == true)
            || ( _cardServerConfig.CustomConfigs.LocalMatchingConfigs?.Enable != true))
        {
            return new Response
            {
                Type = request.Type,
                RequestId = request.RequestId,
                Code = ErrorCode.ErrServer
            };
        }
		
		var result = await DoMatching(request.check_matching);
		
        var response = new Response()
        {
            Type = request.Type,
            RequestId = request.RequestId,
            Code = ErrorCode.Success,
            check_matching = result
        };
        return response;
    }

    private async Task<Response.CheckMatching?> DoMatching(Request.CheckMatching matching)
	{
	    Response.CheckMatching? result = null;
	    for (var i = 0; result == null && i <= 1; i++) 
	    {
	        var clientInfo = await matchingCache.GetClientByNodeId(matching.NodeId);
            if (clientInfo?.Client?.PcbSerial is null) 
                break;
		
            
            // try匹配cpu
            result = await DoMatchingWithCpu(matching, clientInfo);
            if (result != null) break;
            
            await Task.Delay(1000);
	    }
	    return result;
	}
    
    
    // 匹配cpu
    async Task<Response.CheckMatching?> DoMatchingWithCpu(Request.CheckMatching matching, MatchingClientInfo clientInfo)
    {
        if (_cardServerConfig.CustomConfigs.LocalMatchingConfigs.Match2.MaxApplyTimeSec != -1)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (now - ((long)clientInfo.EntryAt) <= _cardServerConfig.CustomConfigs.LocalMatchingConfigs.Match2.MaxApplyTimeSec)
                return null;
        }
        else if (clientInfo.ApplyId < _cardServerConfig.CustomConfigs.LocalMatchingConfigs.Match2.MaxApplyId)
        {
            return null;
        }
        
        var npcCount = GetNpcCount(clientInfo);
        if (npcCount == 0) 
        {
            // 匹配failed
            return null;
        }
        
        var result = new Response.CheckMatching();
        //result.ApplyId = clientInfo.ApplyId;
		result.ApplyId = npcCount > 1 ? clientInfo.ApplyId : 1u;
		result.RoomId = (uint)(npcCount > 1 ? Random.Shared.Next(201, 999) : Random.Shared.Next(111, 199));
        //
        var groupId = (uint)(npcCount > 1 ? Random.Shared.Next(1111, 9999) : Random.Shared.Next(111, 199));
        var groupId2 = groupId + 1;
        // add self
        result.Clients.Add(new() { GroupId = groupId, Client = clientInfo.Client });
        // add 1/2/3 cpu npc
        var npcNodeId0 = Random.Shared.Next(101, 8000);
        for (var i = 0; i < npcCount; i++)
        {
            var npcClient = CreateCpuClient((uint)(npcNodeId0 + i), i, clientInfo);

            result.Clients.Add(new() { GroupId = (i % 2 == 0 ? groupId2 : groupId), Client = npcClient });
        }
        
        await AddToBattling(clientInfo, result, groupId);
        
        return result;
    }
    
    internal static Client CreateCpuClient(uint nodeId, int i, MatchingClientInfo? clientInfo = null)
    {
        Client cpuClient = new()
        {
            NodeId = nodeId,
            PcbSerial = $"CPU{i}",
            LocId = clientInfo?.Client?.LocId ?? "-",
            SubnetId = clientInfo?.Client?.SubnetId ?? "-",
            PrefCode = 0,
            GlobalIpaddress = "-",
            GlobalPort = 0,
            LocalIpaddress = "-",
            LocalPort = 0,
            NatMappingType = NatMappingType.NmNone,
            NatFilteringType = NatFilteringType.NfNone,
            NatAllocationPolicy = NatAllocationPolicy.NaRandom, // p102 NaNone
        };

        cpuClient.Properties.Add(new() { PropertyId = 0, PropertyValue = "N" }); // ? N n 也行
        cpuClient.Properties.Add(new() { PropertyId = 1, PropertyValue = "0" });
        cpuClient.Properties.Add(new() { PropertyId = 2, PropertyValue = "2" });
        cpuClient.Properties.Add(new() { PropertyId = 3, PropertyValue = "2" });
        cpuClient.Properties.Add(new() { PropertyId = 4, PropertyValue = "6" });
        cpuClient.Properties.Add(new() { PropertyId = 200, PropertyValue = "CostRandom" });

        return cpuClient;
    }
    
	int GetNpcCount(MatchingClientInfo clientInfo)
	{
		var worldId = clientInfo.World.WorldId;
		var fesType = _cardServerConfig.GameConfigurations.FesConfigurations.GetTodayFesType();
        //var isFes1v1 = Enum.GetName(fesType)?.Contains("OneOnOne", StringComparison.OrdinalIgnoreCase) == true;
        var c = worldId switch 
		{
		    13u => 3, // 店外1人 class match
			15u => 3, // 店内1人 offline match
			//22u => 3, // 店外招募 会重新选择机体?!
			23u => 3, // mix match
			//20u => 1, // Fes1v1 // pcb还是会匹配3个cpu然后会闪退..
            18u when fesType != FesType.DualSelect => 3, // Fes2v2
			18u when fesType == FesType.DualSelect => 6, // Fes2v2
			6u => 3, // 私房1人 custom match
            //33u => 1, // 房1人 custom match // pcb还是会匹配3个cpu然后会闪退..
			_ => 0
		};
		if (c == 0 && _cardServerConfig.CustomConfigs.Room?.Enable == true && worldId == _cardServerConfig.CustomConfigs.Room.WorldId)
		{
			c = 3;
		}
		logger.LogWarning("do-matching world_id={world_id} got {npcCount} cpu npc !!", worldId, c);
		return c;
	}
	
    async Task AddToBattling(MatchingClientInfo clientInfo, Response.CheckMatching result, uint groupId)
    {
        var clients = result.Clients;
        var roomId = result.RoomId;
        
        if (clientInfo != null)
        {
            await matchingCache.RemoveClientByNodeId(clientInfo.Client.NodeId, clientInfo.ApplyId);
        }
		
    }
}