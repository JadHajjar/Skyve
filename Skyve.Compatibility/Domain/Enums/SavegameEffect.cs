namespace Skyve.Compatibility.Domain.Enums;

public enum SavegameEffect
{
	[CRN(NotificationType.Info)]
	None,
	[CRN(NotificationType.Info)]
	AssetsRemain,
	[CRN(NotificationType.Caution)]
	EffectsLinger,
	[CRN(NotificationType.AttentionRequired)]
	RequiresManualAction,
	[CRN(NotificationType.Warning)]
	BreaksSave
}