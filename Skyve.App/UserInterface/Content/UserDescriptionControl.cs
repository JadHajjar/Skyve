using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class UserDescriptionControl : SlickImageControl
{
	private Rectangles? rects;
	public IUser? User { get; private set; }
	public PC_UserPage? UserPage { get; private set; }

	private readonly IPackageManager _contentManager;
	private readonly ICompatibilityManager _compatibilityManager;

	public UserDescriptionControl()
	{
		_contentManager = ServiceCenter.Get<IPackageManager>();
		_compatibilityManager = ServiceCenter.Get<ICompatibilityManager>();
	}

	public void SetUser(IUser user, PC_UserPage? page)
	{
		UserPage = page;
		User = user;

		Invalidate();
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(4), UI.FontScale);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (rects?.SteamRect.Contains(e.Location) == true)
		{
			SlickTip.SetTo(this, Locale.OpenAuthorPage, offset: rects.SteamRect.Location);

			Cursor = Cursors.Hand;
		}
		else
		{
			if (rects?.TextRect.Pad(0, 0, (int)(-24 * UI.FontScale), 0).Contains(e.Location) == true && User is not null && _compatibilityManager.IsUserVerified(User))
			{
				SlickTip.SetTo(this, "VerifiedAuthor");
			}
			else
			{
				SlickTip.SetTo(this, null);
			}

			Cursor = Cursors.Default;
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left && rects?.SteamRect.Contains(e.Location) == true)
		{
			PlatformUtil.OpenUrl(User!.ProfileUrl);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (UserPage is not null)
		{
			e.Graphics.Clear(FormDesign.Design.BackColor);
			e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.AccentBackColor), ClientRectangle.Pad(0, 0, 0, Height / 2));
			e.Graphics.SetUp();
		}
		else
		{
			e.Graphics.SetUp(FormDesign.Design.AccentBackColor);
			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.BackColor), ClientRectangle.Pad(1, Height / 2 + 1, 1, 1), (int)(5 * UI.FontScale));
		}

		if (User == null)
		{
			return;
		}

		rects = new Rectangles();

		DrawTitle(e);
		DrawButtons(e);

		var count = _contentManager.Packages.Count(x => User.Equals(x.GetWorkshopInfo()?.Author));

		if (count == 0)
		{
			return;
		}

		var text = Locale.YouHavePackagesUser.FormatPlural(count, User.Name);
		using var font = UI.Font(9F);
		var textSize = e.Graphics.Measure(text, font);

		var rect = ClientRectangle.Pad(0, Height / 2, 0, 0).Pad(Padding).Align(Size.Ceiling(textSize), ContentAlignment.TopLeft);

		using var brush = new SolidBrush(FormDesign.Design.InfoColor);
		e.Graphics.DrawString(text, font, brush, rect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}

	private void DrawButtons(PaintEventArgs e)
	{
		rects!.SteamRect = ClientRectangle.Pad(0, 0, 0, Height / 2).Pad(Padding).Align(UI.Scale(new Size(28, 28), UI.FontScale), ContentAlignment.BottomRight);

		using var icon = IconManager.GetIcon("I_Steam", rects.SteamRect.Height * 3 / 4);

		SlickButton.DrawButton(e, rects.SteamRect, string.Empty, Font, icon, null, rects.SteamRect.Contains(CursorLocation) ? HoverState & ~HoverState.Focused : HoverState.Normal);
	}

	private void DrawTitle(PaintEventArgs e)
	{
		var text = User!.Name;
		using var font = UI.Font(11.25F, FontStyle.Bold);
		var textSize = e.Graphics.Measure(text, font);

		rects!.TextRect = ClientRectangle.Pad(0, 0, 0, Height / 2).Pad(Padding).Align(Size.Ceiling(textSize), ContentAlignment.BottomLeft);

		using var brush = new SolidBrush(FormDesign.Design.ForeColor);
		e.Graphics.DrawString(text, font, brush, rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

		if (_compatibilityManager.IsUserVerified(User))
		{
			var checkRect = rects.TextRect.Align(UI.Scale(new Size(16, 16), UI.FontScale), ContentAlignment.MiddleLeft);
			checkRect.X += rects.TextRect.Width;

			e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect);

			checkRect = checkRect.Pad((int)(3 * UI.FontScale));

			using var img = IconManager.GetIcon("I_Check", checkRect.Height);
			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}
	}

	private class Rectangles
	{
		public Rectangle TextRect;
		public Rectangle SteamRect;

		public bool Contain(Point location)
		{
			return
				SteamRect.Contains(location);
		}
	}
}
