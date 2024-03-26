using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IWorkshopService
{
	bool IsReady { get; }
	IDisposable Lock { get; }

	void ClearCache();
	Task<IEnumerable<ITag>> GetAvailableTags();
	IWorkshopInfo? GetInfo(IPackageIdentity identity);
	Task<IWorkshopInfo?> GetInfoAsync(IPackageIdentity identity);
	IPackage GetPackage(IPackageIdentity identity);
	Task<IPackage> GetPackageAsync(IPackageIdentity identity);
	Task<IEnumerable<IWorkshopInfo>> GetWorkshopItemsByUserAsync(object userId);
	Task<IEnumerable<IWorkshopInfo>> QueryFilesAsync(WorkshopQuerySorting sorting, string? query = null, string[]? requiredTags = null, bool all = false, int? limit = null, int? page = null);

#if CS2
	event Action? ContextAvailable;

	bool IsLoggedIn { get; }
	bool IsLoginPending { get; }
	bool IsAvailable { get; }

	Task Initialize();
	Task Login();
	Task<bool> Login(string email, string password, bool rememberMe);
	Task WaitUntilReady();
	Task RunSync();
	Task<int> GetActivePlaysetId();
	Task<List<IPlayset>> GetPlaysets(bool localOnly);
	Task<bool> ToggleVote(IPackageIdentity packageIdentity);
#endif
}
