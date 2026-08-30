namespace ServerOver.Models.Config;

public sealed class CustomConfigs
{
    public int MaxFavouriteMs { get; set; } = 12;
    public uint PLvBoostPointBonus { get; set; }
    public float GpBoostRate { get; set; }

    public bool RecordCpusToBattleHistory { get; set; } = false;

    public uint[]? p36_stages { get; set; } = null;
    
    public ClassMatchGConfigs ClassMatchG { get; set; } = new();

    public RoomConfigs? Room { get; set; }

    public RemoteCardServerConfigs RemoteCardServerConfigs { get; set; } = new();
    public LocalMatchingConfigs LocalMatchingConfigs { get; set; } = new();

    public sealed class ClassMatchGConfigs
    {
        public bool EnableRate { get; set; } = false;

        public uint ConstantRate { get; set; } = 10;
        public uint WinScoreCoefficient { get; set; } = 25000;
        public uint InitialRate { get; set; } = 1500;

        public uint RateDiff { get; set; }
        
        public GConfigs CustomSolo { get; set; } = new();
        public GConfigs CustomTeam { get; set; } = new();
        
        public class GConfigs
        {
            public uint ClassId { get; set; }
            public uint GradeId { get; set; }
            public bool EnableRndNumOnExx { get; set; } = true;
        }
    }
    
    public sealed class RoomConfigs
    {
        public bool Enable { get; set; } = false;
        
        public string Code { get; set; } = "";
        public uint TagType { get; set; }
        public uint MatchingType { get; set; }
        public uint MatchingAttribute { get; set; } = 0;
        public uint[] SelectableMsIds { get; set; } = [];
        public uint RuleType { get; set; } = 0;
        public bool RevengeFlag { get; set; }
        public uint Timer { get; set; } = 0;
        public uint WorldId { get; set; } = 13;
        public uint FesRuleType { get; set; } = 0;
    }
}