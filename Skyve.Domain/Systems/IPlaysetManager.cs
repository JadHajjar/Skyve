using Skyve.Domain.Enums;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IPlaysetManager
{
	IPlayset? CurrentPlayset { get; }
	ICustomPlayset? CurrentCustomPlayset { get; }
	IEnumerable<IPlayset> Playsets { get; }

	Task<IPlayset?> AddPlayset(IPlayset newPlayset);
	void CreateShortcut(IPlayset item);
	Task<bool> DeletePlayset(IPlayset? playset);
	Task<bool> ExcludeFromCurrentPlayset(IPlayset playset);
	string GetFileName(IPlayset playset);
	List<IPackage> GetInvalidPackages(IPlayset playset, PackageUsage usage);
	Task<IPlayset?> CreateNewPlayset(string playsetName);
	Task<IPlayset?> ImportPlayset(string fileName, bool createNew = true);
	Task<IPlayset?> CreateLogPlayset(string file);
	Task<bool> MergeIntoCurrentPlayset(IPlayset playset);
	Task<bool> RenamePlayset(IPlayset playset, string text);
	Task ActivatePlayset(IPlayset playset);
	Task Initialize();
	Task SetIncludedForAll(IPackageIdentity package, bool value, bool withVersion = true, bool promptForDependencies = true);
	Task SetIncludedForAll(IEnumerable<IPackageIdentity> packages, bool value, bool withVersion = true, bool promptForDependencies = true);
	Task SetEnabledForAll(IPackageIdentity package, bool value);
	Task SetEnabledForAll(IEnumerable<IPackageIdentity> packages, bool value);
	Task<IPlayset?> ClonePlayset(IPlayset playset);
	IPlayset? GetPlayset(int id);
	ICustomPlayset GetCustomPlayset(IPlayset playset);
	Task DeactivateActivePlayset();
	void Save(ICustomPlayset customPlayset);
	Task<IEnumerable<IPlaysetPackage>> GetPlaysetContents(IPlayset playset, bool includeOnline = true);
	Task<object> GenerateImportPlayset(IPlayset? playset, bool sharing = false, bool includeOnline = true);

#if CS1
	IPlayset TemporaryPlayset { get; }

	event PromptMissingItemsDelegate PromptMissingItems;

	void OnAutoSave();
	void SaveLsmSettings(IPlayset profile);
	void RunFirstTimeSetup();
	bool Save(IPlayset? playset, bool forced = false);
	void GatherInformation(IPlayset playset);
	IAsset? GetAsset(IPlaysetEntry asset);
	IMod? GetMod(IPlaysetEntry mod);
	string GetNewPlaysetName();
	IPlayset? ConvertLegacyPlayset(string profilePath, bool removeLegacyFile = true);
#endif
}
