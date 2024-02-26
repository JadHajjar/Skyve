using Extensions;

using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Skyve.Domain;
public interface IPlayset : IThumbnailObject
{
	int Id { get; }
	string? Name { get; }
	DateTime DateUpdated { get; }
	int ModCount { get; }
	ulong ModSize { get; }
	bool Temporary { get; }
#if CS1
	int AssetCount { get; }
#endif
}

public interface ICustomPlayset : IThumbnailObject
{
	int Id { get; }
	DateTime DateUsed { get; }
	DateTime DateCreated { get; }
	PackageUsage Usage { get; set; }
	Color? Color { get; set; }
	bool IsFavorite { get; set; }

	IOnlinePlayset? OnlineInfo { get; }
	bool IsCustomThumbnailSet { get; }

	void SetThumbnail(Image? image);

#if CS1
	bool DisableWorkshop { get; }
	bool IsMissingItems { get; }
	bool AutoSave { get; }
	bool UnsavedChanges { get; }
	bool Save();
#endif
}

public interface IOnlinePlayset : IThumbnailObject
{
	int Id { get; }
	bool Public { get; set; }
	int Downloads { get; }
	[CloneIgnore] IUser? Author { get; }
}