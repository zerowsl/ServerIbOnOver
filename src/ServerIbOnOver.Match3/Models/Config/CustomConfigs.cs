namespace ServerOver.Models.Config;

public sealed class CustomConfigs
{
    public MatchRoomConfigs? MatchRoom { get; set; }

    public LocalMatchingConfigs LocalMatchingConfigs { get; set; } = new();

    public sealed class MatchRoomConfigs
    {
        public bool Enable { get; set; } = false;
        
        public uint SoloWorldId { get; set; } = 1013;
        public uint TeamWorldId { get; set; } = 1014;
    }
}