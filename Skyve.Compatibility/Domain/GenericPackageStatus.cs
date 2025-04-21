using Extensions;

using Newtonsoft.Json;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Collections.Generic;
using System.Linq;


namespace Skyve.Compatibility.Domain;
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
			Packages = status.Packages.Cast<CompatibilityPackageReference>().ToList();
			Note = status.Note;
			IntType = status.IntType;
			Type = status.GetType().Name;
		}
	}

	public StatusAction Action { get; set; }
	public List<CompatibilityPackageReference>? Packages { get; set; }
	public string? Header { get; set; }
	public string? Note { get; set; }
	public int IntType { get; set; }
	public string? Type { get; set; }
	[JsonIgnore] public string LocaleKey => string.Empty;
	[JsonIgnore] public NotificationType Notification { get; set; }
	IEnumerable<ICompatibilityPackageIdentity> IGenericPackageStatus.Packages { get => Packages ?? []; set => Packages = value.ToList(x => new CompatibilityPackageReference(x)); }

	public override bool Equals(object? obj)
	{
		return obj is GenericPackageStatus status &&
			   (Packages?.SequenceEqual(status.Packages) ?? status.Packages is null) &&
			   Type == status.Type;
	}

	public override int GetHashCode()
	{
		var hashCode = 1386127205;
		hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<ulong>>.Default.GetHashCode(Packages?.Select(x => x.Id) ?? []);
		hashCode = hashCode * -1521134295 + EqualityComparer<string?>.Default.GetHashCode(Type);
		return hashCode;
	}

	public IGenericPackageStatus ToGenericPackage()
	{
		var type = Type?.Contains(".") ?? false ? Type.Substring(Type.LastIndexOf('.') + 1) : Type;

		var instance = (IGenericPackageStatus)(type switch
		{
			nameof(PackageInteraction) => new PackageInteraction { Packages = [.. Packages] },
			nameof(PackageStatus) => new PackageStatus { Packages = [.. Packages] },
			nameof(StabilityStatus) => new StabilityStatus { Packages = [.. Packages] },
			nameof(SavegameEffectStatus) => new SavegameEffectStatus { Packages = [.. Packages] },
			_ => new GenericPackageStatus { Packages = [.. Packages] },
		});

		instance.Action = Action;
		instance.Note = Note;
		instance.IntType = IntType;

		return instance;
	}
}
