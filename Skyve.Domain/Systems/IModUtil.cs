using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IModUtil
{
	bool IsIncluded(IPackageIdentity mod);
	bool IsEnabled(IPackageIdentity mod);
	Task SetIncluded(IPackageIdentity mod, bool value);
	Task SetEnabled(IPackageIdentity mod, bool value);
	Task SetIncluded(IEnumerable<IPackageIdentity> mods, bool value);
	Task SetEnabled(IEnumerable<IPackageIdentity> mods, bool value);
	void SaveChanges();
	bool GetModInfo(string folder, out string? modDll, out Version? version);
	int GetLoadOrder(IPackage package);
}
