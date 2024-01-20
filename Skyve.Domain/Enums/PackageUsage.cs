using System;

namespace Skyve.Domain.Enums;

[Flags]
public enum PackageUsage
{
	CityBuilding = 1 << 0,
	AssetCreation = 1 << 1,
	MapCreation = 1 << 2,
#if CS1
	ScenarioMaking = 1 << 3,
	ThemeMaking = 1 << 4,
#endif
}
