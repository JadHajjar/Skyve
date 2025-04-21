namespace Skyve.Compatibility.Domain.Enums;

public enum StatusAction
{
	[CRN(NotificationType.None)]
	NoAction = 0,

	[CRN(NotificationType.None)]
	SubscribeToPackages = 1,

	[CRN(NotificationType.AttentionRequired)]
	RequiresConfiguration = 2,

	[CRN(NotificationType.None)]
	SelectOne = 3,

	[CRN(NotificationType.ActionRequired)]
	UnsubscribeThis = 4,

	[CRN(NotificationType.ActionRequired)]
	UnsubscribeOther = 5,

	[CRN(NotificationType.Obsolete)]
	Switch = 6,

	[CRN(NotificationType.ActionRequired)]
	ExcludeThis = 7,

	[CRN(NotificationType.ActionRequired)]
	ExcludeOther = 8,

	[CRN(NotificationType.RequiredItem)]
	IncludeThis = 9,

	[CRN(NotificationType.Info)]
	IncludeOther = 10,

	[CRN(NotificationType.Caution, false)]
	UpdateVersion = 11,

	[CRN(NotificationType.None, false)]
	DoNotAdd = 97,

	[CRN(NotificationType.None, false)]
	MarkAsRead = 98,

	[CRN(NotificationType.None, false)]
	RequestReview = 99,
}
