using System;
using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface INotificationsService
{
	event Action OnNewNotification;
	public IEnumerable<INotificationInfo> GetNotifications();
	public IEnumerable<TNotificationInfo> GetNotifications<TNotificationInfo>() where TNotificationInfo : INotificationInfo;
	void SendNotification(INotificationInfo notification);
	void RemoveNotificationsOfType<TNotificationInfo>() where TNotificationInfo : INotificationInfo;
	void RemoveNotification(INotificationInfo notification);
}
