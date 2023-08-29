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
	private int mainSectionHeight;

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

		return Draw;
	}

	private void DrawLoading(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		DrawLoadingSection(e, applyDrawing, Locale.CompatibilityReport, "I_CompatibilityReport", ref preferredHeight);
	}

	private void Draw(PaintEventArgs e, bool applyDrawing, ref int preferredHeight)
	{
		//if (_compatibilityCounts.Count == 0)
		//{
		//	return;
		//}

		//preferredHeight += Margin.Top;

		//DrawSection(e, applyDrawing, new Rectangle(e.ClipRectangle.X, preferredHeight, e.ClipRectangle.Width, e.ClipRectangle.Bottom - preferredHeight), Locale.CompatibilityReport, "I_CompatibilityReport", out _, ref preferredHeight);

		//foreach (var group in _compatibilityCounts.OrderBy(x => x.Key))
		//{
		//	if (group.Key <= NotificationType.Info)
		//	{
		//		continue;
		//	}

		//	e.Graphics.DrawStringItem(LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower())
		//		, Font
		//		, group.Key.GetColor()
		//		, e.ClipRectangle.Pad(Margin.Left, 0, Margin.Right, 0)
		//		, ref preferredHeight
		//		, applyDrawing);
		//}

		//preferredHeight += Margin.Top;
	}
}
