using Skyve.App.UserInterface.Bubbles;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;

internal class D_ModsInfo :  IDashboardItem
{

	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IPackageManager _contentManager;
	private readonly IUpdateManager _updateManager;

	private readonly Dictionary<NotificationType, int> _compatibilityCounts;
	private int mainSectionHeight;
	private bool contentLoading;

	public D_ModsInfo()
	{
		_compatibilityCounts = new();
		ServiceCenter.Get(out _settings, out _notifier, out _packageUtil, out _contentManager, out _updateManager);
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
		else
		{
			Notifier_CompatibilityReportProcessed();
		}

		_notifier.WorkshopInfoUpdated += CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated += PackageInformationUpdated;
		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
		_notifier.CompatibilityReportProcessed += Notifier_CompatibilityReportProcessed;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.WorkshopInfoUpdated -= CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated -= PackageInformationUpdated;
		_notifier.PlaysetChanged -= ProfileManager_ProfileChanged;
		_notifier.CompatibilityReportProcessed -= Notifier_CompatibilityReportProcessed;
	}

	private void PackageInformationUpdated()
	{
		OnResizeRequested();
	}

	private void Notifier_CompatibilityReportProcessed()
	{
		_compatibilityCounts.Clear();

		foreach (var mod in _contentManager.Mods)
		{
			if (!_packageUtil.IsIncluded(mod))
			{
				continue;
			}

			var notif = mod.GetCompatibilityInfo(cacheOnly: true).GetNotification();

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

		OnResizeRequested();
	}

	private void ProfileManager_ProfileChanged()
	{
		Invalidate();
	}

	private void CentralManager_WorkshopInfoUpdated()
	{
		if (Loading)
		{
			OnResizeRequested();

			Loading = false;
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

				OnResizeRequested();
			}

			return DrawLoading;
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

		int modsTotal = 0, modsIncluded = 0, modsEnabled = 0, modsOutOfDate = 0, modsIncomplete = 0;
		var newMods = _updateManager.GetNewPackages().ToList();

		foreach (var mod in _contentManager.Mods)
		{
			modsTotal++;

			if (!_packageUtil.IsIncluded(mod))
			{
				continue;
			}

			modsIncluded++;

			if (_packageUtil.IsEnabled(mod))
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

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}
		else if (modsIncluded == modsEnabled)
		{
			e.Graphics.DrawStringItem(Locale.IncludedEnabledCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}
		else
		{
			e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);

			e.Graphics.DrawStringItem(Locale.EnabledCount.FormatPlural(modsEnabled, Locale.Mod.FormatPlural(modsEnabled).ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		e.Graphics.DrawStringItem(Locale.TotalCount.FormatPlural(modsTotal, Locale.Mod.FormatPlural(modsTotal).ToLower())
			, Font
			, fore
			, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
			, ref preferredHeight
			, applyDrawing);

		if (newMods.Count > 0)
		{
			e.Graphics.DrawStringItem(Locale.NewUpdatedCount.FormatPlural(newMods.Count, Locale.Mod.FormatPlural(newMods.Count).ToLower())
				, Font
				, FormDesign.Design.ActiveColor
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsOutOfDate > 0)
		{
			e.Graphics.DrawStringItem(Locale.OutOfDateCount.FormatPlural(modsOutOfDate, Locale.Mod.FormatPlural(modsOutOfDate).ToLower())
				, Font
				, FormDesign.Design.YellowColor
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsIncomplete > 0)
		{
			e.Graphics.DrawStringItem(Locale.IncompleteCount.FormatPlural(modsIncomplete, Locale.Mod.FormatPlural(modsIncomplete).ToLower())
				, Font
				, FormDesign.Design.RedColor
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		preferredHeight += Margin.Top;

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		if (_compatibilityCounts.Count == 0)
			return;

		preferredHeight += Margin.Top;

		DrawSection(e, applyDrawing, new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, e.ClipRectangle.Bottom - preferredHeight), Locale.CompatibilityReport, "I_CompatibilityReport", out _, ref preferredHeight);

		foreach (var group in _compatibilityCounts.OrderBy(x => x.Key))
		{
			if (group.Key <= NotificationType.Info)
			{
				continue;
			}

			e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower())
				, Font
				, group.Key.GetColor()
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		preferredHeight += Margin.Top;
	}
}
