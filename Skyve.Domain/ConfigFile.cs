using Extensions;

using Newtonsoft.Json;

using Skyve.Domain.Systems;

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Skyve.Domain;
public abstract class ConfigFile
{
	private FileWatcher? watcher;
	private bool autoRefresh;

	protected string FilePath { get; set; }
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

	protected ConfigFile(string filePath, bool autoRefresh = false)
	{
		FilePath = filePath;
		this.autoRefresh = autoRefresh;
	}

	protected virtual void OnLoad()
	{
		if (!autoRefresh)
		{
			return;
		}

		watcher = new FileWatcher
		{
			Path = Path.GetDirectoryName(FilePath),
			NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
			Filter = Path.GetFileName(FilePath),
			EnableRaisingEvents = true
		};

		watcher.Changed += ReLoad;
		watcher.Created += ReLoad;
		watcher.Deleted += Clear;
	}

	public void Reset()
	{
		Clear(this, new(WatcherChangeTypes.Deleted, FilePath));
		Save();
	}

	public virtual void Save()
	{
		try
		{
			var isXml = Path.GetExtension(FilePath).Equals(".xml", StringComparison.InvariantCultureIgnoreCase);

			if (watcher is not null && autoRefresh)
			{
				watcher.EnableRaisingEvents = false;
			}

			if (isXml)
			{
				SerializeXml();
			}
			else
			{
				SerializeJson();
			}

			if (watcher is not null && autoRefresh)
			{
				watcher.EnableRaisingEvents = autoRefresh;
			}
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, $"Failed to save the {GetType().Name.FormatWords()} config file: {Path.GetFileName(FilePath)}");
		}
	}

	protected static T Load<T>(string filePath) where T : ConfigFile
	{
		try
		{
			T obj;

			if (!CrossIO.FileExists(filePath))
			{
				obj = (T)Activator.CreateInstance(typeof(T));
			}
			else
			{
				var isXml = Path.GetExtension(filePath).Equals(".xml", StringComparison.InvariantCultureIgnoreCase);

				obj = (isXml ? DeserializeXml<T>(filePath) : DeserializeJson<T>(filePath))
					?? (T)Activator.CreateInstance(typeof(T));
			}

			obj.OnLoad();

			return obj;
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, $"Failed to load the {typeof(T).Name.FormatWords()} config file: {Path.GetFileName(filePath)}");
		}

		return (T)Activator.CreateInstance(typeof(T));
	}

	private void SerializeJson()
	{
		ISave.Write(FilePath, JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented), false);
	}

	private static T DeserializeJson<T>(string filePath) where T : ConfigFile
	{
		var fileContents = ISave.Read(filePath);

		if (string.IsNullOrEmpty(fileContents))
		{
			throw new Exception("File contents empty");
		}

		return JsonConvert.DeserializeObject<T>(fileContents);
	}

	private void SerializeXml()
	{
		var serializer = new XmlSerializer(GetType());
		using var writer = new StreamWriter(FilePath);

		Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

		using var xmlWriter = new XmlTextWriter(writer);
		xmlWriter.Formatting = System.Xml.Formatting.Indented;
		serializer.Serialize(xmlWriter, this, new(new[] { XmlQualifiedName.Empty }));
	}

	private static T? DeserializeXml<T>(string filePath) where T : class
	{
		var ser = new XmlSerializer(typeof(T));
		using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

		return ser.Deserialize(fs) as T;
	}

	#region FileWatcherMethods

	private void Clear(object sender, FileWatcherEventArgs e)
	{
		var type = GetType();
		var obj = (ConfigFile)Activator.CreateInstance(type);

		obj.AutoRefresh = false;
		obj.OnLoad();

		foreach (var item in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
		{
			if (item.CanWrite && item.CanRead)
			{
				item.SetValue(this, item.GetValue(obj));
			}
		}
	}

	private void ReLoad(object sender, FileWatcherEventArgs e)
	{
		if (!CrossIO.FileExists(FilePath))
		{
			return;
		}

		try
		{
			var type = GetType();
			var isXml = Path.GetExtension(FilePath).Equals(".xml", StringComparison.InvariantCultureIgnoreCase);

			var obj = (ConfigFile?)(isXml ? DeserializeXml(FilePath, type) : DeserializeJson(FilePath, type));

			if (obj is not null)
			{
				obj.AutoRefresh = false;
				obj.OnLoad();

				foreach (var item in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
				{
					if (item.CanWrite && item.CanRead)
					{
						item.SetValue(this, item.GetValue(obj));
					}
				}
			}
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load the config file: " + Path.GetFileName(FilePath));
		}
	}

	private static object DeserializeJson(string filePath, Type type)
	{
		var fileContents = ISave.Read(filePath);

		if (string.IsNullOrEmpty(fileContents))
		{
			throw new Exception("File contents empty");
		}

		return JsonConvert.DeserializeObject(fileContents, type);
	}

	private static object DeserializeXml(string filePath, Type type)
	{
		var ser = new XmlSerializer(type);
		using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

		return ser.Deserialize(fs);
	}

	#endregion
}