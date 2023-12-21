using Skyve.Domain.Enums;

namespace Skyve.Domain.Systems;
public interface ILocationManager
{
	string DataPath { get; }
	string ManagedDLL { get; }
	string SkyveDataPath { get; }
	string SkyveSettingsPath { get; }
	string SteamPathWithExe { get; }

	void CreateShortcut();
	void RunFirstTimeSetup();
	string ToLocalPath(string? relativePath);
	string ToRelativePath(string? localPath);
	void SetPaths(string gamePath, string appDataPath, string steamPath);
}
