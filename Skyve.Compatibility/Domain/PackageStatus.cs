using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

namespace Skyve.Compatibility.Domain;

public class PackageStatus : IPackageStatus<StatusType>
{
	public StatusType Type { get; set; }
	public StatusAction Action { get; set; }
	public ulong[]? Packages { get; set; }
	public string? Note { get; set; }

	public NotificationType Notification
	{
		get
		{
			var type = CRNAttribute.GetNotification(Type);
			var action = CRNAttribute.GetNotification(Action);

			return type > action ? type : action;
		}
	}

	public int IntType { get => (int)Type; set => Type = (StatusType)value; }

	public string LocaleKey => $"Status_{Type}";

	public PackageStatus()
	{

	}

	public PackageStatus(StatusType type, StatusAction action = StatusAction.NoAction)
	{
		Type = type;
		Action = action;
	}
}
