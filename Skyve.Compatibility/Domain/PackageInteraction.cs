using Newtonsoft.Json;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

namespace Skyve.Compatibility.Domain;

public class PackageInteraction : IPackageStatus<InteractionType>
{
	public InteractionType Type { get; set; }
	public StatusAction Action { get; set; }
	public ulong[]? Packages { get; set; }
	public string? Note { get; set; }
	[JsonIgnore] public int IntType { get => (int)Type; set => Type = (InteractionType)value; }
	[JsonIgnore] public string LocaleKey => $"Interaction_{Type}";
	public NotificationType Notification
	{
		get
		{
			var type = CRNAttribute.GetNotification(Type);
			var action = CRNAttribute.GetNotification(Action);

			return type > action ? type : action;
		}
	}

	public PackageInteraction()
	{

	}

	public PackageInteraction(InteractionType type, StatusAction action = StatusAction.NoAction)
	{
		Type = type;
		Action = action;
	}
}
