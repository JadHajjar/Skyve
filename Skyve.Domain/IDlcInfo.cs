using System;

namespace Skyve.Domain;
public interface IDlcInfo
{
	ulong Id { get; }
	string Name { get; }
	DateTime ReleaseDate { get; }
	string Description { get; }
	string? Price { get; set; }
	float Discount { get; set; }
	string[]? Creators { get; set; }
	string? Url { get; }
}
