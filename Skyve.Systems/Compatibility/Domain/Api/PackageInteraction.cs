using Extensions.Sql;

using Skyve.Domain;
using Skyve.Domain.Enums;

namespace Skyve.Systems.Compatibility.Domain.Api;

#if API
[DynamicSqlClass("PackageInteractions")]
public class PackageInteraction : IPackageStatus<InteractionType>, IDynamicSql
{
	[DynamicSqlProperty(PrimaryKey = true, Indexer = true), System.Text.Json.Serialization.JsonIgnore]
	public ulong PackageId { get; set; }
#else
public class PackageInteraction : IPackageStatus<InteractionType>, IDynamicSql
{
#endif
	[DynamicSqlProperty(PrimaryKey = true)]
	public InteractionType Type { get; set; }

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
			var type = Type is InteractionType.OptionalPackages && ServiceCenter.Get<Skyve.Domain.Systems.ISettings>().UserSettings.TreatOptionalAsRequired
				? NotificationType.MissingDependency
				: CRNAttribute.GetNotification(Type);
			var action = CRNAttribute.GetNotification(Action);

			return type > action ? type : action;
		}
	}

	public int IntType { get => (int)Type; set => Type = (InteractionType)value; }

	public string LocaleKey => $"Interaction_{Type}";
#endif

	public PackageInteraction()
	{

	}

	public PackageInteraction(InteractionType type, StatusAction action = StatusAction.NoAction)
	{
		Type = type;
		Action = action;
	}
}
