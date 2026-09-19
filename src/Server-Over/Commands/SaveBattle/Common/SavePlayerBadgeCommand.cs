using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Persistence;
using ServerOver.Utils;
using WebUIOver.Shared.Dto.Player;

namespace ServerOver.Commands.SaveBattle.Common;

public class SavePlayerBadgeCommand(ServerDbContext context,
	ILogger? log = null) 
	: ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var cardId = (uint)cardProfile.Id;
        var badgeDomain = battleResultContext.PlayerBadgeDomain;
        
        if (badgeDomain == null)
        {
            log?.LogWarning("can't up player_badge for exp incr is {incr}", badgeDomain?.ExpIncrement);
            return;
        }
        
        var playerBadgeData = context.PlayerBadgeDbSet.FirstOrDefault(x => x.CardId == cardId);
        playerBadgeData ??= new() { CardId = cardId };
        
        log?.LogWarning("up player_badge {badgeIdAfter}, before badge={badgeId}, exp={expBefore}, incr={incr}.", 
            badgeDomain.BadgeIdAfter, playerBadgeData.BadgeId, playerBadgeData.BadgeExp, badgeDomain.ExpIncrement);
        
        if (!GlobalVars.CanUpPlayerBadgeExp)
            return;

        playerBadgeData.BadgeExp += badgeDomain.ExpIncrement;
        if (playerBadgeData.BadgeExp < 0)
        {
            playerBadgeData.BadgeExp = 0;

            // 好像并不会降级..
            //playerBadgeData.BadgeId = playerBadgeData.BadgeId == 0u ? 0u : (playerBadgeData.BadgeId - 1u);
        }

        // 徽章升级会延迟保存..
        // 连续游戏第2次保存或者gameover时..
        if (badgeDomain.BadgeIdAfter != null && badgeDomain.BadgeIdAfter != playerBadgeData.BadgeId)
        {
            playerBadgeData.BadgeId = badgeDomain.BadgeIdAfter.Value;
            playerBadgeData.BadgeExp = Math.Max(0, badgeDomain.ExpIncrement);
        }
        else
        {
            // 有时候发现exp到达最大时也会升级..
            FixBadgeIdByExpMax(playerBadgeData, badgeDomain.PlayerLevelId);
        }

        if (playerBadgeData.Id <= 0) context.PlayerBadgeDbSet.Add(playerBadgeData);
        else context.PlayerBadgeDbSet.Update(playerBadgeData);
        context.SaveChanges();
    }

    private void FixBadgeIdByExpMax(PlayerBadge playerBadgeData, uint plaverLv)
    {
		if (playerBadgeData.BadgeExp <= 0) return;
        
        var playerBadgeExpDtos = MyUtils.TryLoadFileJson<PlayerBadgeExpDto[]>(GlobalVars.PlayerBadgeExpsJsonFile).GetAwaiter().GetResult();
        var curr = playerBadgeExpDtos?.FirstOrDefault(x => x.BadgeId == playerBadgeData.BadgeId);
        curr ??= new() { BadgeId = playerBadgeData.BadgeId, MaxBadgeExp = GlobalVars.DefaultPlayerBadgeMaxExp, MinLv = 0 };
        
        if (playerBadgeData.BadgeExp >= curr.MaxBadgeExp)
        {
            if (plaverLv >= curr.MinLv) 
            {
                playerBadgeData.BadgeId += 1u;
            }
            playerBadgeData.BadgeExp = 0;
        }
    }
}