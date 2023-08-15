using Extensions;

using SkyveApp.Domain;
using SkyveApp.Domain.Enums;
using SkyveApp.Domain.Systems;
using SkyveApp.Systems.Compatibility.Domain;
using SkyveApp.Systems.Compatibility.Domain.Api;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkyveApp.Systems.Compatibility;
public class CompatibilityHelper
{
	private readonly CompatibilityManager _compatibilityManager;
	private readonly IPackageManager _contentManager;
	private readonly IPackageUtil _contentUtil;
	private readonly IPackageNameUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly ILocale _locale;

	public CompatibilityHelper(CompatibilityManager compatibilityManager, IPackageManager contentManager, IPackageUtil contentUtil, IPackageNameUtil packageUtil, IWorkshopService workshopService, ILocale locale)
	{
		_compatibilityManager = compatibilityManager;
		_contentManager = contentManager;
		_contentUtil = contentUtil;
		_packageUtil = packageUtil;
		_workshopService = workshopService;
		_locale = locale;
	}

	public void HandleStatus(CompatibilityInfo info, IndexedPackageStatus status)
	{
		var type = status.Status.Type;

		if (type is StatusType.SourceAvailable or StatusType.StandardMod)
		{
			return;
		}

		if (type is StatusType.DependencyMod && info.Package is not null && _contentUtil.GetPackagesThatReference(info.Package, true).Any())
		{
			return;
		}

		if (type is StatusType.Deprecated && status.Status.Action is StatusAction.Switch && (status.Status.Packages?.Any() ?? false))
		{
			if ((info.Data?.Interactions.ContainsKey(InteractionType.SucceededBy) ?? false) || HandleSucceededBy(info, status.Status.Packages))
			{
				return;
			}
		}

		var packages = status.Status.Packages?.ToList() ?? new();

		if (status.Status.Action is StatusAction.Switch && status.Status.Type is not StatusType.MissingDlc and not StatusType.TestVersion)
		{
			packages = packages.Select(x => GetFinalSuccessor(new GenericPackageIdentity(x)).Id).Distinct().ToList();
		}

		if (status.Status.Action is StatusAction.SelectOne or StatusAction.Switch or StatusAction.SubscribeToPackages)
		{
			packages.RemoveAll(ShouldNotBeUsed);

			if (packages.Count == 0)
			{
				return;
			}
		}

		var reportType = type switch
		{
			StatusType.Deprecated => packages.Count == 0 ? ReportType.Stability : ReportType.Successors,
			StatusType.CausesIssues or StatusType.SavesCantLoadWithoutIt or StatusType.AutoDeprecated => ReportType.Stability,
			StatusType.DependencyMod or StatusType.TestVersion or StatusType.MusicCanBeCopyrighted => ReportType.Status,
			StatusType.SourceCodeNotAvailable or StatusType.IncompleteDescription or StatusType.Reupload => ReportType.Ambiguous,
			StatusType.MissingDlc => ReportType.DlcMissing,
			_ => ReportType.Status,
		};

		if (status.Status.Action is StatusAction.SelectOne)
		{
			packages.Insert(0, info.Package?.Id ?? 0);
		}

		info.Add(reportType, status.Status, _packageUtil.CleanName(info.Package, true), packages.ToArray());
	}

	public void HandleInteraction(CompatibilityInfo info, IndexedPackageInteraction interaction)
	{
		var type = interaction.Interaction.Type;

		if (type is InteractionType.Successor or InteractionType.RequirementAlternative or InteractionType.LoadAfter)
		{
			return;
		}

		if (type is InteractionType.SucceededBy && interaction.Interaction.Action is StatusAction.NoAction)
		{
			return;
		}

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages && info.LocalPackage?.IsIncluded() != true)
		{
			return;
		}

