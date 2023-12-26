using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;
using Skyve.Systems.Compatibility.Domain.Api;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Skyve.Systems.Compatibility;
internal class CompatibilityService
{
	private readonly ILocale _locale;
	private readonly ILogger _logger;
	private readonly IPackageManager _contentManager;
	private readonly ICompatibilityUtil _compatibilityUtil;
	private readonly IPackageUtil _contentUtil;
	private readonly IPackageNameUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly IDlcManager _dlcManager;
	private readonly CompatibilityHelper _compatibilityHelper;
	private readonly CompatibilityManager _compatibilityManager;

	public CompatibilityService(ILocale locale, ILogger logger, IPackageManager contentManager, ICompatibilityUtil compatibilityUtil, IPackageUtil contentUtil, IPackageNameUtil packageUtil, IWorkshopService workshopService, IDlcManager dlcManager, CompatibilityHelper compatibilityHelper, CompatibilityManager compatibilityManager)
	{
		_locale = locale;
		_logger = logger;
		_contentManager = contentManager;
		_compatibilityUtil = compatibilityUtil;
		_contentUtil = contentUtil;
		_packageUtil = packageUtil;
		_workshopService = workshopService;
		_dlcManager = dlcManager;
		_compatibilityHelper = compatibilityHelper;
		_compatibilityManager = compatibilityManager;
	}

	internal void CacheReport(Dictionary<IPackage, CompatibilityInfo> cache, CancellationToken token)
	{
		foreach (var item in _contentManager.Packages)
		{
			var info = GenerateCompatibilityInfo(item);

			if (token.IsCancellationRequested)
			{
				return;
			}

			cache[item] = info;
		}
	}

	internal CompatibilityInfo GenerateCompatibilityInfo(IPackage package)
	{
#if DEBUG
		var sw = Stopwatch.StartNew();
		var sw2 = Stopwatch.StartNew();
		_logger.Debug("[Compatibility] Starting " + package);
#endif

		var packageData = _compatibilityManager.GetPackageData(package);

#if DEBUG
		_logger.Debug($"[Compatibility] GetPackageData took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		var info = new CompatibilityInfo(package, packageData);
		var workshopInfo = package.GetWorkshopInfo();

#if DEBUG
		_logger.Debug($"[Compatibility] GetWorkshopInfo took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (package.IsCodeMod && package.LocalData is not null)
		{
			var modName = Path.GetFileName(package.LocalData.ModFilePath);
			var duplicate = _contentManager.GetModsByName(modName);

			if (duplicate.Count > 1 && duplicate.Count(_contentUtil.IsIncluded) > 1)
			{
				info.Add(ReportType.Compatibility
					, new PackageInteraction { Type = InteractionType.Identical, Action = StatusAction.SelectOne }
					, string.Empty
					, duplicate.Select(x => new PseudoPackage(x)).ToArray());
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] GetDuplicates took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (workshopInfo?.IsIncompatible == true)
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.Incompatible, null, false), string.Empty, new PseudoPackage[0]);
		}

		if (packageData is null)
		{
			return info;
		}

		_compatibilityUtil.PopulatePackageReport(packageData, info, _compatibilityHelper);

#if DEBUG
		_logger.Debug($"[Compatibility] PopulatePackageReport took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		var author = _compatibilityManager.CompatibilityData.Authors.TryGet(packageData.Package.AuthorId) ?? new();

		if (packageData.Package.Stability is not PackageStability.Stable && workshopInfo?.IsIncompatible != true && !author.Malicious)
		{
			info.Add(ReportType.Stability, new StabilityStatus(packageData.Package.Stability, null, false), string.Empty, new PseudoPackage[0]);
		}

		foreach (var status in packageData.Statuses)
		{
			foreach (var item in status.Value)
			{
				if (item.Status.Action is StatusAction.Switch && packageData.SucceededBy is not null)
				{
					continue;
				}

				_compatibilityHelper.HandleStatus(info, item);
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] HandleStatuses took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (!package.IsLocal && package.IsCodeMod && packageData.Package.Type is PackageType.GenericPackage)
		{
			if (!packageData.Statuses.ContainsKey(StatusType.TestVersion) && !packageData.Statuses.ContainsKey(StatusType.SourceAvailable) && packageData.Links?.Any(x => x.Type is LinkType.Github) != true)
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus { Type = StatusType.SourceCodeNotAvailable, Action = StatusAction.NoAction });
			}

			if (!packageData.Statuses.ContainsKey(StatusType.TestVersion) && workshopInfo?.Description is not null && workshopInfo.Description.GetWords().Length <= 30)
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus { Type = StatusType.IncompleteDescription, Action = StatusAction.UnsubscribeThis });
			}

