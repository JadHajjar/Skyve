using Extensions;

using Skyve.Domain.Enums;

namespace Skyve.Domain;

public interface IFolderSettings
{
	string AppDataPath { get; set; }
	string GamePath { get; set; }
	string SteamPath { get; set; }
	Platform Platform { get; set; }
#if CS2
	GamingPlatform GamingPlatform { get; set; }
	string UserIdentifier { get; set; }
#endif

	void Save();
}