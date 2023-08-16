using System;

namespace Skyve.Domain;
public interface IDlcInfo
{
	uint Id { get; }
	string Name { get; }
	string ThumbnailUrl { get; }
	DateTime ReleaseDate { get; }
	string Description { get; }
}
