using MediatR;
using nue.protocol.exvs;
using ServerOver.Utils;

namespace ServerOver.Handlers.Game;

public record LoadRankingQuery2(Request Request) : IRequest<Response2>;

public class LoadRankingQuery2Handler(ILogger<LoadRankingQuery2Handler> logger) 
    : IRequestHandler<LoadRankingQuery2, Response2>
{
    public Task<Response2> Handle(LoadRankingQuery2 request, CancellationToken cancellationToken)
    {
        var loadRanking = new Response2.LoadRanking();

        var loadRankingRequest = request.Request.load_ranking;

        LoadPlayerScoreRank(loadRanking, loadRankingRequest);
        LoadClassMatch(loadRanking, loadRankingRequest);
        //...
        
        return Task.FromResult(new Response2
        {
            Type = request.Request.Type,
            RequestId = request.Request.RequestId,
            Error = Error.Success,
            load_ranking = loadRanking
        });
    }
    
    private void LoadPlayerScoreRank(Response2.LoadRanking loadRanking, Request.LoadRanking loadRankingRequest)
    {
        if (loadRankingRequest.RankType == RankMessageType.LmMonthlySpotPlayerScore && loadRankingRequest.CurrentFlag == true)
        {
            //loadRanking.RankType = RankMessageType.LmMonthlySpotPlayerScore;
            loadRanking.RankType = loadRankingRequest.RankType;

            var liveRankingTime = LiveRankingTimeUtil.Get();

            loadRanking.Timestamp = liveRankingTime.CurrentTimeStamp;
            
            var playerScoreRank = new PlayerScoreRank2();
            playerScoreRank.SumStart = liveRankingTime.MonthStartTimeStamp;
            playerScoreRank.SumEnd = liveRankingTime.MonthEndTimeStamp;
            
            for (uint i = 1; i < 11; i++)
            {
                playerScoreRank.Rows.Add(new()
                {
                    RankNo = i,
                    PrevRankNo = i,
                    PilotId = i,
                    PlayerName = "Player" + i.ToString(),
                    Score = 10000 * (11 - i),
                    PlayerLevelId = 1000,
                    BadgeId = 53,
                    OpenPlayerLevel = 1, 
                    TitleTextId = 0,
                    //TitleOrnamentId = 0,
                    //TitleEffectId = 0,
                    //TitleBackgroundPartsId = 0,
                    CustomTxt = "486",
                    GroupName = "486", 
                    FavMsId = 165,
                    MsUsedNum = 1,
                    HomeLocName = "EXTREME",
                    HomeLocPref = 1,
                    OpenRecord = 6,
                    PlayNum = 100,
                    WinNum = 100,
                    ClassIdSolo = 4,
                    ClassIdTeam = 4,
                    SkinId = 0,
                    GradeSolo = 11, 
                    GradeTeam = 11, 
                    TopPointRankSolo = 1, 
                    TopPointRankTeam = 1, 
                });
            }
            
            loadRanking.PlayerScoreRank = playerScoreRank;
        }
    }
    
    private void LoadClassMatch(Response2.LoadRanking loadRanking, Request.LoadRanking loadRankingRequest)
    {
        if (loadRankingRequest.RankType == RankMessageType.LmMonthlyCountryClassMatch && loadRankingRequest.CurrentFlag == true)
        {
            loadRanking.RankType = loadRankingRequest.RankType;

            var liveRankingTime = LiveRankingTimeUtil.Get();

            loadRanking.Timestamp = liveRankingTime.CurrentTimeStamp;
            
            var rank = new ClassMatchScore2();
            rank.SumStart = liveRankingTime.MonthStartTimeStamp;
            rank.SumEnd = liveRankingTime.MonthEndTimeStamp;
            
            for (uint i = 1; i < 11; i++)
            {
                rank.Rows.Add(new()
                {
                    RankNo = i,
                    PrevRankNo = i,
                    PilotId = i,
                    PlayerName = "Player" + i.ToString(),
                    Score = 2000 - i * 50,
                    PlayerLevelId = 1000,
                    BadgeId = 53,
                    TitleTextId = 0,
                    CustomTxt = "486",
					GroupName = "486", 
                    FavMsId = 310,
                    MsUsedNum = 1,
                    HomeLocName = "EXTREME",
                    HomeLocPref = 1,
                    OpenRecord = 6,
                    PlayNum = 100,
                    WinNum = 100,
                    ClassIdSolo = 1,
                    ClassIdTeam = 2,
                    SkinId = 0,
                    GradeIdSolo = 1, 
                    GradeIdTeam = 1, 
                    OpenPlayerLevel = 1, 
                    TopPointRankSolo = 0, 
                    TopPointRankTeam = 0, 
                });
            }
            
            loadRanking.ClassMatchScore = rank;
        }
    }

}