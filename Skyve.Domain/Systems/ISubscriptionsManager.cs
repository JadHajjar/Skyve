using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
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
	bool IsSubscribing(IPackageIdentity package);
	Task<bool> Subscribe(IEnumerable<IPackageIdentity> ids);
	Task<bool> UnSubscribe(IEnumerable<IPackageIdentity> ids);
}
