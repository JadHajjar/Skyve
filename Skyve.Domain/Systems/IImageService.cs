using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IImageService
{
	void ClearCache(bool deleteFiles);
	Task<bool> Ensure(string? url, bool localOnly = false, string? fileName = null, bool square = true, bool isFilePath = false);
	FileInfo File(string url, string? fileName = null);
	string? FindImage(string pattern);
	Bitmap? GetCache(string key);
	Task<Bitmap?> GetImage(string? url);
	Task<Bitmap?> GetImage(string? url, bool localOnly, string? fileName = null, bool square = true, bool isFilePath = false, Size? downscaleTo = null);
}
