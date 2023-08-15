using SkyveApp.Systems.CS1.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.StatusBubbles;
internal class ProfileBubble : StatusBubbleBase
{
	private readonly IPlaysetManager _profileManager;
	private readonly INotifier _notifier;

	public ProfileBubble()
	{
		ServiceCenter.Get(out _notifier, out _profileManager);
	}

	public override Color? TintColor { get => _profileManager.CurrentPlayset.Color; set { } }

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (!Live)
		{
			return;
		}

		Text = Locale.PlaysetBubble;
		ImageName = _profileManager.CurrentPlayset.GetIcon();

		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.PlaysetChanged -= ProfileManager_ProfileChanged;
	}

	private void ProfileManager_ProfileChanged()
	{
		ImageName = _profileManager.CurrentPlayset.GetIcon();
	}

	protected override void CustomDraw(PaintEventArgs e, ref int targetHeight)
	{
		DrawText(e, ref targetHeight, _profileManager.CurrentPlayset.Name ?? "");

		if (_profileManager.CurrentPlayset.Temporary)
		{
			DrawText(e, ref targetHeight, Locale.CreatePlaysetHere, FormDesign.Design.YellowColor);
		}
		else
		{
			DrawText(e, ref targetHeight, _profileManager.CurrentPlayset.AutoSave ? Locale.AutoPlaysetSaveOn : Locale.AutoPlaysetSaveOff, _profileManager.CurrentPlayset.AutoSave ? FormDesign.Design.GreenColor : FormDesign.Design.YellowColor);
		}

		if (ServiceCenter.Get<INotifier>().PlaysetsLoaded)
		{
			DrawText(e, ref targetHeight, Locale.LoadedCount.FormatPlural(_profileManager.Playsets.Count() - 1, Locale.Playset.FormatPlural(_profileManager.Playsets.Count() - 1).ToLower()));
		}
	}
}
