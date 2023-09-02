using System;

namespace Skyve.Domain.Systems;
public interface ICitiesManager
{
	event MonitorTickDelegate MonitorTick;
	event Action<bool> LaunchingStatusChanged;

	bool IsAvailable();
	bool IsRunning();
	void Kill();
	void Launch();
	void RunStub();
	void SetLaunchingStatus(bool launching);
}
