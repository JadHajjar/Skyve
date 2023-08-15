namespace SkyveApp.Domain;

public interface ITag
{
	public string Value { get; }
	public string Icon { get; }
	bool IsCustom { get; }
}