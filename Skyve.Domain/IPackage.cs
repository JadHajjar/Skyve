namespace Skyve.Domain;
public interface IPackage : IPackageIdentity
{
	bool IsCodeMod { get; }
	bool IsLocal { get; }
	bool IsBuiltIn { get; }
	ILocalPackageData? LocalData { get; }
	string? VersionName { get; }
}

public interface IPlaysetPackage : IPackage
{
	bool IsEnabled { get; }
	int LoadOrder { get; }
}