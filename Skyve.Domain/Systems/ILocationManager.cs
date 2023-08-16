namespace Skyve.Domain.Systems;
public interface ILocationManager
{
	string AddonsPath { get; }
	string AppDataPath { get; set; }
	string AssetsPath { get; }
	string CitiesPathWithExe { get; }
	string DataPath { get; }
	string GameContentPath { get; }
	string GamePath { get; set; }
	string ManagedDLL { get; }
	string MapsPath { get; }
	string MapThemesPath { get; }
	string ModsPath { get; }
	string MonoPath { get; }
	string SkyveAppDataPath { get; }
	string SkyvePlaysetsAppDataPath { get; }
	string SteamPath { get; set; }
	string SteamPathWithExe { get; }
	string StylesPath { get; }
	string WorkshopContentPath { get; }

	void CreateShortcut();
	void RunFirstTimeSetup();
	string ToLocalPath(string? relativePath);
	string ToRelativePath(string? localPath);
	void SetPaths(string gamePath, string appDataPath, string steamPath);
}
