using Skyve.Domain;

namespace Skyve.Systems;
public readonly struct GenericPackageIdentity : IPackageIdentity
{
	public ulong Id { get; }
	public readonly string Name => this.GetWorkshopInfo()?.Name ?? string.Empty;
	public readonly string? Url => this.GetWorkshopInfo()?.Url;

	public GenericPackageIdentity(ulong id)
	{
		Id = id;
	}
}
