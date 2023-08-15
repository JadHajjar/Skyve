using SkyveApp.Domain;
using SkyveApp.Domain.Enums;

namespace SkyveApp.Systems.Compatibility.Domain;
internal class StabilityStatus : IPackageStatus<PackageStability>
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
	public int IntType { get => (int)Type; set => Type = (PackageStability)value; }
	public string LocaleKey => $"Stability_{Type}";
}
