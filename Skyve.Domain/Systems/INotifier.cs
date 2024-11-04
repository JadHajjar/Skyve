using System;

namespace Skyve.Domain.Systems;
public interface INotifier
{
	bool IsBulkUpdating { get; set; }
	bool IsContentLoaded { get; }
	bool IsApplyingPlayset { get; set; }
	bool IsPlaysetsLoaded { get; set; }
	bool IsWorkshopSyncInProgress { get; set; }
	bool IsBackingUp { get; set; }

	event Action? ContentLoaded;
	event Action? PackageInformationUpdated;
	event Action? PackageInclusionUpdated;
	event Action? AutoSaveRequested;
	event Action? PlaysetUpdated;
	event Action? PlaysetChanged;
	event Action? RefreshUI;
	event Action? WorkshopInfoUpdated;
	event Action? WorkshopUsersInfoLoaded;
	event Action? CompatibilityReportProcessed;
	event Action? CompatibilityDataLoaded;
	event Action? WorkshopSyncStarted;
	event Action? WorkshopSyncEnded;
	event Action? SkyveUpdateAvailable;
	event Action? SnoozeChanged;
	event Action<Exception>? LoggerFailed;

	void OnLoggerFailed(Exception ex);
	void OnRefreshUI(bool now = false);
	void TriggerAutoSave();
	void OnContentLoaded();
	void OnInclusionUpdated();
	void OnInformationUpdated();
	void OnWorkshopInfoUpdated();
	void OnPlaysetUpdated();
	void OnPlaysetChanged();
	void OnCompatibilityReportProcessed();
	void OnWorkshopUsersInfoLoaded();
	void OnCompatibilityDataLoaded();
	void OnWorkshopSyncStarted();
	void OnWorkshopSyncEnded();
	void OnSkyveUpdateAvailable();
	void OnSnoozeChanged();
}
