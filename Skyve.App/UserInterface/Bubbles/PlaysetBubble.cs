using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Bubbles;
internal class PlaysetBubble : StatusBubbleBase
{
	private readonly IPlaysetManager _playsetManager;
	private readonly INotifier _notifier;

	public PlaysetBubble()
	{
		ServiceCenter.Get(out _notifier, out _playsetManager);
	}

	public override Color? TintColor { get => _playsetManager.CurrentPlayset.Color; set { } }

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (!Live)
		{
			return;
		}

		Text = Locale.PlaysetBubble;
		ImageName = _playsetManager.CurrentPlayset.GetIcon();

		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		_notifier.PlaysetChanged -= ProfileManager_ProfileChanged;
	}

	private void ProfileManager_ProfileChanged()
	{
		ImageName = _playsetManager.CurrentPlayset.GetIcon();
	}

	protected override void CustomDraw(PaintEventArgs e, ref int targetHeight)
	{
		DrawText(e, ref targetHeight, _playsetManager.CurrentPlayset.Name ?? "");

		if (_playsetManager.CurrentPlayset.Temporary)
		{
			DrawText(e, ref targetHeight, Locale.CreatePlaysetHere, FormDesign.Design.YellowColor);
		}
		else
		{
			DrawText(e, ref targetHeight, _playsetManager.CurrentPlayset.AutoSave ? Locale.AutoPlaysetSaveOn : Locale.AutoPlaysetSaveOff, _playsetManager.CurrentPlayset.AutoSave ? FormDesign.Design.GreenColor : FormDesign.Design.YellowColor);
		}

		if (ServiceCenter.Get<INotifier>().PlaysetsLoaded)
		{
			DrawText(e, ref targetHeight, Locale.LoadedCount.FormatPlural(_playsetManager.Playsets.Count() - 1, Locale.Playset.FormatPlural(_playsetManager.Playsets.Count() - 1).ToLower()));
		}
	}
}
