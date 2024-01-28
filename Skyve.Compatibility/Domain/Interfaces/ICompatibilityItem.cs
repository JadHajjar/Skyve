using Skyve.Compatibility.Domain.Enums;
using Skyve.Domain;

using System.Collections.Generic;

namespace Skyve.Compatibility.Domain.Interfaces;
public interface ICompatibilityItem
{
	ulong PackageId { get; }
	IGenericPackageStatus Status { get; }
	ReportType Type { get; }
	string? Message { get; }
	IEnumerable<IPackageIdentity> Packages { get; }
}
