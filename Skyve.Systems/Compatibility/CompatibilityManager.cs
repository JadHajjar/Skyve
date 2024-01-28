using Extensions;

using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Skyve.Systems.Compatibility;

public class CompatibilityManager : ICompatibilityManager
{
	private const string SNOOZE_FILE = "CompatibilitySnoozed.json";

	private readonly DelayedAction _delayedReportCache;
	private readonly List<SnoozedItem> _snoozedItems = [];
	private readonly Dictionary<IPackageIdentity, CompatibilityInfo> _cache = new(new IPackageEqualityComparer());

	private readonly ILocale _locale;
	private readonly ILogger _logger;
	private readonly INotifier _notifier;
	private readonly ISkyveDataManager _skyveDataManager;
	private readonly IPackageManager _packageManager;
	private readonly ICompatibilityUtil _compatibilityUtil;
	private readonly IPackageUtil _contentUtil;
	private readonly IPackageNameUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly IDlcManager _dlcManager;
	private readonly CompatibilityHelper _compatibilityHelper;
	private readonly ISkyveApiUtil _skyveApiUtil;

	private CancellationTokenSource? cancellationTokenSource;

	public event Action? SnoozeChanged;

	public bool FirstLoadComplete { get; private set; }

	public CompatibilityManager(ISkyveDataManager skyveDataManager, IPackageManager packageManager, ILogger logger, INotifier notifier, ICompatibilityUtil compatibilityUtil, IPackageUtil contentUtil, ILocale locale, IPackageNameUtil packageUtil, IWorkshopService workshopService, ISkyveApiUtil skyveApiUtil, IDlcManager dlcManager)
	{
		_skyveDataManager = skyveDataManager;
		_packageManager = packageManager;
		_logger = logger;
		_notifier = notifier;
		_compatibilityUtil = compatibilityUtil;
		_contentUtil = contentUtil;
		_locale = locale;
		_packageUtil = packageUtil;
		_workshopService = workshopService;
		_skyveApiUtil = skyveApiUtil;
		_dlcManager = dlcManager;
		_compatibilityHelper = new CompatibilityHelper(this, _packageManager, _contentUtil, _packageUtil, _workshopService, _logger, _skyveDataManager);

		LoadSnoozedData();

		_delayedReportCache = new(1000, CacheReport);

		_notifier.ContentLoaded += _delayedReportCache.Run;
		_notifier.PackageInclusionUpdated += _delayedReportCache.Run;
	}

	public void CacheReport()
	{
		CacheReport(false);
	}

