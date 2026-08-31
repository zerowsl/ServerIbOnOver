#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace nue.protocol.exvs;

public partial class Response2
{
    // 1401
    [global::ProtoBuf.ProtoContract()]
    public partial class LoadRanking : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"rank_type")]
        [global::System.ComponentModel.DefaultValue(RankMessageType.MsMonthlyCountryPlayerNaviPoint)]
        public RankMessageType RankType
        {
            get => __pbn__RankType ?? RankMessageType.MsMonthlyCountryPlayerNaviPoint;
            set => __pbn__RankType = value;
        }
        public bool ShouldSerializeRankType() => __pbn__RankType != null;
        public void ResetRankType() => __pbn__RankType = null;
        private RankMessageType? __pbn__RankType;

        [global::ProtoBuf.ProtoMember(2, Name = @"timestamp")]
        public ulong Timestamp
        {
            get => __pbn__Timestamp.GetValueOrDefault();
            set => __pbn__Timestamp = value;
        }
        public bool ShouldSerializeTimestamp() => __pbn__Timestamp != null;
        public void ResetTimestamp() => __pbn__Timestamp = null;
        private ulong? __pbn__Timestamp;

        [global::ProtoBuf.ProtoMember(3, Name = @"weekly_rank_no")]
        public uint WeeklyRankNo
        {
            get => __pbn__WeeklyRankNo.GetValueOrDefault();
            set => __pbn__WeeklyRankNo = value;
        }
        public bool ShouldSerializeWeeklyRankNo() => __pbn__WeeklyRankNo != null;
        public void ResetWeeklyRankNo() => __pbn__WeeklyRankNo = null;
        private uint? __pbn__WeeklyRankNo;

        [global::ProtoBuf.ProtoMember(4, Name = @"weekly_rank_course_id")]
        public uint WeeklyRankCourseId
        {
            get => __pbn__WeeklyRankCourseId.GetValueOrDefault();
            set => __pbn__WeeklyRankCourseId = value;
        }
        public bool ShouldSerializeWeeklyRankCourseId() => __pbn__WeeklyRankCourseId != null;
        public void ResetWeeklyRankCourseId() => __pbn__WeeklyRankCourseId = null;
        private uint? __pbn__WeeklyRankCourseId;

        [global::ProtoBuf.ProtoMember(5, Name = @"ms_used_rank")]
        public MsUsedRank MsUsedRank { get; set; }

        [global::ProtoBuf.ProtoMember(11, Name = @"triad_score_rank")]
        public TriadScoreRank TriadScoreRank { get; set; }

        [global::ProtoBuf.ProtoMember(12, Name = @"triad_course_time_rank")]
        public TriadCourseTimeRank TriadCourseTimeRank { get; set; }

        [global::ProtoBuf.ProtoMember(13, Name = @"triad_course_score_rank")]
        public TriadCourseScoreRank TriadCourseScoreRank { get; set; }

        [global::ProtoBuf.ProtoMember(14, Name = @"triad_bounty_rank")]
        public TriadBountyRank TriadBountyRank { get; set; }

        [global::ProtoBuf.ProtoMember(15, Name = @"triad_boss_rank")]
        public TriadBossRank TriadBossRank { get; set; }

        [global::ProtoBuf.ProtoMember(16, Name = @"player_score_rank")]
        public PlayerScoreRank2 PlayerScoreRank { get; set; }

        [global::ProtoBuf.ProtoMember(17, Name = @"tag_score_rank")]
        public TagScoreRank TagScoreRank { get; set; }

        [global::ProtoBuf.ProtoMember(18, Name = @"class_match_score")]
        public ClassMatchScore2 ClassMatchScore { get; set; }

        [global::ProtoBuf.ProtoMember(19, Name = @"tag_class_match_score")]
        public TagClassMatchScore TagClassMatchScore { get; set; }

        [global::ProtoBuf.ProtoMember(20, Name = @"top_point_solo_rank")]
        public TopPointSoloRank TopPointSoloRank { get; set; }

        [global::ProtoBuf.ProtoMember(21, Name = @"top_point_team_rank")]
        public TopPointTeamRank TopPointTeamRank { get; set; }
        
        //[global::ProtoBuf.ProtoMember(, Name = @"route_battle_score_rank")]
        //public nue.protocol.exvs.RouteBattleScoreRank RouteBattleScoreRank { get; set; }
        //[global::ProtoBuf.ProtoMember(, Name = @"route_battle_time_rank")]
        //public nue.protocol.exvs.RouteBattleTimeRank RouteBattleTimeRank { get; set; }
        //[global::ProtoBuf.ProtoMember(, Name = @"group_point_rank")]
        //public nue.protocol.exvs.GroupPointRank GroupPointRank { get; set; }
        
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
