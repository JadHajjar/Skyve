namespace SkyveApp.Domain.Systems;
public interface ICitiesManager
{
	event MonitorTickDelegate MonitorTick;

	bool IsAvailable();
	bool IsRunning();
	void Kill();
	void Launch();
	void RunStub();
}
