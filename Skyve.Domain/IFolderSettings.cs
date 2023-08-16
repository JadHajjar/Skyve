using Extensions;

namespace Skyve.Domain;

public interface IFolderSettings
{
	string AppDataPath { get; set; }
	string GamePath { get; set; }
	Platform Platform { get; set; }
	string SteamPath { get; set; }

	void Save();
}
