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
	protected bool AutoRefresh { get; set; }
	protected string FilePath { get; }

	protected ConfigFile(string filePath)
	{
		FilePath = filePath;
	}

	protected void OnLoad() { }

	public virtual void Save()
	{
		try
		{
			var xml = Path.GetExtension(FilePath).Equals(".xml", StringComparison.InvariantCultureIgnoreCase);

			if (xml)
			{
				SerializeXml();
			}
			else
			{
				SerializeJson();
			}
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, "Failed to save the config file: " + Path.GetFileName(FilePath));
		}
	}

	protected static T? Load<T>(string filePath) where T : ConfigFile
	{
		if (!CrossIO.FileExists(filePath))
		{
			return null;
		}

		try
		{
			var xml = Path.GetExtension(filePath).Equals(".xml", StringComparison.InvariantCultureIgnoreCase);

			var obj = xml ? DeserializeXml<T>(filePath) : DeserializeJson<T>(filePath);

			obj?.OnLoad();

			return obj;
		}
		catch (Exception ex)
		{
			ServiceCenter.Get<ILogger>().Exception(ex, "Failed to load the config file: " + Path.GetFileName(filePath));
		}

		return null;
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
}
