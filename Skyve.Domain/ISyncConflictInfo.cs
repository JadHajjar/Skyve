namespace Skyve.Domain;
public interface ISyncConflictInfo
{
	string? LocalPlaysetName { get; }
	string? OnlinePlaysetName { get; }
}
