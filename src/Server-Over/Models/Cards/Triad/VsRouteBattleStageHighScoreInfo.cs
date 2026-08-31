using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerOver.Models.Cards.Triad;

[Table("djyx_ib_VsRouteBattleStageHighScoreInfo")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class VsRouteBattleStageHighScoreInfo : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public int CardId { get; set; }

    [Required]
    public uint Difficulty { get; set; }

    public uint ClearTime1 { get; set; }
    public uint ClearScore1 { get; set; }
    public uint ClearTime2 { get; set; }
    public uint ClearScore2 { get; set; }
    public uint ClearTime3 { get; set; }
    public uint ClearScore3 { get; set; }
    public uint TotalClearTime { get; set; }
    public uint TotalClearScore { get; set; }

    public bool IsRecord { get; set; }
}