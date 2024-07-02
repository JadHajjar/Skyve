using Skyve.Domain;

namespace Skyve.Systems;
public struct GenericPackageIdentity : IPackageIdentity
{
	private string? _name;
	private string? _url;

	public GenericPackageIdentity()
	{

	}

	public GenericPackageIdentity(ulong id, string? name = null, string? url = null)
	{
		Id = id;
		_name = name;
		_url = url;
	}

	public ulong Id { get; set; }
	public string Name { get => _name ?? this.GetWorkshopInfo()?.Name ?? string.Empty; set => _name = value; }
	public string? Url { get => _url ?? this.GetWorkshopInfo()?.Url; set => _url = value; }
}
