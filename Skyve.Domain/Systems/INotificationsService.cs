using System;
using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface INotificationsService
{
	event Action OnNewNotification;
	public IEnumerable<INotificationInfo> GetNotifications();
	void SendNotification(INotificationInfo notification);
}
