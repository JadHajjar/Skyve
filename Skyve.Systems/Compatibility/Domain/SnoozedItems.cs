using Skyve.Domain;

namespace Skyve.Systems.Compatibility.Domain;
internal struct SnoozedItem
{
	public SnoozedItem()
	{

	}

	public SnoozedItem(ICompatibilityItem report) : this()
	{
		SteamId = report.PackageId;
		ReportType = (int)report.Type;
		StatusType = report.Status.IntType;
		StatusAction = (int)report.Status.Action;
	}

	public ulong SteamId { get; set; }
	public int ReportType { get; set; }
	public int StatusType { get; set; }
	public int StatusAction { get; set; }

	public override bool Equals(object? obj)
	{
		return obj is ICompatibilityItem report
			? SteamId == report.PackageId
				&& ReportType == (int)report.Type
				&& StatusType == report.Status.IntType
				&& StatusAction == (int)report.Status.Action
			: obj is SnoozedItem item &&
			   SteamId == item.SteamId &&
			   ReportType == item.ReportType &&
			   StatusType == item.StatusType &&
			   StatusAction == item.StatusAction;
	}

	public override int GetHashCode()
	{
		var hashCode = -143951897;
		hashCode = hashCode * -1521134295 + SteamId.GetHashCode();
		hashCode = hashCode * -1521134295 + ReportType.GetHashCode();
		hashCode = hashCode * -1521134295 + StatusType.GetHashCode();
		hashCode = hashCode * -1521134295 + StatusAction.GetHashCode();
		return hashCode;
	}
}
