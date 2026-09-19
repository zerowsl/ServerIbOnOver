using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Persistence;
using ServerOver.Utils;

namespace ServerOver.Commands.SaveBattle.Triad;

public class RemoveIbVsRouteBattleDataCommand(ServerDbContext context, uint phaseId) 
    : ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        // 第一小关卡结算时, 尝试删除之前的存档
        if (phaseId > 1u)
        {
            return;
        }

        FileUtils.FileDelNoError(Path.Combine(GlobalVars.VsRouteBattleSaveDataDir, $"{cardProfile.Id}.bin"));
    }
}