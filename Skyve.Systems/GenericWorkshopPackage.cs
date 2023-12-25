using Newtonsoft.Json;

using Skyve.Domain;

using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Skyve.Systems;
public class GenericWorkshopPackage : IPackage
{
	public GenericWorkshopPackage(IPackageIdentity identity)
	{
		Id = (ulong)identity.Id;
		Name = identity.Name;
		Url = identity.Url;
	}

	public GenericWorkshopPackage(ulong id)
	{
		Id = id;
		Name = id.ToString();
		Url = $"https://steamcommunity.com/workshop/filedetails/?id={Id}";
	}

	public GenericWorkshopPackage()
	{
		Name = string.Empty;
	}

	public bool IsMod => this.GetWorkshopInfo()?.IsMod ?? false;
	public bool IsLocal { get; set; }
	public bool IsBuiltIn { get; set; }
	public ulong Id { get; set; }
	public string Name { get; set; }
	public string? Url { get; set; }

	[JsonIgnore] public ILocalPackageWithContents? LocalParentPackage => this.GetLocalPackage()?.LocalParentPackage;
	[JsonIgnore] public ILocalPackage? LocalPackage => this.GetLocalPackage();
	[JsonIgnore] public IEnumerable<IPackageRequirement> Requirements => this.GetWorkshopInfo()?.Requirements ?? Enumerable.Empty<IPackageRequirement>();

	public override string ToString()
	{
		return this.GetWorkshopInfo()?.Name ?? Name;
	}

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
