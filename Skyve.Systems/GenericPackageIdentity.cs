using Skyve.Domain;

using System.Drawing;

namespace Skyve.Systems;
public readonly struct GenericPackageIdentity : IPackageIdentity
{
	public ulong Id { get; }
	public readonly string Name => this.GetWorkshopInfo()?.Name ?? string.Empty;
	public readonly string? Url => this.GetWorkshopInfo()?.Url;

	public GenericPackageIdentity(ulong id)
	{
		Id = id;
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
