using Extensions;

using Newtonsoft.Json;

using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain;

public class ReportItem : ICompatibilityItem
{
	private string? message;

	public ulong PackageId { get; set; }
	public string? PackageName { get; set; }
	public ReportType Type { get; set; }
	public GenericPackageIdentity[]? Packages { get; set; }

	public string? LocaleKey { get; set; }
	public object[]? LocaleParams { get; set; }

	[JsonIgnore] public string? Message => message ??= GetMessage();

#nullable disable
	[JsonIgnore] public IGenericPackageStatus Status { get; set; }
	[JsonIgnore] internal IPackage Package { get; set; }
	public GenericPackageStatus StatusDTO { get => Status is null ? null : new GenericPackageStatus(Status); set => Status = value?.ToGenericPackage(); }
#nullable enable

	IEnumerable<IPackageIdentity> ICompatibilityItem.Packages => Packages?.Cast<IPackageIdentity>() ?? [];

	private string GetMessage()
	{
		if (LocaleKey is not null && LocaleParams is not null)
		{
			return LocaleHelper.GetGlobalText(LocaleKey).Format(LocaleParams);
		}

		var workshopService = ServiceCenter.Get<IWorkshopService>();
		var packageUtil = ServiceCenter.Get<IPackageNameUtil>();
		var translation = LocaleHelper.GetGlobalText(Status.LocaleKey);
		var action = LocaleHelper.GetGlobalText($"Action_{Status.Action}");
		var text = Packages?.Length switch { 0 => translation.Zero, 1 => translation.One, _ => translation.Plural } ?? translation.One;
		var actionText = Packages?.Length switch { 0 => action.Zero, 1 => action.One, _ => action.Plural } ?? action.One;

		return string.Format($"{text}\r\n\r\n{actionText}", PackageName, Packages?.Length is null or 0 ? string.Empty : packageUtil.CleanName(workshopService.GetInfo(Packages.FirstOrDefault())), true).Trim();
	}
}
