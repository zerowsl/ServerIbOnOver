using MediatR;
using ServerOver.Utils;

namespace ServerOver.Handlers.Game;

public record LoadVsRouteBattleDataByUrlQuery(uint CardId) : IRequest<byte[]>;

public class LoadVsRouteBattleDataByUrlQueryHandler : IRequestHandler<LoadVsRouteBattleDataByUrlQuery, byte[]>
{
    public async Task<byte[]> Handle(LoadVsRouteBattleDataByUrlQuery request, CancellationToken cancellationToken)
    {
        // Data 其实就是 '{游戏根目录}\29\route_battle\save_data.bin' 去掉头和尾
        //
        // 等到进入stage的第一个小关结算才删除上次的存档
        //

        var cardId = request.CardId;

        FileUtils.DirMakeSureExists(GlobalVars.VsRouteBattleSaveDataDir);

        byte[] data;
        try
        {
            data = await File.ReadAllBytesAsync(Path.Combine(GlobalVars.VsRouteBattleSaveDataDir, $"{cardId}.bin"), cancellationToken);
        }
        catch
        {
            data = [];
        }

        return data;
    }
}
