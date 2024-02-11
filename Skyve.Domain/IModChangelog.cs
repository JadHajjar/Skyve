using System;

namespace Skyve.Domain;
public interface IModChangelog
{
	string? Version { get; set; }
	DateTime? ReleasedDate { get; set; }
	string? Details { get; set; }
}
