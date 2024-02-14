using Extensions;

using Skyve.Domain;
using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Skyve.Systems;

internal class ImageSystem : IImageService
{
	private readonly Dictionary<string, object> _lockObjects = [];
	private readonly System.Timers.Timer _cacheClearTimer;
	private readonly Dictionary<string, (Bitmap image, DateTime lastAccessed)> _cache = [];
	private readonly TimeSpan _expirationTime = TimeSpan.FromMinutes(1);
	private readonly HttpClient _httpClient = new();
	private readonly ImageProcessor _imageProcessor;
	private readonly INotifier _notifier;
	private readonly SaveHandler _saveHandler;

	public ImageSystem(INotifier notifier, SaveHandler saveHandler)
	{
		_imageProcessor = new(this);
		_cacheClearTimer = new System.Timers.Timer(_expirationTime.TotalMilliseconds);
		_cacheClearTimer.Elapsed += CacheClearTimer_Elapsed;
		_cacheClearTimer.Start();
		_notifier = notifier;
		_saveHandler = saveHandler;

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
		var filePath = CrossIO.Combine(_saveHandler.SaveDirectory, SaveHandler.AppName, "Thumbs", fileName ?? Path.GetFileNameWithoutExtension(RemoveQueryParamsFromUrl(url).TrimEnd('/', '\\')) + Path.GetExtension(url).IfEmpty(".png"));

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

	public async Task<Bitmap?> GetImage(string? url, bool localOnly, string? fileName = null, bool square = true, bool isFilePath = false)
	{
		if (url is null || !await Ensure(url, localOnly, fileName, square, isFilePath))
		{
			return null;
		}

		var cache = GetCache(url);

		if (cache != null)
		{
			return cache;
		}

		var filePath = File(url, fileName);

		if (filePath.Exists)
		{
			lock (LockObj(url))
			{
				try
				{
					return AddCache(url, (Bitmap)Image.FromFile(filePath.FullName));
				}
				catch { }
			}
		}

		return null;
	}

	public async Task<bool> Ensure(string? url, bool localOnly = false, string? fileName = null, bool square = true, bool isFilePath = false)
	{
		if (url is null or "")
		{
			return false;
		}

		var filePath = File(url, fileName);

		lock (LockObj(url))
		{
			if (isFilePath)
			{
				if (CrossIO.FileExists(url))
				{
					if (!filePath.Exists || new FileInfo(url).Length != filePath.Length)
					{
						Directory.CreateDirectory(CrossIO.Combine(_saveHandler.SaveDirectory, "Thumbs"));

						System.IO.File.Copy(url, filePath.FullName, true);
					}

					return true;
				}
			}

			else if (filePath.Exists)
			{
				return true;
			}

			if (localOnly)
			{
				_imageProcessor.Add(new(url, fileName, square));

				return false;
			}
		}

		var tries = 1;
		start:

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

	private Bitmap AddCache(string key, Bitmap image)
	{
		if (key is null or "")
		{
			return image;
		}

		if (_cache.ContainsKey(key))
		{
			_cache[key] = (image, DateTime.Now);
		}
		else
		{
			_cache.Add(key, (image, DateTime.Now));
		}

		return image;
	}

	public Bitmap? GetCache(string key)
	{
		if (_cache.TryGetValue(key, out var value))
		{
			if (DateTime.Now - value.lastAccessed > _expirationTime)
			{
				value.image.Dispose();
				_ = _cache.Remove(key);
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

	public void ClearCache()
	{
		lock (_lockObjects)
		{
			foreach (var (image, lastAccessed) in _cache.Values)
			{
				image?.Dispose();
			}

			_cache.Clear();

			foreach (var item in Directory.EnumerateFiles(CrossIO.Combine(_saveHandler.SaveDirectory, "Thumbs")))
			{
				try
				{
					CrossIO.DeleteFile(item);
				}
				catch { }
			}
		}
	}

	private void ClearOldImages()
	{
		foreach (var item in new DirectoryInfo(CrossIO.Combine(_saveHandler.SaveDirectory, "Thumbs")).EnumerateFiles())
		{
			try
			{
				if (item.LastAccessTimeUtc < DateTime.Now.AddDays(-30))
				{
					CrossIO.DeleteFile(item.FullName);
				}
			}
			catch { }
		}
	}
}