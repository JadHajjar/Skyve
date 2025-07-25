using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IInterfaceService
{
	void OpenParadoxLogin();
	void OpenSyncConflictPrompt(ISyncConflictInfo[] conflicts);
	void OpenOptionsPage();
	void ViewSpecificPackages(List<IPackageIdentity> packages, string title);
	void OpenPackagePage(IPackageIdentity package, bool openCompatibilityPage = false, bool openCommentsPage = false);
	void OpenPlaysetPage(IPlayset playset, bool settingsTab = false, bool editName = false);
	bool AskForDependencyConfirmation(List<IPackageIdentity> packages, List<IPackageIdentity> dependencies);
	void OpenLogReport(bool save);
	void RestoreBackup(string restoreBackup);
}
