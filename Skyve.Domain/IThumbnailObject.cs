using Skyve.Domain.Systems;

using System.Drawing;

namespace Skyve.Domain;

public interface IThumbnailObject
{
	bool GetThumbnail(IImageService imageService, out Bitmap? thumbnail, out string? thumbnailUrl);
}
