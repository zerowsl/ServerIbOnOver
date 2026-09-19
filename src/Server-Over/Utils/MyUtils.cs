using System.IO.Hashing;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ServerOver.Utils;

internal static class MyUtils
{

    public static async Task<T> TryLoadFileJson<T>(string path)
    {
        try
        {
            var s = await File.ReadAllTextAsync(path);
            var o = JsonSerializer.Deserialize<T>(s);
            return o!;
        }
        catch 
        { 
            return default!;
        }
    }

    public static void ShowTipAfterSaveBattleResult(ILogger logger, string s)
    {
        _ = Task.Run(async () =>
        {
            Console.Title = GlobalVars.Title;
            for (var i = 0; i < 30; i++)
            {
                logger.LogWarning(s);
                logger.LogWarning(GlobalVars.LogFangDaoMai);
            }
            await Task.Delay(1500).ConfigureAwait(false);
            for (var i = 0; i < 50; i++)
            {
                logger.LogWarning(s);
                logger.LogWarning(GlobalVars.LogFangDaoMai);
            }
        });
    }

    public static uint ToUintByCrc32(string str)
    {
        using var ha = SHA256.Create();
        var by = ha.ComputeHash(Encoding.UTF8.GetBytes(str));
        var i = Crc32.HashToUInt32(by);
        return i;
    }
}