using Extensions;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;

namespace Skyve.Compatibility.Domain;
public class _CompatibilityPackageData : IPackageCompatibilityInfo
{
	public ulong Id { get; set; }
	public string? Name { get; set; }
	public string? FileName { get; set; }
	public string? AuthorId { get; set; }
	public string? Note { get; set; }
	public DateTime ReviewDate { get; set; }
	public PackageStability Stability { get; set; }
	public PackageUsage Usage { get; set; } = (PackageUsage)(-1);
	public PackageType Type { get; set; }

	public uint[]? RequiredDLCs { get; set; }
	public List<string>? Tags { get; set; }
	public List<PackageLink>? Links { get; set; }
	public List<PackageStatus>? Statuses { get; set; }
	public List<PackageInteraction>? Interactions { get; set; }

	List<ILink>? IPackageCompatibilityInfo.Links => Links?.ToList(x => (ILink)x);
	List<IPackageStatus<StatusType>> IPackageCompatibilityInfo.Statuses { get => Statuses?.ToList(x => (IPackageStatus<StatusType>)x) ?? []; }
	List<IPackageStatus<InteractionType>> IPackageCompatibilityInfo.Interactions { get => Interactions?.ToList(x => (IPackageStatus<InteractionType>)x) ?? []; }
}
