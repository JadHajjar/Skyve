using System;

namespace Skyve.Compatibility.Domain.Enums;

public enum NotificationType
{
	ReviewRequest = -2,
	Snoozed = -1,

	None = 0,
	Info = 10,
	LocalMod = 11,
	RequiredItem = 20,
	Caution = 30,
	MissingDependency = 40,
	AttentionRequired = 49,
	Warning = 50,
	ActionRequired = 71,
	Obsolete = 79,
	Broken = 81,
}
