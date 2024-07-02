using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;
using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;

namespace Skyve.Compatibility.Domain.Interfaces;
public interface IPackageCompatibilityInfo
{
	ulong Id { get; }
	string? Name { get; }
	string? FileName { get; }
	string? AuthorId { get; }
	string? Note { get; }
	DateTime ReviewDate { get; }
	string? ReviewedGameVersion { get; }
	PackageStability Stability { get; }
	PackageUsage Usage { get; }
	PackageType Type { get; }
	List<uint>? RequiredDLCs { get; }
	List<string>? Tags { get; }
	List<ILink>? Links { get; }
	List<IPackageStatus<InteractionType>> Interactions { get; }
	List<IPackageStatus<StatusType>> Statuses { get; }
}

public interface IIndexedPackageCompatibilityInfo : IPackageCompatibilityInfo
{
	Dictionary<StatusType, IList<IIndexedPackageStatus<StatusType>>> IndexedStatuses { get; }
	Dictionary<InteractionType, IList<IIndexedPackageStatus<InteractionType>>> IndexedInteractions { get; }
	IIndexedPackageStatus<InteractionType>? SucceededBy { get; }
	Dictionary<ulong, IIndexedPackageCompatibilityInfo> RequirementAlternatives { get; }
	Dictionary<ulong, IIndexedPackageCompatibilityInfo> Group { get; }
}

public interface IIndexedPackageStatus<TType> where TType : struct, Enum
{
	IPackageStatus<TType> Status { get; }
	Dictionary<ulong, IIndexedPackageCompatibilityInfo> Packages { get; }
}