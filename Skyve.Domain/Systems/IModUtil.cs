using System;

namespace Skyve.Domain.Systems;
public interface IModUtil
{
	bool IsIncluded(ILocalPackageIdentity mod);
	bool IsEnabled(ILocalPackageIdentity mod);
	void SetIncluded(ILocalPackageIdentity mod, bool value);
	void SetEnabled(ILocalPackageIdentity mod, bool value);
	void SaveChanges();
	bool GetModInfo(string folder, out string? modDll, out Version? version);
	int GetLoadOrder(IPackage package);
}
