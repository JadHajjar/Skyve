using Newtonsoft.Json;

using Skyve.Domain;
using Skyve.Domain.Enums;
using Skyve.Systems.Compatibility.Domain.Api;

namespace Skyve.Systems.Compatibility.Domain;
public class GenericPackageStatus : IGenericPackageStatus
{
	public GenericPackageStatus()
	{

	}

	public GenericPackageStatus(IGenericPackageStatus status)
	{
		if (status is not null)
		{
			Action = status.Action;
			Packages = status.Packages;
			Note = status.Note;
			IntType = status.IntType;
			Type = status.GetType().Name;
		}
	}

	public StatusAction Action { get; set; }
	public ulong[]? Packages { get; set; }
	public string? Note { get; set; }
	public int IntType { get; set; }
	public string? Type { get; set; }
	[JsonIgnore] public string LocaleKey => string.Empty;
	[JsonIgnore] public NotificationType Notification { get; set; }

	public IGenericPackageStatus ToGenericPackage()
	{
		var type = Type?.Contains(".") ?? false ? Type.Substring(Type.LastIndexOf('.') + 1) : Type;

		var instance = (IGenericPackageStatus)(type switch
		{
			nameof(PackageInteraction) => new PackageInteraction(),
			nameof(PackageStatus) => new PackageStatus(),
			nameof(StabilityStatus) => new StabilityStatus(),
			_ => new GenericPackageStatus(),
		});

		instance.Action = Action;
		instance.Packages = Packages;
		instance.Note = Note;
		instance.IntType = IntType;

		return instance;
	}
}
