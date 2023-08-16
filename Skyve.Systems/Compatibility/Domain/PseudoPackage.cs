using Newtonsoft.Json;

using Skyve.Domain;
using Skyve.Domain.Systems;

namespace Skyve.Systems.Compatibility.Domain;

public class PseudoPackage : IPackageIdentity
{
	private readonly IPackage? _iPackage;

	public PseudoPackage(ulong steamId)
	{
		Id = steamId;
	}

	public PseudoPackage(IPackage iPackage)
	{
		Id = iPackage.Id;
		_iPackage = iPackage;
	}

	public PseudoPackage()
	{
	}

	public ulong Id { get; set; }
	[JsonIgnore] public string Name => Package?.Name ?? string.Empty;
	[JsonIgnore] public IPackage Package => _iPackage ?? ServiceCenter.Get<IWorkshopService>().GetPackage(new GenericPackageIdentity(Id));
	[JsonIgnore] public string? Url => Package?.Url;

	public IWorkshopInfo? GetWorkshopInfo()
	{
		return Package?.GetWorkshopInfo();
	}

	public static implicit operator ulong(PseudoPackage pkg)
	{
		return pkg.Id;
	}
}