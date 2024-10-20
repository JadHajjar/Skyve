using Extensions;

using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Skyve.Systems;

internal class ImageService : IImageService
{
	private readonly Dictionary<string, object> _lockObjects = [];
	private readonly System.Timers.Timer _cacheClearTimer;
	private readonly Dictionary<string, (Bitmap image, DateTime lastAccessed, Size? downscale)> _cache = [];
	private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(30);
	private readonly HttpClient _httpClient = new();
	private readonly ImageProcessor _imageProcessor;
	private readonly INotifier _notifier;
	private readonly ILogger _logger;
	private readonly SaveHandler _saveHandler;

	internal string ThumbnailFolder { get; }

	public ImageService(INotifier notifier, ILogger logger, SaveHandler saveHandler)
	{
		_imageProcessor = new(this);
		_cacheClearTimer = new System.Timers.Timer(_expirationTime.TotalMilliseconds);
		_cacheClearTimer.Elapsed += CacheClearTimer_Elapsed;
		_cacheClearTimer.Start();
		_notifier = notifier;
		_logger = logger;
		_saveHandler = saveHandler;

		ThumbnailFolder = CrossIO.Combine(_saveHandler.SaveDirectory, SaveHandler.AppName, ".Thumbs");

		new BackgroundAction(ClearOldImages).Run();
	}

	private object LockObj(string path)
	{
		lock (_lockObjects)
		{
			if (!_lockObjects.ContainsKey(path))
			{
				_lockObjects.Add(path, new object());
			}

			return _lockObjects[path];
		}
	}

	public FileInfo File(string url, string? fileName = null)
	{
		var filePath = CrossIO.Combine(ThumbnailFolder, fileName ?? Path.GetFileNameWithoutExtension(RemoveQueryParamsFromUrl(url).TrimEnd('/', '\\')) + Path.GetExtension(url).IfEmpty(".png"));

		return new FileInfo(filePath);
	}

	private string RemoveQueryParamsFromUrl(string url)
	{
		var index = url.IndexOf('?');
		return index >= 0 ? url.Substring(0, index) : url;
	}

	public async Task<Bitmap?> GetImage(string? url)
	{
		var image = await GetImage(url, false);

		return image is not null ? new(image) : null;
	}

	public async Task<Bitmap?> GetImage(string? url, bool localOnly, string? fileName = null, bool square = true, bool isFilePath = false, Size? downscaleTo = null)
	{
		try
		{
			if (url is null)
			{
				return null;
			}

			var filePath = File(url, fileName);
			var cache = GetCache(filePath.Name, downscaleTo);

			if (cache != null)
			{
				return cache;
			}

			if (await Ensure(url, localOnly, fileName, square, isFilePath, downscaleTo))
			{
				return GetCache(filePath.Name, downscaleTo);
			}
		}
		catch (Exception ex)
		{
			_logger.Exception(ex, "Unexpected error in ImageService");
		}

		return null;
	}

