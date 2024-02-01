using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;

using System;

namespace Skyve.Compatibility.Domain.Interfaces;
public interface ICompatibilityManager
{
	bool FirstLoadComplete { get; }

	event Action? SnoozeChanged;

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
