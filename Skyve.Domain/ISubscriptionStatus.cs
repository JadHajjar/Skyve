using System;

namespace Skyve.Domain;
public interface ISubscriptionStatus
{
	Guid Id { get; }
	ModDownloadStage Stage { get; }
	ulong DownloadedBytes { get; }
	ulong TotalBytesToDownload { get; }
	float TotalProgress { get; }
	float StageProgress { get; }
	IWorkshopInfo Mod { get; }
}

public enum ModDownloadStage
{
	/// <summary>
	/// The download is queued and waiting to be started
	/// </summary>
	Pending,
	/// <summary>
	/// The download is starting, required information is being gathered and local setup is being done
	/// </summary>
	Started,
	/// <summary>
	/// The mod's contents are being downloaded, including images
	/// </summary>
	Downloading,
	/// <summary>
	/// The integrity of the downloaded files are being checked to make sure nothing is corrupted
	/// </summary>
	CheckingIntegrity,
	/// <summary>
	/// The downloaded files are being installed and any required patches are being applied
	/// </summary>
	Processing,
	/// <summary>
	/// Temporary files are being cleaned up and the mod's files are being moved to their final location
	/// </summary>
	CleaningUp,
	/// <summary>
	/// The download and installation is complete
	/// </summary>
	Completed,

	/// <summary>
	/// The download has been canceled by the user
	/// </summary>
	Canceled,
	/// <summary>
	/// The download has failed
	/// </summary>
	Failed,
}