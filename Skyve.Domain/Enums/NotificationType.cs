namespace Skyve.Domain.Enums;

public enum NotificationType
{
	None = 0,
	Info = 10,
	RequiredItem = 20,

	Caution = 30,
	MissingDependency = 40,
	Warning = 50,

	AttentionRequired = 60,
	Exclude = 70,
	Unsubscribe = 80,
	Switch = 90,
}
