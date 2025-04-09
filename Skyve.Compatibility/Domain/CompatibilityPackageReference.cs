using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;
using Skyve.Domain.Systems;

using System;
using System.Drawing;

namespace Skyve.Compatibility.Domain;

public class CompatibilityPackageReference : ICompatibilityPackageIdentity, ILocalPackageIdentity, IEquatable<CompatibilityPackageReference?>, IEquatable<ICompatibilityPackageIdentity?>
{
	public ulong Id { get; set; }
	public bool IsDlc { get; set; }
	public string Name { get; set; }
	public string? Url { get; set; }
	public string? Version { get; set; }
	public string Folder { get; set; }
	public string FilePath { get; set; }
	public long FileSize { get; set; }
	public DateTime LocalTime { get; set; }

	public CompatibilityPackageReference()
	{
		Name = Folder = FilePath = string.Empty;
	}

	public CompatibilityPackageReference(ICompatibilityPackageIdentity package)
	{
		Id = package.Id;
		Name = package.Name;
		Url = package.Url;
		Version = package.Version;
		IsDlc = package.IsDlc;
		Folder = FilePath = string.Empty;
	}

	public CompatibilityPackageReference(IPackageIdentity package)
	{
		Id = package.Id;
		Name = package.Name;
		Url = package.Url;
		Version = package.Version;
		Folder = FilePath = string.Empty;
	}

	public CompatibilityPackageReference(ILocalPackageIdentity package)
	{
		Id = package.Id;
		Name = package.Name;
		Url = package.Url;
		Version = package.Version;
		Folder = package.Folder;
		FilePath = package.FilePath;
		FileSize = package.FileSize;
		LocalTime = package.LocalTime;
	}

	public CompatibilityPackageReference(IDlcInfo dlc)
	{
		Id = dlc.Id;
		Name = dlc.Name;
		Url = dlc.Url;
		IsDlc = true;
		Folder = FilePath = string.Empty;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as ICompatibilityPackageIdentity);
	}

	public bool Equals(CompatibilityPackageReference? other)
	{
		return other is not null &&
			   Id == other.Id &&
			   Name == other.Name &&
			   IsDlc == other.IsDlc;
	}

	public bool Equals(ICompatibilityPackageIdentity? other)
	{
		return other is not null &&
			   Id == other.Id &&
			   Name == other.Name &&
			   IsDlc == other.IsDlc;
	}

	public override int GetHashCode()
	{
		var hashCode = 1568141914;
		hashCode = hashCode * -1521134295 + Id.GetHashCode();
		hashCode = hashCode * -1521134295 + Name.GetHashCode();
		hashCode = hashCode * -1521134295 + IsDlc.GetHashCode();
		return hashCode;
	}
}
