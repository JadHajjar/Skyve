using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;

internal class D_CompatibilityInfo : IDashboardItem
{
	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IPackageManager _contentManager;
	private readonly ICompatibilityManager _compatibilityManager;

	private Dictionary<NotificationType, int> compatibilityModCounts;
	private Dictionary<NotificationType, int> compatibilityAssetCounts;
	private int mainSectionHeight;
	private int modsSectionHeight;

	public D_CompatibilityInfo()
	{
		compatibilityModCounts = new();
		compatibilityAssetCounts = new();
		ServiceCenter.Get(out _settings, out _notifier, out _packageUtil, out _contentManager, out _compatibilityManager);
	}

	private void ViewCompatibilityReport()
	{
		Program.MainForm.PushPanel<PC_CompatibilityReport>();
	}

	private void ViewAssetsWithIssues()
	{
		if (Program.MainForm.PushPanel<PC_Assets>())
		{
			(Program.MainForm.CurrentPanel as PC_Assets)?.SetSorting(PackageSorting.CompatibilityReport, true);
			(Program.MainForm.CurrentPanel as PC_Assets)?.SetCompatibilityFilter(Dropdowns.CompatibilityNotificationFilter.AnyIssue);
			(Program.MainForm.CurrentPanel as PC_Assets)?.SetIncludedFilter(Generic.ThreeOptionToggle.Value.Option1);
		}
	}

	private void ViewModsWithIssues()
	{
		if (Program.MainForm.PushPanel<PC_Mods>())
		{
			(Program.MainForm.CurrentPanel as PC_Mods)?.SetSorting(PackageSorting.CompatibilityReport, true);
			(Program.MainForm.CurrentPanel as PC_Mods)?.SetCompatibilityFilter(Dropdowns.CompatibilityNotificationFilter.AnyIssue);
			(Program.MainForm.CurrentPanel as PC_Mods)?.SetIncludedFilter(Generic.ThreeOptionToggle.Value.Option1);
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
			Loading = true;

			_notifier.ContentLoaded += Invalidate;
		}
		else
		{
			Notifier_CompatibilityReportProcessed();
		}

		_notifier.WorkshopInfoUpdated += CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated += Notifier_CompatibilityReportProcessed;
		_notifier.PlaysetChanged += Notifier_CompatibilityReportProcessed;
		_notifier.CompatibilityReportProcessed += Notifier_CompatibilityReportProcessed;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.ContentLoaded -= Invalidate;
		_notifier.WorkshopInfoUpdated -= CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated -= Notifier_CompatibilityReportProcessed;
		_notifier.PlaysetChanged -= Notifier_CompatibilityReportProcessed;
		_notifier.CompatibilityReportProcessed -= Notifier_CompatibilityReportProcessed;
	}

	private void Notifier_CompatibilityReportProcessed()
	{
		lock (this)
		{
			var compatibilityModCounts = new Dictionary<NotificationType, int>();
			var compatibilityAssetCounts = new Dictionary<NotificationType, int>();

			foreach (var mod in _contentManager.Packages)
			{
				if (!_packageUtil.IsIncluded(mod))
				{
					continue;
				}

				var notif = mod.GetCompatibilityInfo(cacheOnly: true).GetNotification();

				if (notif <= NotificationType.Info)
				{
					continue;
				}

				if (mod.IsCodeMod)
				{
					if (compatibilityModCounts.ContainsKey(notif))
					{
						compatibilityModCounts[notif]++;
					}
					else
					{
						compatibilityModCounts[notif] = 1;
					}
				}
				else
				{
					if (compatibilityAssetCounts.ContainsKey(notif))
					{
						compatibilityAssetCounts[notif]++;
					}
					else
					{
						compatibilityAssetCounts[notif] = 1;
					}
				}
			}

			this.compatibilityModCounts = compatibilityModCounts;
			this.compatibilityAssetCounts = compatibilityAssetCounts;

			if (Loading)
			{
				Loading = false;
			}

			OnResizeRequested();
		}
	}

	private void CentralManager_WorkshopInfoUpdated()
	{
		if (Loading && _compatibilityManager.FirstLoadComplete)
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
		if (!_compatibilityManager.FirstLoadComplete)
		{
			return DrawLoading;
		}

		if (compatibilityModCounts.Count == 0 && compatibilityAssetCounts.Count == 0)
		{
			return DrawNoIssues;
		}

		if (width > 450 * UI.FontScale)
		{
			return DrawSplit;
		}

		return Draw;
	}

	private void DrawLoading(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawLoadingSection(e, applyDrawing, Locale.CompatibilityReport, "I_CompatibilityReport", ref preferredHeight);
	}

	private void DrawNoIssues(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle.ClipTo(mainSectionHeight), Locale.CompatibilityReport, "I_CompatibilityReport", out var fore, ref preferredHeight);

		e.Graphics.DrawStringItem(Locale.NoCompatibilityIssues
			, Font
			, fore
			, e.ClipRectangle.Pad(Margin)
			, ref preferredHeight
			, applyDrawing);

		preferredHeight += Margin.Top;

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += Margin.Top;

		DrawButton(e, applyDrawing, ref preferredHeight, ViewCompatibilityReport, new()
		{
			Text = Locale.ViewCompatibilityReport,
			Icon = "I_ViewFile",
			Rectangle = e.ClipRectangle
		});

