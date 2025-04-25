namespace Skyve.Domain;

public interface ITag
{
	public string Value { get; }
	public string Icon { get; }
	bool IsCustom { get; }
	bool IsWorkshop { get; }
}