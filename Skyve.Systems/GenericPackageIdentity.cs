using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Drawing;

namespace Skyve.Systems;
public readonly struct GenericPackageIdentity(ulong id) : IPackageIdentity
{
	public ulong Id { get; } = id;
	public readonly string Name => this.GetWorkshopInfo()?.Name ?? string.Empty;
	public readonly string? Url => this.GetWorkshopInfo()?.Url;

	public bool GetThumbnail(IImageService imageService, out Bitmap? thumbnail, out string? thumbnailUrl)
	{
		var info = this.GetWorkshopInfo();

		if (info is not null)
		{
			return info.GetThumbnail(imageService, out thumbnail, out thumbnailUrl);
		}

		thumbnail = null;
		thumbnailUrl = null;
		return false;
	}
}
