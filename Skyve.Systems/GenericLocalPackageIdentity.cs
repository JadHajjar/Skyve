using Skyve.Domain;

using System;
using System.Collections.Generic;

namespace Skyve.Systems;

public class GenericLocalPackageIdentity : ILocalPackageIdentity, IEquatable<GenericLocalPackageIdentity?>
{
	private string? _name;
	private string? _url;
	private string? _folder;
	private string? _filePath;

	public GenericLocalPackageIdentity()
	{

	}

	public GenericLocalPackageIdentity(ulong id, string? name = null, string? url = null, string? folder=null, string?filePath=null, long fileSize = default, DateTime localTime=default)
	{
		Id = id;
		_name = name;
		_url = url;
		_folder = folder;
		_filePath = filePath;
		FileSize = fileSize;
		LocalTime = localTime;
	}

	public ulong Id { get; set; }
	public string Name { get => _name ?? this.GetWorkshopInfo()?.Name ?? string.Empty; set => _name = value; }
	public string? Url { get => _url ?? this.GetWorkshopInfo()?.Url; set => _url = value; }
	public string Folder { get => _folder ?? string.Empty; set => _folder = value; }
	public string FilePath { get => _filePath ?? string.Empty; set => _filePath = value; }
	public long FileSize { get; set; }
	public DateTime LocalTime { get; set; }

	public override bool Equals(object? obj)
	{
		return Equals(obj as GenericLocalPackageIdentity);
	}

	public bool Equals(GenericLocalPackageIdentity? other)
	{
		return other is not null &&
			   Id == other.Id &&
			   FilePath == other.FilePath;
	}

	public override int GetHashCode()
	{
		var hashCode = 329219044;
		hashCode = hashCode * -1521134295 + Id.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FilePath);
		return hashCode;
	}
}