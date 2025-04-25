namespace Skyve.Compatibility.Domain.Enums;

public enum StatusType
{
	[CRN(NotificationType.None, false)]
	None = 0,

	[CRN(NotificationType.Obsolete, [StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.Switch, StatusAction.IncludeOther, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	Deprecated = 1,

	[CRN(NotificationType.Warning, [StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.Switch, StatusAction.IncludeOther, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	Reupload = 2,

	[CRN(NotificationType.Warning, [StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.DisableThis, StatusAction.RequiresConfiguration, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	CausesIssues = 3,

	[CRN(NotificationType.AttentionRequired, [StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.DisableThis, StatusAction.Switch, StatusAction.RequiresConfiguration, StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	SavesCantLoadWithoutIt = 4,

	[CRN(NotificationType.Info, [StatusAction.NoAction, StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.DisableThis, StatusAction.Switch], AllowedChange = CRNAttribute.ChangeType.Deny)]
	TestVersion = 5,

	[CRN(NotificationType.None, [StatusAction.NoAction, StatusAction.ExcludeThis, StatusAction.DisableThis, StatusAction.UnsubscribeThis], AllowedChange = CRNAttribute.ChangeType.Deny)]
	DependencyMod = 6,

	[CRN(NotificationType.Caution, false)]
	SourceCodeNotAvailable = 7,

	[CRN(NotificationType.Caution, [StatusAction.NoAction])]
	MusicCanBeCopyrighted = 8,

	[CRN(NotificationType.Caution, false)]
	IncompleteDescription = 9,

	[CRN(NotificationType.None, [StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	SourceAvailable = 10,

	[CRN(NotificationType.None, [StatusAction.NoAction], AllowedChange = CRNAttribute.ChangeType.Deny)]
	StandardMod = 11,

	/********************************/

	[CRN(NotificationType.MissingDependency, false)]
	MissingDlc = 1000,

	[CRN(NotificationType.Obsolete, false)]
	Succeeded = 1001,

	[CRN(NotificationType.Info, false)]
	AutoDeprecated = 1002,
}
