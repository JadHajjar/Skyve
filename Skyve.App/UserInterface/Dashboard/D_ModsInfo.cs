using Skyve.App.UserInterface.Panels;

using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;

internal class D_ModsInfo : IDashboardItem
{
	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IPackageManager _contentManager;
	private readonly IUpdateManager _updateManager;

	private int mainSectionHeight;
	private bool contentLoading;
	private int modsTotal, modsIncluded, modsEnabled, modsOutOfDate, modsIncomplete;
	private readonly List<ILocalPackageData> newMods;

	public D_ModsInfo()
	{
		ServiceCenter.Get(out _settings, out _notifier, out _packageUtil, out _contentManager, out _updateManager);

		newMods = _updateManager.GetNewOrUpdatedPackages().Where(x => x.IsCodeMod).ToList();
		RefreshModCounts();
	}

	private void OpenRecentModsPanel()
	{
		if (Program.MainForm.PushPanel<PC_Mods>())
		{
			(Program.MainForm.CurrentPanel as PC_Mods)?.SetSorting(PackageSorting.UpdateTime, true);
		}
	}

	private void OpenModsPanel()
	{
		Program.MainForm.PushPanel<PC_Mods>();
	}

	private void RefreshModCounts()
	{
		lock (this)
		{
			int modsTotal = 0, modsIncluded = 0, modsEnabled = 0, modsOutOfDate = 0, modsIncomplete = 0;

			foreach (var mod in _contentManager.Packages)
			{
				modsTotal++;

				if (!mod.IsIncluded())
				{
					continue;
				}

				modsIncluded++;

				if (mod.IsEnabled())
				{
					modsEnabled++;
				}

				if (Loading)
				{
					continue;
				}

				switch (_packageUtil.GetStatus(mod, out _))
				{
					case DownloadStatus.OutOfDate:
						modsOutOfDate++;
						break;
					case DownloadStatus.PartiallyDownloaded:
						modsIncomplete++;
						break;
				}
			}

			this.modsEnabled = modsEnabled;
			this.modsTotal = modsTotal;
			this.modsIncluded = modsIncluded;
			this.modsOutOfDate = modsOutOfDate;
			this.modsIncomplete = modsIncomplete;
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
		RefreshModCounts();
		OnResizeRequested();
	}

	private void ProfileManager_ProfileChanged()
	{
		RefreshModCounts();
		OnResizeRequested();
	}

	private void CentralManager_WorkshopInfoUpdated()
	{
		if (Loading)
		{
			Loading = false;

			RefreshModCounts();
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
				RefreshModCounts();
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
		DrawLoadingSection(e, applyDrawing, Locale.ModsBubble, "I_Mods", ref preferredHeight);
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle.ClipTo(mainSectionHeight), Locale.ModsBubble, "I_Mods", out var fore, ref preferredHeight);

		var textRect = e.ClipRectangle.Pad(Margin);

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}
		else if (modsIncluded == modsEnabled)
		{
			e.Graphics.DrawStringItem(Locale.IncludedEnabledCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}
		else
		{
			e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);

			e.Graphics.DrawStringItem(Locale.EnabledCount.FormatPlural(modsEnabled, Locale.Mod.FormatPlural(modsEnabled).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		e.Graphics.DrawStringItem(Locale.TotalCount.FormatPlural(modsTotal, Locale.Mod.FormatPlural(modsTotal).ToLower())
			, Font
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		if (newMods.Count > 0)
		{
			e.Graphics.DrawStringItem(Locale.NewUpdatedCount.FormatPlural(newMods.Count, Locale.Mod.FormatPlural(newMods.Count).ToLower())
				, Font
				, FormDesign.Design.ActiveColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsOutOfDate > 0)
		{
			e.Graphics.DrawStringItem(Locale.OutOfDateCount.FormatPlural(modsOutOfDate, Locale.Mod.FormatPlural(modsOutOfDate).ToLower())
				, Font
				, FormDesign.Design.YellowColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsIncomplete > 0)
		{
			e.Graphics.DrawStringItem(Locale.IncompleteCount.FormatPlural(modsIncomplete, Locale.Mod.FormatPlural(modsIncomplete).ToLower())
				, Font
				, FormDesign.Design.RedColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += Margin.Top;

		DrawButton(e, applyDrawing, ref preferredHeight, OpenModsPanel, new()
		{
			Text = Locale.ViewAllYourItems.Format(Locale.Mod.Plural.ToLower()),
			Icon = "I_ViewFile",
			Rectangle = e.ClipRectangle
		});

		DrawButton(e, applyDrawing, ref preferredHeight, OpenRecentModsPanel, new()
		{
			Text = Locale.ViewRecentlyUpdatedItems.Format(Locale.Mod.Plural.ToLower()),
			Icon = "I_UpdateTime",
			Rectangle = e.ClipRectangle
		});
		preferredHeight -= Margin.Top;
	}

	private void DrawLandscape(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		var mainRect = e.ClipRectangle.Pad(0, 0, e.ClipRectangle.Width / 2, 0);

		DrawSection(e, applyDrawing, mainRect.ClipTo(mainSectionHeight), Locale.ModsBubble, "I_Mods", out var fore, ref preferredHeight);

		var textRect = mainRect.Pad(Margin);

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}
		else if (modsIncluded == modsEnabled)
		{
			e.Graphics.DrawStringItem(Locale.IncludedEnabledCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}
		else
		{
			e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);

			e.Graphics.DrawStringItem(Locale.EnabledCount.FormatPlural(modsEnabled, Locale.Mod.FormatPlural(modsEnabled).ToLower())
				, Font
				, fore
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		e.Graphics.DrawStringItem(Locale.TotalCount.FormatPlural(modsTotal, Locale.Mod.FormatPlural(modsTotal).ToLower())
			, Font
			, fore
			, textRect
			, ref preferredHeight
			, applyDrawing);

		if (newMods.Count > 0)
		{
			e.Graphics.DrawStringItem(Locale.NewUpdatedCount.FormatPlural(newMods.Count, Locale.Mod.FormatPlural(newMods.Count).ToLower())
				, Font
				, FormDesign.Design.ActiveColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsOutOfDate > 0)
		{
			e.Graphics.DrawStringItem(Locale.OutOfDateCount.FormatPlural(modsOutOfDate, Locale.Mod.FormatPlural(modsOutOfDate).ToLower())
				, Font
				, FormDesign.Design.YellowColor
				, textRect
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsIncomplete > 0)
		{
			e.Graphics.DrawStringItem(Locale.IncompleteCount.FormatPlural(modsIncomplete, Locale.Mod.FormatPlural(modsIncomplete).ToLower())
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

		DrawButton(e, applyDrawing, ref preferredHeight, OpenModsPanel, new()
		{
			Text = Locale.ViewAllYourItems.Format(Locale.Mod.Plural.ToLower()),
			Icon = "I_ViewFile",
			Rectangle = mainRect
		});

		DrawButton(e, applyDrawing, ref preferredHeight, OpenRecentModsPanel, new()
		{
			Text = Locale.ViewRecentlyUpdatedItems.Format(Locale.Mod.Plural.ToLower()),
			Icon = "I_UpdateTime",
			Rectangle = mainRect
		});

		preferredHeight -= Margin.Top;
		//preferredHeight += (int)(16 * UI.FontScale);

		preferredHeight = Math.Max(mainSectionHeight + mainRect.Y, preferredHeight);
	}
}
