using nue.protocol.exvs;
using ServerOver.Models.Cards;
using ServerOver.Models.Cards.Battle;
using ServerOver.Models.Config;
using ServerOver.Persistence;
using ServerOver.Processor.Class.Effect;

namespace ServerOver.Commands.PreLoadCard.LoadPlayer;

[Obsolete("ob")]
public class SoloClassInformationCommand : IPreLoadPlayerCommand
{
    private readonly ServerDbContext _context;
    private readonly ClassEffectDeterminer _classEffectDeterminer = new ();

    public SoloClassInformationCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.PreLoadCard.LoadPlayer loadPlayer)
    {
        var soloClassInformation = _context.SoloClassMatchRecordDbSet
            .First(x => x.CardProfile == cardProfile);

        loadPlayer.ClassIdSolo = soloClassInformation.ClassId;
        loadPlayer.ClassChangeStatusSolo = soloClassInformation.ClassChangeStatus;

        loadPlayer.TopPointRankNumSolo = soloClassInformation.TopPointRankEntryCount;
        loadPlayer.RateSolo = soloClassInformation.Rate;

        if (soloClassInformation.ClassId < 4)
        {
            return;
        }
        
        var soloRank = _context.SoloOverRankViews
            .FirstOrDefault(rank => rank.CardId == cardProfile.Id && rank.Rank <= 1000);

        if (soloRank is not null)
        {
            loadPlayer.TopPointRankSolo = soloRank.Rank;
            _classEffectDeterminer.Determine(4, soloRank.Rank);
        }
    }
}

public class IbSoloClassInformationCommand(ServerDbContext context, CustomConfigs customConfigs) 
    : BasePreLoadCard2Command
{
    public override void Fill(CardProfile cardProfile, Response2.PreLoadCard.LoadPlayer loadPlayer)
    {
        var cardId = cardProfile.Id;
        if (customConfigs.ClassMatchG == null) return;
		
        if (!customConfigs.ClassMatchG.EnableRate)
        {
            loadPlayer.ClassIdSolo = customConfigs.ClassMatchG.CustomSolo.ClassId;

            var g = customConfigs.ClassMatchG.CustomSolo.GradeId;
            if (g is not (0u or 1u or 3u or 11u)) g = GlobalVars.ClassRankGrade;
            loadPlayer.GradeSolo = g;

            SetTopPoint(loadPlayer);

            return;
        }

        var classInformation = context.SoloClassMatchGRecordDbSet.FirstOrDefault(x => x.CardId == cardId);
        Ensure(ref classInformation!, cardId);
        loadPlayer.ClassIdSolo = classInformation.ClassId;
        loadPlayer.GradeSolo = classInformation.GradeId;
        loadPlayer.RateSolo = classInformation.Rate;
        loadPlayer.grade_1_solo = classInformation.Grade_1;
        loadPlayer.VsmAfterClassChangeSolo = classInformation.ClassChangeStatus;
        loadPlayer.TopPointRankSolo = classInformation.TopPointRank;
        loadPlayer.TopPointRankNumSolo = classInformation.TopPointRankEntryCount;
        SetTopPoint(loadPlayer);
    }

    static void Ensure(ref SoloClassMatchGRecord classInformation, int cardId)
    {
        if (classInformation != null) return;
        classInformation = SoloClassMatchGRecord.New(cardId);
    }

    void SetTopPoint(Response2.PreLoadCard.LoadPlayer loadPlayer)
    {
        var exRankNum = 0u;
        if (customConfigs.ClassMatchG.CustomSolo.EnableRndNumOnExx)
        {
            var i = Random.Shared.Next(0, GlobalVars.ExRankNums.Length);
            exRankNum = (uint)GlobalVars.ExRankNums[i];
        }
        loadPlayer.TopPointRankSolo = exRankNum;
        loadPlayer.TopPointRankNumSolo = 0;
    }
}