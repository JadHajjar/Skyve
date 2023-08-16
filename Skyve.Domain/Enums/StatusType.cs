namespace Skyve.Domain.Enums;

public enum StatusType
{
	[CRN(NotificationType.None, false)]
	None = 0,

	[CRN(NotificationType.Caution, new[] { StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.Switch, StatusAction.SubscribeToPackages, StatusAction.NoAction }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	Deprecated = 1,

	[CRN(NotificationType.Warning, new[] { StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.Switch, StatusAction.SubscribeToPackages, StatusAction.NoAction }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	Reupload = 2,

	[CRN(NotificationType.Warning, new[] { StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.RequiresConfiguration, StatusAction.NoAction }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	CausesIssues = 3,

	[CRN(NotificationType.Warning, new[] { StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.Switch, StatusAction.RequiresConfiguration, StatusAction.NoAction }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	SavesCantLoadWithoutIt = 4,

	[CRN(NotificationType.Info, new[] { StatusAction.NoAction, StatusAction.UnsubscribeThis, StatusAction.ExcludeThis, StatusAction.Switch }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	TestVersion = 5,

	[CRN(NotificationType.None, new[] { StatusAction.NoAction }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	DependencyMod = 6,

	[CRN(NotificationType.Warning, false)]
	SourceCodeNotAvailable = 7,

	[CRN(NotificationType.Caution, new[] { StatusAction.NoAction })]
	MusicCanBeCopyrighted = 8,

	[CRN(NotificationType.Caution, false)]
	IncompleteDescription = 9,

	[CRN(NotificationType.None, new[] { StatusAction.NoAction }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	SourceAvailable = 10,

	[CRN(NotificationType.None, new[] { StatusAction.NoAction }, AllowedChange = CRNAttribute.ChangeType.Deny)]
	StandardMod = 11,

	/********************************/

	[CRN(NotificationType.MissingDependency, false)]
	MissingDlc = 1000,

	[CRN(NotificationType.Unsubscribe, false)]
	Succeeded = 1001,

	[CRN(NotificationType.Info, false)]
	AutoDeprecated = 1002,
}
