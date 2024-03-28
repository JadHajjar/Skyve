using Skyve.App.UserInterface.Panels;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;

internal class D_CompatibilityInfo : IDashboardItem
{
	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IPackageManager _contentManager;
	private readonly ICompatibilityManager _compatibilityManager;

	private Dictionary<NotificationType, int> compatibilityModCounts = [];
	private Dictionary<NotificationType, int> compatibilityAssetCounts = [];
	private int mainSectionHeight;
	private int modsSectionHeight;

	public D_CompatibilityInfo()
	{
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

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (_notifier.IsContentLoaded)
		{
			LoadData();
		}
		else
		{
			Loading = true;
		}

		_notifier.ContentLoaded += LoadData;
		_notifier.WorkshopInfoUpdated += LoadData;
		_notifier.PackageInformationUpdated += LoadData;
		_notifier.PackageInclusionUpdated += LoadData;
		_notifier.PlaysetChanged += LoadData;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.ContentLoaded -= LoadData;
		_notifier.WorkshopInfoUpdated -= LoadData;
		_notifier.PackageInformationUpdated -= LoadData;
		_notifier.PackageInclusionUpdated -= LoadData;
		_notifier.PlaysetChanged -= LoadData;
	}

	protected override Task<bool> ProcessDataLoad(CancellationToken token)
	{
		if (!_notifier.IsContentLoaded)
		{
			return Task.FromResult(false);
		}

		var compatibilityModCounts = new Dictionary<NotificationType, int>();
		var compatibilityAssetCounts = new Dictionary<NotificationType, int>();

		foreach (var mod in _contentManager.Packages)
		{
			if (token.IsCancellationRequested)
			{
				return Task.FromResult(false);
			}

			if (!mod.IsIncluded())
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

		if (token.IsCancellationRequested)
		{
			return Task.FromResult(false);
		}

		this.compatibilityModCounts = compatibilityModCounts;
		this.compatibilityAssetCounts = compatibilityAssetCounts;

		OnResizeRequested();

		return Task.FromResult(true);
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
		DrawLoadingSection(e, applyDrawing, ref preferredHeight, Locale.CompatibilityReport);
	}

	protected override void DrawHeader(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, ref preferredHeight, Locale.CompatibilityReport, "CompatibilityReport");
	}

	private void DrawNoIssues(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, ref preferredHeight, Locale.CompatibilityReport, "CompatibilityReport");