		var packages = interaction.Interaction.Packages?.ToList() ?? new();

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages || interaction.Interaction.Action is StatusAction.Switch)
		{
			packages = packages.Select(x => GetFinalSuccessor(new GenericPackageIdentity(x)).Id).Distinct().ToList();
		}

		if (type is InteractionType.SameFunctionality or InteractionType.CausesIssuesWith or InteractionType.IncompatibleWith)
		{
			if (info.LocalPackage?.IsIncluded() != true)
			{
				return;
			}

			packages.RemoveAll(x => !IsPackageEnabled(x, false, false));
		}
		else if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages)
		{
			packages.RemoveAll(x => IsPackageEnabled(x, true, true));
		}

		if (interaction.Interaction.Action is StatusAction.SelectOne or StatusAction.Switch or StatusAction.SubscribeToPackages)
		{
			packages.RemoveAll(ShouldNotBeUsed);
		}

		packages.Remove(info.Package?.Id ?? 0);

		if (packages.Count == 0)
		{
			return;
		}

		if (type is InteractionType.SucceededBy && HandleSucceededBy(info, packages))
		{
			return;
		}

		var reportType = type switch
		{
			InteractionType.SucceededBy => ReportType.Successors,
			InteractionType.Alternative => ReportType.Alternatives,
			InteractionType.SameFunctionality or InteractionType.CausesIssuesWith or InteractionType.IncompatibleWith => ReportType.Compatibility,
			InteractionType.RequiredPackages => ReportType.RequiredPackages,
			InteractionType.OptionalPackages => ReportType.OptionalPackages,
			_ => ReportType.Compatibility
		};

		if (interaction.Interaction.Action is StatusAction.SelectOne)
		{
			packages.Add(info.Package?.Id ?? 0);
		}

		info.Add(reportType, interaction.Interaction, _packageUtil.CleanName(info.Package, true), packages.ToArray());
	}

	private bool HandleSucceededBy(CompatibilityInfo info, IEnumerable<ulong> packages)
	{
		foreach (var item in packages)
		{
			if (IsPackageEnabled(item, true, true))
			{
				HandleStatus(info, new PackageStatus(StatusType.Succeeded, StatusAction.UnsubscribeThis) { Packages = new[] { item } });

				return true;
			}
		}

		return false;
	}

	public IPackageIdentity GetFinalSuccessor(IPackageIdentity package)
	{
		if (!_compatibilityManager.CompatibilityData.Packages.TryGetValue(package.Id, out var indexedPackage))
		{
			return package;
		}

		if (indexedPackage.SucceededBy is not null)
		{
			return new GenericPackageIdentity(indexedPackage.SucceededBy.Packages.First().Key);
		}

		if (_contentManager.GetPackageById(package) is null)
		{
			foreach (var item in indexedPackage.RequirementAlternatives.Keys)
			{
				if (_contentManager.GetPackageById(new GenericPackageIdentity(item)) is IPackageIdentity identity)
				{
					return identity;
				}
			}
		}

		return package;
	}

	private bool IsPackageEnabled(ulong steamId, bool withAlternatives, bool withSuccessors)
	{
		var indexedPackage = _compatibilityManager.CompatibilityData.Packages.TryGet(steamId);

		if (isEnabled(_contentManager.GetPackageById(new GenericPackageIdentity(steamId))))
		{
			return true;
		}

		if (indexedPackage is null)
		{
			return false;
		}

		if (withAlternatives)
		{
			foreach (var item in indexedPackage.RequirementAlternatives)
			{
				if (item.Key != steamId)
				{
					foreach (var package in FindPackage(item.Value, withSuccessors))
					{
						if (isEnabled(package))
						{
							return true;
						}
					}
				}
			}
		}

		foreach (var package in FindPackage(indexedPackage, withSuccessors))
		{
			if (isEnabled(package))
			{
				return true;
			}
		}

		foreach (var item in indexedPackage.Group)
		{
			if (item.Key != steamId)
			{
				foreach (var package in FindPackage(item.Value, withSuccessors))
				{
					if (isEnabled(package))
					{
						return true;
					}
				}
			}
		}

		return false;

		bool isEnabled(ILocalPackage? package) => package is not null && _contentUtil.IsIncludedAndEnabled(package);
	}

	private bool ShouldNotBeUsed(ulong steamId)
	{
		var workshopItem = _workshopService.GetInfo(new GenericPackageIdentity(steamId));

		return (workshopItem is not null && (_compatibilityManager.IsBlacklisted(workshopItem) || workshopItem.IsRemoved))
			|| (_compatibilityManager.CompatibilityData.Packages.TryGetValue(steamId, out var package)
			&& (package.Package.Stability is PackageStability.Broken
			|| (package.Package.Statuses?.Any(x => x.Type is StatusType.Deprecated) ?? false)));
	}

	public IndexedPackage? GetPackageData(IPackageIdentity identity)
	{
		var steamId = identity.Id;

		if (steamId > 0)
		{
			var packageData = _compatibilityManager.CompatibilityData.Packages.TryGet(steamId);

			if (packageData is null && identity is IPackage package)
			{
				packageData = new IndexedPackage(_compatibilityManager.GetAutomatedReport(package));

				packageData.Load(_compatibilityManager.CompatibilityData.Packages);
			}

			return packageData;
		}

		return null;
	}

	internal IEnumerable<ILocalPackage> FindPackage(IndexedPackage package, bool withSuccessors)
	{
		var localPackage = _contentManager.GetPackageById(new GenericPackageIdentity(package.Package.SteamId));

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		localPackage = _contentManager.Mods.FirstOrDefault(x => x.IsLocal && Path.GetFileName(x.FilePath) == package.Package.FileName)?.LocalParentPackage;

		if (localPackage is not null)
		{
			yield return localPackage;
		}

		if (!withSuccessors || !package.Interactions.ContainsKey(InteractionType.SucceededBy))
		{
			yield break;
		}

		var packages = package.Interactions[InteractionType.SucceededBy]
					.SelectMany(x => x.Packages.Values)
					.Where(x => x.Package != package.Package)
					.Select(x => FindPackage(x, true))
					.FirstOrDefault(x => x is not null);

		if (packages is not null)
		{
			foreach (var item in packages)
			{
				yield return item;
			}
		}
	}
}
