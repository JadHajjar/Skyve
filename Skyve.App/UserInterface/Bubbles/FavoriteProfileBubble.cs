using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.StatusBubbles;
internal class FavoriteProfileBubble : StatusBubbleBase
{
	private readonly INotifier _notifier;
	private readonly IPlaysetManager _profileManager;

	public ICustomPlayset Profile { get; }

	public FavoriteProfileBubble(ICustomPlayset profile)
	{
		ServiceCenter.Get(out _notifier, out _profileManager);
		Profile = profile;
	}

	public override Color? TintColor
	{
		get => Profile.Color;
		set { }
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (!Live)
		{
			return;
		}

		Text = Profile.Name;
		ImageName = Profile.GetIcon();

		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Margin = UI.Scale(new Padding(10, 0, 5, 10), UI.FontScale);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notifier.PlaysetChanged -= ProfileManager_ProfileChanged;
		}

		base.Dispose(disposing);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			Loading = true;
			_profileManager.SetCurrentPlayset(Profile);
		}
	}

	private void ProfileManager_ProfileChanged()
	{
		Loading = false;
		Text = Profile.Name;
		ImageName = Profile.GetIcon();
		Invalidate();
	}

	protected override void CustomDraw(PaintEventArgs e, ref int targetHeight)
	{
		if (_profileManager.CurrentPlayset == Profile)
		{
			e.Graphics.DrawRoundedRectangle(new Pen(FormDesign.Design.ActiveColor, (float)(1.5 * UI.FontScale)), ClientRectangle.Pad(1 + (int)Math.Floor(1.5 * UI.FontScale)), Padding.Left);
		}
	}
}
