using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;
using Skyve.Systems.Compatibility.Domain.Api;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Skyve.Systems.Compatibility;

public class CompatibilityManager : ICompatibilityManager
{
	private const string DATA_CACHE_FILE = "CompatibilityDataCache.json";
	private const string SNOOZE_FILE = "CompatibilitySnoozed.json";

	private readonly DelayedAction _delayedReportCache;
	private readonly Dictionary<IPackageIdentity, CompatibilityInfo> _cache = new(new IPackageEqualityComparer());
	private readonly List<SnoozedItem> _snoozedItems = new();
	private readonly Regex _bracketsRegex = new(@"[\[\(](.+?)[\]\)]", RegexOptions.Compiled);
	private readonly Regex _urlRegex = new(@"(https?|ftp)://(?:www\.)?([\w-]+(?:\.[\w-]+)*)(?:/[^?\s]*)?(?:\?[^#\s]*)?(?:#.*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

	private readonly ILocale _locale;
	private readonly ILogger _logger;
	private readonly INotifier _notifier;
	private readonly IPackageManager _packageManager;
	private readonly ICompatibilityUtil _compatibilityUtil;
	private readonly IPackageUtil _contentUtil;
	private readonly IPackageNameUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly IDlcManager _dlcManager;
	private readonly SkyveApiUtil _skyveApiUtil;

	private CompatibilityService? compatibilityService;
	private CancellationTokenSource? cancellationTokenSource;

	public event Action? SnoozeChanged;

	public IndexedCompatibilityData CompatibilityData { get; private set; }
	public bool FirstLoadComplete { get; private set; }

	public CompatibilityManager(IPackageManager contentManager, ILogger logger, INotifier notifier, ICompatibilityUtil compatibilityUtil, IPackageUtil contentUtil, ILocale locale, IPackageNameUtil packageUtil, IWorkshopService workshopService, SkyveApiUtil skyveApiUtil, IDlcManager dlcManager)
	{
		_packageManager = contentManager;
		_logger = logger;
		_notifier = notifier;
		_compatibilityUtil = compatibilityUtil;
		_contentUtil = contentUtil;
		_locale = locale;
		_packageUtil = packageUtil;
		_workshopService = workshopService;
		_skyveApiUtil = skyveApiUtil;
		_dlcManager = dlcManager;

		CompatibilityData = new(null);

		LoadSnoozedData();

		ConnectionHandler.WhenConnected(() => new BackgroundAction(DownloadData).Run());

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

			compatibilityService = new CompatibilityService(_locale, _logger, _packageManager, _compatibilityUtil, _contentUtil, _packageUtil, _workshopService, _dlcManager,
				new CompatibilityHelper(this, _packageManager, _contentUtil, _packageUtil, _workshopService, _logger), this);

			_logger.Info("[Compatibility] Compatibility Service Ready");

			compatibilityService.CacheReport(_cache, cancellationTokenSource.Token);

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
		if (compatibilityService is null)
		{
			return;
		}

		var reportItem = (ReportItem)compatibilityItem;

		if (reportItem.Package is not null)
		{
			compatibilityService.UpdateInclusionStatus(reportItem.Package);
		}

		if (reportItem.Packages is not null)
		{
			foreach (var item in reportItem.Packages)
			{
				var package = item.Package;

				if (package is not null)
				{
					compatibilityService.UpdateInclusionStatus(package);
				}
			}
		}

		if (reportItem.Package is not null)
		{
			_cache[reportItem.Package] = compatibilityService.GenerateCompatibilityInfo(reportItem.Package);
		}

		if (reportItem.Packages is not null)
		{
			foreach (var item in reportItem.Packages)
			{
				var package = item.Package;

				if (package is not null)
				{
					_cache[package] = compatibilityService.GenerateCompatibilityInfo(package);
				}
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

	public void Start(List<IPackage> packages)
	{
		try
		{
			var path = ISave.GetPath(DATA_CACHE_FILE);

			ISave.Load(out CompatibilityData? data, DATA_CACHE_FILE);

			CompatibilityData = new IndexedCompatibilityData(data);
		}
		catch { }
	}

	public void DoFirstCache()
	{
		CacheReport(true);
	}

	public async void DownloadData()
	{
		try
		{
			var data = await _skyveApiUtil.Catalogue();

			if (data is not null)
			{
				ISave.Save(data, DATA_CACHE_FILE);

				CompatibilityData = new IndexedCompatibilityData(data);

				_delayedReportCache.Run();

				_notifier.OnCompatibilityDataLoaded();

#if DEBUG
				if (System.Diagnostics.Debugger.IsAttached)
				{
					var dic = await _skyveApiUtil.Translations();

					if (dic is not null)
					{
						File.WriteAllText("../../../../SkyveApp.Systems/Properties/CompatibilityNotes.json", Newtonsoft.Json.JsonConvert.SerializeObject(dic, Newtonsoft.Json.Formatting.Indented));
					}
				}
#endif
				return;
			}
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, "Failed to get compatibility data");
		}

		CompatibilityData ??= new IndexedCompatibilityData(new());
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

		if (compatibilityItem is ReportItem reportItem && reportItem.Package is not null && compatibilityService is not null)
		{
			_cache[reportItem.Package] = compatibilityService.GenerateCompatibilityInfo(reportItem.Package);
		}

		SnoozeChanged?.Invoke();
		_notifier.OnRefreshUI();
	}

	public bool IsBlacklisted(IPackageIdentity package)
	{
		return CompatibilityData.BlackListedIds.Contains((ulong)package.Id)
			|| CompatibilityData.BlackListedNames.Contains(package.Name ?? string.Empty)
			|| (package.GetWorkshopInfo()?.IsIncompatible ?? false);
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
		else if (compatibilityService is not null)
		{
			return _cache[package] = compatibilityService.GenerateCompatibilityInfo(package);
		}

		return new CompatibilityInfo(package, GetPackageData(package));
	}

	public CompatibilityPackageData GetAutomatedReport(IPackageIdentity package)
	{
		var info = new CompatibilityPackageData
		{
			Stability = package.GetPackage()?.IsCodeMod == true ? PackageStability.NotReviewed : PackageStability.AssetNotReviewed,
			//SteamId = package.Id,
			Name = package.Name,
			FileName = package.GetLocalPackageIdentity()?.FilePath,
			Links = new(),
			Interactions = new(),
			Statuses = new(),
		};

		var workshopInfo = package.GetWorkshopInfo();

		if (workshopInfo?.Requirements.Any() ?? false)
		{
			info.Interactions.AddRange(workshopInfo.Requirements.GroupBy(x => x.IsOptional).Select(o =>
				new PackageInteraction
				{
					Type = o.Key ? InteractionType.OptionalPackages : InteractionType.RequiredPackages,
					Action = StatusAction.SubscribeToPackages,
					Packages = o.ToArray(x => (ulong)x.Id)
				}));
		}

		var tagMatches = _bracketsRegex.Matches(workshopInfo?.Name ?? string.Empty);

		foreach (Match match in tagMatches)
		{
			var tag = match.Value.ToLower();

			if (tag.ToLower() is "broken")
			{
				info.Stability = PackageStability.Broken;
			}
			else if (tag.ToLower() is "obsolete" or "deprecated" or "abandoned")
			{
				info.Statuses.Add(new(StatusType.Deprecated));
			}
			else if (tag.ToLower() is "alpha" or "experimental" or "beta" or "test" or "testing")
			{
				info.Statuses.Add(new(StatusType.TestVersion));
			}
		}

		_compatibilityUtil.PopulateAutomaticPackageInfo(info, package, workshopInfo);

		if (workshopInfo?.Description is not null)
		{
			var matches = _urlRegex.Matches(workshopInfo.Description);

			foreach (Match match in matches)
			{
				var type = match.Groups[2].Value.ToLower() switch
				{
					"youtube.com" or "youtu.be" => LinkType.YouTube,
					"github.com" => LinkType.Github,
					"discord.com" or "discord.gg" => LinkType.Discord,
					"crowdin.com" => LinkType.Crowdin,
					"buymeacoffee.com" or "patreon.com" or "ko-fi.com" or "paypal.com" => LinkType.Donation,
					_ => LinkType.Other
				};

				if (type is not LinkType.Other)
				{
					info.Links.Add(new PackageLink
					{
						Url = match.Value,
						Type = type,
					});
				}
			}
		}

		return info;
	}

	public void ResetCache()
	{
		_cache.Clear();

		try
		{
			CrossIO.DeleteFile(ISave.GetPath(DATA_CACHE_FILE));
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, "Failed to clear CR cache");
		}

		_delayedReportCache.Run();
	}

	IPackageCompatibilityInfo? ICompatibilityManager.GetPackageInfo(IPackageIdentity package)
	{
		return GetPackageData(package);
	}

	public NotificationType GetNotification(ICompatibilityInfo info)
	{
		return info.ReportItems?.Any() == true ? info.ReportItems.Max(x => IsSnoozed(x) ? 0 : x.Status.Notification) : NotificationType.None;
	}

	public ulong GetIdFromModName(string fileName)
	{
		return CompatibilityData.PackageNames.TryGet(fileName);
	}

	public bool IsUserVerified(IUser author)
	{
		return CompatibilityData.Authors.TryGet(ulong.Parse(author.Id?.ToString()))?.Verified ?? false;
	}

	public IndexedPackage? GetPackageData(IPackageIdentity identity)
	{
		var steamId = identity.Id;

		if (steamId > 0)
		{
			//var packageData = CompatibilityData.Packages.TryGet(steamId);

			//if (packageData is null && identity is IPackage package)
			//{
			//	packageData = new IndexedPackage(GetAutomatedReport(package));

			//	packageData.Load(CompatibilityData.Packages);
			//}

			//return packageData;
		}

		return null;
	}

	public IPackageIdentity GetFinalSuccessor(IPackageIdentity package)
	{
		throw new NotImplementedException();
		if (!CompatibilityData.Packages.TryGetValue((ulong)package.Id, out var indexedPackage))
		{
			return package;
		}

		if (indexedPackage.SucceededBy is not null)
		{
			return new GenericPackageIdentity(indexedPackage.SucceededBy.Packages.First().Key);
		}

		if (_packageManager.GetPackageById(package) is null)
		{
			foreach (var item in indexedPackage.RequirementAlternatives.Keys)
			{
				if (_packageManager.GetPackageById(new GenericPackageIdentity(item)) is IPackageIdentity identity)
				{
					return identity;
				}
			}
		}

		return package;
	}

	internal IEnumerable<IPackage> FindPackage(IndexedPackage package, bool withAlternativesAndSuccessors)
	{
		var localPackage = _packageManager.GetPackageById(new GenericPackageIdentity(package.Package.SteamId));

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		localPackage = _packageManager.GetModsByName(Path.GetFileName(package.Package.FileName)).FirstOrDefault(x => x.IsLocal);

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

	internal List<ulong>? GetRequiredFor(ulong id)
	{
		return compatibilityService?.GetRequiredFor(id);
	}
}
