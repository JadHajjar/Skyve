using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Skyve.Systems;

public class PlaysetEntryPackage : IPackage, IPlaysetEntry
{
	public PlaysetEntryPackage(IPlaysetEntry entry)
	{
		Id = entry.Id;
		Name = entry.Name;
		Url = entry.Url;
		IsMod = entry.IsMod;
		IsBuiltIn = entry.FilePath.StartsWith("%CITIES%");
		IsLocal = entry.FilePath.StartsWith("%LOCALAPPDATA%");
		RelativePath = entry.FilePath;
		FilePath = entry.FilePath;
	}

	public bool IsMod { get; }
	public bool IsLocal { get; }
	public bool IsBuiltIn { get; }
	public ulong Id { get; }
	public string Name { get; }
	public string? Url { get; }
	public string? RelativePath { get; set; }
	public string FilePath { get; }
	public ILocalPackageWithContents? LocalParentPackage => LocalPackage?.LocalParentPackage;
	public ILocalPackage? LocalPackage => null;//IsMod ? ServiceCenter.Get<IPlaysetManager>().GetMod(this) : ServiceCenter.Get<IPlaysetManager>().GetAsset(this);
	public IEnumerable<IPackageRequirement> Requirements => this.GetWorkshopInfo()?.Requirements ?? Enumerable.Empty<IPackageRequirement>();

	public bool GetThumbnail(out Bitmap? thumbnail, out string? thumbnailUrl)
	{
		var info = this.GetWorkshopInfo();

		if (info is not null)
		{
			return info.GetThumbnail(out thumbnail, out thumbnailUrl);
		}

		thumbnail = null;
		thumbnailUrl = null;
		return false;
	}
}