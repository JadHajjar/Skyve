using Skyve.Domain.Enums;

namespace Skyve.Domain;
public interface ILink
{
	public LinkType Type { get; set; }
	public string? Url { get; }
	public string? Title { get; }
}