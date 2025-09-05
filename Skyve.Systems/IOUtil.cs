using Extensions;

using Skyve.Domain.Systems;

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Skyve.Systems;
internal class IOUtil : IIOUtil
{
	private readonly ILogger _logger;

	public IOUtil(ILogger logger)
	{
		_logger = logger;
	}

	public Process? Execute(string exeFile, string args, bool useShellExecute = true, bool createNoWindow = false, bool administrator = false)
	{
		try
		{
			exeFile = exeFile.FormatPath();

			if (!CrossIO.FileExists(exeFile))
			{
				_logger.Error("Execute failed, could not find file: " + exeFile);

				return null;
			}

			_logger.Info($"Executing: {exeFile} {args}");

			var startInfo = new ProcessStartInfo
			{
				WorkingDirectory = Path.GetDirectoryName(exeFile).FormatPath(),
				FileName = exeFile,
				Arguments = args,
				WindowStyle = ProcessWindowStyle.Normal,
				UseShellExecute = useShellExecute && CrossIO.CurrentPlatform is not Platform.MacOSX,
				CreateNoWindow = createNoWindow || CrossIO.CurrentPlatform is not Platform.MacOSX,
			};

			if (administrator)
			{
				startInfo.Verb = "runas";
			}

			var process = new Process { StartInfo = startInfo };

			try
			{
				process.Start();
			}
			catch
			{
				if (CrossIO.CurrentPlatform is not Platform.MacOSX)
				{
					throw;
				}
			}

			return process;
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, $"Failed to run '{exeFile}'");
			return null;
		}
	}

	public void RunBatch(string command)
	{
		try
		{
			File.WriteAllText(CrossIO.Combine(Application.StartupPath, "batch.bat"), command);

			Execute(CrossIO.Combine(Application.StartupPath, "batch.bat"), "", true, true);
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, "Failed to start batch to restart the tool after update");
		}
	}

	public void WaitForUpdate()
	{
		RunBatch($"@echo off\r\nsetlocal\r\n\r\nset FILENAME=SkyveApp.exe\r\nset MAX_WAIT_TIME_SECONDS=15\r\n\r\nset FILE_LAST_WRITE_TIME=\r\nfor /f \"usebackq\" %%i in (`dir /b /a-d \"%FILENAME%\"`) do (\r\n  set FILE_LAST_WRITE_TIME=%%~ti\r\n)\r\n\r\nif not defined FILE_LAST_WRITE_TIME (\r\n  echo Error: File \"%FILENAME%\" not found.\r\n  exit /b 1\r\n)\r\n\r\necho {LocaleHelper.GetGlobalText("UpdatingLot")}\r\nset WAIT_START_TIME=%TIME%\r\n:WAIT_LOOP\r\nping -n 2 127.0.0.1 > nul\r\nfor /f \"usebackq\" %%i in (`dir /b /a-d \"%FILENAME%\"`) do (\r\n  if not \"%%~ti\"==\"%FILE_LAST_WRITE_TIME%\" (\r\n    echo File \"%FILENAME%\" has changed. Launching...\r\n    start \"\" \"%FILENAME%\"\r\n    exit /b 0\r\n  start %FILENAME%\r\n)\r\n)\r\nset WAIT_CURRENT_TIME=%TIME%\r\nset /a WAIT_TIME_SECONDS=(1%WAIT_CURRENT_TIME:~6,2%-1%WAIT_START_TIME:~6,2%)+(1%WAIT_CURRENT_TIME:~3,2%-1%WAIT_START_TIME:~3,2%)*60+(1%WAIT_CURRENT_TIME:~0,2%-1%WAIT_START_TIME:~0,2%)\r\nif %WAIT_TIME_SECONDS% gtr %MAX_WAIT_TIME_SECONDS% (\r\n  echo Error: Wait time exceeded %MAX_WAIT_TIME_SECONDS% seconds.\r\n  exit /b 1\r\n  start %FILENAME%\r\n)\r\ngoto WAIT_LOOP\r\n");
	}

	public string? ToRealPath(string? path)
	{
		return path is null
			? null
			: CrossIO.CurrentPlatform is not Platform.Windows && char.IsLetter(path[0]) ? path.Substring(2).FormatPath() : path.FormatPath();
	}
}
