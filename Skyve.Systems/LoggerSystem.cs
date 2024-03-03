using Extensions;

using Microsoft.Extensions.DependencyInjection;

using Skyve.Domain.Systems;

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Skyve.Systems;

public class LoggerSystem : ILogger
{
	private bool failed;
	private readonly bool _disabled;
	private readonly Stopwatch? _stopwatch;
	private readonly IServiceProvider _provider;

	public string LogFilePath { get; }
	public string PreviousLogFilePath { get; }

	public LoggerSystem(string name, SaveHandler saveHandler, IServiceProvider provider)
	{
		var folder = CrossIO.Combine(saveHandler.SaveDirectory, SaveHandler.AppName, "Logs");

		PreviousLogFilePath = CrossIO.Combine(folder, $"{name}_Previous.log");
		LogFilePath = CrossIO.Combine(folder, $"{name}.log");

		_provider = provider;
		_stopwatch = Stopwatch.StartNew();

		try
		{
			Directory.CreateDirectory(folder);

			if (CrossIO.FileExists(PreviousLogFilePath))
			{
				CrossIO.DeleteFile(PreviousLogFilePath, true);
			}

			if (CrossIO.FileExists(LogFilePath))
			{
				File.Move(LogFilePath, PreviousLogFilePath);
			}

			File.WriteAllBytes(LogFilePath, new byte[0]);

			var assembly = Assembly.GetEntryAssembly();
			var details = assembly.GetName();

			_stopwatch = Stopwatch.StartNew();

#if STABLE
			Info($"Skyve Stable v{details.Version}");
#elif Release
			Info($"Skyve Beta v{details.Version}");
#else
			Info($"Skyve Debug v{details.Version}");
#endif

			Info($"Now  = {DateTime.Now:yyyy-MM-dd hh:mm:ss tt}");
			Info($"Here = {Application.StartupPath}");
		}
		catch
		{
			_disabled = true;
		}
	}

	public void Debug(object message)
	{
		ProcessLog("DEBUG", message);
	}

	public void Info(object message)
	{
		ProcessLog("INFO ", message);
	}

	public void Warning(object message)
	{
		ProcessLog("WARN ", message);
	}

	public void Error(object message)
	{
		ProcessLog("ERROR", message);
	}

	public void Exception(Exception exception, object message)
	{
		ProcessLog("FATAL", $"{message}\r\n{exception}\r\n");
	}

	public void Exception(object message)
	{
		ProcessLog("FATAL", message);
	}

	protected void ProcessLog(string type, object content)
	{
		if (_disabled)
		{
			return;
		}

		var secs = _stopwatch!.ElapsedMilliseconds / 1000M;
		var sb = new StringBuilder();
		var time = secs.ToString("0.000");

		sb.AppendFormat("\r\n[{0} ", type);

		if (time.Length < 10)
		{
			sb.Append(' ', 10 - time.Length);
		}

		sb.Append(time);

		sb.Append("] ");

		sb.Append(content?.ToString().Replace("\n", "\n                      "));

		lock (LogFilePath)
		{
			try
			{
				File.AppendAllText(LogFilePath, sb.ToString());
			}
			catch (Exception ex)
			{
				if (!failed)
				{
					failed = true;

					_provider.GetService<INotifier>()?.OnLoggerFailed(ex);
				}
			}
		}
	}
}