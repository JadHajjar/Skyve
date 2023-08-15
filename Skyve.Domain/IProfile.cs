using SkyveApp.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace SkyveApp.Domain;
public interface IPlayset
{
	string? Name { get; set; }
	IUser? Author { get; }
	string? BannerUrl { get; }
	PackageUsage Usage { get; }
	DateTime DateUpdated { get; }
	DateTime DateUsed { get; }
	DateTime DateCreated { get; }
	bool IsFavorite { get; set; }
	int AssetCount { get; }
	int ModCount { get; }
	IEnumerable<IPlaysetEntry> Entries { get; }
	IEnumerable<IPackage> Packages { get; }
	bool Temporary { get; }
}

public interface ICustomPlayset : IPlayset
{
	Color? Color { get; set; }
	Bitmap? Banner { get; set; }
	bool AutoSave { get; }
	int ProfileId { get; }
	bool Public { get; }
	bool IsMissingItems { get; }
	bool UnsavedChanges { get; }

	bool Save();
}

public interface IOnlinePlayset : IPlayset
{
	int ProfileId { get; }
	bool Public { get; set; }
	int Downloads { get; }
}