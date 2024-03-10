namespace Skyve.App.Interfaces;
public interface IAppInterfaceService : IInterfaceService
{
	INotificationInfo GetLastVersionNotification();
	PC_Changelog ChangelogPanel();
	PanelContent NewPlaysetPanel();
	PanelContent UtilitiesPanel();
	PanelContent RequestReviewPanel(IPackageIdentity package);
}