	private void CacheReport(bool first)
	{
		if (!FirstLoadComplete && !first)
		{
			return;
		}

		_logger.Info("[Compatibility] Caching Compatibility Report");

		try
		{
			cancellationTokenSource?.Cancel();
			cancellationTokenSource = new();

			_logger.Info("[Compatibility] Compatibility Service Ready");

			CacheReport(_cache, cancellationTokenSource.Token);

			if (first)
			{
				FirstLoadComplete = true;
			}

			_notifier.OnInformationUpdated();

			_notifier.OnCompatibilityReportProcessed();
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, "Error while caching the Compatibility Report");
		}
	}

	public void QuickUpdate(ICompatibilityItem compatibilityItem)
	{
		var reportItem = (ReportItem)compatibilityItem;

		if (reportItem.Package is not null)
		{
			_compatibilityHelper.UpdateInclusionStatus(reportItem.Package);
		}

		if (reportItem.Packages is not null)
		{
			foreach (var item in reportItem.Packages)
			{
				_compatibilityHelper.UpdateInclusionStatus(item);
			}
		}

		if (reportItem.Package is not null)
		{
			_cache[reportItem.Package] = GenerateCompatibilityInfo(reportItem.Package);
		}

		if (reportItem.Packages is not null)
		{
			foreach (var item in reportItem.Packages)
			{
				_cache[item] = GenerateCompatibilityInfo(item);
			}
		}

		_notifier.OnCompatibilityReportProcessed();
	}

	internal void LoadSnoozedData()
	{
		try
		{
			var path = ISave.GetPath(SNOOZE_FILE);

			ISave.Load(out List<SnoozedItem>? data, SNOOZE_FILE);

			if (data is not null)
			{
				_snoozedItems.AddRange(data);
			}
		}
		catch { }
	}

	public void DoFirstCache()
	{
		CacheReport(true);
	}

	public void ResetSnoozes()
	{
		_snoozedItems.Clear();

		try
		{
			CrossIO.DeleteFile(ISave.GetPath(SNOOZE_FILE));
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, "Failed to clear Snoozes");
		}
	}

	public bool IsSnoozed(ICompatibilityItem reportItem)
	{
		return _snoozedItems.Any(x => x.Equals(reportItem));
	}

	public void ToggleSnoozed(ICompatibilityItem compatibilityItem)
	{
		lock (this)
		{
			if (IsSnoozed(compatibilityItem))
			{
				_snoozedItems.RemoveAll(x => x.Equals(compatibilityItem));
			}
			else
			{
				_snoozedItems.Add(new SnoozedItem(compatibilityItem));
			}

			ISave.Save(_snoozedItems, SNOOZE_FILE);
		}

		if (compatibilityItem is ReportItem reportItem && reportItem.Package is not null)
		{
			_cache[reportItem.Package] = GenerateCompatibilityInfo(reportItem.Package);
		}

		SnoozeChanged?.Invoke();
		_notifier.OnRefreshUI();
	}

	public ICompatibilityInfo GetCompatibilityInfo(IPackageIdentity package, bool noCache = false, bool cacheOnly = false)
	{
		if (!FirstLoadComplete)
		{
		}
		else if (!noCache && _cache.TryGetValue(package, out var info))
		{
			return info;
		}
		else if (cacheOnly)
		{
		}
		else
		{
			return _cache[package] = GenerateCompatibilityInfo(package);
		}

		return new CompatibilityInfo(package, _skyveDataManager.GetPackageCompatibilityInfo(package));
	}

	public NotificationType GetNotification(ICompatibilityInfo info)
	{
		return info.ReportItems?.Any() == true ? info.ReportItems.Max(x => IsSnoozed(x) ? 0 : x.Status.Notification) : NotificationType.None;
	}

	public IPackageIdentity GetFinalSuccessor(IPackageIdentity package)
	{
		throw new NotImplementedException();
		//if (!CompatibilityData.Packages.TryGetValue(package.Id, out var indexedPackage))
		//{
		//	return package;
		//}

		//if (indexedPackage.SucceededBy is not null)
		//{
		//	return new GenericPackageIdentity(indexedPackage.SucceededBy.Packages.First().Key);
		//}

		//if (_packageManager.GetPackageById(package) is null)
		//{
		//	foreach (var item in indexedPackage.RequirementAlternatives.Keys)
		//	{
		//		if (_packageManager.GetPackageById(new GenericPackageIdentity(item)) is IPackageIdentity identity)
		//		{
		//			return identity;
		//		}
		//	}
		//}

		//return package;
	}

	internal IEnumerable<IPackage> FindPackage(IIndexedPackageCompatibilityInfo package, bool withAlternativesAndSuccessors)
	{
		var localPackage = _packageManager.GetPackageById(new GenericPackageIdentity(package.Id));

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		localPackage = _packageManager.GetModsByName(Path.GetFileName(package.FileName)).FirstOrDefault(x => x.IsLocal);

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		if (!withAlternativesAndSuccessors || package.SucceededBy is null)
		{
			yield break;
		}

		var packages = package.SucceededBy.Packages
			.SelectMany(x => FindPackage(x.Value, false));

		foreach (var item in packages)
		{
			yield return item;
		}
	}

	internal void CacheReport(Dictionary<IPackageIdentity, CompatibilityInfo> cache, CancellationToken token)
	{
		foreach (var item in _packageManager.Packages)
		{
			var info = GenerateCompatibilityInfo(item);

			if (token.IsCancellationRequested)
			{
				return;
			}

			cache[item] = info;
		}
	}

	internal CompatibilityInfo GenerateCompatibilityInfo(IPackageIdentity package)
	{
#if DEBUG
		var sw = Stopwatch.StartNew();
		var sw2 = Stopwatch.StartNew();
		_logger.Debug("[Compatibility] Starting " + package);
#endif

		var packageData = _skyveDataManager.GetPackageCompatibilityInfo(package);

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

		if (package.GetPackage()?.IsCodeMod == true && package.GetLocalPackageIdentity() is not null)
		{
			var modName = Path.GetFileName(package.GetLocalPackageIdentity()!.FilePath);
			var duplicate = _packageManager.GetModsByName(modName);

			if (duplicate.Count > 1 && duplicate.Count(x => x.LocalData is not null && _contentUtil.IsIncluded(x.LocalData)) > 1)
			{
				info.Add(ReportType.Compatibility
					, new PackageInteraction { Type = InteractionType.Identical, Action = StatusAction.SelectOne }
					, string.Empty
					, duplicate.Select(x => new GenericPackageIdentity(x.Id, x.Name, x.Url)).ToArray());
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] GetDuplicates took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (workshopInfo?.IsIncompatible == true)
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.Incompatible, null, false), string.Empty, []);
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

		var author = _skyveDataManager.TryGetAuthor(packageData.AuthorId);

		if (packageData.Stability is not PackageStability.Stable && workshopInfo?.IsIncompatible != true && !author.Malicious)
		{
			info.Add(ReportType.Stability, new StabilityStatus(packageData.Stability, null, false), string.Empty, []);
		}

		foreach (var status in packageData.IndexedStatuses)
		{
			foreach (var item in status.Value)
			{
				if (item.Status.Action is StatusAction.Switch && packageData.SucceededBy is not null)
				{
					continue;
				}

				_compatibilityHelper.HandleStatus(info, item.Status);
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] HandleStatuses took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (!package.IsLocal() && package.GetPackage()?.IsCodeMod == true && packageData.Type is PackageType.GenericPackage)
		{
			if (!packageData.IndexedStatuses.ContainsKey(StatusType.TestVersion) && !packageData.IndexedStatuses.ContainsKey(StatusType.SourceAvailable) && packageData.Links?.Any(x => x.Type is LinkType.Github) != true)
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus { Type = StatusType.SourceCodeNotAvailable, Action = StatusAction.NoAction });
			}

			if (!packageData.IndexedStatuses.ContainsKey(StatusType.TestVersion) && workshopInfo?.Description is not null && workshopInfo.Description.GetWords().Length <= 30)
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus { Type = StatusType.IncompleteDescription, Action = StatusAction.NoAction });
			}

			if (!author.Malicious && workshopInfo?.ServerTime.Date < _compatibilityUtil.MinimumModDate && DateTime.UtcNow - workshopInfo?.ServerTime > TimeSpan.FromDays(365) && !packageData.IndexedStatuses.ContainsKey(StatusType.Deprecated))
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
			_compatibilityHelper.HandleInteraction(info, packageData.SucceededBy.Status);
		}

		foreach (var interaction in packageData.IndexedInteractions)
		{
			foreach (var item in interaction.Value)
			{
				_compatibilityHelper.HandleInteraction(info, item.Status);
			}
		}

