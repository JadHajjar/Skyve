using System.Drawing;

namespace Skyve.Domain;
public interface ISessionSettings
{
	string? CurrentPlayset { get; set; }
	bool FirstTimeSetupCompleted { get; set; }
	bool CleanupFirstTimeShown { get; set; }
	bool FpsBoosterLogWarning { get; set; }
	string? LastVersionNotification { get; set; }
	Rectangle? LastWindowsBounds { get; set; }
	bool SubscribeFirstTimeShown { get; set; }
	bool WindowWasMaximized { get; set; }
	int LastVersioningNumber { get; set; }

	void Save();
}
