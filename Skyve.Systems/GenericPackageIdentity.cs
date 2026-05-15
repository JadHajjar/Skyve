using Skyve.Domain;

using System.Collections.Generic;

namespace Skyve.Systems;

public class GenericPackageIdentity : IPackageIdentity
{
	private string? _name;
	private string? _url;

	public GenericPackageIdentity()
	{
		Source = Id = string.Empty;
	}

	public GenericPackageIdentity(IPackageIdentity packageIdentity)
	{
		Source = packageIdentity.Source;
		Id = packageIdentity.Id;
		Name = packageIdentity.Name;
		Url = packageIdentity.Url;
		Version = packageIdentity.Version;
	}

	public GenericPackageIdentity(string source, string id, string? name = null, string? url = null, string? version = null)
	{
		Source = source;
		Id = id;
		_name = name;
		_url = url;
		Version = version;
	}

	public string Source { get; set; }
	public string Id { get; set; }
	public string Name { get => _name ?? this.GetWorkshopInfo()?.Name ?? string.Empty; set => _name = value; }
	public string? Url { get => _url ?? this.GetWorkshopInfo()?.Url; set => _url = value; }
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
