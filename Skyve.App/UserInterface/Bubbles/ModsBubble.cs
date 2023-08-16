using System.Windows.Forms;

namespace Skyve.App.UserInterface.Bubbles;

internal class ModsBubble : StatusBubbleBase
{
	private readonly ISettings _settings;
	private readonly INotifier _notifier;
	private readonly IPackageUtil _packageUtil;
	private readonly IPackageManager _contentManager;

	public ModsBubble()
	{
		ServiceCenter.Get(out _settings, out _notifier, out _packageUtil, out _contentManager);
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

		_notifier.WorkshopInfoUpdated += CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated += Invalidate;
		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.ContentLoaded -= Invalidate;
		_notifier.WorkshopInfoUpdated -= CentralManager_WorkshopInfoUpdated;
		_notifier.PackageInformationUpdated -= Invalidate;
		_notifier.PlaysetChanged -= ProfileManager_ProfileChanged;
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

		int modsIncluded = 0, modsEnabled = 0, modsOutOfDate = 0, modsIncomplete = 0;

		var crDic = new Dictionary<NotificationType, int>();

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

			var notif = mod.GetCompatibilityInfo().GetNotification();

			if (crDic.ContainsKey(notif))
			{
				crDic[notif]++;
			}
			else
			{
				crDic[notif] = 1;
			}
		}

		if (!_settings.UserSettings.AdvancedIncludeEnable)
		{
			DrawText(e, ref targetHeight, Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower()));
		}
		else if (modsIncluded == modsEnabled)
		{
			DrawText(e, ref targetHeight, Locale.IncludedEnabledCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower()));
		}
		else
		{
			DrawText(e, ref targetHeight, Locale.IncludedCount.FormatPlural(modsIncluded, Locale.Mod.FormatPlural(modsIncluded).ToLower()));
			DrawText(e, ref targetHeight, Locale.EnabledCount.FormatPlural(modsEnabled, Locale.Mod.FormatPlural(modsEnabled).ToLower()));
		}

		if (modsOutOfDate > 0)
		{
			DrawText(e, ref targetHeight, Locale.OutOfDateCount.FormatPlural(modsOutOfDate, Locale.Mod.FormatPlural(modsOutOfDate).ToLower()), FormDesign.Design.YellowColor);
		}

		if (modsIncomplete > 0)
		{
			DrawText(e, ref targetHeight, Locale.IncompleteCount.FormatPlural(modsIncomplete, Locale.Mod.FormatPlural(modsIncomplete).ToLower()), FormDesign.Design.RedColor);
		}

		foreach (var group in crDic.OrderBy(x => x.Key))
		{
			if (group.Key <= NotificationType.Info)
			{
				continue;
			}

			DrawText(e, ref targetHeight, LocaleCR.Get($"{group.Key}Count").FormatPlural(group.Value, Locale.Mod.FormatPlural(group.Value).ToLower()), group.Key.GetColor());
		}
	}
}
