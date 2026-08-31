using nue.protocol.exvs;

namespace ServerOver.Context.Battle.Domain.PvP;

public class NetworkDataDomain
{
    public List<NetworkReport.NetworkReportPcbData> PcbDatas { get; set; } = [];
}