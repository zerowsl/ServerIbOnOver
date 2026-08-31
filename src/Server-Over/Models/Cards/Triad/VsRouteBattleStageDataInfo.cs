using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerOver.Models.Cards.Triad;

[Table("djyx_ib_VsRouteBattleStageDataInfo")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class VsRouteBattleStageDataInfo : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }

    [Required]
    public uint CardId { get; set; }

    [Required]
    public uint PatternId { get; set; }

    [Required]
    public uint Difficulty { get; set; }

    [Required]
    public uint StageId { get; set; }

    [Required]
    public uint TeamCostMax { get; set; }
    [Required]
    public uint TeamCostCurr { get; set; }

    [Required]
    public uint MobileSuitId { get; set; }

    public string? Chips { get; set; }

    public string? BossIds { get; set; }
}