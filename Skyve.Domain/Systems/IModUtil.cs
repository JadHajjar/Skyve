namespace Skyve.Domain.Systems;
public interface IModUtil
{
	bool IsIncluded(ILocalPackageData mod);
	bool IsEnabled(ILocalPackageData mod);
	void SetIncluded(ILocalPackageData mod, bool value);
	void SetEnabled(ILocalPackageData mod, bool value);
	void SaveChanges();
	ILocalPackageData? GetMod(ILocalPackageData package);
	int GetLoadOrder(IPackage package);
}
