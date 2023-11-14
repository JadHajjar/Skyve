using Extensions;

using System;
using System.IO;

namespace Skyve.Domain;
public class FileWatcher : IDisposable
{
	private readonly FileSystemWatcher _watcher;

	public event FileWatcherEventHandler? Changed;
	public event FileWatcherEventHandler? Created;
	public event FileWatcherEventHandler? Deleted;

	public FileWatcher()
	{
		_watcher = new();

		_watcher.Changed += FileChanged;
		_watcher.Created += FileCreated;
		_watcher.Deleted += FileDeleted;
	}

	public string Path { get => _watcher.Path; set { if (Directory.Exists(value)) _watcher.Path = value; } }
	public NotifyFilters NotifyFilter { get => _watcher.NotifyFilter; set => _watcher.NotifyFilter = value; }
	public string Filter { get => _watcher.Filter; set => _watcher.Filter = value; }
	public bool IncludeSubdirectories { get => _watcher.IncludeSubdirectories; set => _watcher.IncludeSubdirectories = value; }
	public bool EnableRaisingEvents { get => _watcher.EnableRaisingEvents; set => _watcher.EnableRaisingEvents = value && Directory.Exists(Path); }

	private void FileDeleted(object sender, FileSystemEventArgs e)
	{
		Deleted?.Invoke(this, FilterPath(e));
	}

	private void FileCreated(object sender, FileSystemEventArgs e)
	{
		Created?.Invoke(this, FilterPath(e));
	}

	private void FileChanged(object sender, FileSystemEventArgs e)
	{
		Changed?.Invoke(this, FilterPath(e));
	}

	private FileWatcherEventArgs FilterPath(FileSystemEventArgs e)
	{
		if (CrossIO.CurrentPlatform is Platform.Windows)
		{
			return new FileWatcherEventArgs(e.ChangeType, e.FullPath);
		}

		var badIndex = Math.Max(e.FullPath.IndexOf("z:\\", StringComparison.InvariantCultureIgnoreCase), e.FullPath.IndexOf("z:/", StringComparison.InvariantCultureIgnoreCase));

		if (badIndex > 0)
		{
			var folder = e.FullPath.Substring(0, badIndex).TrimEnd('/', '\\');
			var fileName = System.IO.Path.GetFileName(e.FullPath);

			return new FileWatcherEventArgs(e.ChangeType, CrossIO.Combine(folder, fileName));
		}

		return new FileWatcherEventArgs(e.ChangeType, e.FullPath);
	}

	public void Dispose()
	{
		_watcher?.Dispose();
	}
}

public delegate void FileWatcherEventHandler(object sender, FileWatcherEventArgs e);

public class FileWatcherEventArgs
{
	public FileWatcherEventArgs(WatcherChangeTypes changeType, string path)
	{
		ChangeType = changeType;
		FullPath = path;
	}

	public WatcherChangeTypes ChangeType { get; }
	public string FullPath { get; }
}
