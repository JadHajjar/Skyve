namespace Skyve.Domain;
public interface IPackage : IPackageIdentity
{
	bool IsCodeMod { get; }
	bool IsLocal { get; }
#if CS1
	bool IsBuiltIn { get; }
#endif
	ILocalPackageData? LocalData { get; }
	IWorkshopInfo? WorkshopInfo { get; }
}
