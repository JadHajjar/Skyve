using System;

namespace Skyve.Domain;

public class PackageDownloadProgress
{
	public ulong Id { get; set; }
	public float Progress { get; set; }
	public ulong ProcessedBytes { get; set; }
	public ulong Size { get; set; }
}
