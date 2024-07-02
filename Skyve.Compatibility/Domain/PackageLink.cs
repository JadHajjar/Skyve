using Skyve.Domain;
using Skyve.Domain.Enums;

namespace Skyve.Compatibility.Domain;

public struct PackageLink : ILink
{
	public LinkType Type { get; set; }
	public string? Url { get; set; }
	public string? Title { get; set; }
}
