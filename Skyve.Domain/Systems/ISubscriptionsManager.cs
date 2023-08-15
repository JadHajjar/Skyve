using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface ISubscriptionsManager
{
	List<ulong> PendingSubscribingTo { get; }
	List<ulong> PendingUnsubscribingFrom { get; }
	List<ulong> SubscribingTo { get; }
	List<ulong> UnsubscribingFrom { get; }
	bool SubscriptionsPending { get; }
	bool Redownload { get; set; }

	void Start();
	void CancelPendingItems();
	bool IsSubscribing(IPackage package);
	bool Subscribe(IEnumerable<IPackageIdentity> ids);
	bool UnSubscribe(IEnumerable<IPackageIdentity> ids);
}
