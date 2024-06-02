using Extensions;

using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Systems;
using Skyve.Systems.Compatibility.Domain;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility;
public class CompatibilityHelper
{
	private readonly CompatibilityManager _compatibilityManager;
	private readonly ISettings _settings;
	private readonly IPackageManager _contentManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IWorkshopService _workshopService;
	private readonly ISkyveDataManager _skyveDataManager;
	private readonly PackageAvailabilityService _packageAvailabilityService;

	private readonly Dictionary<ulong, List<ulong>> _missingItems = [];

	public CompatibilityHelper(CompatibilityManager compatibilityManager, ISettings settings, IPackageManager contentManager, IPackageUtil packageUtil, IWorkshopService workshopService, ISkyveDataManager skyveDataManager)
	{
		_compatibilityManager = compatibilityManager;
		_settings = settings;
		_contentManager = contentManager;
		_packageUtil = packageUtil;
		_workshopService = workshopService;
		_skyveDataManager = skyveDataManager;
		_packageAvailabilityService = new PackageAvailabilityService(_contentManager, _packageUtil, _skyveDataManager, _compatibilityManager);
	}

	public void HandleStatus(CompatibilityInfo info, IPackageStatus<StatusType> status)
	{
		var type = status.Type;

		if (type is StatusType.SourceAvailable or StatusType.StandardMod)
		{
			return;
		}

		if (type is StatusType.DependencyMod && (!_packageAvailabilityService.IsPackageEnabled(info.Id, false) || _compatibilityManager.GetPackagesThatReference(info, true).Any()))
		{
			return;
		}

		if (type is StatusType.Deprecated && status.Action is StatusAction.Switch && (status.Packages?.Any() ?? false))
		{
			if (info.Data?.SucceededBy is not null || HandleSucceededBy(info, status.Packages))
			{
				return;
			}
		}

		var packages = status.Packages?.ToList() ?? [];

		if (status.Action is StatusAction.Switch && status.Type is not StatusType.MissingDlc and not StatusType.TestVersion)
		{
			packages = packages.Select(x => _compatibilityManager.GetFinalSuccessor(new GenericPackageIdentity(x)).Id).Distinct().ToList();
		}

		if (status.Action is StatusAction.SelectOne or StatusAction.Switch or StatusAction.SubscribeToPackages)
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

		if (status.Action is StatusAction.SelectOne)
		{
			packages.Insert(0, info.Id);
		}

		info.Add(reportType, status, info.CleanName(true), packages.Select(x => new GenericLocalPackageIdentity(x)).ToArray());
	}

	public void HandleInteraction(CompatibilityInfo info, IPackageStatus<InteractionType> interaction)
	{
		var type = interaction.Type;

		if (type is InteractionType.Successor or InteractionType.RequirementAlternative or InteractionType.LoadAfter)
		{
			return;
		}

		if (type is InteractionType.SucceededBy && interaction.Action is StatusAction.NoAction)
		{
			return;
		}

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages && info.IsIncluded() != true)
		{
			return;
		}

		var packages = interaction.Packages?.ToList() ?? [];

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages || interaction.Action is StatusAction.Switch)
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

		if (interaction.Action is StatusAction.SelectOne or StatusAction.Switch or StatusAction.SubscribeToPackages)
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

		if (type is InteractionType.OptionalPackages && _settings.UserSettings.TreatOptionalAsRequired)
		{
			type = InteractionType.RequiredPackages;
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

		if (type is InteractionType.RequiredPackages or InteractionType.OptionalPackages && info.Data is not null && _packageAvailabilityService.IsPackageEnabled(info.Data.Id, false))
		{
			lock (_missingItems)
			{
				foreach (var item in packages)
				{
					if (_missingItems.ContainsKey(item))
					{
						_missingItems[item].AddIfNotExist(info.Data.Id);
					}
					else
					{
						_missingItems[item] = [info.Data.Id];
					}
				}
			}
		}

		if (interaction.Action is StatusAction.SelectOne)
		{
			packages.Add(info.Id);
		}

		info.Add(reportType, interaction, info.CleanName(true), packages.Select(x => new GenericLocalPackageIdentity(x)).ToArray());
	}

	private bool HandleSucceededBy(CompatibilityInfo info, IEnumerable<ulong> packages)
	{
		foreach (var item in packages)
		{
			if (_packageAvailabilityService.IsPackageEnabled(item, true)  )
			{
				if(_packageAvailabilityService.IsPackageEnabled(info.Id, false))
				HandleStatus(info, new PackageStatus(StatusType.Succeeded, StatusAction.UnsubscribeThis) { Packages = new[] { item } });

				return true;
			}
		}

		return false;
	}

	private bool ShouldNotBeUsed(ulong id)
	{
		var workshopItem = _workshopService.GetInfo(new GenericPackageIdentity(id));

		if (workshopItem is not null && (_skyveDataManager.IsBlacklisted(workshopItem) || workshopItem.IsRemoved))
		{
			return true;
		}

		var package = _skyveDataManager.TryGetPackageInfo(id);

		return package is not null && (package.Stability is PackageStability.Broken || (package.Statuses?.Any(x => x.Type is StatusType.Deprecated) ?? false));
	}

	internal void UpdateInclusionStatus(IPackageIdentity package)
	{
		_packageAvailabilityService.UpdateInclusionStatus(package.Id);
	}

	internal bool IsPackageEnabled(ulong id, bool withAlternativesAndSuccessors)
	{
		return _packageAvailabilityService.IsPackageEnabled(id, withAlternativesAndSuccessors);
	}

	internal List<ulong>? GetRequiredFor(ulong id)
	{
		lock (_missingItems)
		{
			return _missingItems.TryGet(id);
		}
	}

	internal void RefreshCache()
	{
		_missingItems.Clear();
		_packageAvailabilityService.RefreshCache();
	}
}
