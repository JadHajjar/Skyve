using System.Windows.Forms;

namespace Skyve.App.UserInterface.Bubbles;

public class AssetsBubble : StatusBubbleBase
{
	private readonly INotifier _notifier;
	private readonly IPackageUtil _contentUtil;
	private readonly IPackageManager _contentManager;

	private readonly Dictionary<NotificationType, int> _compatibilityCounts;

	public AssetsBubble()
	{
		_compatibilityCounts = new();

		ServiceCenter.Get(out _notifier, out _contentUtil, out _contentManager);

	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (!Live)
		{
			return;
		}

		ImageName = "I_Assets";
		Text = Locale.AssetsBubble;

		if (!_notifier.IsContentLoaded)
		{
			Loading = true;

			_notifier.ContentLoaded += Invalidate;
		}
		else
		{
			Notifier_CompatibilityReportProcessed();
		}

		_notifier.WorkshopInfoUpdated += CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated += Invalidate;
		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
		_notifier.CompatibilityReportProcessed += Notifier_CompatibilityReportProcessed;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.ContentLoaded -= Invalidate;
		_notifier.WorkshopInfoUpdated -= CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated -= Invalidate;
		_notifier.PlaysetChanged -= ProfileManager_ProfileChanged;
		_notifier.CompatibilityReportProcessed -= Notifier_CompatibilityReportProcessed;
	}

	private void Notifier_CompatibilityReportProcessed()
	{
		_compatibilityCounts.Clear();

		foreach (var asset in _contentManager.Assets)
		{
			if (!_contentUtil.IsIncluded(asset))
			{
				continue;
			}

			if (asset.IsCodeMod || Loading)
			{
				continue;
			}

			var notif = asset.GetCompatibilityInfo(cacheOnly: true).GetNotification();

			if (_compatibilityCounts.ContainsKey(notif))
			{
				_compatibilityCounts[notif]++;
			}
			else
			{
				_compatibilityCounts[notif] = 1;
			}
		}

		if (Loading)
		{
			Loading = false;
		}
	}

	private void ProfileManager_ProfileChanged()
	{
		Invalidate();
	}

	private void CentralManager_WorkshopInfoUpdated()
	{
		if (Loading)
		{
			Loading = false;
		}
		else
		{
			Invalidate();
		}
	}

	protected override void CustomDraw(PaintEventArgs e, ref int targetHeight)
	{
		if (!_notifier.IsContentLoaded)
		{
			DrawText(e, ref targetHeight, Locale.Loading, FormDesign.Design.InfoColor);
			return;
		}

		int assetsIncluded = 0, assetsOutOfDate = 0, assetsIncomplete = 0;
		var assetSize = 0L;

		foreach (var asset in _contentManager.Assets)
		{
			if (!_contentUtil.IsIncluded(asset))
			{
				continue;
			}

			assetsIncluded++;
			assetSize += asset.LocalSize;

			if (asset.IsCodeMod || Loading)
			{
				continue;
			}

			switch (_contentUtil.GetStatus(asset, out _))
			{
				case DownloadStatus.OutOfDate:
					assetsOutOfDate++;
					break;
				case DownloadStatus.PartiallyDownloaded:
					assetsIncomplete++;
					break;
			}
		}

		DrawText(e, ref targetHeight, Locale.IncludedCount.FormatPlural(assetsIncluded, Locale.Asset.FormatPlural(assetsIncluded).ToLower()));
		DrawValue(e, ref targetHeight, assetSize.SizeString(), Locale.TotalAssetSize);

		if (assetsOutOfDate > 0)
		{
			DrawText(e, ref targetHeight, Locale.OutOfDateCount.FormatPlural(assetsOutOfDate, Locale.Asset.FormatPlural(assetsOutOfDate).ToLower()), FormDesign.Design.YellowColor);
		}

		if (assetsIncomplete > 0)
		{
			DrawText(e, ref targetHeight, Locale.IncompleteCount.FormatPlural(assetsIncomplete, Locale.Asset.FormatPlural(assetsIncomplete).ToLower()), FormDesign.Design.RedColor);
		}

		foreach (var group in _compatibilityCounts.OrderBy(x => x.Key))
		{
			if (group.Key <= NotificationType.Info)
			{
				continue;
			}

			DrawText(e, ref targetHeight, LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Asset.FormatPlural(group.Value).ToLower()), group.Key.GetColor());
		}
	}
}
