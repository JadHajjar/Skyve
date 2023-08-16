namespace Skyve.Domain.Enums;

public enum NotificationType
{
	None = 0,
	Info = 1,

	Caution = 2,
	MissingDependency = 3,
	Warning = 4,

	AttentionRequired = 5,
	Exclude = 6,
	Unsubscribe = 7,
	Switch = 8,
}
