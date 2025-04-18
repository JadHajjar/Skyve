﻿namespace Skyve.Compatibility.Domain.Enums;

public enum PackageStability
{
	[CRN(NotificationType.None)] Stable = 1,
	[CRN(NotificationType.Info)] NotEnoughInformation = 2,
	[CRN(NotificationType.Warning)] HasIssues = 3,
	[CRN(NotificationType.Unsubscribe)] Broken = 4,
	[CRN(NotificationType.Exclude)] BrokenFromPatch = 5,
	[CRN(NotificationType.Exclude)] BrokenFromNewVersion = 10,
	[CRN(NotificationType.None)] StableNoNewFeatures = 6,
	[CRN(NotificationType.None)] StableNoFutureUpdates = 7,
	[CRN(NotificationType.Warning)] HasIssuesNoFutureUpdates = 8,
	[CRN(NotificationType.Caution)] BreaksOnPatch = 9,
	[CRN(NotificationType.Caution)] NumerousReports = 11,

	[CRN(NotificationType.Info, false)] NotReviewed = 0,
	[CRN(NotificationType.Warning, false)] Incompatible = 99,
	[CRN(NotificationType.Info, false)] AssetNotReviewed = 98,
	[CRN(NotificationType.Caution, false)] Local = 97,
	[CRN(NotificationType.Caution, false)] AuthorRetired = 96,
	[CRN(NotificationType.Info, false)] BrokenFromPatchSafe = 95,
	[CRN(NotificationType.Info, false)] AssetIncompatible = 94,
	[CRN(NotificationType.Warning, false)] BrokenFromPatchUpdated = 93,
	[CRN(NotificationType.Warning, false)] BrokenFromNewVersionSafe = 92,
}
