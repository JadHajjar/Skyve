using Skyve.Domain.Enums;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IPlaysetManager
{
	ICustomPlayset? CurrentPlayset { get; }
	IEnumerable<ICustomPlayset> Playsets { get; }

	void AddPlayset(ICustomPlayset newPlayset);
	void CreateShortcut(IPlayset item);
	Task<bool> DeletePlayset(ICustomPlayset playset);
	Task<bool> ExcludeFromCurrentPlayset(IPlayset playset);
	string GetFileName(IPlayset playset);
	List<IPackage> GetInvalidPackages(PackageUsage usage);
	Task<ICustomPlayset?> CreateNewPlayset(string playsetName);
	ICustomPlayset? ImportPlayset(string obj);
	Task<bool> MergeIntoCurrentPlayset(IPlayset playset);
	Task<bool> RenamePlayset(IPlayset playset, string text);
	void SetCurrentPlayset(ICustomPlayset playset);
	Task Initialize();
	Task SetIncludedForAll(IPackageIdentity package, bool value);
	Task SetIncludedForAll(IEnumerable<IPackageIdentity> packages, bool value);
	Task SetEnabledForAll(IPackageIdentity package, bool value);
	Task SetEnabledForAll(IEnumerable<IPackageIdentity> packages, bool value);

#if CS1
	ICustomPlayset TemporaryPlayset { get; }

	event PromptMissingItemsDelegate PromptMissingItems;

	void OnAutoSave();
	void SaveLsmSettings(IPlayset profile);
	void RunFirstTimeSetup();
	bool Save(IPlayset? playset, bool forced = false);
	void GatherInformation(IPlayset playset);
	IAsset? GetAsset(IPlaysetEntry asset);
	IMod? GetMod(IPlaysetEntry mod);
	string GetNewPlaysetName();
	ICustomPlayset? ConvertLegacyPlayset(string profilePath, bool removeLegacyFile = true);
#endif
}
