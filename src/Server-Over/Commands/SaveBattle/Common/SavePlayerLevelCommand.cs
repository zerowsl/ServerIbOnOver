using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Common;

public class SavePlayerLevelCommand(ServerDbContext _context,
	ILogger? log = null) 
	: ISaveBattleDataCommand
{
    //private const uint NormalMaxLv = 200; // ob
	//private const uint ExMaxLv = 999; // ob
    private const uint NormalMaxLv = 1000;
    private const uint ExMaxLv = 1000;

    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var playerLevelDomain = battleResultContext.PlayerLevelDomain;
        var playerLevelData = _context.PlayerLevelDbSet.First(x => x.CardProfile == cardProfile);
        var maxLevel = GetMaxLevel(playerLevelData);

        // Skip Processing if LevelIdBefore >= 200 / 999, because EXP can't be further incremented
        if (playerLevelDomain.LevelIdBefore >= maxLevel)
        {
            return;
        }

        if (playerLevelDomain.LevelIdBefore == playerLevelDomain.LevelIdAfter)
        {
            playerLevelData.PlayerExp += playerLevelDomain.ExpIncrement;
            return;
        }
		log?.LogWarning("up to lv={levelIdAfter}, before lv={playerLevelId}, exp={expBefore}, +{incr}", playerLevelDomain.LevelIdAfter, playerLevelData.PlayerLevelId, playerLevelData.PlayerExp, playerLevelDomain.ExpIncrement);
		
        playerLevelData.PlayerLevelId = playerLevelDomain.LevelIdAfter;
        playerLevelData.PlayerExp = 0;

        // If Level After >= 200 and PrestigeId < 3, eligible to increment Prestige ID and reset Player Lv to 1 through Web UI
        //if (playerLevelDomain.LevelIdAfter >= NormalMaxLv && playerLevelDomain.PrestigeId < 3)
        if (playerLevelDomain.LevelIdAfter >= NormalMaxLv)
        {
            playerLevelData.LevelMaxDispFlag = true;
        }
    }

    private uint GetMaxLevel(PlayerLevel playerLevelData)
    {
        if (playerLevelData.PrestigeId < 3)
        {
            return NormalMaxLv;
        }

        return ExMaxLv;
    }
}