using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface INotificationsService
{
	event Action OnNewNotification;
	public IEnumerable<INotificationInfo> GetNotifications();
	void SendNotification(INotificationInfo notification);
}
