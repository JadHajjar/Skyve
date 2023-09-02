using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skyve.App.Interfaces;
public interface IInterfaceService
{
	INotificationInfo GetLastVersionNotification();
	PC_Changelog ChangelogPanel();
	PlaysetSettingsPanel PlaysetSettingsPanel();
	PanelContent UtilitiesPanel();
}
