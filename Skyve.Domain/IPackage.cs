namespace Skyve.Domain;
public interface IPackage : IPackageIdentity
{
	string? Version { get; }
	bool IsCodeMod { get; }
	bool IsLocal { get; }
#if CS1
	bool IsBuiltIn { get; }
#endif
	ILocalPackageData? LocalData { get; }
	//#if CS2
	//	bool IsIncluded(int playsetId);
	//	bool IsEnabled(int playsetId);
	//#endif
}