#if DEBUG
		_logger.Debug($"[Compatibility] HandleInteraction took {sw2.ElapsedMilliseconds * 1000:0.000} secs");
		sw2.Restart();
#endif

		if (packageData.RequiredDLCs?.Any() ?? false)
		{
			var missing = packageData.RequiredDLCs.Where(x => !_dlcManager.IsAvailable(x));

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
			info.AddWithLocale(ReportType.Stability, new StabilityStatus(PackageStability.Broken, null, false) { Action = StatusAction.UnsubscribeThis }, "AuthorMalicious", new object[] { _packageUtil.CleanName(package, true), (workshopInfo?.Author?.Name).IfEmpty(author.Name) });
		}
		else if (package.GetPackage()?.IsCodeMod == true && author.Retired)
		{
			info.AddWithLocale(ReportType.Stability, new StabilityStatus(PackageStability.AuthorRetired, null, false), "AuthorRetired", new object[] { _packageUtil.CleanName(package, true), (workshopInfo?.Author?.Name).IfEmpty(author.Name) });
		}

		if (!string.IsNullOrEmpty(packageData.Note))
		{
			info.Add(ReportType.Stability, new GenericPackageStatus() { Notification = NotificationType.Info, Note = packageData.Note }, string.Empty, []);
		}

		if (package.IsLocal())
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.Local, null, false), _packageUtil.CleanName(_workshopService.GetInfo(new GenericPackageIdentity(packageData.Id)), true), [new GenericPackageIdentity(packageData.Id)]);
		}

		if (!package.IsLocal() && !author.Malicious && workshopInfo?.IsIncompatible != true)
		{
			info.Add(ReportType.Stability, new StabilityStatus(PackageStability.Stable, string.Empty, true), (packageData.Stability is not PackageStability.NotReviewed and not PackageStability.AssetNotReviewed ? _locale.Get("LastReviewDate").Format(packageData.ReviewDate.ToReadableString(packageData.ReviewDate.Year != DateTime.Now.Year, ExtensionClass.DateFormat.TDMY)) + "\r\n\r\n" : string.Empty) + _locale.Get("RequestReviewInfo"), []);
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
}
