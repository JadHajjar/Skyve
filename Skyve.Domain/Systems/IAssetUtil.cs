﻿using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;

public interface IAssetUtil
{
	bool IsIncluded(IAsset asset);
	void SetIncluded(IAsset asset, bool value);
	void SaveChanges();
	IAsset? GetAssetByFile(string? fileName);
	IEnumerable<IAsset> GetAssets(ILocalPackageWithContents package, bool withSubDirectories = true);
}
