using ServerOver.Models.Config;

namespace ServerOver;

internal static partial class GlobalVars
{
	internal const string Version = "1.3.7.864";
    internal const string Title0 = $"ServerIbOnOver v{Version}";
    internal const string Title = $"ib刷卡服务魔改ob系列.单机有限版.v{Version} - 自由软件,免费使用,提防倒卖,小心受骗";
	internal const string LogFangDaoMai = "自由軟件，免費使用，提防倒賣，小心受騙 !!!";
	
	internal const uint LoadGameDataVer = 10427; // 1.04.27.77635
	
	internal const long Ttl4CardIdPcbSerial = 600; // sec
	internal static readonly bool IsTtlSlide1 = true;
	
	internal const uint NaviBoostRemains = 486;

	internal const uint ClassRankGrade = 3;
	internal static readonly int[] ExRankNums = [1, 618, 520, 61, 51, 52, 53, 233, 486, 505, 607, 287, 315, 88, 20, 110, 666, 17, 996, 999, 8, 616, 0];
	internal static uint PatternId { get; private set; } = 0;
	internal const uint DefaultRateRange = 1000;
	internal static uint RateDiff { get; private set; }
	internal const uint MinClassGradeMinRate1 = 16;
    internal const uint MinClassGradeMinRate2 = DefaultRateRange * 10 + 16;

#if DEBUG
    internal static readonly string VsRouteBattleSaveDataDir = Path.Combine(Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule!.FileName)!, "savedatas/vsrb").Replace('\\', '/').TrimEnd('/');
#else
	internal static readonly string VsRouteBattleSaveDataDir = Path.Combine(Directory.GetCurrentDirectory(), "savedatas/vsrb").Replace('\\', '/').TrimEnd('/');
#endif

	internal static readonly bool CanUpPlayerBadgeExp = true;
    internal const int DefaultPlayerBadgeMaxExp = 5000000;
	internal static readonly string PlayerBadgeExpsJsonFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/display/player_badge_max_exps.json").Replace('\\', '/').TrimEnd('/');

	internal static void Init(CardServerConfig config)
	{
		RateDiff = config.CustomConfigs?.ClassMatchG?.RateDiff ?? 5u;
	}
}