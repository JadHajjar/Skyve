using System;
using System.Collections.Generic;

namespace Skyve.Domain;

public class ReviewRequest : IPackageIdentity, IEquatable<ReviewRequest?>
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
	public string? SaveUrl { get; set; }

	ulong IPackageIdentity.Id => PackageId;
	string IPackageIdentity.Name => string.Empty;
	string? IPackageIdentity.Url => string.Empty;

	public override bool Equals(object? obj)
	{
		return Equals(obj as ReviewRequest);
	}

	public bool Equals(ReviewRequest? other)
	{
		return other is not null &&
			   PackageId == other.PackageId &&
			   UserId == other.UserId;
	}

	public override int GetHashCode()
	{
		var hashCode = 1424482213;
		hashCode = hashCode * -1521134295 + PackageId.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(UserId);
		return hashCode;
	}

	public static bool operator ==(ReviewRequest left, ReviewRequest right)
	{
		return EqualityComparer<ReviewRequest>.Default.Equals(left, right);
	}

	public static bool operator !=(ReviewRequest left, ReviewRequest right)
	{
		return !(left == right);
	}
}