	public async Task<bool> Ensure(string? url, bool localOnly = false, string? fileName = null, bool square = true, bool isFilePath = false, Size? downscaleTo = null)
	{
		if (url is null or "")
		{
			return false;
		}

		var filePath = File(url, fileName);

		lock (LockObj(filePath.Name))
		{
			if (_cache.TryGetValue(filePath.Name, out var data) && data.downscale == downscaleTo)
			{
				return true;
			}

			if (localOnly)
			{
				_imageProcessor.Add(new(url, fileName, square, isFilePath, downscaleTo));

				return false;
			}

			if (filePath.Exists)
			{
				AddCache(filePath.Name, (Bitmap)Image.FromFile(filePath.FullName), downscaleTo);

				_notifier.OnRefreshUI();

				return true;
			}
		}

		var tries = 1;
		start:

		if (isFilePath)
		{
			if (CrossIO.FileExists(url))
			{
				if (!filePath.Exists || new FileInfo(url).Length != filePath.Length)
				{
					filePath.Directory.Create();

					System.IO.File.Copy(url, filePath.FullName, true);
				}

				return true;
			}
		}

		if (!ConnectionHandler.IsConnected)
		{
			return false;
		}

		try
		{
			using var ms = await _httpClient.GetStreamAsync(url);
			using var img = Image.FromStream(ms);

			var squareSize = Math.Min(img.Width, 512);
			var size = string.IsNullOrEmpty(fileName) ? img.Size.GetProportionalDownscaledSize(squareSize) : img.Size;
			using var image = string.IsNullOrEmpty(fileName) ? square ? new Bitmap(squareSize, squareSize) : new Bitmap(size.Width, size.Height) : img;

			if (string.IsNullOrEmpty(fileName))
			{
				using var imageGraphics = Graphics.FromImage(image);

				imageGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
				imageGraphics.DrawImage(img, square
					? new Rectangle((squareSize - size.Width) / 2, (squareSize - size.Height) / 2, size.Width, size.Height)
					: new Rectangle(Point.Empty, size));
			}

			Directory.GetParent(filePath.FullName).Create();

			lock (LockObj(url))
			{
				if (filePath.FullName.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase) || filePath.FullName.EndsWith(".jpeg", StringComparison.InvariantCultureIgnoreCase))
				{
					image.Save(filePath.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);
				}
				else
				{
					image.Save(filePath.FullName);
				}
			}

			_notifier.OnRefreshUI();

			return true;
		}
		catch (Exception ex)
		{
			if (ex is WebException we && we.Response is HttpWebResponse hwr && hwr.StatusCode == HttpStatusCode.BadGateway)
			{
				await Task.Delay(1000);

				goto start;
			}
			else if (tries < 2)
			{
				tries++;
				goto start;
			}

			return false;
		}
	}

	private Bitmap AddCache(string key, Bitmap image, Size? downscaleTo)
	{
		if (key is null or "")
		{
			return image;
		}

		if (downscaleTo.HasValue)
		{
			using var img = image;

			image = new Bitmap(image, WinExtensionClass.CalculateNewSize(image.Size, downscaleTo.Value));
		}

		if (_cache.ContainsKey(key))
		{
			_cache[key].image.Dispose();
			_cache[key] = (image, DateTime.Now, downscaleTo);
		}
		else
		{
			_cache.Add(key, (image, DateTime.Now, downscaleTo));
		}

		return image;
	}

	public Bitmap? GetCache(string key, Size? downscaleTo)
	{
		if (_cache.TryGetValue(key, out var value) && value.downscale == downscaleTo)
		{
			if (DateTime.Now - value.lastAccessed > _expirationTime)
			{
				value.image.Dispose();
				_cache.Remove(key);
				return null;
			}

			value.lastAccessed = DateTime.Now;
			return value.image;
		}

		return null;
	}

	private void CacheClearTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		try
		{
			var keys = _cache.Keys.ToList();

			foreach (var key in keys)
			{
				if (_cache.TryGetValue(key, out var value))
				{
					if (DateTime.Now - value.lastAccessed > _expirationTime)
					{
						value.image.Dispose();
						_cache.Remove(key);
					}
				}
			}
		}
		catch { }
	}

	public void ClearCache(bool deleteFiles)
	{
		lock (_lockObjects)
		{
			foreach (var (image, lastAccessed, downscale) in _cache.Values)
			{
				image?.Dispose();
			}

			_cache.Clear();

			if (deleteFiles)
			{
				foreach (var item in Directory.EnumerateFiles(ThumbnailFolder))
				{
					try
					{
						CrossIO.DeleteFile(item, true);
					}
					catch { }
				}
			}
		}
	}

	private void ClearOldImages()
	{
		UpdateFolderLocation();

		foreach (var item in new DirectoryInfo(ThumbnailFolder).EnumerateFiles())
		{
			try
			{
				if (item.LastAccessTimeUtc < DateTime.Now.AddDays(-30))
				{
					CrossIO.DeleteFile(item.FullName, true);
				}
			}
			catch { }
		}
	}

	private void UpdateFolderLocation()
	{
		var oldThumbnailFolder = CrossIO.Combine(_saveHandler.SaveDirectory, SaveHandler.AppName, "Thumbs");

		try
		{
			if (Directory.Exists(oldThumbnailFolder))
			{
				if (Directory.Exists(ThumbnailFolder))
				{
					new DirectoryInfo(oldThumbnailFolder).Delete(true);
				}
				else
				{
					Directory.Move(CrossIO.Combine(_saveHandler.SaveDirectory, SaveHandler.AppName, "Thumbs"), CrossIO.Combine(_saveHandler.SaveDirectory, SaveHandler.AppName, ".Thumbs"));
				}
			}
		}
		catch { }
	}

	public string? FindImage(string pattern)
	{
		if (!Directory.Exists(ThumbnailFolder))
		{
			return null;
		}

		var file = Directory.EnumerateFiles(ThumbnailFolder, pattern).OrderByDescending(System.IO.File.GetCreationTime).FirstOrDefault();

		return !string.IsNullOrEmpty(file) ? Path.GetFileName(file) : null;
	}
}