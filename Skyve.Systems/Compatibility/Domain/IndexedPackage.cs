using Extensions;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Systems.Compatibility.Domain.Api;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain;

public class IndexedPackage : IPackageCompatibilityInfo
{
	public CompatibilityPackageData Package { get; }
	public Dictionary<ulong, IndexedPackage> Group { get; }
	public Dictionary<ulong, IndexedPackage> RequirementAlternatives { get; }
	public Dictionary<StatusType, List<IndexedPackageStatus>> Statuses { get; }
	public Dictionary<InteractionType, List<IndexedPackageInteraction>> Interactions { get; }
	public IndexedPackageInteraction? SucceededBy { get; set; }

	public IndexedPackage(CompatibilityPackageData package)
	{
		Package = package;
		Statuses = new();
		Group = new();
		RequirementAlternatives = new();
		Interactions = new();
	}

	public void Load(Dictionary<ulong, IndexedPackage> packages)
	{
		if (Package.Statuses is not null)
		{
			foreach (var grp in Package.Statuses.GroupBy(x => x.Type))
			{
				Statuses[grp.Key] = grp.Select(x => new IndexedPackageStatus(x, packages)).ToList();
			}
		}

		if (Package.Interactions is not null)
		{
			foreach (var grp in Package.Interactions.GroupBy(x => x.Type))
			{
				Interactions[grp.Key] = grp.Select(x => new IndexedPackageInteraction(x, packages)).ToList();
			}
		}

		if (Statuses.ContainsKey(StatusType.TestVersion))
		{
			foreach (var item in Statuses[StatusType.TestVersion].SelectMany(x => x.Packages))
			{
				Group[item.Key] = item.Value;

				item.Value.Group[Package.SteamId] = this;
			}
		}

		if (Statuses.ContainsKey(StatusType.Deprecated))
		{
			foreach (var item in Statuses[StatusType.Deprecated].SelectMany(x => x.Packages))
			{
				RequirementAlternatives[item.Key] = item.Value;
			}
		}

		if (Statuses.ContainsKey(StatusType.Reupload))
		{
			foreach (var item in Statuses[StatusType.Reupload].SelectMany(x => x.Packages))
			{
				RequirementAlternatives[item.Key] = item.Value;
			}
		}

		if (Interactions.ContainsKey(InteractionType.RequirementAlternative))
		{
			foreach (var item in Interactions[InteractionType.RequirementAlternative].SelectMany(x => x.Packages))
			{
				RequirementAlternatives[item.Key] = item.Value;
			}
		}

		if (Interactions.ContainsKey(InteractionType.Alternative))
		{
			foreach (var item in Interactions[InteractionType.Alternative].SelectMany(x => x.Packages))
			{
				RequirementAlternatives[item.Key] = item.Value;
			}
		}

		if (Statuses.ContainsKey(StatusType.Deprecated) && Statuses[StatusType.Deprecated].Any(x => x.Packages.Any()))
		{
			SucceededBy = new IndexedPackageInteraction(new() { Type = InteractionType.SucceededBy, Action = StatusAction.Switch, Packages = Statuses[StatusType.Deprecated].SelectMany(x => x.Packages.Keys).ToArray() }, packages);

			//if (!Interactions.ContainsKey(InteractionType.SucceededBy))
			//{
			//	Interactions[InteractionType.SucceededBy] = new() { interaction };
			//}
			//else
			//{
			//	Interactions[InteractionType.SucceededBy].Add(interaction);
			//}
		}
	}

	public void SetUpInteractions(Dictionary<ulong, IndexedPackage> packages)
	{
		if (Interactions.ContainsKey(InteractionType.Successor))
		{
			RecursiveSetSuccessor();
		}

		//if (Interactions.ContainsKey(InteractionType.Alternative))
		//{
		//	foreach (var item in Interactions[InteractionType.Alternative])
		//	{
		//		var linkedPackages = item.Interaction.Packages.ToList();

		//		linkedPackages.Add(Package.SteamId);

		//		foreach (var package in item.Packages.Values)
		//		{
		//			var replacedInteraction = item.Interaction.Clone();

		//			replacedInteraction.Packages = linkedPackages.Where(x => x != package.Package.SteamId).ToArray();

		//			if (package.Interactions.ContainsKey(InteractionType.Alternative))
		//			{
		//				package.Interactions[InteractionType.Alternative][0].Interaction.Packages = linkedPackages.Concat(package.Interactions[InteractionType.Alternative][0].Interaction.Packages ?? new ulong[0]).Distinct().ToArray();
		//			}
		//			else
		//			{
		//				package.Interactions[InteractionType.Alternative] = new() { new(replacedInteraction, packages) };
		//			}
		//		}
		//	}
		//}
	}

	private void RecursiveSetSuccessor()
	{
		foreach (var item in Interactions[InteractionType.Successor])
		{
			foreach (var package in item.Packages.Values)
			{
				if (package.Package.SteamId == Package.SteamId)
				{
					continue;
				}

				if (package.SucceededBy?.Packages.ContainsKey(Package.SteamId) ?? false)
				{
					continue;
				}

				package.SucceededBy = SucceededBy ?? new IndexedPackageInteraction(new()
				{
					Type = InteractionType.SucceededBy,
					Action = item.Interaction.Action,
					Note = item.Interaction.Note,
					Packages = new[] { Package.SteamId }
				}, new() { [Package.SteamId] = this });

				if (package.Interactions.ContainsKey(InteractionType.Successor))
				{
					package.RecursiveSetSuccessor();
				}
			}
		}
	}

	public override bool Equals(object? obj)
	{
		return obj is IndexedPackage package && Package.SteamId == package.Package.SteamId;
	}

	public override int GetHashCode()
	{
		return Package.SteamId.GetHashCode();
	}

	#region IPackageCompatibilityInfo
	public string? Name => ((IPackageCompatibilityInfo)Package).Name;
	public string? FileName => ((IPackageCompatibilityInfo)Package).FileName;
	public ulong AuthorId => ((IPackageCompatibilityInfo)Package).AuthorId;
	public string? Note => ((IPackageCompatibilityInfo)Package).Note;
	public DateTime ReviewDate => ((IPackageCompatibilityInfo)Package).ReviewDate;
	public PackageStability Stability => ((IPackageCompatibilityInfo)Package).Stability;
	public PackageUsage Usage => ((IPackageCompatibilityInfo)Package).Usage;
	public PackageType Type => ((IPackageCompatibilityInfo)Package).Type;
	public uint[]? RequiredDLCs => ((IPackageCompatibilityInfo)Package).RequiredDLCs;
	public List<string>? Tags => ((IPackageCompatibilityInfo)Package).Tags;
	public List<ILink>? Links => ((IPackageCompatibilityInfo)Package).Links;
	List<IPackageStatus<InteractionType>>? IPackageCompatibilityInfo.Interactions { get => ((IPackageCompatibilityInfo)Package).Interactions; set => ((IPackageCompatibilityInfo)Package).Interactions = value; }
	List<IPackageStatus<StatusType>>? IPackageCompatibilityInfo.Statuses { get => ((IPackageCompatibilityInfo)Package).Statuses; set => ((IPackageCompatibilityInfo)Package).Statuses = value; }
	Dictionary<ulong, IPackageCompatibilityInfo> IPackageCompatibilityInfo.Group => Group.ToDictionary(x => x.Key, x => (IPackageCompatibilityInfo)x.Value);
	#endregion
}
