using Extensions;

using Skyve.Domain;
using Skyve.Domain.Systems;

using System.Collections.Generic;
using System.Linq;

namespace Skyve.Systems;
internal class BulkUtil : IBulkUtil
{
	private readonly INotifier _notifier;
	private readonly IModUtil _modUtil;
	private readonly IAssetUtil _assetUtil;

	public BulkUtil(INotifier notifier, IModUtil modUtil, IAssetUtil assetUtil)
	{
		_notifier = notifier;
		_modUtil = modUtil;
		_assetUtil = assetUtil;
	}

	public void SetBulkIncluded(IEnumerable<ILocalPackageIdentity> packages, bool value)
	{
		var packageList = packages.ToList();

		if (packageList.Count == 0)
		{
			return;
		}

		if (packageList[0] is ILocalPackageData)
		{
			packageList = packageList.SelectMany(getPackageContents).ToList();
		}

		if (packageList.Count == 0)
		{
			return;
		}

		_notifier.BulkUpdating = true;

		foreach (var package in packageList)
		{
			if (package is IAsset asset)
			{
				_assetUtil.SetIncluded(asset, value);
			}
			else
			{
				_modUtil.SetIncluded(package, value);
			}
		}

		_notifier.BulkUpdating = false;

		if (_notifier.IsContentLoaded)
		{
			_notifier.OnRefreshUI(true);
			_notifier.OnInclusionUpdated();
			_modUtil.SaveChanges();
			_assetUtil.SaveChanges();
			_notifier.TriggerAutoSave();
		}

		static IEnumerable<ILocalPackageIdentity> getPackageContents(ILocalPackageIdentity package)
		{
			if (package is ILocalPackageData packageContainer)
			{
				for (var i = 0; i < packageContainer.Assets.Length; i++)
				{
					yield return packageContainer.Assets[i];
				}
			}
		}
	}

	public void SetBulkEnabled(IEnumerable<ILocalPackageIdentity> packages, bool value)
	{
		var modList = packages.ToList();

		if (modList.Count == 0)
		{
			return;
		}

		_notifier.BulkUpdating = true;

		for (var i = 0; i < modList.Count; i++)
		{
			if (i == modList.Count - 1)
			{
				_notifier.BulkUpdating = false;
			}

			_modUtil.SetEnabled(modList[i], value);
		}

		_notifier.BulkUpdating = false;

		if (_notifier.IsContentLoaded)
		{
			_notifier.OnRefreshUI(true);
			_notifier.OnInclusionUpdated();
			_modUtil.SaveChanges();
			_notifier.TriggerAutoSave();
		}
	}
}
