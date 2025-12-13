namespace Skyve.Domain;

public interface ITag
{
	string Key { get; set; }
	public string Value { get; }
	public string Icon { get; }
	bool IsCustom { get; }
	bool IsWorkshop { get; }
}

public interface IWorkshopTag : ITag
{
	IWorkshopTag[] Children { get; }
	bool IsSelectable { get; set; }
	int? UsageCount { get; set; }
	int Depth { get; set; }
	int? Order { get; set; }
}