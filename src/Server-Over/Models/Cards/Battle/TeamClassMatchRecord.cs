using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ServerOver.Models.Cards.Battle;

[Obsolete("ob")]
[Table("exvs2ob_battle_team_class_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TeamClassMatchRecord : BaseClassMatchRecord
{
}

[Table("djyx_ib_battle_team_class_record")]
[Index(nameof(Id))]
[Index(nameof(CardId))]
public class TeamClassMatchGRecord : BaseClassMatchGRecord
{
    public static TeamClassMatchGRecord New(int cardId)
    {
        var classInformation = new TeamClassMatchGRecord();
        classInformation.CardId = cardId;
        classInformation.ClassId = 2;
        classInformation.GradeId = 1; // Grade 10
        classInformation.Rate = GlobalVars.MinClassGradeMinRate2;
        return classInformation;
    }
}