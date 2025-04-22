namespace Skyve.Compatibility.Domain.Enums;

public enum SavegameEffect
{
	[CRN(NotificationType.Info)]
	Unknown = 0,
	[CRN(NotificationType.None)]
	None = 1,
	[CRN(NotificationType.Info)]
	AssetsRemain = 2,
	[CRN(NotificationType.Caution)]
	EffectsLinger = 3,
	[CRN(NotificationType.AttentionRequired)]
	RequiresManualAction = 4,
	[CRN(NotificationType.Warning)]
	BreaksSave = 5
}