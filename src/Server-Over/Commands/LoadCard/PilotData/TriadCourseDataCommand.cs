using nue.protocol.exvs;
using ServerOver.Mapper.Card.Triad;
using ServerOver.Models.Cards;
using ServerOver.Persistence;

namespace ServerOver.Commands.LoadCard.PilotData;

public class TriadCourseDataCommand : IPilotDataCommand
{
    private readonly ServerDbContext _context;

    public TriadCourseDataCommand(ServerDbContext context)
    {
        _context = context;
    }

    public void Fill(CardProfile cardProfile, Response.LoadCard.PilotDataGroup pilotDataGroup)
    {
        _context.TriadCourseDataDbSet
            .Where(x => x.CardProfile == cardProfile)
            .ToList()
            .ForEach(triadCourseData => pilotDataGroup.CpuScenes.Add(triadCourseData.ToCpuSceneData()));

        if (pilotDataGroup.CpuScenes.Count == 0)
        {
            pilotDataGroup.CpuScenes.Add(new() 
            {
                CourseId = 0,
                ReleasedAt = (ulong)DateTimeOffset.Now.ToUnixTimeSeconds(),
                TotalPlayNum = 0,
                TotalClearNum = 0,
                Highscore = 0
            });
        }
    }
}