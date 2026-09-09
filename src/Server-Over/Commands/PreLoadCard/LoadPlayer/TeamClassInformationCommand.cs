using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using ServerOver.Processor.Class.Effect;

namespace ServerOver.Commands.PreLoadCard.LoadPlayer;

[Obsolete("ob")]
public class TeamClassInformationCommand : IPreLoadPlayerCommand
{
    private readonly ServerDbContext _context;
    private readonly ClassEffectDeterminer _classEffectDeterminer = new ();

    public TeamClassInformationCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        var teamClassInformation = _context.TeamClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        loadPlayer.ClassIdTeam = teamClassInformation.ClassId;
        loadPlayer.ClassChangeStatusTeam = teamClassInformation.ClassChangeStatus;
        
        loadPlayer.TopPointRankNumTeam = teamClassInformation.TopPointRankEntryCount;
        loadPlayer.RateTeam = teamClassInformation.Rate;
        
        if (teamClassInformation.ClassId < 4)
        {
            return;
        }
        
        var teamRank = _context.TeamOverRankViews
            .FirstOrDefault(rank => rank.CardId == cardProfile.Id && rank.Rank <= 1000);

        if (teamRank is not null)
        {
            loadPlayer.TopPointRankTeam = teamRank.Rank;
            _classEffectDeterminer.Determine(4, teamRank.Rank);
        }
    }
}

public class IbTeamClassInformationCommand(ServerDbContext context, CustomConfigs customConfigs)
    : IPreLoadCard2Command
{
    public void Fill(CardProfile cardProfile, Response2.PreLoadCard preLoadCard)
    {
        var cardId = cardProfile.Id;
        var loadPlayer = preLoadCard.load_player;
        if (loadPlayer == null) return;
        if (customConfigs.ClassMatchG == null) return;

        if (!customConfigs.ClassMatchG.EnableRate)
        {
            loadPlayer.ClassIdTeam = customConfigs.ClassMatchG.CustomTeam.ClassId;

            var g = customConfigs.ClassMatchG.CustomTeam.GradeId;
            if (g is not (0u or 1u or 3u or 11u)) g = GlobalVars.ClassRankGrade;
            loadPlayer.GradeTeam = g;

            SetTopPoint(loadPlayer);

            return;
        }

        var classInformation = context.TeamClassMatchGRecordDbSet.FirstOrDefault(x => x.CardId == cardId);
        Ensure(ref classInformation!, cardId);
        loadPlayer.ClassIdTeam = classInformation.ClassId;
        loadPlayer.GradeTeam = classInformation.GradeId;
        loadPlayer.RateTeam = classInformation.Rate;
        loadPlayer.grade_1_team = classInformation.Grade_1;
        loadPlayer.VsmAfterClassChangeTeam = classInformation.ClassChangeStatus;
        loadPlayer.TopPointRankTeam = classInformation.TopPointRank;
        loadPlayer.TopPointRankNumTeam = classInformation.TopPointRankEntryCount;
        SetTopPoint(loadPlayer);
    }

    static void Ensure(ref TeamClassMatchGRecord classInformation, int cardId)
    {
        if (classInformation != null) return;
        classInformation = TeamClassMatchGRecord.New(cardId);
    }

    void SetTopPoint(Response2.PreLoadCard.LoadPlayer loadPlayer)
    {
        var exRankNum = 0u;
        if (customConfigs.ClassMatchG.CustomTeam.EnableRndNumOnExx)
        {
            var i = Random.Shared.Next(0, GlobalVars.ExRankNums.Length);
            exRankNum = (uint)GlobalVars.ExRankNums[i];
        }
        loadPlayer.TopPointRankTeam = exRankNum;
        loadPlayer.TopPointRankNumTeam = 0;
    }
}