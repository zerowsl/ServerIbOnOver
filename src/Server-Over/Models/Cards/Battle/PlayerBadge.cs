using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Table("djyx_ib_battle_player_badge")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class PlayerBadge : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }
    
    [Required]
    public uint CardId { get; set; }

    [Required]
    public uint BadgeId { get; set; } = 0;

    [Required]
    public int BadgeExp { get; set; } = 0;

}