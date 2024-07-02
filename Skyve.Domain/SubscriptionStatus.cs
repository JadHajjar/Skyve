namespace Skyve.Domain;
public readonly struct SubscriptionStatus(bool isActive, ulong modId, float progress, ulong processedBytes, ulong totalSize)
{
	public readonly bool IsActive = isActive;
	public readonly ulong ModId = modId;
	public readonly float Progress = progress;
	public readonly ulong ProcessedBytes = processedBytes;
	public readonly ulong TotalSize = totalSize;
}
