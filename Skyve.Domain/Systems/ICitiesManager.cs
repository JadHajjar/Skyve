using System;

namespace Skyve.Domain.Systems;
public interface ICitiesManager
{
	string GameVersion { get; }

	event MonitorTickDelegate MonitorTick;
	event Action<bool> LaunchingStatusChanged;
	event Action? GameClosed;

	bool IsAvailable();
	bool IsRunning();
	void Kill();
	void Launch();
	void RunSafeMode();
	void RunStub();
	void SetLaunchingStatus(bool launching);
}
