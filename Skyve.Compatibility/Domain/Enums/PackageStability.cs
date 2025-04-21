namespace Skyve.Compatibility.Domain.Enums;

public enum PackageStability
{
	[CRN(NotificationType.None)] Stable = 1,
	[CRN(NotificationType.Info)] NotEnoughInformation = 2,
	[CRN(NotificationType.AttentionRequired)] HasIssues = 3,
	[CRN(NotificationType.Broken)] Broken = 4,
	[CRN(NotificationType.Broken)] BrokenFromPatch = 5,
	[CRN(NotificationType.Broken)] BrokenFromNewVersion = 10,
	[CRN(NotificationType.Info)] StableNoNewFeatures = 6,
	[CRN(NotificationType.Info)] StableNoFutureUpdates = 7,
	[CRN(NotificationType.AttentionRequired)] HasIssuesNoFutureUpdates = 8,
	[CRN(NotificationType.Caution)] BreaksOnPatch = 9,
	[CRN(NotificationType.Caution)] NumerousReports = 11,
	[CRN(NotificationType.Obsolete)] Obsolete = 12,

	[CRN(NotificationType.Info, false)] NotReviewed = 0,
	[CRN(NotificationType.None, false)] ReiewRequest = 100,
	[CRN(NotificationType.Warning, false)] Incompatible = 99,
	[CRN(NotificationType.Info, false)] AssetNotReviewed = 98,
	[CRN(NotificationType.Caution, false)] Local = 97,
	[CRN(NotificationType.Caution, false)] AuthorRetired = 96,
	[CRN(NotificationType.Info, false)] BrokenFromPatchSafe = 95,
	[CRN(NotificationType.Info, false)] AssetIncompatible = 94,
	[CRN(NotificationType.Warning, false)] BrokenFromPatchUpdated = 93,
	[CRN(NotificationType.Warning, false)] BrokenFromNewVersionSafe = 92,
	[CRN(NotificationType.AttentionRequired, false)] AuthorMalicious = 91,
}
