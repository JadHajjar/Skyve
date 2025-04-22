using Skyve.Compatibility.Domain.Interfaces;

namespace Skyve.Compatibility.Domain;
public class SnoozedItem
{
	public SnoozedItem()
	{

	}

	public SnoozedItem(ICompatibilityItem report) : this()
	{
		PackageId = report.PackageId;
		ReportType = (int)report.Type;
		StatusClass = report.Status.Class;
		StatusType = report.Status.IntType;
		StatusAction = (int)report.Status.Action;
	}

	public ulong PackageId { get; set; }
	public int ReportType { get; set; }
	public string StatusClass { get; }
	public int StatusType { get; set; }
	public int StatusAction { get; set; }

	public override bool Equals(object? obj)
	{
		return obj is ICompatibilityItem report
			? PackageId == report.PackageId
				&& ReportType == (int)report.Type
				&& StatusClass == report.Status.Class
				&& StatusType == report.Status.IntType
				&& StatusAction == (int)report.Status.Action
			: obj is SnoozedItem item &&
			   PackageId == item.PackageId &&
			   ReportType == item.ReportType &&
			   StatusClass == item.StatusClass &&
			   StatusType == item.StatusType &&
			   StatusAction == item.StatusAction;
	}

	public override int GetHashCode()
	{
		var hashCode = -143951897;
		hashCode = (hashCode * -1521134295) + PackageId.GetHashCode();
		hashCode = (hashCode * -1521134295) + ReportType.GetHashCode();
		hashCode = (hashCode * -1521134295) + StatusClass.GetHashCode();
		hashCode = (hashCode * -1521134295) + StatusType.GetHashCode();
		hashCode = (hashCode * -1521134295) + StatusAction.GetHashCode();
		return hashCode;
	}
}
