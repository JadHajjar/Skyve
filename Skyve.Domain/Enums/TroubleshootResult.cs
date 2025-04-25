namespace Skyve.Domain.Enums;
public enum TroubleshootResult
{
	Success,

	Error = 100,
	Busy,
	NoPacakgesToProcess,
	CouldNotCreatePlayset,
	InvalidState,
	NoActivePlayset,
}
