﻿using Newtonsoft.Json;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Collections.Generic;
using System.Linq;

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

	[JsonIgnore] public int IntType { get => (int)Type; set => Type = (StatusType)value; }
	[JsonIgnore] public string LocaleKey => $"Status_{Type}";

	public PackageStatus()
	{

	}

	public PackageStatus(StatusType type, StatusAction action = StatusAction.NoAction)
	{
		Type = type;
		Action = action;
	}

	public override bool Equals(object? obj)
	{
		return obj is PackageStatus status &&
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
