using System;

namespace Skyve.Domain;

public class ReviewRequest : IPackageIdentity
{
	public byte[]? LogFile { get; set; }
	public ulong PackageId { get; set; }
	public string? UserId { get; set; }
	public int PackageStability { get; set; }
	public int PackageUsage { get; set; }
	public int PackageType { get; set; }
	public int StatusType { get; set; }
	public int StatusAction { get; set; }
	public string? RequiredDLCs { get; set; }
	public string? StatusPackages { get; set; }
	public string? StatusNote { get; set; }
	public string? PackageNote { get; set; }
	public bool IsInteraction { get; set; }
	public bool IsStatus { get; set; }
	public DateTime Timestamp { get; set; }

	ulong IPackageIdentity.Id => PackageId;
	string IPackageIdentity.Name => string.Empty;
	string? IPackageIdentity.Url => string.Empty;
}
