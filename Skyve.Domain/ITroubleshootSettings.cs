namespace Skyve.Domain;
public interface ITroubleshootSettings
{
	bool ItemIsCausingIssues { get; }
	bool ItemIsMissing { get; }
	bool NewItemCausingIssues { get; }
	bool Mods { get; }
}
