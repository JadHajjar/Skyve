using Extensions;

using Newtonsoft.Json;

using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain;

public class ReportItem : ICompatibilityItem
{
	public ulong PackageId { get; set; }
	public string? PackageName { get; set; }
	public ReportType Type { get; set; }
	public string? LocaleKey { get; set; }
	[JsonIgnore] public ILocalPackageIdentity[]? Packages { get; set; }
	public object[]? LocaleParams { get; set; }
	[JsonProperty("Packages")] public GenericLocalPackageIdentity[]? DtoPackages { get => Packages?.Select(x => new GenericLocalPackageIdentity(x)).ToArray() ?? []; set => Packages = value.Cast<ILocalPackageIdentity>().ToArray(); }

#nullable disable
	[JsonIgnore] public IGenericPackageStatus Status { get; set; }
	[JsonIgnore] internal IPackage Package { get; set; }
	public GenericPackageStatus StatusDTO { get => Status is null ? null : new GenericPackageStatus(Status); set => Status = value?.ToGenericPackage(); }
#nullable enable

	ulong IPackageIdentity.Id => PackageId;
	string IPackageIdentity.Name => PackageName ?? PackageId.ToString();
	string? IPackageIdentity.Url { get; }
	IEnumerable<IPackageIdentity> ICompatibilityItem.Packages => Packages?.Cast<IPackageIdentity>() ?? [];
	string? IPackageIdentity.Version { get; set; }

	public string GetMessage(IWorkshopService workshopService, IPackageNameUtil packageNameUtil)
	{
		try
		{
			if (LocaleKey is not null && LocaleParams is not null)
			{
				return LocaleHelper.GetGlobalText(LocaleKey).Format(LocaleParams);
			}

			var translation = LocaleHelper.GetGlobalText(Status.LocaleKey);
			var action = LocaleHelper.GetGlobalText($"Action_{Status.Action}");
			var text = Packages?.Length switch { 0 => translation.Zero, 1 => translation.One, _ => translation.Plural } ?? translation.One;
			var actionText = Packages?.Length switch { 0 => action.Zero, 1 => action.One, _ => action.Plural } ?? action.One;

			return string.Format($"{text}\r\n\r\n{actionText}", PackageName, Packages?.Length is null or 0 ? string.Empty : packageNameUtil.CleanName(workshopService.GetInfo(Packages.FirstOrDefault())), true).Trim();
		}
		catch (Exception ex)
		{
			return $"TEXT_ERROR: {ex.GetType().Name} - {ex.Message}";
		}
	}

	public override bool Equals(object? obj)
	{
		return obj is ReportItem item &&
			   PackageId == item.PackageId &&
			   Type == item.Type &&
			   (Packages?.SequenceEqual(item.Packages ?? []) ?? item.Packages is null) &&
			   Status.Equals(item.Status);
	}

	public override int GetHashCode()
	{
		var hashCode = 109806218;
		hashCode = (hashCode * -1521134295) + PackageId.GetHashCode();
		hashCode = (hashCode * -1521134295) + Type.GetHashCode();
		hashCode = (hashCode * -1521134295) + EqualityComparer<GenericLocalPackageIdentity[]?>.Default.GetHashCode(DtoPackages);
		hashCode = (hashCode * -1521134295) + Status.GetHashCode();
		return hashCode;
	}

	public static bool operator ==(ReportItem? left, ReportItem? right)
	{
		return EqualityComparer<ReportItem>.Default.Equals(left, right);
	}

	public static bool operator !=(ReportItem? left, ReportItem? right)
	{
		return !(left == right);
	}
}
