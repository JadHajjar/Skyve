using Extensions;

using System;
using System.IO;
using System.Reflection;

namespace Skyve.Domain;
public abstract class ConfigFile : IExtendedSaveObject
{
	private FileWatcher? watcher;
	private bool autoRefresh;
	private string? filePath;

	protected bool AutoRefresh
	{
		get => autoRefresh;
		set
		{
			autoRefresh = value;

			if (watcher is not null)
			{
				watcher.EnableRaisingEvents = value;
			}
		}
	}

	public SaveHandler? Handler { get; set; }

	protected ConfigFile()
	{
	}

	public void Reset()
	{
		Clear(this, new(WatcherChangeTypes.Deleted, string.Empty));
	}

	protected virtual void OnLoad(string filePath)
	{
		if (!autoRefresh || (this.filePath == filePath && watcher is not null))
		{
			return;
		}

		this.filePath = filePath;

		watcher = new FileWatcher
		{
			Path = Path.GetDirectoryName(filePath),
			NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
			Filter = Path.GetFileName(filePath),
			EnableRaisingEvents = true
		};

		watcher.Changed += ReLoad;
		watcher.Created += ReLoad;
		watcher.Deleted += Clear;
	}

	void IExtendedSaveObject.OnPreSave(string filePath)
	{
		if (watcher is not null && autoRefresh)
		{
			watcher.EnableRaisingEvents = false;
		}
	}

	void IExtendedSaveObject.OnPostSave(string filePath)
	{
		if (watcher is not null && autoRefresh)
		{
			watcher.EnableRaisingEvents = autoRefresh;
		}
	}

	void IExtendedSaveObject.OnLoad(string filePath)
	{
		OnLoad(filePath);
	}

	private void Clear(object sender, FileWatcherEventArgs e)
	{
		var type = GetType();
		var obj = (ConfigFile)Activator.CreateInstance(type);

		foreach (var item in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
		{
			if (item.CanWrite && item.CanRead && item.PropertyType != typeof(SaveHandler))
			{
				item.SetValue(this, item.GetValue(obj));
			}
		}
	}

	private void ReLoad(object sender, FileWatcherEventArgs e)
	{
		var saveName = GetType().GetCustomAttribute<SaveNameAttribute>(false);

		Handler?.Load(this, saveName.FileName, saveName.AppName, saveName.Local);
	}
}