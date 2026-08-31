using ServerOver.Context.Battle;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Triad;
using ServerOver.Persistence;

namespace ServerOver.Commands.SaveBattle.Triad;

public class SaveIbVsRouteBattleStageHighScoreCommand(ServerDbContext context) 
    : ISaveBattleDataCommand
{
    public void Save(CardProfile cardProfile, BattleResultContext battleResultContext)
    {
        var domain = battleResultContext.StageResultInfoDomain;
        if (domain == null)
        {
            return;
        }

        var ls = context.VsRouteBattleStageHighScoreInfoDbSet.Where(x => x.CardId == cardProfile.Id && 
            (!x.IsRecord || x.Difficulty == domain.Difficulty))
            .ToList();

        var m = ls.FirstOrDefault(x => !x.IsRecord);
        m ??= new() { IsRecord = false };
        m.CardId = cardProfile.Id;
        m.Difficulty = domain.Difficulty;
        var isAllStageCompleted = domain.IsCompleted && domain.CompletedResult == null;
        switch (domain.StageId)
        {
            case 1:
                {
                    ClearScore(ref m);
                    m.ClearScore1 = domain.TotalClearScore;
                    m.ClearTime1 = domain.TotalClearTime;
                }
                break;
            case 2:
                {
                    m.ClearScore2 = domain.TotalClearScore;
                    m.ClearTime2 = domain.TotalClearTime;
                }
                break;
            case 3:
                {
                    m.ClearScore3 = domain.TotalClearScore;
                    m.ClearTime3 = domain.TotalClearTime;
                }
                break;
        }
        if (isAllStageCompleted)
        {
            m.TotalClearScore = m.ClearScore1 + m.ClearScore2 + m.ClearScore3;
            m.TotalClearTime = m.ClearTime1 + m.ClearTime2 + m.ClearTime3;
        }
        else if (!domain.IsCompleted)
        {
            m.TotalClearTime = 0;
            m.TotalClearScore = m.ClearScore1 + m.ClearScore2 + m.ClearScore3;
        }
        if (m.Id <= 0) context.VsRouteBattleStageHighScoreInfoDbSet.Add(m);
        else context.VsRouteBattleStageHighScoreInfoDbSet.Update(m);

        var mh = ls.FirstOrDefault(x => x.IsRecord);
        if ((isAllStageCompleted || !domain.IsCompleted) && ((mh?.TotalClearScore ?? 0u) <= m.TotalClearScore))
        {
            // '已全通关'或'已失败'时才会记录，并且只记录新的最高分
            
            mh ??= new() { CardId = m.CardId, Difficulty = m.Difficulty, IsRecord = true };
            CopyRecordScore(ref mh, m);

            if (mh.Id <= 0) context.VsRouteBattleStageHighScoreInfoDbSet.Add(mh);
            else context.VsRouteBattleStageHighScoreInfoDbSet.Update(mh);
        }

        context.SaveChanges();
    }

    static void ClearScore(ref VsRouteBattleStageHighScoreInfo m)
    {
        m.ClearScore1 = m.ClearScore2 = m.ClearScore3 = 0;
        m.ClearTime1 = m.ClearTime2 = m.ClearTime3 = 0;
        m.TotalClearScore = 0;
        m.TotalClearTime = 0;
    }

    static void CopyRecordScore(ref VsRouteBattleStageHighScoreInfo mh, VsRouteBattleStageHighScoreInfo m)
    {
        mh.IsRecord = true;
        mh.ClearScore1 = m.ClearScore1;
        mh.ClearScore2 = m.ClearScore2;
        mh.ClearScore3 = m.ClearScore3;
        mh.ClearTime1 = m.ClearTime1;
        mh.ClearTime2 = m.ClearTime2;
        mh.ClearTime3 = m.ClearTime3;
        mh.TotalClearScore = m.TotalClearScore;
        mh.TotalClearTime = m.TotalClearTime;
    }
}