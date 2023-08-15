using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface IPackageManager
{
	IEnumerable<IAsset> Assets { get; }
	IEnumerable<IMod> Mods { get; }
	IEnumerable<ILocalPackageWithContents> Packages { get; }

	ILocalPackageWithContents? GetPackageById(IPackageIdentity identity);
	ILocalPackageWithContents? GetPackageByFolder(string folder);
	List<IMod> GetModsByName(string modName);
	void AddPackage(ILocalPackageWithContents package);
	void RemovePackage(ILocalPackageWithContents package);
	void SetPackages(List<ILocalPackageWithContents> content);
	void DeleteAll(IEnumerable<ulong> ids);
	void DeleteAll(string folder);
	void MoveToLocalFolder(ILocalPackage package);
}
