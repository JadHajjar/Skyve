using System;

namespace Skyve.Domain;
public interface IModChangelog
{
	string? VersionId { get; set; }
	string? Version { get; set; }
	DateTime? ReleasedDate { get; set; }
	string? Details { get; set; }
}
