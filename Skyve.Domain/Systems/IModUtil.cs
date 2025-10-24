using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IModUtil
{
	bool IsIncluded(IPackageIdentity mod, string? playsetId = null, bool withVersion = true);
	bool IsEnabled(IPackageIdentity mod, string? playsetId = null, bool withVersion = true);
	Task SetIncluded(IPackageIdentity mod, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true);
	Task SetEnabled(IPackageIdentity mod, bool value, string? playsetId = null);
	Task SetIncluded(IEnumerable<IPackageIdentity> mods, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true);
	Task SetEnabled(IEnumerable<IPackageIdentity> mods, bool value, string? playsetId = null);
	string? GetSelectedVersion(IPackageIdentity package, string? playsetId = null);
	void SaveChanges();
	bool GetModInfo(string folder, out string? modDll, out Version? version);
	int GetLoadOrder(IPackage package);
	Task Initialize();
	bool IsEnabling(IPackageIdentity package);
	Task UndoChanges();
	Task RedoChanges();
	bool IsIncludedInOtherPlaysets(ILocalPackageIdentity mod, string? playsetId = null, bool withVersion = true, bool andEnabled = false);
}
