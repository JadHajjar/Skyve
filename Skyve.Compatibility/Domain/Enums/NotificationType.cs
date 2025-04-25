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

	//PrevAttentionRequired = 60,

	//Exclude = 70,
	ActionRequired = 71,
	Obsolete = 79,

	//Unsubscribe = 80,
	Broken = 81,

	//Switch = 90,
}
