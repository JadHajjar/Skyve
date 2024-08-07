﻿namespace Skyve.Domain;
public interface ILocalPackageData : ILocalPackageIdentity
{
	IPackage Package { get; }
	string? Version { get; }
	bool IsCodeMod { get; }
	IAsset[] Assets { get; }
	IThumbnailObject[] Images { get; }
#if CS2
	string? SuggestedGameVersion { get; }
#endif
}
