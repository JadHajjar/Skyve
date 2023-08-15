using Extensions;

using SkyveApp.Domain.Systems;

using System;
using System.Diagnostics;

namespace SkyveApp.Systems;
internal class NotifierSystem : INotifier
{
	private readonly ILogger _logger;

	public event Action? ContentLoaded;
	public event Action? PackageInformationUpdated;
	public event Action? PackageInclusionUpdated;
	public event Action? AutoSaveRequested;
	public event Action? PlaysetUpdated;
	public event Action? PlaysetChanged;
	public event Action? RefreshUI;
	public event Action<Exception>? LoggerFailed;
	public event Action? CompatibilityReportProcessed;
	public event Action? WorkshopPackagesInfoLoaded;
	public event Action? WorkshopInfoUpdated;
	public event Action? WorkshopUsersInfoLoaded;
	public event Action? CompatibilityDataLoaded;

	private readonly DelayedAction _delayedPackageInformationUpdated;
	private readonly DelayedAction _delayedPackageInclusionUpdated;
	private readonly DelayedAction _delayedWorkshopInfoUpdated;
	private readonly DelayedAction _delayedWorkshopUsersInfoUpdated;
	private readonly DelayedAction _delayedContentLoaded;
	private readonly DelayedAction _delayedAutoSaveRequested;
	private readonly DelayedAction _delayedImageLoaded;

	public NotifierSystem(ILogger logger)
	{
		_logger = logger;

		_delayedContentLoaded = new(350, () => { _logger.Info("[Auto] ContentLoaded"); ContentLoaded?.Invoke(); });
		_delayedPackageInformationUpdated = new(300, () => { _logger.Info("[Auto] PackageInformationUpdated"); PackageInformationUpdated?.Invoke(); });
		_delayedPackageInclusionUpdated = new(250, () => { _logger.Info("[Auto] PackageInclusionUpdated"); PackageInclusionUpdated?.Invoke(); });
		_delayedWorkshopInfoUpdated = new(200, () => { _logger.Info("[Auto] WorkshopInfoUpdated"); WorkshopInfoUpdated?.Invoke(); });
		_delayedWorkshopUsersInfoUpdated = new(200, () => { _logger.Info("[Auto] WorkshopUsersInfoLoaded"); WorkshopUsersInfoLoaded?.Invoke(); });
		_delayedAutoSaveRequested = new(300, () => { _logger.Info("[Auto] AutoSaveRequested"); AutoSaveRequested?.Invoke(); });
		_delayedImageLoaded = new(300, () => { _logger.Info("[Auto] RefreshUI"); RefreshUI?.Invoke(); });
	}

	public bool IsContentLoaded { get; private set; }
	public bool BulkUpdating { get; set; }
	public bool ApplyingPlayset { get; set; }
	public bool PlaysetsLoaded { get; set; }

	public void OnContentLoaded()
	{
		IsContentLoaded = true;

		_delayedContentLoaded.Run();
	}

	public void OnWorkshopInfoUpdated()
	{
		if (IsContentLoaded)
		{
			_delayedWorkshopInfoUpdated.Run();
		}
	}

	public void OnWorkshopUsersInfoLoaded()
	{
		if (IsContentLoaded)
		{
			_delayedWorkshopUsersInfoUpdated.Run();
		}
	}

	public void OnInformationUpdated()
	{
		if (IsContentLoaded)
		{
			_delayedPackageInformationUpdated.Run();
		}
	}

	public void OnInclusionUpdated()
	{
		if (IsContentLoaded)
		{
			_delayedPackageInclusionUpdated.Run();
			_delayedPackageInformationUpdated.Run();
		}
	}

	public void TriggerAutoSave()
	{
		_delayedAutoSaveRequested.Run();
	}

	public void OnRefreshUI(bool now = false)
	{
		if (now)
		{
			RefreshUI?.Invoke();
		}
		else
		{
			_delayedImageLoaded.Run();
		}
	}

	public void OnPlaysetUpdated()
	{
#if DEBUG
		_logger.Debug("[Auto] Playset Updated \r\n" + new StackTrace());
#else
		_logger.Info("[Auto] Playset Updated");
#endif

		PlaysetUpdated?.Invoke();
	}

	public void OnPlaysetChanged()
	{
		PlaysetChanged?.Invoke();
	}

	public void OnLoggerFailed(Exception ex)
	{
		LoggerFailed?.Invoke(ex);
	}

	public void OnCompatibilityReportProcessed()
	{
#if DEBUG
		_logger.Debug("[Auto] Compatibility Report Processed \r\n" + new StackTrace());
#else
		_logger.Info("[Auto] Compatibility Report Processed");
#endif

		CompatibilityReportProcessed?.Invoke();
	}

	public void OnWorkshopPackagesInfoLoaded()
	{
		_logger.Info("[Auto] Workshop Packages Info Loaded");

		WorkshopPackagesInfoLoaded?.Invoke();
		WorkshopPackagesInfoLoaded = null;
	}

	public void OnCompatibilityDataLoaded()
	{
		_logger.Info("[Auto] Compatibility Data Loaded");

		CompatibilityDataLoaded?.Invoke();
	}
}
