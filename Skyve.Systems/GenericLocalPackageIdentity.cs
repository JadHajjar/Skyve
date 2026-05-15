using Skyve.Domain;

using System;
using System.Collections.Generic;

namespace Skyve.Systems;

public class GenericLocalPackageIdentity : ILocalPackageIdentity
{
	private string? _name;
	private string? _url;
	private string? _folder;
	private string? _filePath;

	[Obsolete("Reserved for DTO", true)]
	public GenericLocalPackageIdentity()
	{
		Source = Id = string.Empty;
	}

	public GenericLocalPackageIdentity(ILocalPackageIdentity localPackageIdentity)
	{
		Source = localPackageIdentity.Source;
		Id = localPackageIdentity.Id;
		Name = localPackageIdentity.Name;
		Url = localPackageIdentity.Url;
		Folder = localPackageIdentity.Folder;
		FilePath = localPackageIdentity.FilePath;
		FileSize = localPackageIdentity.FileSize;
		LocalTime = localPackageIdentity.LocalTime;
		Version = localPackageIdentity.Version;
	}

	public GenericLocalPackageIdentity(string source, string id, string? name = null, string? url = null, string? folder = null, string? filePath = null, long fileSize = default, DateTime localTime = default, string? version = null)
	{
		Source = source;
		Id = id;
		_name = name;
		_url = url;
		_folder = folder;
		_filePath = filePath;
		FileSize = fileSize;
		LocalTime = localTime;
		Version = version;
	}

	public string Source { get; set; }
	public string Id { get; set; }
	public string Name { get => _name ?? this.GetWorkshopInfo()?.Name ?? string.Empty; set => _name = value; }
	public string? Url { get => _url ?? this.GetWorkshopInfo()?.Url; set => _url = value; }
	public string Folder { get => _folder ?? string.Empty; set => _folder = value; }
	public string FilePath { get => _filePath ?? string.Empty; set => _filePath = value; }
	public long FileSize { get; set; }
	public DateTime LocalTime { get; set; }
	public string? Version { get; set; }

	public override bool Equals(object? obj)
	{
		return obj is IPackageIdentity identity &&
			   Source == identity.Source &&
			   Id == identity.Id &&
			   Version == identity.Version;
	}

	public override int GetHashCode()
	{
		var hashCode = -781363793;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Source);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
		hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(Version);
		return hashCode;
	}
}