using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Skyve.Domain;

public interface IPlayset : IThumbnailObject
{
	string? Id { get; }
	string? Name { get; }
	DateTime DateUpdated { get; }
	int ModCount { get; set; }
	ulong ModSize { get; set; }
	IOnlinePlayset? OnlineInfo { get; }
	PlaysetOwnership Ownership { get; }
#if CS1
	bool Temporary { get; }
	int AssetCount { get; }
#endif
}

public interface ICustomPlayset : IThumbnailObject
{
	string? Id { get; }
	DateTime DateUsed { get; set; }
	DateTime DateCreated { get; set; }
	PackageUsage Usage { get; set; }
	Color? Color { get; set; }
	bool IsFavorite { get; set; }

	bool IsCustomThumbnailSet { get; }
	bool NoBanner { get; set; }

	void SetThumbnail(Image? image);

#if CS1
	bool DisableWorkshop { get; }
	bool IsMissingItems { get; }
	bool AutoSave { get; }
	bool UnsavedChanges { get; }
	bool Save();
#endif
}

public interface IOnlinePlayset
{
	IUser Author { get; }
	string? Version { get; }
	string? PreferredVersion { get; }
	string LatestPublicVersion { get; }
	int SubscriptionsTotal { get; }
	int Rating { get; }
	int RatingsTotal { get; }
	IPlaysetVersion[]? PublicVersions { get; }
	IPlaysetVersion? LatestPublicVersionData { get; }
}

public interface IPlaysetVersion
{
	string Version { get; }
	string DisplayName { get; }
	DateTime? Created { get; }
	int ModsCount { get; }
}

public interface ITemporaryPlayset : IPlayset
{
	Task<IEnumerable<IPackageIdentity>> GetPackages();
}

public enum PlaysetOwnership
{
	Other,
	OwnedByCurrentUser,
	OwnedByOtherLocalUser,
	SubscribedByCurrentUser
}