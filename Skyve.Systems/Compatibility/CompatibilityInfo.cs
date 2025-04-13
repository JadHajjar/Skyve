using Newtonsoft.Json;

using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;


using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain;
public class CompatibilityInfo : ICompatibilityInfo, ICompatibilityPackageIdentity
{
	public ulong Id { get; set; }
	public string Name { get; set; }
	public string Url { get; set; }
	public List<ReportItem> ReportItems { get; set; }
	[JsonIgnore] public IIndexedPackageCompatibilityInfo? Data { get; }
	[JsonIgnore] public ILocalPackageData? LocalData { get; }

	IPackageCompatibilityInfo? ICompatibilityInfo.Info => Data;
	IEnumerable<ICompatibilityItem> ICompatibilityInfo.ReportItems => ReportItems.Cast<ICompatibilityItem>();
	string? IPackageIdentity.Version { get; set; }
	bool ICompatibilityPackageIdentity.IsDlc { get; }

	[Obsolete("Reserved for DTO", true)]
	public CompatibilityInfo()
	{
		Name = string.Empty;
		Url = string.Empty;
		ReportItems = [];
	}

	public CompatibilityInfo(IPackageIdentity package, IIndexedPackageCompatibilityInfo? packageData)
	{
		Id = package.Id;
		Name = package.Name;
		Url = package.Url ?? string.Empty;
		Data = packageData;
		ReportItems = [];
		LocalData = package.GetLocalPackage();
	}

	public void Add(ReportType type, IGenericPackageStatus status, string? packageName, CompatibilityPackageReference[] packages)
	{
		ReportItems.Add(new ReportItem
		{
			Package = this.GetPackage(),
			PackageId = Data?.Id ?? 0,
			Type = type,
			Status = status,
			PackageName = packageName,
			Packages = packages
		});
	}

	public void AddWithLocale(ReportType type, IGenericPackageStatus status, string? _, string localeKey, object[] localeParams)
	{
		ReportItems.Add(new ReportItem
		{
			Package = this.GetPackage(),
			PackageId = Data?.Id ?? 0,
			Type = type,
			Status = status,
			LocaleKey = localeKey,
			LocaleParams = localeParams
		});
	}

	public override string ToString()
	{
		return Name;
	}
}
