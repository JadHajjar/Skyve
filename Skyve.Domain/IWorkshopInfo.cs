using System;
using System.Collections.Generic;

namespace Skyve.Domain;
public interface IWorkshopInfo : IPackageIdentity, IThumbnailObject
{
	IUser? Author { get; }
	string? ThumbnailUrl { get; }
	string? ShortDescription { get; }
	string? Description { get; }
	string? VersionName { get; }
	string? SuggestedGameVersion { get; }
	DateTime ServerTime { get; }
	long ServerSize { get; }
	int Subscribers { get; }
	bool HasVoted { get; set; }
	int VoteCount { get; set; }
	bool IsCodeMod { get; }
	bool IsRemoved { get; }
	bool IsBanned { get; }
	bool IsCollection { get; }
	bool IsInvalid { get; }
	Dictionary<string, string> Tags { get; }
	IEnumerable<IPackageRequirement> Requirements { get; }
	IEnumerable<IModChangelog> Changelog { get; }
	IEnumerable<IThumbnailObject> Images { get; }
	IEnumerable<ILink> Links { get; }
	bool IsPartialInfo { get; }

	bool HasComments();
}
