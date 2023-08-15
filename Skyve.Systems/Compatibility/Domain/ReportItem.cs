﻿using Extensions;

using Newtonsoft.Json;

using SkyveApp.Domain;

using SkyveApp.Domain.Enums;
using SkyveApp.Domain.Systems;

using System.Linq;

namespace SkyveApp.Systems.Compatibility.Domain;

public class ReportItem : ICompatibilityItem
{
	private string? message;

	public ulong PackageId { get; set; }
	public string? PackageName { get; set; }
	public ReportType Type { get; set; }
	public PseudoPackage[]? Packages { get; set; }
	public GenericPackageStatus? StatusDTO { get => Status is null ? null : new GenericPackageStatus(Status); set => Status = value?.ToGenericPackage(); }

    public string? LocaleKey { get; set; }
    public object[]? LocaleParams { get; set; }

    [JsonIgnore] public string? Message => message ??= GetMessage();
	[JsonIgnore] public IGenericPackageStatus Status { get; set; }

	IPackage[] ICompatibilityItem.Packages => Packages?.Select(x => x.Package).ToArray() ?? new IPackage[0];

	private string GetMessage()
	{
		if (LocaleKey is not null && LocaleParams is not null)
			return LocaleHelper.GetGlobalText(LocaleKey).Format(LocaleParams);

		var workshopService = ServiceCenter.Get<IWorkshopService>();
		var packageUtil = ServiceCenter.Get<IPackageNameUtil>();
		var translation = LocaleHelper.GetGlobalText(Status.LocaleKey);
		var action = LocaleHelper.GetGlobalText($"Action_{Status.Action}");
		var text = Packages?.Length switch { 0 => translation.Zero, 1 => translation.One, _ => translation.Plural } ?? translation.One;
		var actionText = Packages?.Length switch { 0 => action.Zero, 1 => action.One, _ => action.Plural } ?? action.One;

		return string.Format($"{text}\r\n\r\n{actionText}", PackageName, Packages is null ? string.Empty : packageUtil.CleanName(workshopService.GetInfo(Packages.FirstOrDefault())), true).Trim();
	}
}