		preferredHeight -= Margin.Bottom;
	}

	private void DrawSplit(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle.ClipTo(mainSectionHeight), Locale.CompatibilityReport, "I_CompatibilityReport", out var fore, ref preferredHeight);

		var rect = new Rectangle(e.ClipRectangle.X, preferredHeight, (e.ClipRectangle.Width / 2) - (Padding.Left / 2), mainSectionHeight - preferredHeight + e.ClipRectangle.Y);

		if (applyDrawing)
		{
			using var modIcon = IconManager.GetIcon("I_Mods", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
			e.Graphics.DrawImage(modIcon, rect.CenterR(modIcon.Size));
		}

		if (compatibilityModCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Mod.Plural.ToLower())
			, Font
			, fore
			, rect.Pad(Margin)
			, ref preferredHeight
			, applyDrawing
			, "I_Check");
		}
		else
		{
			foreach (var group in compatibilityModCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, rect.Pad(Margin	)
					, ref preferredHeight
					, applyDrawing
					, group.Key.GetIcon(true));
			}
		}

		var modsPreferredHeight = preferredHeight;

		if (applyDrawing)
		{
			using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale));
			e.Graphics.DrawLine(pen, rect.Right + (Padding.Left / 2), rect.Y, rect.Right + (Padding.Left / 2), e.ClipRectangle.Y + mainSectionHeight - Padding.Vertical);
		}

		rect.X += Padding.Left + rect.Width;

		//DrawDivider(e, rect.Pad(-(Padding.Left / 2),0,0,Margin.Bottom), applyDrawing, ref preferredHeight, true);

		preferredHeight = rect.Y;

		if (applyDrawing)
		{
			using var assetIcon = IconManager.GetIcon("I_Assets", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
			e.Graphics.DrawImage(assetIcon, rect.CenterR(assetIcon.Size));
		}

		if (compatibilityAssetCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Asset.Plural.ToLower())
				, Font
				, fore
				, rect.Pad(Margin)
				, ref preferredHeight
				, applyDrawing
				, "I_Check");
		}
		else
		{
			foreach (var group in compatibilityAssetCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Asset.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, rect.Pad(Margin)
					, ref preferredHeight
					, applyDrawing
					, group.Key.GetIcon(true));
			}
		}

		preferredHeight = Math.Max(preferredHeight, modsPreferredHeight) + Margin.Top;

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += Margin.Top;

		var buttonRect = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width / 3, 1);

		DrawButton(e, applyDrawing, ref preferredHeight, ViewModsWithIssues, new()
		{
			Text = Locale.ViewModsWithIssues,
			Icon = "I_Mods",
			Rectangle = buttonRect.Pad(0,0,Margin.Right,0)
		});

		var maxHeight = preferredHeight;

		buttonRect.X += buttonRect.Width;
		preferredHeight = buttonRect.Y;

		DrawButton(e, applyDrawing, ref preferredHeight, ViewCompatibilityReport, new()
		{
			Text = Locale.ViewCompatibilityReport,
			Icon = "I_ViewFile",
			Rectangle = buttonRect.Pad(Margin.Left, 0, Margin.Right, 0)
		});

		maxHeight = Math.Max(maxHeight, preferredHeight);

		buttonRect.X += buttonRect.Width;
		preferredHeight = buttonRect.Y;

		DrawButton(e, applyDrawing, ref preferredHeight, ViewAssetsWithIssues, new()
		{
			Text = Locale.ViewAssetsWithIssues,
			Icon = "I_Assets",
			Rectangle = buttonRect.Pad(Margin.Left, 0, 0, 0)
		});

		preferredHeight = Math.Max(maxHeight, preferredHeight) - Margin.Bottom;
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle.ClipTo(mainSectionHeight), Locale.CompatibilityReport, "I_CompatibilityReport", out var fore, ref preferredHeight);
		
		var rect = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, modsSectionHeight - preferredHeight);

		if (compatibilityModCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Mod.Plural.ToLower())
			, Font
			, fore
			, e.ClipRectangle.Pad(Margin)
			, ref preferredHeight
			, applyDrawing
			, "I_Check");
		}
		else
		{
			if (applyDrawing)
			{
				using var modIcon = IconManager.GetIcon("I_Mods", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
				e.Graphics.DrawImage(modIcon, rect.CenterR(modIcon.Size));
			}

			foreach (var group in compatibilityModCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, e.ClipRectangle.Pad(Margin)
					, ref preferredHeight
					, applyDrawing
					, group.Key.GetIcon(true));
			}
		}

		modsSectionHeight = preferredHeight;

		preferredHeight += Margin.Top;

		if (applyDrawing)
		{
			using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale));
			e.Graphics.DrawLine(pen, e.ClipRectangle.X + Padding.Left, preferredHeight, e.ClipRectangle.Right - (Padding.Left * 2), preferredHeight);
		}

		preferredHeight += (int)(4 * UI.FontScale) + Margin.Top;

		rect.Y = preferredHeight;
		rect.Height = e.ClipRectangle.ClipTo(mainSectionHeight).Bottom - preferredHeight;

		if (compatibilityAssetCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Asset.Plural.ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin)
				, ref preferredHeight
				, applyDrawing
				, "I_Check");
		}
		else
		{
			if (applyDrawing)
			{
				using var assetIcon = IconManager.GetIcon("I_Assets", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
				e.Graphics.DrawImage(assetIcon, rect.CenterR(assetIcon.Size));
			}

			foreach (var group in compatibilityAssetCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Asset.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, e.ClipRectangle.Pad(Margin)
					, ref preferredHeight
					, applyDrawing
					, group.Key.GetIcon(true));
			}
		}

		preferredHeight += Margin.Top;

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += Margin.Top;

		DrawButton(e, applyDrawing, ref preferredHeight, ViewCompatibilityReport, new()
		{
			Text = Locale.ViewCompatibilityReport,
			Icon = "I_ViewFile",
			Rectangle = e.ClipRectangle
		});

		preferredHeight -= Margin.Bottom;
	}
}
