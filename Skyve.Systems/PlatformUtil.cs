using Extensions;

using SkyveApp.Domain;
using SkyveApp.Domain.Systems;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SkyveApp.Systems;
public static class PlatformUtil
{
	public static void OpenUrl(string? url)
	{
		try
		{
			if (url is null or "")
			{
				return;
			}

			if (ServiceCenter.Get<ISettings>().UserSettings.OpenLinksInBrowser)
			{
				Process.Start(url);
				return;
			}

			var domain = Regex.Match(url, @"^(?:https?:\/\/)?(?:[^@\n]+@)?(?:www\.)?([^:\/\n?]+)").Groups[1].Value;

			if (domain.Contains("store.steam", StringComparison.OrdinalIgnoreCase))
			{
				if (ExecuteSteam("steam://store/" + Regex.Match(url, "\\d+$").Value))
				{
					return;
				}
			}
			else if (domain.Contains("steam", StringComparison.OrdinalIgnoreCase))
			{
				if (ExecuteSteam("steam://openurl/" + url))
				{
					return;
				}
			}
			else
			{
				Process.Start(url);
			}
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, $"Failed to open the URL: '{url}'");
		}
	}

	public static bool ExecuteSteam(string args)
	{
		var file = ServiceCenter.Get<ILocationManager>().SteamPathWithExe;

		ServiceCenter.Get<IIOUtil>().Execute(file, args);

		return true;
	}

	public static void OpenFolder(string? folder)
	{
		try
		{
			if (folder is null or "")
			{
				return;
			}

			folder = folder.FormatPath();

			if (CrossIO.FileExists(folder))
			{
				OpenFileInFolder(folder);
				return;
			}

			if (!Directory.Exists(folder))
			{
				ServiceCenter.Get<ILogger>().Error($"Failed to open the folder: '{folder}' | Directory not found");
				return;
			}

			_ = (char.IsLetter(folder[0]) ? Platform.Windows : CrossIO.CurrentPlatform) switch
			{
				Platform.MacOSX => Process.Start("/bin/zsh", $"-c \" open -R '{Directory.EnumerateFiles(folder).FirstOrDefault()?.FormatPath() ?? folder}' \""),
				Platform.Linux => Process.Start("/usr/bin/bash", $"-c \" xdg-open '{folder}' \""),
				Platform.Windows or _ => Process.Start(folder),
			};
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, $"Failed to open the folder: '{folder}'");
		}
	}

	private static void OpenFileInFolder(string file)
	{
		try
		{
			_ = (char.IsLetter(file[0]) ? Platform.Windows : CrossIO.CurrentPlatform) switch
			{
				Platform.MacOSX => Process.Start("/bin/zsh", $"-c \" open -R '{file}' \""),
				Platform.Linux => Process.Start("/usr/bin/bash", $"-c \" xdg-open '{Path.GetDirectoryName(file).FormatPath()}' \""),
				Platform.Windows or _ => Process.Start("explorer.exe", $"/select, \"{file}\""),
			};
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, $"Failed to open the file: '{file}'");
		}
	}

	public static void SetFileInClipboard(string path)
	{
		SystemsProgram.MainForm.TryInvoke(() =>
		{
			if (CrossIO.CurrentPlatform is not Platform.Windows)
			{
				if (path[0] is 'c' or 'C')
				{
					var file = CrossIO.Combine(ServiceCenter.Get<ILocationManager>().SkyveAppDataPath, "Support Logs", Path.GetFileName(path));

					CrossIO.CopyFile(path, file, true);

					path = file;
				}

				path = $"file://{ServiceCenter.Get<IIOUtil>().ToRealPath(path)}";
			}

			Clipboard.SetData(DataFormats.FileDrop, new[] { path });
		});
	}
}
