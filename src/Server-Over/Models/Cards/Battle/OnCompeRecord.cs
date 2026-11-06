using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

public class BaseOnCompeRecord : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }

    [Required]
    public int CardId { get; set; }

    [Required]
    public uint RoomId { get; set; } = 0;

    [Required]
    public uint Point { get; set; } = 0;

    [Required]
    public uint Rank { get; set; } = 0;
    
    [Required]
    public bool IsOutdated { get; set; } = false;
}

[Table("djyx_ib_battle_solo_oncompe_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class SoloOnCompeRecord : BaseOnCompeRecord
{
    public static SoloOnCompeRecord New(int cardId)
    {
        var r = new SoloOnCompeRecord();
        r.CardId = cardId;
        r.Point = GlobalVars.DefaultOnCompePoint;
        r.Rank = GlobalVars.DefaultOnCompeRank;
        return r;
    }
}

[Table("djyx_ib_battle_team_oncompe_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TeamOnCompeRecord : BaseOnCompeRecord
{
    public static TeamOnCompeRecord New(int cardId)
    {
        var r = new TeamOnCompeRecord();
        r.CardId = cardId;
        r.Point = GlobalVars.DefaultOnCompePoint;
        r.Rank = GlobalVars.DefaultOnCompeRank;
        return r;
    }
}
