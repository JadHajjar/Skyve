namespace Skyve.Domain.Systems;
public interface ILocationService
{
	string DataPath { get; }
	string ManagedDLL { get; }
	string SkyveDataPath { get; }
	string SkyveSettingsPath { get; }
	string SteamPathWithExe { get; }
	string CitiesPathWithExe { get; }

#if CS1
	string AddonsPath { get; }
	string AppDataPath { get; set; }
	string AssetsPath { get; }
	string CitiesPathWithExe { get; }
	string GameContentPath { get; }
	string GamePath { get; set; }
	string MapsPath { get; }
	string MapThemesPath { get; }
	string ModsPath { get; }
	string MonoPath { get; }
	string SkyvePlaysetsAppDataPath { get; }
	string SteamPath { get; set; }
	string StylesPath { get; }
	string WorkshopContentPath { get; }
#endif

	void CreateShortcut();
	void RunFirstTimeSetup();
	string ToLocalPath(string? relativePath);
	string ToRelativePath(string? localPath);
}
