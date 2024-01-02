﻿using System;
using System.Collections.Generic;

namespace Skyve.Domain;
public interface IWorkshopInfo : IPackageIdentity
{
	IUser? Author { get; }
	string? ThumbnailUrl { get; }
	string? Description { get; }
	string? Version { get; }
	DateTime ServerTime { get; }
	long ServerSize { get; }
	int Subscribers { get; }
	bool HasVoted { get; set; }
	int VoteCount { get; set; }
	bool IsCodeMod { get; }
	bool IsRemoved { get; }
	bool IsIncompatible { get; }
	bool IsBanned { get; }
	bool IsCollection { get; }
	bool IsInvalid { get; }
	Dictionary<string, string> Tags { get; }
	IEnumerable<IPackageRequirement> Requirements { get; }
}
