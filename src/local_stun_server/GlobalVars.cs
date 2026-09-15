using System.Net;

namespace StunService.Models;

internal static partial class GlobalVars
{
	internal const string Version = "1.1.0";
	internal const string Title = $"local stun server v{Version} - 自由软件,免费使用,提防倒卖,小心受骗";
	internal const string LogFangDaoMai = "自由软件,免费使用,提防倒卖,小心受骗 !!!";

	public static IPAddress IPAddress = IPAddress.Any;
    public static int PrimaryPort = 3478;

	public static void Init(string[] args)
	{
		switch (args.Length)
		{
			case 2:
				IPAddress = IPAddress.TryParse(args[0], out var _ip) ? _ip : IPAddress;
                PrimaryPort = int.TryParse(args[1], out var _p1) ? _p1 : PrimaryPort;
                break;
			case 1:
                PrimaryPort = int.TryParse(args[0], out var _p) ? _p : PrimaryPort;
                break;
		}
    }
}