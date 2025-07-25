﻿using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;

using System;

namespace Skyve.Systems;
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
	public event Action? WorkshopInfoUpdated;
	public event Action? WorkshopUsersInfoLoaded;
	public event Action? CompatibilityDataLoaded;
	public event Action? WorkshopSyncStarted;
	public event Action? WorkshopSyncEnded;
	public event Action? SkyveUpdateAvailable;
	public event Action? SnoozeChanged;
	public event Action? BackupEnded;
	public event Action? BackupStarted;

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

		_delayedContentLoaded = new(3000, () => RunAndLog(ContentLoaded, nameof(ContentLoaded)));
		_delayedPackageInformationUpdated = new(300, () => RunAndLog(PackageInformationUpdated, nameof(PackageInformationUpdated)));
		_delayedPackageInclusionUpdated = new(250, () => RunAndLog(PackageInclusionUpdated, nameof(PackageInclusionUpdated)));
		_delayedWorkshopInfoUpdated = new(200, () => RunAndLog(WorkshopInfoUpdated, nameof(WorkshopInfoUpdated)));
		_delayedWorkshopUsersInfoUpdated = new(200, () => RunAndLog(WorkshopUsersInfoLoaded, nameof(WorkshopUsersInfoLoaded)));
		_delayedAutoSaveRequested = new(300, () => RunAndLog(AutoSaveRequested, nameof(AutoSaveRequested)));
		_delayedImageLoaded = new(300, () => RunAndLog(RefreshUI, nameof(RefreshUI)));
	}

	public bool IsContentLoaded { get; private set; }
	public bool IsBulkUpdating { get; set; }
	public bool IsApplyingPlayset { get; set; }
	public bool IsPlaysetsLoaded { get; set; }
	public bool IsWorkshopSyncInProgress { get; set; }
	public bool IsBackingUp { get; set; }
	public SkyveContext Context { get; set; }

	public void OnContentLoaded()
	{
		IsContentLoaded = true;

		RunAndLog(ContentLoaded, nameof(ContentLoaded));
		//_delayedContentLoaded.Run();
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
			//_delayedPackageInformationUpdated.Run();
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
			RunAndLog(RefreshUI, nameof(RefreshUI));
		}
		else
		{
			_delayedImageLoaded.Run();
		}
	}

	public void OnPlaysetUpdated()
	{
		RunAndLog(PlaysetUpdated, nameof(PlaysetUpdated));
	}

	public void OnPlaysetChanged()
	{
		RunAndLog(PlaysetChanged, nameof(PlaysetChanged));
	}

	public void OnLoggerFailed(Exception ex)
	{
		LoggerFailed?.Invoke(ex);
	}

	public void OnCompatibilityReportProcessed()
	{
		RunAndLog(CompatibilityReportProcessed, nameof(CompatibilityReportProcessed));
	}

	public void OnCompatibilityDataLoaded()
	{
		RunAndLog(CompatibilityDataLoaded, nameof(CompatibilityDataLoaded));
	}

	private void RunAndLog(Action? action, string name)
	{
		_logger.Info(string.Format("[Auto] {0}", name));

		action?.Invoke();
	}

	public void OnWorkshopSyncStarted()
	{
		WorkshopSyncStarted?.Invoke();
	}

	public void OnWorkshopSyncEnded()
	{
		WorkshopSyncEnded?.Invoke();
	}

	public void OnSkyveUpdateAvailable()
	{
		SkyveUpdateAvailable?.Invoke();
	}

	public void OnSnoozeChanged()
	{
		SnoozeChanged?.Invoke();
	}

	public void OnBackupEnded()
	{
		BackupEnded?.Invoke();
	}

	public void OnBackupStarted()
	{
		BackupStarted?.Invoke();
	}
}
