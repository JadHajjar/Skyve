using System.Drawing;

namespace Skyve.Domain;

public interface IThumbnailObject
{
	bool GetThumbnail(out Bitmap? thumbnail, out string? thumbnailUrl);
}
