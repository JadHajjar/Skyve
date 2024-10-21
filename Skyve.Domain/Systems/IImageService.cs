using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IImageService
{
	string ThumbnailFolder { get; }

	void ClearCache(bool deleteFiles);
	Task<bool> Ensure(string? url, bool localOnly = false, string? fileName = null, bool square = true, bool isFilePath = false, Size? downscaleTo = null);
	FileInfo File(string url, string? fileName = null);
	string? FindImage(string pattern);
	Bitmap? GetCache(string key, Size? downscaleTo = null);
	Task<Bitmap?> GetImage(string? url);
	Task<Bitmap?> GetImage(string? url, bool localOnly, string? fileName = null, bool square = true, bool isFilePath = false, Size? downscaleTo = null);
}
