using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;

public interface IAssetUtil
{
	bool IsIncluded(IAsset asset, int? playsetId = null);
	Task SetIncluded(IAsset asset, bool value, int? playsetId = null);
	void SaveChanges();
	IAsset? GetAssetByFile(string? fileName);
	IEnumerable<IAsset> GetAssets(string folder, out int assetCount, bool withSubDirectories = true);
	void DeleteAsset(IAsset asset);
}
