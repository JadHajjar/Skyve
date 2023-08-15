using SkyveApp.Domain.Enums;

namespace SkyveApp.Domain;
public interface ICompatibilityItem
{
	ulong PackageId { get; }
	IGenericPackageStatus Status { get; }
	ReportType Type { get; }
	string? Message { get; }
	IPackage[] Packages { get; }
}
