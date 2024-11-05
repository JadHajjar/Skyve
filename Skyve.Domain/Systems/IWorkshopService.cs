using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IWorkshopService
{
	bool IsReady { get; }

	void ClearCache();
	Task<IEnumerable<ITag>> GetAvailableTags();
	IWorkshopInfo? GetInfo(IPackageIdentity identity);
	Task<IWorkshopInfo?> GetInfoAsync(IPackageIdentity identity);
	IPackage GetPackage(IPackageIdentity identity);
	Task<IPackage> GetPackageAsync(IPackageIdentity identity);
	Task<IEnumerable<IWorkshopInfo>> GetWorkshopItemsByUserAsync(object userId, WorkshopQuerySorting sorting = WorkshopQuerySorting.DateCreated, string? query = null, string[]? requiredTags = null, bool all = false, int? limit = null, int? page = null);
	Task<IEnumerable<IWorkshopInfo>> QueryFilesAsync(WorkshopQuerySorting sorting, WorkshopSearchTime searchTime = WorkshopSearchTime.AllTime, string? query = null, string[]? requiredTags = null, bool all = false, int? limit = null, int? page = null);

#if CS2
	event Action? OnContextAvailable;
	event Action? OnLogin;
	event Action? OnLogout;

	bool IsLoggedIn { get; }
	bool IsLoginPending { get; }
	bool IsAvailable { get; }

	Task Initialize();
	Task<bool> Login();
	Task<bool> Login(string email, string password, bool rememberMe);
	Task RunSync();
	Task<int> GetActivePlaysetId();
	Task<List<IPlayset>> GetPlaysets(bool localOnly);
	Task<IPlayset?> GetCurrentPlayset();
	Task<bool> ToggleVote(IPackageIdentity packageIdentity);
	Task<IModCommentsInfo?> GetComments(IPackageIdentity packageIdentity, int page = 1, int limit = 20);
	Task<IModComment?> PostNewComment(IPackageIdentity packageIdentity, string comment);
	IAuthor? GetUser(IUser? user);
	Task<IAuthor?> GetUserAsync(IUser? user);
	ILink? GetCommentsPageUrl(IPackageIdentity packageIdentity);
	bool IsLocal(IPackageIdentity identity);
	Task Shutdown();
	void RepairContext();
#endif
}
