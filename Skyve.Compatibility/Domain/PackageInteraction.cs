using Extensions;

using Newtonsoft.Json;

using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Compatibility.Domain;

public class PackageInteraction : IPackageStatus<InteractionType>
{
	public InteractionType Type { get; set; }
	public StatusAction Action { get; set; }
	public List<CompatibilityPackageReference>? Packages { get; set; }
	public string? Header { get; set; }
	public string? Note { get; set; }
	[JsonIgnore] public int IntType { get => (int)Type; set => Type = (InteractionType)value; }
	[JsonIgnore] public string LocaleKey => $"Interaction_{Type}";
	public NotificationType Notification
	{
		get
		{
			var type = CRNAttribute.GetNotification(Type);
			var action = CRNAttribute.GetNotification(Action);

			return type > action ? type : action;
		}
	}
	IEnumerable<ICompatibilityPackageIdentity> IGenericPackageStatus.Packages { get => Packages ?? []; set => Packages = value.ToList(x => new CompatibilityPackageReference(x)); }
	string IGenericPackageStatus.Class => nameof(PackageInteraction);

	public PackageInteraction()
	{

	}

	public PackageInteraction(InteractionType type, StatusAction action = StatusAction.NoAction)
	{
		Type = type;
		Action = action;
	}

	public override bool Equals(object? obj)
	{
		return obj is PackageInteraction interaction &&
			   Type == interaction.Type &&
			   (Packages?.SequenceEqual(interaction.Packages) ?? interaction.Packages is null);
	}

	public override int GetHashCode()
	{
		var hashCode = 498602157;
		hashCode = hashCode * -1521134295 + Type.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<IEnumerable<ulong>>.Default.GetHashCode(Packages?.Select(x => x.Id) ?? []);
		return hashCode;
	}

	public IPackageStatus<InteractionType> Duplicate()
	{
		return new PackageInteraction
		{
			Type = Type,
			Action = Action,
			Packages = Packages,
			Note = Note,
		};
	}
}
