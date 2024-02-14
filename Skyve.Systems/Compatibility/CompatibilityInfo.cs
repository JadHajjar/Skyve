using Extensions;

using Newtonsoft.Json;

using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Enums;


using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain;
public class CompatibilityInfo : ICompatibilityInfo
{
	public ulong Id { get; set; }
	public string Name { get; set; }
	public string Url { get; set; }
	public List<ReportItem> ReportItems { get; set; }
	[JsonIgnore] public IIndexedPackageCompatibilityInfo? Data { get; }

	IPackageCompatibilityInfo? ICompatibilityInfo.Info => Data;
	IEnumerable<ICompatibilityItem> ICompatibilityInfo.ReportItems => ReportItems.Cast<ICompatibilityItem>();

	[Obsolete("Reserved for DTO", true)]
	public CompatibilityInfo()
	{
		Name = string.Empty;
		Url = string.Empty;
		ReportItems = [];
	}

	public CompatibilityInfo(IPackageIdentity package, IIndexedPackageCompatibilityInfo	? packageData)
	{
		Id = package.Id;
		Name = package.Name;
		Url = package.Url ?? string.Empty;
		Data = packageData;
		ReportItems = [];
	}

	public void Add(ReportType type, IGenericPackageStatus status, string packageName, GenericPackageIdentity[] packages)
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

	public void AddWithLocale(ReportType type, IGenericPackageStatus status, string localeKey, object[] localeParams)
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
