using Extensions;

using Newtonsoft.Json;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Compatibility.Domain;

public class PackageTypeStatus : IPackageStatus<PackageType>
{
	public PackageTypeStatus(PackageType type)
	{
		Notification = NotificationType.Info;
		Header = nameof(PackageType);
		Type = type;
	}

	public PackageTypeStatus()
	{

	}

	public PackageType Type { get; set; }
	public StatusAction Action { get; set; }
	public List<CompatibilityPackageReference>? Packages { get; set; }
	public string? Header { get; set; }
	public string? Note { get; set; }
	public NotificationType Notification { get; }
	[JsonIgnore] public int IntType { get => (int)Type; set => Type = (PackageType)value; }
	[JsonIgnore] public string LocaleKey => $"PackageType_{Type}";
	IEnumerable<ICompatibilityPackageIdentity> IGenericPackageStatus.Packages { get => Packages ?? []; set => Packages = value.ToList(x => new CompatibilityPackageReference(x)); }
	string IGenericPackageStatus.Class => nameof(PackageTypeStatus);

	public override bool Equals(object? obj)
	{
		return obj is PackageTypeStatus status &&
			   Type == status.Type &&
			   (Packages?.SequenceEqual(status.Packages) ?? status.Packages is null);
	}

	public override int GetHashCode()
	{
		var hashCode = 498602157;
		hashCode = hashCode * -1521134295 + Type.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<ulong>>.Default.GetHashCode(Packages?.Select(x => x.Id) ?? []);
		return hashCode;
	}

	public IPackageStatus<PackageType> Duplicate()
	{
		return new PackageTypeStatus
		{
			Type = Type,
			Action = Action,
			Packages = Packages,
			Note = Note,
		};
	}
}

