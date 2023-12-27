using Extensions;

using Newtonsoft.Json;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain.Api;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain;
public class CompatibilityInfo : ICompatibilityInfo
{
	private IPackage? package;
	private DtoLocalPackage? dtoPackage;

	[JsonIgnore] public IPackage? Package => package ?? (dtoPackage is null ? null : package = ServiceCenter.Get<IPackageManager>().GetPackageById(dtoPackage));
	[JsonIgnore] public ILocalPackageData? LocalPackage => Package?.LocalData;
	[JsonIgnore] public IndexedPackage? Data { get; }
	public List<ReportItem> ReportItems { get; set; }
	public DtoLocalPackage? DtoPackage { get => dtoPackage ?? package?.CloneTo<IPackageIdentity, DtoLocalPackage>(); set => dtoPackage = value; }

	ILocalPackageData? ICompatibilityInfo.Package => LocalPackage;
	IPackageCompatibilityInfo? ICompatibilityInfo.Info => Data?.Package;
	IEnumerable<ICompatibilityItem> ICompatibilityInfo.ReportItems
	{
		get
		{
			foreach (var item in ReportItems)
			{
				yield return item;
			}

			var id = Data?.Package.SteamId;

			if (id is not null and not 0 && LocalPackage?.IsIncluded() == false)
			{
				var requiredFor = ServiceCenter.Get<ICompatibilityManager, CompatibilityManager>().GetRequiredFor(id.Value);

				if (requiredFor is not null)
				{
					yield return new ReportItem
					{
						Package = Package,
						PackageId = Data?.Package.SteamId ?? 0,
						Type = ReportType.RequiredItem,
						Status = new PackageInteraction(InteractionType.RequiredItem, StatusAction.IncludeThis),
						PackageName = Package?.CleanName(true),
						Packages = requiredFor.ToArray(x => new PseudoPackage(x))
					};
				}
			}
		}
	}

	[Obsolete("Reserved for DTO", true)]
	public CompatibilityInfo()
	{
		ReportItems = new();
	}

	public CompatibilityInfo(IPackage package, IndexedPackage? packageData)
	{
		this.package = package;
		Data = packageData;
		ReportItems = new();
	}

	public void Add(ReportType type, IGenericPackageStatus status, string packageName, ulong[] packages)
	{
		ReportItems.Add(new ReportItem
		{
			Package = Package,
			PackageId = Data?.Package.SteamId ?? 0,
			Type = type,
			Status = status,
			PackageName = packageName,
			Packages = packages.Select(x => new PseudoPackage(x)).ToArray()
		});
	}

	public void Add(ReportType type, IGenericPackageStatus status, string packageName, PseudoPackage[] packages)
	{
		ReportItems.Add(new ReportItem
		{
			Package = Package,
			PackageId = Data?.Package.SteamId ?? 0,
			Type = type,
			Status = status,
			PackageName = packageName,
			Packages = packages
		});
	}

	public void Add(ReportType type, IGenericPackageStatus status, string localeKey, object[] localeParams)
	{
		ReportItems.Add(new ReportItem
		{
			Package = Package,
			PackageId = Data?.Package.SteamId ?? 0,
			Type = type,
			Status = status,
			LocaleKey = localeKey,
			LocaleParams = localeParams
		});
	}

	#region DtoLocalPackage
#nullable disable

	public class DtoLocalPackage : IPackageIdentity
	{
		public ulong Id { get; }
		public string Name { get; }
		public string Url { get; }

		public bool GetThumbnail(out Bitmap thumbnail, out string thumbnailUrl)
		{
			var info = this.GetWorkshopInfo();

			if (info is not null)
			{
				return info.GetThumbnail(out thumbnail, out thumbnailUrl);
			}

			thumbnail = null;
			thumbnailUrl = null;
			return false;
		}
	}

#nullable enable
	#endregion
}
