using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface ISubscriptionsManager
{
	bool IsSubscribing(IPackageIdentity package);
	Task<bool> Subscribe(IEnumerable<IPackageIdentity> ids);
	Task<bool> UnSubscribe(IEnumerable<IPackageIdentity> ids);

#if CS1
	List<ulong> PendingSubscribingTo { get; }
	List<ulong> PendingUnsubscribingFrom { get; }
	List<ulong> SubscribingTo { get; }
	List<ulong> UnsubscribingFrom { get; }
	bool SubscriptionsPending { get; }
	bool Redownload { get; set; }

	void Start();
	void CancelPendingItems();
#else
	SubscriptionStatus Status { get; }

	event Action UpdateDisplayNotification;

	void OnInstallFinished(PackageInstallProgress info);
	void OnInstallProgress(PackageInstallProgress info);
	void OnInstallStarted(PackageInstallProgress info);
	void OnDownloadProgress(PackageDownloadProgress info);
#endif
}
