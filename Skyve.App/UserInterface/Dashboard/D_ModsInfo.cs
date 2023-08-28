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
	private readonly INotificationsService _notificationsService;

	private readonly Dictionary<NotificationType, int> _compatibilityCounts;
	private int mainSectionHeight;

	public D_ModsInfo()
	{
		_compatibilityCounts = new();
		ServiceCenter.Get(out _settings, out _notifier, out _packageUtil, out _contentManager, out _notificationsService);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (!Live)
		{
			return;
		}

		ImageName = "I_Mods";
		Text = Locale.ModsBubble;

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
			Loading = false;
		}
		else
		{
			Invalidate();
		}
	}

	//public override void DrawItem(PaintEventArgs e)
	//{
	//	DrawSection(e, e.ClipRectangle, null);

		//if (!_notifier.IsContentLoaded)
		//{
		//	DrawText(e, ref targetHeight, Locale.Loading, FormDesign.Design.InfoColor);
		//	return;
		//}

		//int modsIncluded = 0, modsEnabled = 0, modsOutOfDate = 0, modsIncomplete = 0;

		//foreach (var mod in _contentManager.Mods)
		//{
		//	if (!_packageUtil.IsIncluded(mod))
		//	{
		//		continue;
		//	}

		//	modsIncluded++;

		//	if (_packageUtil.IsEnabled(mod))
		//	{
		//		modsEnabled++;
		//	}

		//	if (Loading)
		//	{
		//		continue;
		//	}

		//	switch (_packageUtil.GetStatus(mod, out _))
		//	{
		//		case DownloadStatus.OutOfDate:
		//			modsOutOfDate++;
		//			break;
		//		case DownloadStatus.PartiallyDownloaded:
		//			modsIncomplete++;
		//			break;
		//	}
		//}

		//if (!_settings.UserSettings.AdvancedIncludeEnable)
		//{
		//	DrawText(e, ref targetHeight, Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower()));
		//}
		//else if (modsIncluded == modsEnabled)
		//{
		//	DrawText(e, ref targetHeight, Locale.IncludedEnabledCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower()));
		//}
		//else
		//{
		//	DrawText(e, ref targetHeight, Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower()));
		//	DrawText(e, ref targetHeight, Locale.EnabledCount.FormatPlural(modsEnabled, Locale.Mod.FormatPlural(modsEnabled).ToLower()));
		//}

		//if (modsOutOfDate > 0)
		//{
		//	DrawText(e, ref targetHeight, Locale.OutOfDateCount.FormatPlural(modsOutOfDate, Locale.Mod.FormatPlural(modsOutOfDate).ToLower()), FormDesign.Design.YellowColor);
		//}

		//if (modsIncomplete > 0)
		//{
		//	DrawText(e, ref targetHeight, Locale.IncompleteCount.FormatPlural(modsIncomplete, Locale.Mod.FormatPlural(modsIncomplete).ToLower()), FormDesign.Design.RedColor);
		//}

		//foreach (var group in _compatibilityCounts.OrderBy(x => x.Key))
		//{
		//	if (group.Key <= NotificationType.Info)
		//	{
		//		continue;
		//	}

		//	DrawText(e, ref targetHeight, LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower()), group.Key.GetColor());
		//}

		//height = targetHeight;
	//}

	protected override DrawingDelegate GetDrawingMethod(int width)
	{
		return Draw;
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle.ClipTo(mainSectionHeight), Locale.ModsBubble, "I_Mods", out var fore, ref preferredHeight);

		int modsIncluded = 0, modsEnabled = 0, modsOutOfDate = 0, modsIncomplete = 0;

		foreach (var mod in _contentManager.Mods)
		{
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
				, e.ClipRectangle.Pad(Margin.Left, 0, 0, 0)
				, ref preferredHeight
				, applyDrawing);
		}
		else if (modsIncluded == modsEnabled)
		{
			e.Graphics.DrawStringItem(Locale.IncludedEnabledCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, 0, 0)
				, ref preferredHeight
				, applyDrawing);
		}
		else
		{
			e.Graphics.DrawStringItem(Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, 0, 0)
				, ref preferredHeight
				, applyDrawing);

			e.Graphics.DrawStringItem(Locale.EnabledCount.FormatPlural(modsEnabled, Locale.Mod.FormatPlural(modsEnabled).ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, 0, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsOutOfDate > 0)
		{
			e.Graphics.DrawStringItem(Locale.OutOfDateCount.FormatPlural(modsOutOfDate, Locale.Mod.FormatPlural(modsOutOfDate).ToLower())
				, Font
				, FormDesign.Design.YellowColor
				, e.ClipRectangle.Pad(Margin.Left, 0, 0, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		if (modsIncomplete > 0)
		{
			e.Graphics.DrawStringItem(Locale.IncompleteCount.FormatPlural(modsIncomplete, Locale.Mod.FormatPlural(modsIncomplete).ToLower())
				, Font
				, FormDesign.Design.RedColor
				, e.ClipRectangle.Pad(Margin.Left, 0, 0, 0)
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
				, e.ClipRectangle.Pad(Margin.Left, 0, 0, 0)
				, ref preferredHeight
				, applyDrawing);
		}

		preferredHeight += Margin.Top;
	}
}
