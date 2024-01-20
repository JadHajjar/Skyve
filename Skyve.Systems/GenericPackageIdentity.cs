using Skyve.Domain;

namespace Skyve.Systems;
public readonly struct GenericPackageIdentity(ulong id) : IPackageIdentity
{
	public ulong Id { get; } = id;
	public readonly string Name => this.GetWorkshopInfo()?.Name ?? string.Empty;
	public readonly string? Url => this.GetWorkshopInfo()?.Url;
}
