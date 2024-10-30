namespace Skyve.Domain;
public interface IPackage : IPackageIdentity
{
	bool IsCodeMod { get; }
	bool IsLocal { get; }
	bool IsBuiltIn { get; }
	ILocalPackageData? LocalData { get; }
	//#if CS2
	//	bool IsIncluded(int playsetId);
	//	bool IsEnabled(int playsetId);
	//#endif
}

public interface IPlaysetPackage : IPackage
{
	bool IsEnabled { get; }
	int LoadOrder { get; }
}