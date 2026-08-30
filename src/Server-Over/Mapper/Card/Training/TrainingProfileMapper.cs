using Riok.Mapperly.Abstractions;
using WebUIOver.Shared.Dto.Training;

namespace ServerOver.Mapper.Card.Training;

[Mapper]
public static partial class TrainingProfileMapper
{
	[MapperIgnoreTarget(nameof(Models.Cards.Profile.TrainingProfile.CpuHpAutoRegen))]
	[MapperIgnoreTarget(nameof(Models.Cards.Profile.TrainingProfile.ExOverLimit))]
	[MapperIgnoreTarget(nameof(Models.Cards.Profile.TrainingProfile.QuickFill))]
	private static partial TrainingProfile ToTrainingProfile0(Models.Cards.Profile.TrainingProfile trainingProfile);
	
	[UserMapping(Default = true)]
    public static TrainingProfile ToTrainingProfile(this Models.Cards.Profile.TrainingProfile trainingProfile)
	{
		// before mapping
		var r = ToTrainingProfile0(trainingProfile); 
		// after mapping 
		r.CpuHpAutoRegen = trainingProfile.CpuHpAutoRegen > 0;
		r.ExOverLimit = trainingProfile.ExOverLimit > 0;
		r.QuickFill = trainingProfile.QuickFill > 0;
		return r;
	}
}