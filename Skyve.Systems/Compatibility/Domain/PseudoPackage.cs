﻿using Newtonsoft.Json;

using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Drawing;

namespace Skyve.Systems.Compatibility.Domain;

public class PseudoPackage : IPackageIdentity
{
	private readonly IPackage? _iPackage;

	public PseudoPackage(ulong id)
	{
		Id = id;
	}

	public PseudoPackage(IPackage iPackage)
	{
		Id = Package.Id;
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