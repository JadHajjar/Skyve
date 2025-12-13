using Extensions;

using Skyve.Domain.Systems;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Skyve.Systems;
internal class ImageProcessor : PeriodicProcessor<ImageProcessor.ImageRequest, ImageProcessor.TimeStampedImage>
{
	private readonly IImageService _imageManager;

	public ImageProcessor(IImageService imageManager) : base(10, 150, 0, null)
	{
		_imageManager = imageManager;
	}

	protected override bool CanProcess()
	{
		return true;
	}

	protected override async Task<(ConcurrentDictionary<ImageRequest, TimeStampedImage>, bool)> ProcessItems(List<ImageRequest> entities)
	{
		foreach (var img in entities)
		{
			if (!string.IsNullOrWhiteSpace(img.Url))
			{
				await _imageManager.Ensure(img.Url, false, img.FileName, img.Square, img.IsFilePath, img.DownscaleTo);
			}
		}

		return ([], false);
	}

	protected override void CacheItems(ConcurrentDictionary<ImageRequest, TimeStampedImage> results)
	{ }

	internal readonly struct ImageRequest
	{
		public ImageRequest(string url, string? fileName, bool square, bool isFilePath, Size? downscaleTo)
		{
			Url = url;
			FileName = fileName;
			Square = square;
			IsFilePath = isFilePath;
			DownscaleTo = downscaleTo;
		}

		public string Url { get; }
		public string? FileName { get; }
		public bool Square { get; }
		public bool IsFilePath { get; }
		public Size? DownscaleTo { get; }
	}

	internal readonly struct TimeStampedImage : ITimestamped
	{
		public DateTime Timestamp { get; }
	}
}
