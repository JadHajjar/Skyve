using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IWorkshopService
{
	void CleanDownload(List<ILocalPackageData> packages);
	void ClearCache();
	IEnumerable<IWorkshopInfo> GetAllPackages();
	IWorkshopInfo? GetInfo(IPackageIdentity identity);
	Task<IWorkshopInfo?> GetInfoAsync(IPackageIdentity identity);
	IPackage GetPackage(IPackageIdentity identity);
	Task<IPackage> GetPackageAsync(IPackageIdentity identity);
	IUser? GetUser(object authorId);
	Task<IEnumerable<IWorkshopInfo>> GetWorkshopItemsByUserAsync(object userId);
	Task<IEnumerable<IWorkshopInfo>> QueryFilesAsync(PackageSorting sorting, string? query = null, string[]? requiredTags = null, string[]? excludedTags = null, (DateTime, DateTime)? dateRange = null, bool all = false);
#if CS2
	Task Initialize();
	Task<List<ILocalPackageData>> GetInstalledPackages();
	Task<List<ICustomPlayset>> GetAllPlaysets(bool localOnly);
	Task Login();
#endif
}
