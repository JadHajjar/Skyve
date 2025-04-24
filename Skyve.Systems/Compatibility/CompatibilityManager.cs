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
	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IUserService _userService;
	private readonly ISkyveDataManager _skyveDataManager;
	private readonly IPackageManager _packageManager;
	private readonly ICompatibilityUtil _compatibilityUtil;
	private readonly IPackageUtil _packageUtil;
	private readonly IPackageNameUtil _packageNameUtil;
	private readonly IWorkshopService _workshopService;
	private readonly IDlcManager _dlcManager;
	private readonly ICitiesManager _citiesManager;
	private readonly SaveHandler _saveHandler;
	private readonly CompatibilityHelper _compatibilityHelper;

	private CancellationTokenSource? cancellationTokenSource;

	public bool FirstLoadComplete { get; private set; }

	public CompatibilityManager(ISkyveDataManager skyveDataManager, IPackageManager packageManager, ILogger logger, ISettings settings, INotifier notifier, ICompatibilityUtil compatibilityUtil, IPackageUtil contentUtil, ILocale locale, IPackageNameUtil packageUtil, IWorkshopService workshopService, IDlcManager dlcManager, SaveHandler saveHandler, IUserService userService, ICitiesManager citiesManager)
	{
		_skyveDataManager = skyveDataManager;
		_packageManager = packageManager;
		_logger = logger;
		_notifier = notifier;
		_compatibilityUtil = compatibilityUtil;
		_packageUtil = contentUtil;
		_locale = locale;
		_settings = settings;
		_packageNameUtil = packageUtil;
		_workshopService = workshopService;
		_dlcManager = dlcManager;
		_userService = userService;
		_saveHandler = saveHandler;
		_citiesManager = citiesManager;
		_compatibilityHelper = new CompatibilityHelper(this, _settings, _packageManager, _packageUtil, _workshopService, _skyveDataManager, _dlcManager);

		LoadSnoozedData();

		_delayedReportCache = new(1000, CacheReport);

		_notifier.ContentLoaded += _delayedReportCache.Run;
		_notifier.PackageInclusionUpdated += _delayedReportCache.Run;
		_notifier.PlaysetChanged += _delayedReportCache.Run;
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

			_compatibilityHelper.RefreshCache();

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
			var path = _saveHandler.GetPath(SNOOZE_FILE);

			_saveHandler.Load(out List<SnoozedItem>? data, SNOOZE_FILE);

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
			CrossIO.DeleteFile(_saveHandler.GetPath(SNOOZE_FILE));
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

			try
			{
				_saveHandler.Save(_snoozedItems, SNOOZE_FILE);
			}
			catch (Exception ex)
			{
				_logger.Exception(ex);
			}
		}

		if (compatibilityItem is ReportItem reportItem && reportItem.Package is not null)
		{
			_cache[reportItem.Package] = GenerateCompatibilityInfo(reportItem.Package);
		}

		_notifier.OnSnoozeChanged();
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

	public StatusAction GetAction(ICompatibilityInfo info)
	{
		var item = info.ReportItems?.Any() == true ? info.ReportItems.OrderBy(x => IsSnoozed(x) ? 0 : x.Status.Notification).LastOrDefault() : null;

		return item?.Status.Action ?? StatusAction.NoAction;
	}

	public ICompatibilityPackageIdentity GetFinalSuccessor(ICompatibilityPackageIdentity package)
	{
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

		return package;
	}

	internal IEnumerable<ILocalPackageIdentity> FindPackage(IIndexedPackageCompatibilityInfo package, bool withAlternativesAndSuccessors)
	{
		var localPackage = _packageManager.GetPackageById(new GenericPackageIdentity(package.Id));

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		if (localPackage is null || withAlternativesAndSuccessors)
		{
			foreach (var item in _packageManager.GetModsByName(Path.GetFileName(package.FileName)))
			{
				yield return item;
			}
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
		var workshopInfo = package.GetWorkshopInfo();
		var packageData = _skyveDataManager.GetPackageCompatibilityInfo(workshopInfo ?? package);
		var info = new CompatibilityInfo(package, packageData);
		var localPackage = package.GetLocalPackage();
		var isCodeMod = (workshopInfo?.IsCodeMod ?? localPackage?.IsCodeMod ?? false) && IsCodeMod(packageData?.Type);

		if (isCodeMod && localPackage?.FilePath is string filePath && Path.GetExtension(filePath).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
		{
			var modName = Path.GetFileName(filePath);
			var duplicate = _packageManager.GetModsByName(modName);

			if (duplicate.Count > 1)
			{
				duplicate.RemoveAll(x => !_packageUtil.IsIncludedAndEnabled(x));

				if (duplicate.Count > 1)
				{
					info.Add(ReportType.Compatibility
						, new PackageInteraction { Type = InteractionType.Identical, Action = StatusAction.SelectOne }
						, workshopInfo?.Name
						, duplicate.Select(x => new CompatibilityPackageReference(x)).ToArray());
				}
			}
		}

		var isCompatible = IsCompatible((localPackage?.SuggestedGameVersion).IfEmpty(workshopInfo?.SuggestedGameVersion), packageData?.ReviewedGameVersion);

		if (packageData is null)
		{
			return info;
		}

		if (packageData.Id > 0 && !_compatibilityHelper.IsPackageEnabled(package, workshopInfo, true))
		{
			var requiredFor = GetRequiredFor(packageData.Id);

			if (requiredFor is not null)
			{
				info.ReportItems.Add(new ReportItem
				{
					Package = package.GetPackage(),
					PackageId = packageData.Id,
					Type = ReportType.RequiredItem,
					Status = new PackageInteraction(InteractionType.RequiredItem, StatusAction.IncludeThis),
					PackageName = package.CleanName(true),
					Packages = [.. requiredFor]
				});
			}
		}

		var stability = packageData.Stability;
		var author = _userService.TryGetUser(packageData.AuthorId);
		var packageName = workshopInfo?.CleanName(true);

		if (!isCompatible && CRNAttribute.GetNotification(stability) < NotificationType.Warning)
		{
			stability = isCodeMod ? PackageStability.Incompatible : PackageStability.AssetIncompatible;
		}

		if (stability is PackageStability.BreaksOnPatch)
		{
			if (!IsCompatible(packageData.ReviewedGameVersion, null))
			{
				stability = PackageStability.BrokenFromPatch;
			}
		}
		else if (stability is PackageStability.BrokenFromPatch)
		{
			if (!_compatibilityHelper.IsPackageEnabled(package, workshopInfo, false))
			{
				stability = PackageStability.BrokenFromPatchSafe;
			}
			else if (workshopInfo?.ServerTime > packageData.ReviewDate)
			{
				stability = PackageStability.BrokenFromPatchUpdated;
			}
		}

		if (stability is PackageStability.BrokenFromNewVersion)
		{
			if (!_compatibilityHelper.IsPackageEnabled(package, workshopInfo, false))
			{
				stability = PackageStability.BrokenFromNewVersionSafe;
			}
			else if (workshopInfo?.ServerTime > packageData.ReviewDate)
			{
				stability = PackageStability.NotEnoughInformation;
			}
		}

		_compatibilityUtil.PopulatePackageReport(packageData, info, _compatibilityHelper);

		if (author.Malicious && CRNAttribute.GetNotification(stability) <= NotificationType.Caution)
		{
			stability = PackageStability.AuthorMalicious;
		}

		if (packageData.ActiveReports > 4 && CRNAttribute.GetNotification(stability) < NotificationType.Caution)
		{
			stability = PackageStability.NumerousReports;
		}

		if (packageData.Type != PackageType.GenericPackage)
		{
			info.Add(ReportType.Info, new PackageTypeStatus(packageData.Type) { Note = stability is PackageStability.Stable ? packageData.Note : null }, packageName, []);
		}

		if (packageData.SavegameEffect > SavegameEffect.None)
		{
			info.Add(ReportType.Info, new SavegameEffectStatus(packageData.SavegameEffect, packageData.RemovalSteps), packageName, []);
		}

		if (stability is PackageStability.BrokenFromPatch or PackageStability.BrokenFromPatchSafe or PackageStability.BrokenFromPatchUpdated)
		{
			info.AddWithLocale(ReportType.Stability,
				new StabilityStatus(stability, null, false) { Note = packageData.Note },
				packageName,
				$"Stability_{stability}",
				[_citiesManager.GameVersion ?? packageData.ReviewedGameVersion ?? string.Empty, packageName ?? package.Name]);
		}
		else if (stability is PackageStability.BrokenFromNewVersion or PackageStability.BrokenFromNewVersionSafe)
		{
			info.AddWithLocale(ReportType.Stability,
				new StabilityStatus(stability, null, false) { Note = packageData.Note },
				packageName,
				$"Stability_{stability}",
				[workshopInfo?.VersionName is null ? string.Empty : $" (v{workshopInfo?.VersionName})", packageName ?? package.Name]);
		}
		else if (stability is not PackageStability.Stable)
		{
			info.Add(stability is PackageStability.NotReviewed or PackageStability.AssetNotReviewed ? ReportType.Info : ReportType.Stability, new StabilityStatus(stability, null, false) { Note = packageData.Note }, packageName, []);
		}
		else if (!string.IsNullOrEmpty(packageData.Note) && packageData.Type == PackageType.GenericPackage)
		{
			info.Add(ReportType.Info,
				new GenericPackageStatus() { Notification = NotificationType.Info, Note = packageData.Note },
				packageName,
				[]);
		}

		foreach (var status in packageData.IndexedStatuses)
		{
			foreach (var item in status.Value)
			{
				_compatibilityHelper.HandleStatus(info, item.Status);
			}
		}

		if (!package.IsLocal() && isCodeMod && packageData.Type is PackageType.GenericPackage)
		{
			if (!packageData.IndexedStatuses.ContainsKey(StatusType.TestVersion) && !packageData.IndexedStatuses.ContainsKey(StatusType.SourceAvailable) && packageData.Links?.Any(x => x.Type is LinkType.Github) != true && !(workshopInfo?.IsPartialInfo ?? false) && workshopInfo?.Links?.Any(x => x.Type is LinkType.Github) != true && !_userService.IsUserVerified(workshopInfo?.Author))
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus { Type = StatusType.SourceCodeNotAvailable, Action = StatusAction.NoAction });
			}

			if (!packageData.IndexedStatuses.ContainsKey(StatusType.TestVersion) && !(workshopInfo?.IsPartialInfo ?? false) && workshopInfo?.Description is not null && workshopInfo.Description.GetWords().Length <= 25 && !_userService.IsUserVerified(workshopInfo?.Author))
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus { Type = StatusType.IncompleteDescription, Action = StatusAction.NoAction });
			}

			if (!author.Malicious && workshopInfo?.ServerTime.Date < _compatibilityUtil.MinimumModDate && workshopInfo?.ServerTime.Date > DateTime.MinValue && DateTime.UtcNow - workshopInfo?.ServerTime > TimeSpan.FromDays(365) && !packageData.IndexedStatuses.ContainsKey(StatusType.Deprecated))
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus(StatusType.AutoDeprecated));
			}
		}

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

		if (!packageData.IndexedInteractions.ContainsKey(InteractionType.RequiredPackages)
			&& _compatibilityHelper.IsPackageEnabled(package, workshopInfo, false)
			&& (workshopInfo?.Requirements.Any(x => x is not IDlcInfo) ?? false))
		{
			_compatibilityHelper.HandleInteraction(info, new PackageInteraction
			{
				Type = InteractionType.RequiredPackages,
				Action = StatusAction.SubscribeToPackages,
				Packages = workshopInfo?.Requirements.Where(x => x is not IDlcInfo).ToList(x => new CompatibilityPackageReference(x))
			});
		}

		if (_compatibilityHelper.IsPackageEnabled(package, workshopInfo, true)
			&& ((packageData.RequiredDLCs?.Any() ?? false) || (workshopInfo?.Requirements.Any(x => x is IDlcInfo) ?? false)))
		{
			var missing = new List<CompatibilityPackageReference>();

			foreach (var dlc in packageData.RequiredDLCs ?? [])
			{
				if (dlc > 10 && !_dlcManager.IsAvailable(dlc))
				{
					missing.Add(new CompatibilityPackageReference(_dlcManager.TryGetDlc(dlc)));
				}
			}

			foreach (var dlc in workshopInfo?.Requirements?.OfType<IDlcInfo>() ?? [])
			{
				if (dlc.Id > 10 && !_dlcManager.IsAvailable(dlc))
				{
					missing.Add(new CompatibilityPackageReference(dlc));
				}
			}

			if (missing.Count > 0)
			{
				_compatibilityHelper.HandleStatus(info, new PackageStatus
				{
					Type = StatusType.MissingDlc,
					Action = StatusAction.NoAction,
					Packages = missing
				});
			}
		}

		if (author.Malicious)
		{
			info.AddWithLocale(ReportType.Info,
				new StabilityStatus(PackageStability.Broken, null, false) { Action = StatusAction.UnsubscribeThis },
				packageName,
				"AuthorMalicious",
				[_packageNameUtil.CleanName(package, true), (workshopInfo?.Author?.Name).IfEmpty(author.Name)]);
		}
		else if (package.GetPackage()?.IsCodeMod == true && author.Retired)
		{
			info.AddWithLocale(ReportType.Stability,
				new StabilityStatus(PackageStability.AuthorRetired, null, false),
				packageName,
				"AuthorRetired",
				[_packageNameUtil.CleanName(package, true), (workshopInfo?.Author?.Name).IfEmpty(author.Name)]);
		}

		if (package.IsLocal())
		{
			info.Add(ReportType.Info,
				new StabilityStatus(PackageStability.Local, null, false) { Action = _compatibilityHelper.IsPackageEnabled(packageData, false) ? StatusAction.NoAction : StatusAction.Switch },
				_packageNameUtil.CleanName(workshopInfo, true),
				[new CompatibilityPackageReference(packageData)]);
		}

		if (!package.IsLocal() && !author.Malicious)
		{
			info.AddWithLocale(ReportType.Info,
				new StabilityStatus(stability is PackageStability.NotReviewed ? PackageStability.NotReviewed : PackageStability.ReiewRequest, string.Empty, true),
				packageName,
				nameof(LocaleCR.RequestReviewInfo),
				[]);
		}

		return info;
	}

	private bool IsCodeMod(PackageType? type)
	{
		return type is null or PackageType.GenericPackage or PackageType.VisualMod or PackageType.SimulationMod;
	}

	private bool IsCompatible(string? version, string? reviewedGameVersion)
	{
		if (version is null or "" || _citiesManager.GameVersion is "" || reviewedGameVersion == _citiesManager.GameVersion)
		{
			return true;
		}

		return ExtensionClass.IsPatternMatch(_citiesManager.GameVersion, version);
	}

	internal List<ICompatibilityPackageIdentity>? GetRequiredFor(ulong id)
	{
		return _compatibilityHelper.GetRequiredFor(id);
	}

	public IEnumerable<IPackage> GetPackagesThatReference(IPackageIdentity package, bool withExcluded = false)
	{
		var packages = withExcluded || _settings.UserSettings.ShowAllReferencedPackages
			? _packageManager.Packages.ToList()
			: _packageManager.Packages.AllWhere(x => _compatibilityHelper.IsPackageEnabled(x, false));

		foreach (var localPackage in packages)
		{
			foreach (var requirement in localPackage.GetWorkshopInfo()?.Requirements ?? [])
			{
				if (GetFinalSuccessor(new CompatibilityPackageReference(requirement))?.Id == package.Id)
				{
					yield return localPackage;
				}
			}
		}
	}
}
