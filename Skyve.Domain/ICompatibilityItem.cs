using Skyve.Domain.Enums;

namespace Skyve.Domain;
public interface ICompatibilityItem
{
	ulong PackageId { get; }
	IGenericPackageStatus Status { get; }
	ReportType Type { get; }
	string? Message { get; }
	IPackage[] Packages { get; }
}
