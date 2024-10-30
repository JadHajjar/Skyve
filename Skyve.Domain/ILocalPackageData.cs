namespace Skyve.Domain;
public interface ILocalPackageData : ILocalPackageIdentity
{
	IPackage Package { get; }
	bool IsCodeMod { get; }
	string? VersionName { get; }
	IAsset[] Assets { get; }
	IThumbnailObject[] Images { get; }
#if CS2
	string? SuggestedGameVersion { get; }
#endif
}
