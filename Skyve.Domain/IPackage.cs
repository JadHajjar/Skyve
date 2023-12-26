namespace Skyve.Domain;
public interface IPackage : IPackageIdentity
{
	bool IsCodeMod { get; }
	bool IsLocal { get; }
	bool IsBuiltIn { get; }
	ILocalPackageData? LocalData { get; }
	IWorkshopInfo? WorkshopInfo { get; }
}
