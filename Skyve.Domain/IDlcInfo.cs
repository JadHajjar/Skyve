using System;

namespace Skyve.Domain;
public interface IDlcInfo
{
	uint Id { get; }
	string Name { get; }
	string ThumbnailUrl { get; }
	DateTime ReleaseDate { get; }
	string Description { get; }
	string? Price { get; set; }
	float Discount { get; set; }
	string[]? Creators { get; set; }
}
