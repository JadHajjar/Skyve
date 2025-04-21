using System;

namespace Skyve.Compatibility.Domain.Enums;

public enum NotificationType
{
	None = 0,
	Info = 10,
	RequiredItem = 20,

	Caution = 30,
	MissingDependency = 40,
	AttentionRequired = 49,
	Warning = 50,

	[Obsolete("NO LONGER USED", true)]
	PrevAttentionRequired = 60,
	[Obsolete("NO LONGER USED", true)]
	Exclude = 70,
	ActionRequired = 71,
	Obsolete = 79,
	[Obsolete("NO LONGER USED", true)]
	Unsubscribe = 80,
	Broken = 81,
	[Obsolete("NO LONGER USED", true)]
	Switch = 90,
}
