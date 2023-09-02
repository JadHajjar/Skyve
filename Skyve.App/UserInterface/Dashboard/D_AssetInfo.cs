using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;

internal class D_AssetsInfo : IDashboardItem
{
	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IPackageManager _contentManager;
	private readonly IUpdateManager _updateManager;

	private int mainSectionHeight;
	private bool contentLoading;
	private long assetsSize;
	private int assetsTotal;
	private int assetsIncluded;
	private int assetsOutOfDate;
	private int assetsIncomplete;
	private readonly List<ILocalPackageWithContents> newAssets;

	public D_AssetsInfo()
	{
		ServiceCenter.Get(out _settings, out _notifier, out _packageUtil, out _contentManager, out _updateManager);

		newAssets = _updateManager.GetNewOrUpdatedPackages().ToList();
		RefreshAssetCounts();
	}

	private void OpenRecentAssetsPanel()
	{
		if (Program.MainForm.PushPanel<PC_Assets>())
		{
			(Program.MainForm.CurrentPanel as PC_Assets)?.SetSorting(PackageSorting.UpdateTime, true); 
		}
	}

	private void OpenAssetsPanel()
	{
		Program.MainForm.PushPanel<PC_Assets>();
	}

	private void RefreshAssetCounts()
	{
		lock (this)
		{
			int assetsTotal = 0, assetsIncluded = 0, assetsOutOfDate = 0, assetsIncomplete = 0;
			var assetsSize = 0L;

			foreach (var asset in _contentManager.Assets)
			{
				assetsTotal++;

				if (!_packageUtil.IsIncluded(asset))
				{
					continue;
				}

				assetsIncluded++;
				assetsSize += asset.LocalSize;

				if (Loading)
				{
					continue;
				}

				switch (_packageUtil.GetStatus(asset, out _))
				{
					case DownloadStatus.OutOfDate:
						assetsOutOfDate++;
						break;
					case DownloadStatus.PartiallyDownloaded:
						assetsIncomplete++;
						break;
				}
			}

			this.assetsSize = assetsSize;
			this.assetsTotal = assetsTotal;
			this.assetsIncluded = assetsIncluded;
			this.assetsOutOfDate = assetsOutOfDate;
			this.assetsIncluded = assetsIncluded;
			this.assetsIncomplete = assetsIncomplete;
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (!Live)
		{
			return;
		}

		if (!_notifier.IsContentLoaded)
		{
			Loading = contentLoading = true;
		}

		_notifier.WorkshopInfoUpdated += CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated += PackageInformationUpdated;
		_notifier.PackageInclusionUpdated += PackageInformationUpdated;
		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.WorkshopInfoUpdated -= CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated -= PackageInformationUpdated;
		_notifier.PackageInclusionUpdated -= PackageInformationUpdated;
		_notifier.PlaysetChanged -= ProfileManager_ProfileChanged;
	}

	private void PackageInformationUpdated()
	{
		RefreshAssetCounts();
		OnResizeRequested();
	}

	private void ProfileManager_ProfileChanged()
	{
		RefreshAssetCounts();
		OnResizeRequested();
	}

	private void CentralManager_WorkshopInfoUpdated()
	{
		if (Loading)
		{
			Loading = false;

			RefreshAssetCounts();
			OnResizeRequested();
		}
		else
		{
			Invalidate();
		}
	}

	protected override DrawingDelegate GetDrawingMethod(int width)
	{
		if (contentLoading)
		{
			if (_notifier.IsContentLoaded)
			{
				contentLoading = false;
				RefreshAssetCounts();
				OnResizeRequested();
			}
			else
			{
				return DrawLoading;
			}
		}

		if (width > 450 * UI.FontScale)
		{
			return DrawLandscape;
		}

		return Draw;
	}

	private void DrawLoading(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawLoadingSection(e, applyDrawing, Locale.AssetsBubble, "I_Assets", ref preferredHeight);
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle.ClipTo(mainSectionHeight), Locale.AssetsBubble, "I_Assets", out var fore, ref preferredHeight);

		var textRect = e.ClipRectangle.Pad(Margin);

		e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(assetsIncluded, Locale.Asset.FormatPlural(assetsIncluded).ToLower())
			, Font
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		using var bold = new Font(Font, FontStyle.Bold);
		e.Graphics.DrawStringItem(Locale.TotalAssetSize.Format(assetsSize.SizeString())
			, bold
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		e.Graphics.DrawStringItem(Locale.TotalCount.FormatPlural(assetsTotal, Locale.Asset.FormatPlural(assetsTotal).ToLower())
			, Font
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		if (newAssets.Count > 0)
		{
			e.Graphics.DrawStringItem(Locale.NewUpdatedCount.FormatPlural(newAssets.Count, Locale.Asset.FormatPlural(newAssets.Count).ToLower())
				, Font
				, FormDesign.Design.ActiveColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (assetsOutOfDate > 0)
		{
			e.Graphics.DrawStringItem(Locale.OutOfDateCount.FormatPlural(assetsOutOfDate, Locale.Asset.FormatPlural(assetsOutOfDate).ToLower())
				, Font
				, FormDesign.Design.YellowColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (assetsIncomplete > 0)
		{
			e.Graphics.DrawStringItem(Locale.IncompleteCount.FormatPlural(assetsIncomplete, Locale.Asset.FormatPlural(assetsIncomplete).ToLower())
				, Font
				, FormDesign.Design.RedColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += Margin.Top;

		DrawButton(e, applyDrawing, ref preferredHeight, OpenAssetsPanel, new()
		{
			Text = Locale.ViewAllYourItems.Format(Locale.Asset.Plural.ToLower()),
			Icon = "I_ViewFile",
			Rectangle = e.ClipRectangle
		});

		DrawButton(e, applyDrawing, ref preferredHeight, OpenRecentAssetsPanel, new()
		{
			Text = Locale.ViewRecentlyUpdatedItems.Format(Locale.Asset.Plural.ToLower()),
			Icon = "I_UpdateTime",
			Rectangle = e.ClipRectangle
		});
		preferredHeight -= Margin.Top;
	}

	private void DrawLandscape(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		var mainRect = e.ClipRectangle.Pad(0, 0, e.ClipRectangle.Width / 2, 0);

		DrawSection(e, applyDrawing, mainRect.ClipTo(mainSectionHeight), Locale.AssetsBubble, "I_Assets", out var fore, ref preferredHeight);

		var textRect = mainRect.Pad(Margin);

		e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(assetsIncluded, Locale.Asset.FormatPlural(assetsIncluded).ToLower())
			, Font
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		using var bold = new Font(Font, FontStyle.Bold);
		e.Graphics.DrawStringItem(Locale.TotalAssetSize.Format(assetsSize.SizeString())
			, bold
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		e.Graphics.DrawStringItem(Locale.TotalCount.FormatPlural(assetsTotal, Locale.Asset.FormatPlural(assetsTotal).ToLower())
			, Font
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		if (newAssets.Count > 0)
		{
			e.Graphics.DrawStringItem(Locale.NewUpdatedCount.FormatPlural(newAssets.Count, Locale.Asset.FormatPlural(newAssets.Count).ToLower())
				, Font
				, FormDesign.Design.ActiveColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (assetsOutOfDate > 0)
		{
			e.Graphics.DrawStringItem(Locale.OutOfDateCount.FormatPlural(assetsOutOfDate, Locale.Asset.FormatPlural(assetsOutOfDate).ToLower())
				, Font
				, FormDesign.Design.YellowColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (assetsIncomplete > 0)
		{
			e.Graphics.DrawStringItem(Locale.IncompleteCount.FormatPlural(assetsIncomplete, Locale.Asset.FormatPlural(assetsIncomplete).ToLower())
				, Font
				, FormDesign.Design.RedColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		mainSectionHeight = preferredHeight - mainRect.Y;

		preferredHeight = e.ClipRectangle.Y;

		mainRect.X += mainRect.Width + Padding.Left;
		mainRect.Width -= Padding.Left;

		DrawButton(e, applyDrawing, ref preferredHeight, OpenAssetsPanel, new()
		{
			Text = Locale.ViewAllYourItems.Format(Locale.Asset.Plural.ToLower()),
			Icon = "I_ViewFile",
			Rectangle = mainRect
		});

		DrawButton(e, applyDrawing, ref preferredHeight, OpenRecentAssetsPanel, new()
		{
			Text = Locale.ViewRecentlyUpdatedItems.Format(Locale.Asset.Plural.ToLower()),
			Icon = "I_UpdateTime",
			Rectangle = mainRect
		});

		preferredHeight -= Margin.Top;
		//preferredHeight += (int)(16 * UI.FontScale);

		preferredHeight = Math.Max(mainSectionHeight + mainRect.Y, preferredHeight);
	}
}
