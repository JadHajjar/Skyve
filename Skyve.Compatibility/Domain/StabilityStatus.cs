using Newtonsoft.Json;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Compatibility.Domain;
public class StabilityStatus : IPackageStatus<PackageStability>
{
	public StabilityStatus(PackageStability type, string? note, bool review)
	{
		Type = type;
		Action = type is PackageStability.Broken ? StatusAction.UnsubscribeThis : review ? StatusAction.RequestReview : StatusAction.NoAction;
		Note = note;
	}

	public StabilityStatus()
	{

	}


	public PackageStability Type { get; set; }
	public StatusAction Action { get; set; }
	public ulong[]? Packages { get; set; }
	public string? Note { get; set; }
	public NotificationType Notification => CRNAttribute.GetNotification(Type);
	[JsonIgnore] public int IntType { get => (int)Type; set => Type = (PackageStability)value; }
	[JsonIgnore] public string LocaleKey => $"Stability_{Type}";

	public override bool Equals(object? obj)
	{
		return obj is StabilityStatus status &&
			   Type == status.Type &&
			   (Packages?.SequenceEqual(status.Packages) ?? status.Packages is null);
	}

	public override int GetHashCode()
	{
		var hashCode = 498602157;
		hashCode = hashCode * -1521134295 + Type.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<ulong[]?>.Default.GetHashCode(Packages);
		return hashCode;
	}
}
