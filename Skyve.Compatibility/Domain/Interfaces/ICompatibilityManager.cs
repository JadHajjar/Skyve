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
	IPackageIdentity GetFinalSuccessor(IPackageIdentity item);
	NotificationType GetNotification(ICompatibilityInfo info);
	bool IsSnoozed(ICompatibilityItem reportItem);
	void ResetSnoozes();
	void ToggleSnoozed(ICompatibilityItem reportItem);
	void CacheReport();
	void DoFirstCache();
	void QuickUpdate(ICompatibilityItem compatibilityItem);
}
