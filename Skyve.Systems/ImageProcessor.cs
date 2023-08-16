using Extensions;

using Skyve.Domain.Systems;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Systems;
internal class ImageProcessor : PeriodicProcessor<ImageProcessor.ImageRequest, ImageProcessor.TimeStampedImage>
{
	private readonly IImageService _imageManager;

	public ImageProcessor(IImageService imageManager) : base(100, 500, null)
	{
		_imageManager = imageManager;
	}

	protected override bool CanProcess()
	{
		return ConnectionHandler.IsConnected;
	}


	protected override async Task<(Dictionary<ImageRequest, TimeStampedImage>, bool)> ProcessItems(List<ImageRequest> entities)
	{
		foreach (var img in entities)
		{
			if (!string.IsNullOrWhiteSpace(img.Url))
			{
				await _imageManager.Ensure(img.Url, false, img.FileName, img.Square);
			}
		}

		return (new(), false);
	}

	protected override void CacheItems(Dictionary<ImageRequest, TimeStampedImage> results)
	{ }

	internal readonly struct ImageRequest
	{
		public ImageRequest(string url, string? fileName, bool square)
		{
			Url = url;
			FileName = fileName;
			Square = square;
		}

		public string Url { get; }
		public string? FileName { get; }
		public bool Square { get; }
	}

	internal readonly struct TimeStampedImage : ITimestamped
	{
		public DateTime Timestamp { get; }
	}
}
