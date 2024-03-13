namespace Skyve.Compatibility.Domain.Enums;
public enum PackageType
{
	GenericPackage = 0,
	MusicPack = 1,
#if CS1 || API
	ThemeMix = 2,
	IMTMarkings = 3,
	RenderItPreset = 4,
	POFont = 5,
	NameList = 6,
	CSM = 7,
#endif
	ContentPackage = 8
}
