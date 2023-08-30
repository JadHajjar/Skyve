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

	private readonly Dictionary<NotificationType, int> _compatibilityModCounts;
	private readonly Dictionary<NotificationType, int> _compatibilityAssetCounts;
	private readonly int mainSectionHeight;
	private int modsSectionHeight;

	public D_CompatibilityInfo()
	{
		_compatibilityModCounts = new();
		_compatibilityAssetCounts = new();
		ServiceCenter.Get(out _settings, out _notifier, out _packageUtil, out _contentManager, out _compatibilityManager);
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
		_notifier.PackageInformationUpdated += PackageInformationUpdated;
		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
		_notifier.CompatibilityReportProcessed += Notifier_CompatibilityReportProcessed;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.ContentLoaded -= Invalidate;
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
		_compatibilityModCounts.Clear();
		_compatibilityAssetCounts.Clear();

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

			if (mod.IsMod)
			{
				if (_compatibilityModCounts.ContainsKey(notif))
				{
					_compatibilityModCounts[notif]++;
				}
				else
				{
					_compatibilityModCounts[notif] = 1;
				}
			}
			else
			{
				if (_compatibilityAssetCounts.ContainsKey(notif))
				{
					_compatibilityAssetCounts[notif]++;
				}
				else
				{
					_compatibilityAssetCounts[notif] = 1;
				}
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
		if (!_compatibilityManager.FirstLoadComplete)
		{
			return DrawLoading;
		}

		if (_compatibilityModCounts.Count == 0 && _compatibilityAssetCounts.Count == 0)
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
		DrawSection(e, applyDrawing, new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, e.ClipRectangle.Bottom - preferredHeight), Locale.CompatibilityReport, "I_CompatibilityReport", out var fore, ref preferredHeight);

		e.Graphics.DrawStringItem(Locale.NoCompatibilityIssues
			, Font
			, fore
			, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
			, ref preferredHeight
			, applyDrawing);
	}

	private void DrawSplit(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle, Locale.CompatibilityReport, "I_CompatibilityReport", out var fore, ref preferredHeight);

		var rect = new Rectangle(e.ClipRectangle.X, preferredHeight, (e.ClipRectangle.Width / 2) - (Padding.Left / 2), e.ClipRectangle.Bottom - preferredHeight);

		if (applyDrawing)
		{
			using var modIcon = IconManager.GetIcon("I_Mods", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
			e.Graphics.DrawImage(modIcon, rect.CenterR(modIcon.Size));
		}

		if (_compatibilityModCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Mod.Plural.ToLower())
			, Font
			, fore
			, rect.Pad(Margin.Left, 0, Margin.Right, 0)
			, ref preferredHeight
			, applyDrawing);
		}
		else
		{
			foreach (var group in _compatibilityModCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, rect.Pad(Margin.Left, 0, Margin.Right, 0)
					, ref preferredHeight
					, applyDrawing);
			}
		}

		var modsPreferredHeight = preferredHeight;

		if (applyDrawing)
		{
			using var pen = new Pen(FormDesign.Design.AccentColor, (float)(1.5 * UI.FontScale));
			e.Graphics.DrawLine(pen, rect.Right + (Padding.Left / 2), rect.Y, rect.Right + (Padding.Left / 2), e.ClipRectangle.Bottom - Padding.Vertical);
		}

		rect.X += Padding.Left + rect.Width;

		preferredHeight = rect.Y;

		if (applyDrawing)
		{
			using var assetIcon = IconManager.GetIcon("I_Assets", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
			e.Graphics.DrawImage(assetIcon, rect.CenterR(assetIcon.Size));
		}

		if (_compatibilityAssetCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Asset.Plural.ToLower())
				, Font
				, fore
				, rect.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}
		else
		{
			foreach (var group in _compatibilityAssetCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Asset.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, rect.Pad(Margin.Left, 0, Margin.Right, 0)
					, ref preferredHeight
					, applyDrawing);
			}
		}

		preferredHeight = Math.Max(preferredHeight, modsPreferredHeight) + Margin.Top;
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, e.ClipRectangle, Locale.CompatibilityReport, "I_CompatibilityReport", out var fore, ref preferredHeight);
		
		var rect = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, modsSectionHeight - preferredHeight);

		if (applyDrawing)
		{
			using var modIcon = IconManager.GetIcon("I_Mods", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
			e.Graphics.DrawImage(modIcon, rect.CenterR(modIcon.Size));
		}

		if (_compatibilityModCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Mod.Plural.ToLower())
			, Font
			, fore
			, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
			, ref preferredHeight
			, applyDrawing);
		}
		else
		{
			foreach (var group in _compatibilityModCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
					, ref preferredHeight
					, applyDrawing);
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
		rect.Height = e.ClipRectangle.Bottom - preferredHeight;

		if (applyDrawing)
		{
			using var assetIcon = IconManager.GetIcon("I_Assets", Math.Min(rect.Width / 4, rect.Height - Padding.Left)).Color(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.MenuColor, 20));
			e.Graphics.DrawImage(assetIcon, rect.CenterR(assetIcon.Size));
		}

		if (_compatibilityAssetCounts.Count == 0)
		{
			e.Graphics.DrawStringItem(string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Asset.Plural.ToLower())
				, Font
				, fore
				, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
				, ref preferredHeight
				, applyDrawing);
		}
		else
		{
			foreach (var group in _compatibilityAssetCounts.OrderBy(x => x.Key))
			{
				e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Asset.FormatPlural(group.Value).ToLower())
					, Font
					, group.Key.GetColor()
					, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
					, ref preferredHeight
					, applyDrawing);
			}
		}

		preferredHeight += Margin.Top;
	}
}
