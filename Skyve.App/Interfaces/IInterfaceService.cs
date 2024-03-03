namespace Skyve.App.Interfaces;
public interface IAppInterfaceService : IInterfaceService
{
	INotificationInfo GetLastVersionNotification();
	PC_Changelog ChangelogPanel();
	PlaysetSettingsPanel PlaysetSettingsPanel(IPlayset playset);
	PanelContent NewPlaysetPanel();
	PanelContent UtilitiesPanel();
	PanelContent RequestReviewPanel(IPackageIdentity package);
}