			if (!author.Malicious && workshopInfo?.ServerTime.Date < _compatibilityUtil.MinimumModDate && DateTime.UtcNow - workshopInfo?.ServerTime > TimeSpan.FromDays(365) && !packageData.Statuses.ContainsKey(StatusType.Deprecated))
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus(StatusType.AutoDeprecated));
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] HandleStatusesSecond took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (packageData.SucceededBy is not null)
		{
			_compatibilityHelper.HandleInteraction(info, packageData.SucceededBy);
		}

		foreach (var interaction in packageData.Interactions)
		{
			foreach (var item in interaction.Value)
			{
				_compatibilityHelper.HandleInteraction(info, item);
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] HandleInteraction took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (packageData.Package.RequiredDLCs?.Any() ?? false)
		{
			var missing = packageData.Package.RequiredDLCs.Where(x => !_dlcManager.IsAvailable(x));

			if (missing.Any())
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus
				{
					Type = StatusType.MissingDlc,
					Action = StatusAction.NoAction,
					Packages = missing.Select(x => (ulong)x).ToArray()
				});
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] RequiredDLCs took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (author.Malicious)
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.Broken, null, false) { Action = StatusAction.UnsubscribeThis }, "AuthorMalicious", new object[] { _packageUtil.CleanName(package, true), (workshopInfo?.Author?.Name).IfEmpty(author.Name) });
		}
		else if (package.IsCodeMod && author.Retired)
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.AuthorRetired, null, false), "AuthorRetired", new object[] { _packageUtil.CleanName(package, true), (workshopInfo?.Author?.Name).IfEmpty(author.Name) });
		}

		if (!string.IsNullOrEmpty(packageData.Package.Note))
		{
			info.Add(ReportType.Stability, new GenericPackageStatus() { Notification = NotificationType.Info, Note = packageData.Package.Note }, string.Empty, new PseudoPackage[0]);
		}
		if (package.IsLocal)
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.Local, null, false), _packageUtil.CleanName(_workshopService.GetInfo(new GenericPackageIdentity(packageData.Package.SteamId)), true), new PseudoPackage[] { new(packageData.Package.SteamId) });
		}
		if (!package.IsLocal && !author.Malicious && workshopInfo?.IsIncompatible != true)
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.Stable, string.Empty, true), (packageData.Package.Stability is not PackageStability.NotReviewed and not PackageStability.AssetNotReviewed ? _locale.Get("LastReviewDate").Format(packageData.Package.ReviewDate.ToReadableString(packageData.Package.ReviewDate.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)) + "\r\n\r\n" : string.Empty) + _locale.Get("RequestReviewInfo"), new object[0]);
		}

#if DEBUG
		_logger.Debug($"[Compatibility] Stability took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();

		sw.Stop();
		if (sw.ElapsedMilliseconds > 100)
		{
			_logger.Debug($"[Compatibility] {package.Name} took {sw.ElapsedMilliseconds * 1000:0.000} secs");
		}
#endif

		return info;
	}

	internal List<ulong>? GetRequiredFor(ulong id)
	{
		return _compatibilityHelper.GetRequiredFor(id);
	}

	internal void UpdateInclusionStatus(IPackage package)
	{
		_compatibilityHelper.UpdateInclusionStatus(package);
	}
}
