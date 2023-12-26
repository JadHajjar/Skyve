using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface ICompatibilityManager
{
	bool FirstLoadComplete { get; }

	event Action? SnoozeChanged;

	ICompatibilityInfo GetCompatibilityInfo(IPackage package, bool noCache = false, bool cacheOnly = false);
	IPackageIdentity GetFinalSuccessor(IPackageIdentity item);
	NotificationType GetNotification(ICompatibilityInfo info);
	IPackageCompatibilityInfo? GetPackageInfo(IPackageIdentity package);
	ulong GetIdFromModName(string fileName);
	bool IsBlacklisted(IPackageIdentity package);
	bool IsSnoozed(ICompatibilityItem reportItem);
	void Start(List<ILocalPackageData> packages);
	void ResetCache();
	void ResetSnoozes();
	void ToggleSnoozed(ICompatibilityItem reportItem);
	void DownloadData();
	void CacheReport();
	bool IsUserVerified(IUser author);
	void DoFirstCache();
	void QuickUpdate(ICompatibilityItem compatibilityItem);
}
