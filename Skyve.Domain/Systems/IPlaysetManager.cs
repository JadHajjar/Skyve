using SkyveApp.Domain.Enums;

using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface IPlaysetManager
{
	ICustomPlayset CurrentPlayset { get; }
	IEnumerable<ICustomPlayset> Playsets { get; }

	event PromptMissingItemsDelegate PromptMissingItems;

	string GetNewPlaysetName();
	void AddPlayset(ICustomPlayset playset);
	void DeletePlayset(ICustomPlayset playset);
	ICustomPlayset? ImportPlayset(string obj);
	void ExcludeFromCurrentPlayset(IPlayset playset);
	void MergeIntoCurrentPlayset(IPlayset playset);
	void GatherInformation(IPlayset playset);
	IAsset? GetAsset(IPlaysetEntry asset);
	IMod? GetMod(IPlaysetEntry mod);
	bool IsPackageIncludedInPlayset(IPackage package, IPlayset playset);
	bool RenamePlayset(IPlayset playset, string newName);
	bool Save(IPlayset? playset, bool forced = false);
	void SetIncludedFor(IPackage package, IPlayset playset, bool value);
	void SetIncludedForAll(IPackage package, bool value);
	void SetCurrentPlayset(ICustomPlayset playset);
	string GetFileName(IPlayset profile);
	void CreateShortcut(IPlayset item);
	List<ILocalPackageWithContents> GetInvalidPackages(PackageUsage usage);
	void SaveLsmSettings(IPlayset profile);
	ICustomPlayset? ConvertLegacyPlayset(string profilePath, bool removeLegacyFile = true);
}
