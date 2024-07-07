using System;

namespace Skyve.Domain;

public class ReviewReply
{
	public string? Username { get; set; }
	public ulong PackageId { get; set; }
	public bool RequestUpdate { get; set; }
	public string? Message { get; set; }
	public string? Link { get; set; }
	public DateTime Timestamp { get; set; }
}