		DrawValue(e, e.ClipRectangle.Pad(Margin), string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Package.Plural.ToLower()), string.Empty, applyDrawing, ref preferredHeight, "Check");

		preferredHeight += BorderRadius;
	}

	private void DrawSplit(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, ref preferredHeight, Locale.CompatibilityReport, "CompatibilityReport");

		var rect = new Rectangle(e.ClipRectangle.X, preferredHeight, (e.ClipRectangle.Width / 2) - (Padding.Left / 2), mainSectionHeight - preferredHeight + e.ClipRectangle.Y);

		if (compatibilityModCounts.Count == 0)
		{
			DrawValue(e, rect.Pad(Margin), string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Mod.Plural.ToLower()), string.Empty, applyDrawing, ref preferredHeight, "Check");
		}
		else
		{
			foreach (var group in compatibilityModCounts.OrderBy(x => x.Key))
			{
				DrawValue(e, rect.Pad(Margin), string.Format(LocaleCR.Get($"{group.Key}Count").Plural, string.Empty, Locale.Mod.Plural).Trim(), group.Value.ToString(), applyDrawing, ref preferredHeight, group.Key.GetIcon(true), group.Key.GetColor());
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

		if (compatibilityAssetCounts.Count == 0)
		{
			DrawValue(e, rect.Pad(Margin), string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Asset.Plural.ToLower()), string.Empty, applyDrawing, ref preferredHeight, "Check");
		}
		else
		{
			foreach (var group in compatibilityAssetCounts.OrderBy(x => x.Key))
			{
				DrawValue(e, rect.Pad(Margin), string.Format(LocaleCR.Get($"{group.Key}Count").Plural, string.Empty, Locale.Asset.Plural).Trim(), group.Value.ToString(), applyDrawing, ref preferredHeight, group.Key.GetIcon(true), group.Key.GetColor());
			}
		}

		preferredHeight = Math.Max(preferredHeight, modsPreferredHeight) + Margin.Top;

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += BorderRadius;

		var buttonRect = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width / 3, 1);

		DrawButton(e, applyDrawing, ref preferredHeight, ViewModsWithIssues, new()
		{
			Text = Locale.ViewModsWithIssues,
			Icon = "Mods",
			ButtonType = ButtonType.Dimmed,
			Rectangle = buttonRect.Pad(0, 0, Margin.Right, 0).Pad(Margin)
		});

		var maxHeight = preferredHeight;

		buttonRect.X += buttonRect.Width;
		preferredHeight = buttonRect.Y;

		DrawButton(e, applyDrawing, ref preferredHeight, ViewCompatibilityReport, new()
		{
			Text = Locale.ViewCompatibilityReport,
			Icon = "ViewFile",
			ButtonType = ButtonType.Dimmed,
			Rectangle = buttonRect.Pad(Margin.Left, 0, Margin.Right, 0).Pad(Margin)
		});

		maxHeight = Math.Max(maxHeight, preferredHeight);

		buttonRect.X += buttonRect.Width;
		preferredHeight = buttonRect.Y;

		DrawButton(e, applyDrawing, ref preferredHeight, ViewAssetsWithIssues, new()
		{
			Text = Locale.ViewAssetsWithIssues,
			Icon = "Assets",
			ButtonType = ButtonType.Dimmed,
			Rectangle = buttonRect.Pad(Margin.Left, 0, 0, 0).Pad(Margin)
		});

		preferredHeight = Math.Max(maxHeight, preferredHeight) + BorderRadius / 2;
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawSection(e, applyDrawing, ref preferredHeight, Locale.CompatibilityReport, "CompatibilityReport");

		var rect = new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, modsSectionHeight - preferredHeight);

		if (compatibilityModCounts.Count == 0)
		{
			DrawValue(e, rect.Pad(Margin), string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Mod.Plural.ToLower()), string.Empty, applyDrawing, ref preferredHeight, "Check");
		}
		else
		{
			foreach (var group in compatibilityModCounts.OrderBy(x => x.Key))
			{
				DrawValue(e, rect.Pad(Margin), string.Format(LocaleCR.Get($"{group.Key}Count").Plural, string.Empty, Locale.Mod.Plural).Trim(), group.Value.ToString(), applyDrawing, ref preferredHeight, group.Key.GetIcon(true), group.Key.GetColor());
			}
		}

		modsSectionHeight = preferredHeight;

		preferredHeight += BorderRadius;

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
			DrawValue(e, rect.Pad(Margin), string.Format(Locale.NoCompatibilityIssues.Plural, Locale.Asset.Plural.ToLower()), string.Empty, applyDrawing, ref preferredHeight, "Check");
		}
		else
		{
			foreach (var group in compatibilityAssetCounts.OrderBy(x => x.Key))
			{
				DrawValue(e, rect.Pad(Margin), string.Format(LocaleCR.Get($"{group.Key}Count").Plural, string.Empty, Locale.Asset.Plural).Trim(), group.Value.ToString(), applyDrawing, ref preferredHeight, group.Key.GetIcon(true), group.Key.GetColor());
			}
		}

		preferredHeight += BorderRadius;

		mainSectionHeight = preferredHeight - e.ClipRectangle.Y;

		preferredHeight += BorderRadius;

		DrawButton(e, applyDrawing, ref preferredHeight, ViewCompatibilityReport, new()
		{
			Text = Locale.ViewCompatibilityReport,
			Icon = "ViewFile",
			ButtonType = ButtonType.Dimmed,
			Rectangle = e.ClipRectangle.Pad(Margin)
		});

		preferredHeight += BorderRadius / 2;
	}
}
