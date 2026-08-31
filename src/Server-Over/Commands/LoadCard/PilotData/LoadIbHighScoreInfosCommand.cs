using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PilotData;

public class LoadIbHighScoreInfosCommand(ServerDbContext context) : ILoadCard2Command
{
    public void Fill(CardProfile cardProfile, Response2.LoadCard loadCard)
    {
        var m = context.VsRouteBattleStageHighScoreInfoDbSet.Where(x => x.CardId == cardProfile.Id && x.IsRecord && x.TotalClearTime > 0).ToArray();

        loadCard.pilot_data_group.HighScoreInfos.AddRange(m.Select(x => new Response2.LoadCard.PilotDataGroup.HighScoreInfo()
        {
            Difficulty = x.Difficulty,
            ClearTime1 = x.ClearTime1,
            ClearScore1 = x.ClearScore1,
            ClearTime2 = x.ClearTime2,
            ClearScore2 = x.ClearScore2,
            ClearTime3 = x.ClearTime3,
            ClearScore3 = x.ClearScore3,
            TotalClearTime = x.TotalClearTime,
            TotalClearScore = x.TotalClearScore,
        }));

        // 刷卡后预先开启难度5
        if (loadCard.pilot_data_group.HighScoreInfos.Count == 0)
        {
            loadCard.pilot_data_group.HighScoreInfos.Add(new Response2.LoadCard.PilotDataGroup.HighScoreInfo()
            {
                Difficulty = 4,
                ClearTime1 = 1800,
                ClearScore1 = 0,
                ClearTime2 = 1800,
                ClearScore2 = 0,
                ClearTime3 = 1800,
                ClearScore3 = 1,
                TotalClearTime = 5400,
                TotalClearScore = 1,
            });
        }
    }
}