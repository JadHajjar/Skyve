namespace Skyve.Compatibility.Domain.Enums;

public enum InteractionType
{
	[CRN(NotificationType.None, false)]
	None = 0,

	[CRN(NotificationType.Switch, [StatusAction.Switch, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	Successor = 1,

	[CRN(NotificationType.None, [StatusAction.SelectOne, StatusAction.Switch, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	Alternative = 2,

	[CRN(NotificationType.Caution, [StatusAction.SelectOne, StatusAction.Switch, StatusAction.UnsubscribeThis, StatusAction.UnsubscribeOther, StatusAction.ExcludeThis, StatusAction.ExcludeOther, StatusAction.RequiresConfiguration, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	SameFunctionality = 3,

	[CRN(NotificationType.AttentionRequired, [StatusAction.SelectOne, StatusAction.Switch, StatusAction.UnsubscribeThis, StatusAction.UnsubscribeOther, StatusAction.ExcludeThis, StatusAction.ExcludeOther, StatusAction.RequiresConfiguration, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.CanAdd)]
	IncompatibleWith = 4,

	[CRN(NotificationType.Info, [StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	RequirementAlternative = 5,

	[CRN(NotificationType.Caution, [StatusAction.SelectOne, StatusAction.Switch, StatusAction.UnsubscribeThis, StatusAction.UnsubscribeOther, StatusAction.ExcludeThis, StatusAction.ExcludeOther, StatusAction.RequiresConfiguration, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.CanAdd)]
	CausesIssuesWith = 6,

	[CRN(NotificationType.MissingDependency, [StatusAction.SubscribeToPackages, StatusAction.NoAction])]
	RequiredPackages = 7,

	[CRN(NotificationType.None, [StatusAction.SubscribeToPackages, StatusAction.NoAction])]
	OptionalPackages = 8,

	[CRN(NotificationType.None, [StatusAction.NoAction])]
	LoadAfter = 9,

	/***********************************/

	[CRN(NotificationType.RequiredItem, false)]
	RequiredItem = 9999,

	[CRN(NotificationType.Info, false)]
	SucceededBy = 1000,

	[CRN(NotificationType.AttentionRequired, false)]
	Identical = 1001,
}
