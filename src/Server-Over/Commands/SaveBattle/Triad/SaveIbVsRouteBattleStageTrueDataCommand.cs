using ServerOver.Caches;
using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Utils;

namespace ServerOver.Commands.SaveBattle.Triad;

public class SaveIbVsRouteBattleStageTrueDataCommand(IVsrbStageDataCache vsrbStageDataCache) 
    : ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var cardId = (uint)cardProfile.Id;
        HandleWhenIsGameOver(cardId).GetAwaiter().GetResult();
    }

    private async Task HandleWhenIsGameOver(uint cardId)
    {
        var item = await vsrbStageDataCache.Get(cardId);
        if (item?.Data == null) return;

        FileUtils.DirMakeSureExists(GlobalVars.VsRouteBattleSaveDataDir);
        await File.WriteAllBytesAsync(Path.Combine(GlobalVars.VsRouteBattleSaveDataDir, $"{cardId}.bin"), item.Data);

        await vsrbStageDataCache.Remove(cardId);
    }
}