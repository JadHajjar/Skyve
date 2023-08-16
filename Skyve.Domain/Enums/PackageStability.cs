namespace Skyve.Domain.Enums;

public enum PackageStability
{
	[CRN(NotificationType.None)] Stable = 1,
	[CRN(NotificationType.Info)] NotEnoughInformation = 2,
	[CRN(NotificationType.Warning)] HasIssues = 3,
	[CRN(NotificationType.Unsubscribe)] Broken = 4,
	[CRN(NotificationType.Exclude)] BrokenFromPatch = 5,

	[CRN(NotificationType.Unsubscribe, false)] Incompatible = 99,
	[CRN(NotificationType.Info, false)] AssetNotReviewed = 98,
	[CRN(NotificationType.Caution, false)] Local = 97,
	[CRN(NotificationType.Caution, false)] AuthorRetired = 96,
	[CRN(NotificationType.Info, false)] NotReviewed = 0,
}
