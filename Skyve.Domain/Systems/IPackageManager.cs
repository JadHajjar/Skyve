using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IPackageManager
{
	IEnumerable<IAsset> Assets { get; }
	IEnumerable<IPackage> Packages { get; }
	IEnumerable<IAsset> SaveGames { get; }

	IPackage? GetPackageById(IPackageIdentity identity);
	IPackage? GetPackageByFolder(string folder);
	List<IPackage> GetModsByName(string modName);
	void AddPackage(IPackage package);
	void RemovePackage(IPackage package);
	void SetPackages(List<IPackage> content);
	void DeleteAll(string folder);
	void MoveToLocalFolder(IPackage package);
#if CS1
	void DeleteAll(IEnumerable<ulong> ids);
#endif
}
