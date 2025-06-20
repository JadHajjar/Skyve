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

			File.WriteAllBytes(LogFilePath, []);

			var assembly = Assembly.GetEntryAssembly();
			var details = assembly.GetName();

			_stopwatch = Stopwatch.StartNew();

#if STABLE
			Info($"Skyve Stable v{details.Version}");
#elif RELEASE
			Info($"Skyve Beta v{details.Version}");
#else
			Info($"Skyve Debug v{details.Version}");
#endif

			Info($"Now  = {DateTime.Now:yyyy-MM-dd hh:mm:ss tt}");
			Info($"Here = {Application.StartupPath}");
			Info($"SaveLocation = {saveHandler.SaveDirectory}");
		}
		catch
		{
			_disabled = true;
		}
	}

	public void Debug(object message, int? lineNumber = default, string? memberName = default)
	{
		ProcessLog("DEBUG", message, lineNumber, memberName);
	}

	public void Info(object message, int? lineNumber = default, string? memberName = default)
	{
		ProcessLog("INFO ", message, lineNumber, memberName);
	}

	public void Warning(object message, int? lineNumber = default, string? memberName = default)
	{
		ProcessLog("WARN ", message, lineNumber, memberName);
	}

	public void Error(object message, int? lineNumber = default, string? memberName = default)
	{
		ProcessLog("ERROR", message, lineNumber, memberName);
	}

	public void Exception(Exception exception, object message, int? lineNumber = default, string? memberName = default)
	{
		ProcessLog("FATAL", $"{message}\r\n{exception}\r\n", lineNumber, memberName);
	}

	public void Exception(object message, int? lineNumber = default, string? memberName = default)
	{
		ProcessLog("FATAL", message, lineNumber, memberName);
	}

	protected void ProcessLog(string type, object content, int? lineNumber = default, string? memberName = default)
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

		if (memberName is not null)
		{
			sb.AppendFormat("[{0}", memberName);

			if (memberName.Length < 12)
			{
				sb.Append(' ', 12 - memberName.Length);
			}

			if (lineNumber < 10)
			{
				sb.Append(' ', 3);
			}
			else if (lineNumber < 100)
			{
				sb.Append(' ', 2);
			}
			else if (lineNumber < 1000)
			{
				sb.Append(' ', 1);
			}

			sb.AppendFormat("@{0}", lineNumber);

			sb.Append("] ");
		}

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