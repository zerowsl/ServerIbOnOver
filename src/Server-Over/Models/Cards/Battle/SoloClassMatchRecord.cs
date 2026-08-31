using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Obsolete("ob")]
[Table("exvs2ob_battle_solo_class_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class SoloClassMatchRecord : BaseClassMatchRecord
{
}

[Table("djyx_ib_battle_solo_class_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class SoloClassMatchGRecord : BaseClassMatchGRecord
{
    public static SoloClassMatchGRecord New(int cardId)
    {
        var classInformation = new SoloClassMatchGRecord();
        classInformation.CardId = cardId;
        classInformation.ClassId = 1;
        classInformation.GradeId = 1; // Grade 10
        classInformation.Rate = GlobalVars.MinClassGradeMinRate1;
        return classInformation;
    }
}