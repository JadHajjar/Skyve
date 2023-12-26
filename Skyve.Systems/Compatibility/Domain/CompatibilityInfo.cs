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

using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Skyve.Systems.Compatibility.Domain;
public class CompatibilityInfo : ICompatibilityInfo
{
	private readonly IPackage? package;
	private readonly ILocalPackageData? localPackage;
	private DtoLocalPackage? dtoPackage;

	[JsonIgnore] public IPackage? Package => dtoPackage ?? localPackage ?? package;
	[JsonIgnore] public ILocalPackageData? LocalPackage => dtoPackage ?? localPackage;
	[JsonIgnore] public IndexedPackage? Data { get; }
	public List<ReportItem> ReportItems { get; set; }
	public DtoLocalPackage? DtoPackage { get => dtoPackage ?? localPackage?.CloneTo<ILocalPackageData, DtoLocalPackage>(); set => dtoPackage = value; }

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
						Package = localPackage,
						PackageId = Data?.Package.SteamId ?? 0,
						Type = ReportType.RequiredItem,
						Status = new PackageInteraction(InteractionType.RequiredItem, StatusAction.IncludeThis),
						PackageName = localPackage?.CleanName(true),
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
		localPackage = package is ILocalPackageData lp ? lp : package.LocalPackage;
		Data = packageData;
		ReportItems = new();
	}

	public void Add(ReportType type, IGenericPackageStatus status, string packageName, ulong[] packages)
	{
		ReportItems.Add(new ReportItem
		{
			Package = localPackage,
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
			Package = localPackage,
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
			Package = localPackage,
			PackageId = Data?.Package.SteamId ?? 0,
			Type = type,
			Status = status,
			LocaleKey = localeKey,
			LocaleParams = localeParams
		});
	}

	#region DtoLocalPackage
#nullable disable

	public class DtoLocalPackage : ILocalPackageData
	{
		[JsonIgnore] public ILocalPackageData LocalPackage => this;
		[JsonIgnore] public IEnumerable<IPackageRequirement> Requirements => this.GetWorkshopInfo()?.Requirements ?? Enumerable.Empty<IPackageRequirement>();
		[JsonIgnore] public IAsset[] Assets => Array.Empty<IAsset>();
		public long LocalSize { get; set; }
		public DateTime LocalTime { get; set; }
		public string Folder { get; set; }
		[JsonProperty("IsMod")] public bool IsCodeMod { get; set; }
		public bool IsLocal { get; set; }
		public bool IsBuiltIn { get; set; }
		public string FilePath { get; set; }
		public ulong Id { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string Version { get; set; }

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
