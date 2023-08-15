using SkyveApp.Domain.Enums;

using System;
using System.Collections.Generic;

namespace SkyveApp.Domain;
public interface IPackageCompatibilityInfo
{
	string? Name { get; }
	string? FileName { get; }
	ulong AuthorId { get; }
	string? Note { get; }
	DateTime ReviewDate { get; }
	PackageStability Stability { get; }
	PackageUsage Usage { get; }
	PackageType Type { get; }
	uint[]? RequiredDLCs { get; }
	List<string>? Tags { get; }
	List<ILink>? Links { get; }
	List<IPackageStatus<InteractionType>>? Interactions { get; set; }
	List<IPackageStatus<StatusType>>? Statuses { get; set; }
	Dictionary<ulong, IPackageCompatibilityInfo> Group { get; }
}
