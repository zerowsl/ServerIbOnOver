using ServerOver.Models.Config;

namespace ServerOver;

internal static partial class GlobalVars
{
	internal const string Version = "1.3.7.864";
    internal const string Title0 = $"ServerIbOnOver.Match v{Version}";
    internal const string Title = $"ServerIbOnOver.Match v{Version} - 自由软件,免费使用,提防倒卖,小心受骗";
	internal const string LogFangDaoMai = "自由軟件，免費使用，提防倒賣，小心受騙 !!!";
		
	internal static uint PatternId { get; private set; } = 0;

	internal static void Init(CardServerConfig config)
	{
		PatternId = 0u;
    }
}
