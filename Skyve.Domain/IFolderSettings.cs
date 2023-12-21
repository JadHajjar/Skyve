using Extensions;

using Skyve.Domain.Enums;

namespace Skyve.Domain;

public interface IFolderSettings
{
	string AppDataPath { get; set; }
	string GamePath { get; set; }
	Platform Platform { get; set; }
	GamingPlatform GamingPlatform { get; set; }
	string SteamPath { get; set; }

	void Save();
}
