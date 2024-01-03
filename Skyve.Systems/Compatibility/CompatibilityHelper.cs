using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;
using Skyve.Systems.Compatibility.Domain.Api;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility;
public class CompatibilityHelper
{
	private readonly CompatibilityManager _compatibilityManager;
	private readonly IPackageManager _contentManager;
	private readonly IPackageUtil _contentUtil;
	private readonly IPackageNameUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly PackageAvailabilityService _packageAvailabilityService;

	private readonly Dictionary<ulong, List<ulong>> _missingItems = new();

	public CompatibilityHelper(CompatibilityManager compatibilityManager, IPackageManager contentManager, IPackageUtil contentUtil, IPackageNameUtil packageUtil, IWorkshopService workshopService, ILogger logger)
	{
		_compatibilityManager = compatibilityManager;
		_contentManager = contentManager;
		_contentUtil = contentUtil;
		_packageUtil = packageUtil;
		_workshopService = workshopService;
		_packageAvailabilityService = new(_contentManager, _contentUtil, logger, _compatibilityManager);
	}

	public void HandleStatus(CompatibilityInfo info, IndexedPackageStatus status)
	{
		var type = status.Status.Type;

		if (type is StatusType.SourceAvailable or StatusType.StandardMod)
		{
			return;
		}

		if (type is StatusType.DependencyMod && _contentUtil.GetPackagesThatReference(info, true).Any())
		{
			return;
		}

		if (type is StatusType.Deprecated && status.Status.Action is StatusAction.Switch && (status.Status.Packages?.Any() ?? false))
		{
			if (info.Data?.SucceededBy is not null || HandleSucceededBy(info, status.Status.Packages))
			{
				return;
			}
		}

		var packages = status.Status.Packages?.ToList() ?? new();

		if (status.Status.Action is StatusAction.Switch && status.Status.Type is not StatusType.MissingDlc and not StatusType.TestVersion)
		{
			packages = packages.Select(x => _compatibilityManager.GetFinalSuccessor(new GenericPackageIdentity(x)).Id).Distinct().ToList();
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
			packages.Insert(0, info.Id);
		}

		info.Add(reportType, status.Status, info.CleanName(true), packages.ToArray());
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

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages && info.IsIncluded() != true)
		{
			return;
		}

		var packages = interaction.Interaction.Packages?.ToList() ?? new();

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages || interaction.Interaction.Action is StatusAction.Switch)
		{
			packages = packages.Select(x => _compatibilityManager.GetFinalSuccessor(new GenericPackageIdentity(x)).Id).Distinct().ToList();
		}

		if (type is InteractionType.SameFunctionality or InteractionType.CausesIssuesWith or InteractionType.IncompatibleWith)
		{
			if (info.IsIncluded() != true)
			{
				return;
			}

			packages.RemoveAll(x => !_packageAvailabilityService.IsPackageEnabled(x, false));
		}
		else if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages)
		{
			packages.RemoveAll(x => _packageAvailabilityService.IsPackageEnabled(x, true));
		}

		if (interaction.Interaction.Action is StatusAction.SelectOne or StatusAction.Switch or StatusAction.SubscribeToPackages)
		{
			packages.RemoveAll(ShouldNotBeUsed);
		}

		packages.Remove(info.Id);

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

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages && info.Data is not null && _packageAvailabilityService.IsPackageEnabled(info.Data.Package.Id, false))
		{
			lock (_missingItems)
			{
				foreach (var item in packages)
				{
					if (_missingItems.ContainsKey(item))
					{
						_missingItems[item].AddIfNotExist(info.Data.Package.Id);
					}
					else
					{
						_missingItems[item] = new() { info.Data.Package.Id };
					}
				}
			}
		}

		if (interaction.Interaction.Action is StatusAction.SelectOne)
		{
			packages.Add(info.Id);
		}

		info.Add(reportType, interaction.Interaction, info.CleanName(true), packages.ToArray());
	}

	private bool HandleSucceededBy(CompatibilityInfo info, IEnumerable<ulong> packages)
	{
		foreach (var item in packages)
		{
			if (_packageAvailabilityService.IsPackageEnabled(item, true))
			{
				HandleStatus(info, new PackageStatus(StatusType.Succeeded, StatusAction.UnsubscribeThis) { Packages = new[] { item } });

				return true;
			}
		}

		return false;
	}

	private bool ShouldNotBeUsed(ulong id)
	{
		var workshopItem = _workshopService.GetInfo(new GenericPackageIdentity(id));

		return (workshopItem is not null && (_compatibilityManager.IsBlacklisted(workshopItem) || workshopItem.IsRemoved))
			|| (_compatibilityManager.CompatibilityData.Packages.TryGetValue(id, out var package)
			&& (package.Package.Stability is PackageStability.Broken
			|| (package.Package.Statuses?.Any(x => x.Type is StatusType.Deprecated) ?? false)));
	}

	internal void UpdateInclusionStatus(IPackage package)
	{
		_packageAvailabilityService.UpdateInclusionStatus(package.Id);
	}

	internal List<ulong>? GetRequiredFor(ulong id)
	{
		lock (_missingItems)
		{
			return _missingItems.TryGet(id);
		}
	}
}
