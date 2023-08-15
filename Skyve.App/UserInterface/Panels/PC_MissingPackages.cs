﻿using SkyveApp.Systems.CS1.Utilities;

using System.Threading;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Panels;
internal partial class PC_MissingPackages : PC_GenericPackageList
{
	private readonly IModUtil _modUtil;
	private readonly IAssetUtil _assetUtil;
	private readonly INotifier _notifier;

	private bool allowExit;

	public PC_MissingPackages(IEnumerable<IPlaysetEntry> playsetEntries) : base(playsetEntries, false)
	{
		ServiceCenter.Get(out _notifier, out _modUtil, out _assetUtil);
	}

	protected override void Dispose(bool disposing)
	{
		_notifier.ContentLoaded -= CentralManager_ContentLoaded;

		base.Dispose(disposing);
	}

	protected override void LocaleChanged()
	{
		Text = string.Format(Locale.MissingPackagesPlayset, ServiceCenter.Get<IPlaysetManager>().CurrentPlayset.Name);
	}

	private void CentralManager_ContentLoaded()
	{
		var items = LC_Items.Items.ToList();

		foreach (var item in items)
		{
			if (item is IPlaysetModEntry mod)
			{
				var localMod = ServiceCenter.Get<IPlaysetManager>().GetMod(mod);

				if (localMod is null)
				{
					continue;
				}

				_modUtil.SetIncluded(localMod, true);
				_modUtil.SetEnabled(localMod, mod.IsEnabled);

				LC_Items.Remove(item);
			}
			else if (item is IPlaysetEntry asset)
			{
				var localAsset = ServiceCenter.Get<IPlaysetManager>().GetAsset(asset);

				if (localAsset is null)
				{
					continue;
				}

				_assetUtil.SetIncluded(localAsset, true);

				LC_Items.Remove(item);
			}
		}

		if (LC_Items.ItemCount == 0)
		{
			this.TryInvoke(PushBack);
		}
	}

	public override bool CanExit(bool toBeDisposed)
	{
		if (toBeDisposed && !allowExit && LC_Items.ItemCount > 0)
		{
			if (ShowPrompt(Locale.MissingItemsRemain, PromptButtons.OKCancel, PromptIcons.Hand) == DialogResult.OK)
			{
				allowExit = true;
				Form.PushBack();
			}

			return false;
		}

		return true;
	}

	internal static void PromptMissingPackages(BasePanelForm form, IEnumerable<IPlaysetEntry> playsetEntries)
	{
		var pauseEvent = new AutoResetEvent(false);

		form.TryInvoke(() =>
		{
			var panel = new PC_MissingPackages(playsetEntries);

			form.PushPanel(null, panel);

			panel.Disposed += (s, e) =>
			{
				pauseEvent.Set();
			};
		});

		pauseEvent.WaitOne();
	}
}
