using Extensions.Sql;

using SkyveApp.Domain;
using SkyveApp.Domain.Enums;

namespace SkyveApp.Systems.Compatibility.Domain.Api;

#if API
[DynamicSqlClass("PackageStatuses")]
public class PackageStatus : IPackageStatus<StatusType>, IDynamicSql
{
	[DynamicSqlProperty(PrimaryKey = true, Indexer = true), System.Text.Json.Serialization.JsonIgnore]
	public ulong PackageId { get; set; }
#else
public class PackageStatus : IPackageStatus<StatusType>, IDynamicSql
{
#endif

	[DynamicSqlProperty(PrimaryKey = true)]
	public StatusType Type { get; set; }

	[DynamicSqlProperty]
	public StatusAction Action { get; set; }

	public ulong[]? Packages { get; set; }

	[DynamicSqlProperty]
	public string? Note { get; set; }

#if API
	[DynamicSqlProperty(ColumnName = nameof(Packages)), System.Text.Json.Serialization.JsonIgnore]
	public string? PackageList { get => Packages is null ? null : string.Join(',', Packages); set => Packages = value?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray(); }
#else
	public NotificationType Notification
	{
		get
		{
			var type = CRNAttribute.GetNotification(Type);
			var action = CRNAttribute.GetNotification(Action);

			return type > action ? type : action;
		}
	}

	public int IntType { get => (int)Type; set => Type = (StatusType)value; }

	public string LocaleKey => $"Status_{Type}";
#endif

	public PackageStatus()
	{

	}

	public PackageStatus(StatusType type, StatusAction action = StatusAction.NoAction)
	{
		Type = type;
		Action = action;
	}
}
