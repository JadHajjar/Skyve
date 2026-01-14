using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface ISubscriptionsManager
{
	bool IsSubscribing(IPackageIdentity package);

#if CS1
	List<ulong> PendingSubscribingTo { get; }
	List<ulong> PendingUnsubscribingFrom { get; }
	List<ulong> SubscribingTo { get; }
	List<ulong> UnsubscribingFrom { get; }
	bool SubscriptionsPending { get; }
	bool Redownload { get; set; }

	Task<bool> Subscribe(IEnumerable<IPackageIdentity> ids, int? playsetId = null);
	Task<bool> UnSubscribe(IEnumerable<IPackageIdentity> ids, int? playsetId = null);

	void Start();
	void CancelPendingItems();
#else
	event Action UpdateDisplayNotification;

	void AddSubscribing(IEnumerable<IPackageIdentity> ids);
	void RemoveSubscribing(IEnumerable<IPackageIdentity> ids);
	IEnumerable<SubscriptionStatus> GetDownloads();
	bool TryGetDownloadStatus(ulong id, out SubscriptionStatus status);
#endif
}
