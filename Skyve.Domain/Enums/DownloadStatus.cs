namespace Skyve.Domain.Enums;

public enum DownloadStatus
{
	None,
	OK,
	Unknown,
	OutOfDate,
	PartiallyDownloaded,
	Removed,
}

public enum DownloadStatusFilter
{
	Any,
	AnyIssue,
	//None,
	OK,
	Unknown,
	OutOfDate,
	PartiallyDownloaded,
	Removed,
}