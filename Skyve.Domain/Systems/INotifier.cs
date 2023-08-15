using System;

namespace SkyveApp.Domain.Systems;
public interface INotifier
{
	bool BulkUpdating { get; set; }
	bool IsContentLoaded { get; }
	bool ApplyingPlayset { get; set; }
	bool PlaysetsLoaded { get; set; }

	event Action? ContentLoaded;
	event Action? PackageInformationUpdated;
	event Action? PackageInclusionUpdated;
	event Action? AutoSaveRequested;
	event Action? PlaysetUpdated;
	event Action? PlaysetChanged;
	event Action? RefreshUI;
	event Action? WorkshopPackagesInfoLoaded;
	event Action? WorkshopInfoUpdated;
	event Action? WorkshopUsersInfoLoaded;
	event Action? CompatibilityReportProcessed;
	event Action? CompatibilityDataLoaded;
	event Action<Exception>? LoggerFailed;

	void TriggerAutoSave();
	void OnContentLoaded();
	void OnInclusionUpdated();
	void OnInformationUpdated();
	void OnWorkshopInfoUpdated();
	void OnPlaysetUpdated();
	void OnPlaysetChanged();
	void OnRefreshUI(bool now = false);
	void OnCompatibilityReportProcessed();
	void OnLoggerFailed(Exception ex);
	void OnWorkshopPackagesInfoLoaded();
	void OnWorkshopUsersInfoLoaded();
	void OnCompatibilityDataLoaded();
}
