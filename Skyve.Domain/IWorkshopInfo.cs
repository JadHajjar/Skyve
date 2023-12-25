﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace Skyve.Domain;
public interface IWorkshopInfo : IPackageIdentity
{
	IUser? Author { get; }
	string? ThumbnailUrl { get; }
	string? Description { get; }
	DateTime ServerTime { get; }
	long ServerSize { get; }
	int Subscribers { get; }
	int Score { get; }
	int ScoreVoteCount { get; }
	bool IsMod { get; }
	bool IsRemoved { get; }
	bool IsIncompatible { get; }
	bool IsBanned { get; }
	bool IsCollection { get; }
	bool IsInvalid { get; }
	Dictionary<string, string> Tags { get; }
	IEnumerable<IPackageRequirement> Requirements { get; }

	bool GetThumbnail(out Bitmap? thumbnail, out string? thumbnailUrl);
}
