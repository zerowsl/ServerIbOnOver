#pragma warning disable CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192
namespace nue.protocol.exvs;

[global::ProtoBuf.ProtoContract()]
public partial class PlayerScoreRank2 : global::ProtoBuf.IExtensible
{
    private global::ProtoBuf.IExtension __pbn__extensionData;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

    [global::ProtoBuf.ProtoMember(1, Name = @"sum_start", IsRequired = true)]
    public ulong SumStart { get; set; }

    [global::ProtoBuf.ProtoMember(2, Name = @"sum_end", IsRequired = true)]
    public ulong SumEnd { get; set; }

    [global::ProtoBuf.ProtoMember(3, Name = @"rows")]
    public global::System.Collections.Generic.List<Row> Rows { get; } = new();

    [global::ProtoBuf.ProtoContract()]
    public partial class Row : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"rank_no", IsRequired = true)]
        public uint RankNo { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"prev_rank_no", IsRequired = true)]
        public uint PrevRankNo { get; set; }

        [global::ProtoBuf.ProtoMember(3, Name = @"pilot_id", IsRequired = true)]
        public uint PilotId { get; set; }

        [global::ProtoBuf.ProtoMember(4, Name = @"player_name", IsRequired = true)]
        public string PlayerName { get; set; }

        [global::ProtoBuf.ProtoMember(5, Name = @"score", IsRequired = true)]
        public uint Score { get; set; }

        [global::ProtoBuf.ProtoMember(6, Name = @"player_level_id", IsRequired = true)]
        public uint PlayerLevelId { get; set; }

        [global::ProtoBuf.ProtoMember(7, Name = @"badge_id", IsRequired = true)]
        public uint BadgeId { get; set; }
        
        [global::ProtoBuf.ProtoMember(8, Name = @"open_player_level", IsRequired = true)]
        public uint OpenPlayerLevel { get; set; }
        
        [global::ProtoBuf.ProtoMember(9, Name = @"title_text_id", IsRequired = true)] // 8
        public uint TitleTextId { get; set; }

        //[global::ProtoBuf.ProtoMember(9, Name = @"title_ornament_id", IsRequired = true)]
        //public uint TitleOrnamentId { get; set; }
        //[global::ProtoBuf.ProtoMember(10, Name = @"title_effect_id", IsRequired = true)]
        //public uint TitleEffectId { get; set; }
        //[global::ProtoBuf.ProtoMember(11, Name = @"title_background_parts_id", IsRequired = true)]
        //public uint TitleBackgroundPartsId { get; set; }

        [global::ProtoBuf.ProtoMember(10, Name = @"custom_txt")] // 12
        [global::System.ComponentModel.DefaultValue("")]
        public string CustomTxt
        {
            get => __pbn__CustomTxt ?? "";
            set => __pbn__CustomTxt = value;
        }
        public bool ShouldSerializeCustomTxt() => __pbn__CustomTxt != null;
        public void ResetCustomTxt() => __pbn__CustomTxt = null;
        private string __pbn__CustomTxt;
        
        [global::ProtoBuf.ProtoMember(11, Name = @"group_name", IsRequired = true)]
        public string GroupName { get; set; }
        
        [global::ProtoBuf.ProtoMember(12, Name = @"fav_ms_id", IsRequired = true)]
        public uint FavMsId { get; set; }

        [global::ProtoBuf.ProtoMember(13, Name = @"ms_used_num", IsRequired = true)]
        public uint MsUsedNum { get; set; }

        [global::ProtoBuf.ProtoMember(14, Name = @"home_loc_name", IsRequired = true)]
        public string HomeLocName { get; set; }

        [global::ProtoBuf.ProtoMember(15, Name = @"home_loc_pref", IsRequired = true)]
        public uint HomeLocPref { get; set; }

        [global::ProtoBuf.ProtoMember(16, Name = @"open_record", IsRequired = true)]
        public uint OpenRecord { get; set; }

        [global::ProtoBuf.ProtoMember(17, Name = @"play_num", IsRequired = true)]
        public uint PlayNum { get; set; }

        [global::ProtoBuf.ProtoMember(18, Name = @"win_num", IsRequired = true)]
        public uint WinNum { get; set; }

        [global::ProtoBuf.ProtoMember(19, Name = @"class_id_solo", IsRequired = true)]
        public uint ClassIdSolo { get; set; }

        [global::ProtoBuf.ProtoMember(20, Name = @"class_id_team", IsRequired = true)]
        public uint ClassIdTeam { get; set; }

        [global::ProtoBuf.ProtoMember(21, Name = @"skin_id", IsRequired = true)]
        public uint SkinId { get; set; }

        [global::ProtoBuf.ProtoMember(22, Name = @"top_point_rank_num_solo", IsRequired = true)]
        public uint TopPointRankNumSolo { get; set; }
        [global::ProtoBuf.ProtoMember(23, Name = @"top_point_rank_num_team", IsRequired = true)]
        public uint TopPointRankNumTeam { get; set; }

        //--[global::ProtoBuf.ProtoMember(25, Name = @"name_color_id", IsRequired = true)] // 25 
        //--public uint NameColorId { get; set; }
        
        [global::ProtoBuf.ProtoMember(24, Name = @"grade_solo", IsRequired = true)]
        public uint GradeSolo { get; set; }
        [global::ProtoBuf.ProtoMember(25, Name = @"grade_team", IsRequired = true)]
        public uint GradeTeam { get; set; }
        
        [global::ProtoBuf.ProtoMember(26, Name = @"top_point_rank_solo", IsRequired = true)]
        public uint TopPointRankSolo { get; set; }
        [global::ProtoBuf.ProtoMember(27, Name = @"top_point_rank_team", IsRequired = true)]
        public uint TopPointRankTeam { get; set; }
        
    }

}

#pragma warning restore CS0612, CS0618, CS1591, CS3021, IDE0079, IDE1006, RCS1036, RCS1057, RCS1085, RCS1192