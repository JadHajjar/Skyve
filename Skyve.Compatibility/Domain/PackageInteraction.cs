using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;

namespace Skyve.Compatibility.Domain;

public class PackageInteraction : IPackageStatus<InteractionType>
{
	public InteractionType Type { get; set; }

	public StatusAction Action { get; set; }

	public ulong[]? Packages { get; set; }

	public string? Note { get; set; }

	public NotificationType Notification
	{
		get
		{
			var type = Type is InteractionType.OptionalPackages && ServiceCenter.Get<Skyve.Domain.Systems.ISettings>().UserSettings.TreatOptionalAsRequired
				? NotificationType.MissingDependency
				: CRNAttribute.GetNotification(Type);
			var action = CRNAttribute.GetNotification(Action);

			return type > action ? type : action;
		}
	}

	public int IntType { get => (int)Type; set => Type = (InteractionType)value; }

	public string LocaleKey => $"Interaction_{Type}";

	public PackageInteraction()
	{

	}

	public PackageInteraction(InteractionType type, StatusAction action = StatusAction.NoAction)
	{
		Type = type;
		Action = action;
	}
}
