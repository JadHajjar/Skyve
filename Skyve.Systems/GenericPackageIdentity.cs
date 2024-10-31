using Skyve.Domain;

namespace Skyve.Systems;
public class GenericPackageIdentity : IPackageIdentity
{
	private string? _name;
	private string? _url;

	public GenericPackageIdentity()
	{

	}

	public GenericPackageIdentity(IPackageIdentity packageIdentity)
	{
		Id = packageIdentity.Id;
		Name = packageIdentity.Name;
		Url = packageIdentity.Url;
		Version = packageIdentity.Version;
	}

	public GenericPackageIdentity(ulong id, string? name = null, string? url = null, string? version = null)
	{
		Id = id;
		_name = name;
		_url = url;
		Version = version;
	}

	public ulong Id { get; set; }
	public string Name { get => _name ?? this.GetWorkshopInfo()?.Name ?? string.Empty; set => _name = value; }
	public string? Url { get => _url ?? this.GetWorkshopInfo()?.Url; set => _url = value; }
	public string? Version { get; set; }
}
