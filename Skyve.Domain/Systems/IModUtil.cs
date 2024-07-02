using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IModUtil
{
	bool IsIncluded(IPackageIdentity mod, int? playsetId = null);
	bool IsEnabled(IPackageIdentity mod, int? playsetId = null);
	Task SetIncluded(IPackageIdentity mod, bool value, int? playsetId = null);
	Task SetEnabled(IPackageIdentity mod, bool value, int? playsetId = null);
	Task SetIncluded(IEnumerable<IPackageIdentity> mods, bool value, int? playsetId = null);
	Task SetEnabled(IEnumerable<IPackageIdentity> mods, bool value, int? playsetId = null);
	Task SetVersion(IPackageIdentity package, string version, int? playsetId = null);
	string? GetSelectedVersion(IPackageIdentity package, int? playsetId = null);
	void SaveChanges();
	bool GetModInfo(string folder, out string? modDll, out Version? version);
	int GetLoadOrder(IPackage package);
	Task Initialize();
	bool IsEnabling(IPackageIdentity package);
	Task UndoChanges();
	Task RedoChanges();
}
