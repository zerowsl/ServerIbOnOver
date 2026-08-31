using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerOver.Models.Cards.Profile;

[Table("djyx_ib_quick_start_info")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class QuickStartInfoProfile : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; }

    [Required]
    public uint CardId { get; set; }
    
    // -1=Close 0=Normal 1=SameGameMode 2=SetOnly
    public int SaveMode { get; set; }
    
    public uint GameMode { get; set; }
    
    public uint RuleType { get; set; }
    public uint TeamType { get; set; }
    
    [Required]
    public uint MstMobileSuitId { get; set; }            
    
    public uint BurstType { get; set; }
                
    public uint PartnerMobileSuitId { get; set; }
    public uint PartnerBurstType { get; set; }
    
    public uint BattleStageId { get; set; }
}