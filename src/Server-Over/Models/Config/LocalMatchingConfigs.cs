using nue.protocol.exvs;

namespace ServerOver.Models.Config;

public sealed class LocalMatchingConfigs
{
	public bool Enable { get; set; }

    public bool UseFromRemoteCardServer { get; set; }

    public Match2Config Match2 { get; set; } = new();

    public Response.RegisterPcb.ServerInfo?[]? ServerInfos { get; set; }

    public sealed class Match2Config
    {
        public long MaxApplyTimeSec { get; set; } = 20;
        public uint MaxApplyId { get; set; } = 3;
        public uint CheckingInterval { get; set; } = 10;
    }
}