using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;

using System;
using System.Collections.Generic;

namespace Skyve.Compatibility.Domain.Interfaces;
public interface ICompatibilityManager
{
	bool FirstLoadComplete { get; }

	IEnumerable<IPackage> GetPackagesThatReference(IPackageIdentity package, bool withExcluded = false);
	ICompatibilityInfo GetCompatibilityInfo(IPackageIdentity package, bool noCache = false, bool cacheOnly = false);
	ICompatibilityPackageIdentity GetFinalSuccessor(ICompatibilityPackageIdentity package);
	NotificationType GetNotification(ICompatibilityInfo info);
	StatusAction GetAction(ICompatibilityInfo info);
	bool IsSnoozed(ICompatibilityItem reportItem);
	void ResetSnoozes();
	void ToggleSnoozed(ICompatibilityItem reportItem);
	void CacheReport();
	void DoFirstCache();
	void QuickUpdate(ICompatibilityItem compatibilityItem);
}
