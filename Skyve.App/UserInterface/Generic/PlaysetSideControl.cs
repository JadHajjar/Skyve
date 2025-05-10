using Skyve.App.Interfaces;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class PlaysetSideControl : SlickControl
{
	private readonly IPlaysetManager _playsetManager;
	private Rectangle DotsRect;
	private Rectangle ActivateButton;
	private Rectangle Favorite;
	private Rectangle TextRect;

	private readonly TextBox _textBox;
	private Padding InnerPadding;

	public IPlayset Playset { get; set; }

	public PlaysetSideControl(IPlayset playset)
	{
		Playset = playset;

		ServiceCenter.Get(out _playsetManager);

		_textBox = new TextBox
		{
			Visible = false,
			BorderStyle = BorderStyle.None,
		};

		_textBox.Leave += (s, e) => _textBox.Hide();

		Controls.Add(_textBox);
	}

	protected override void UIChanged()
	{
		InnerPadding = UI.Scale(new Padding(4));
		Padding = UI.Scale(new Padding(10));
		_textBox.Font = UI.Font(12.5F);
	}

	protected override async void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Right)
		{
			SlickToolStrip.Show(Program.MainForm, ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(Playset, true));
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		if (DotsRect.Contains(e.Location))
		{
			SlickToolStrip.Show(Program.MainForm, ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(Playset, true));
			return;
		}

		if (ActivateButton.Contains(e.Location))
		{
			Loading = true;
			await _playsetManager.ActivatePlayset(Playset);
			Loading = false;
			return;
		}

		if (Favorite.Contains(e.Location))
		{
			var customPlayset = Playset.GetCustomPlayset();
			customPlayset.IsFavorite = !customPlayset.IsFavorite;
			_playsetManager.Save(customPlayset);
			return;
		}

		if (TextRect.Contains(e.Location))
		{
			EditName();
			return;
		}
	}

	public void EditName()
	{
		var customPlayset = Playset.GetCustomPlayset();
		var banner = customPlayset.GetThumbnail();
		var backColor = customPlayset.Color ?? banner?.GetThemedAverageColor() ?? FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 5 : -4, Sat: 5);
		_textBox.BackColor = backColor.Tint(Lum: backColor.IsDark() ? 20 : -20, Sat: -15);
		_textBox.ForeColor = _textBox.BackColor.GetTextColor();
		_textBox.Bounds = TextRect.Pad(Padding.Left / 2);
		_textBox.Bounds = TextRect.CenterR(_textBox.Size);
		_textBox.Text = Playset.Name;
		_textBox.Visible = true;
		_textBox.Focus();
		_textBox.SelectAll();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			_textBox.Visible = false;
			Invalidate();
			return true;
		}

		if (keyData == Keys.Enter)
		{
			_textBox.Visible = false;
			_playsetManager.RenamePlayset(Playset, _textBox.Text);
			Invalidate();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		Cursor = TextRect.Contains(e.Location) ? Cursors.IBeam : DotsRect.Contains(e.Location) || ActivateButton.Contains(e.Location) || Favorite.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		Height = Width * 300 / 190;

		var rectangle = ClientRectangle.Pad(InnerPadding.Left + UI.Scale(8));
		var size = UI.Scale(new Size(32, 32));

		ActivateButton = new Rectangle(rectangle.X, rectangle.Bottom - UI.Scale(36) - InnerPadding.Vertical, rectangle.Width, UI.Scale(36) + InnerPadding.Vertical).Pad(InnerPadding);
		var Thumbnail = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Width - InnerPadding.Top).InvertPad(InnerPadding);

		rectangle = rectangle.Pad(UI.Scale(2));

		Favorite = Thumbnail.Pad(Padding).Align(size, ContentAlignment.TopLeft);

		TextRect = new Rectangle(Thumbnail.X + (Padding.Left / 2), Thumbnail.Bottom + (Padding.Top / 2), Thumbnail.Width, size.Height);
		var Content = new Rectangle(rectangle.X, TextRect.Bottom + Padding.Vertical, rectangle.Width, ActivateButton.Y - TextRect.Bottom - Padding.Vertical);

		var EditThumbnail = Thumbnail.Align(size, ContentAlignment.TopRight);
		var EditSettings = Thumbnail.Align(size, ContentAlignment.BottomRight);

		DotsRect = TextRect.Align(UI.Scale(new Size(32, 32)), ContentAlignment.TopRight);
		TextRect.Width -= DotsRect.Width;

		var isActive = Playset.Equals(_playsetManager.CurrentPlayset);
		var borderRadius = Padding.Left;
		var CursorLocation = PointToClient(Cursor.Position);
		var customPlayset = Playset.GetCustomPlayset();
		var banner = customPlayset.GetThumbnail();
		var backColor = customPlayset.Color ?? banner?.GetThemedAverageColor() ?? FormDesign.Design.AccentBackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 7 : -6, Sat: 3);
		var onBannerColor = (banner?.GetThemedAverageColor() ?? FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 7 : -6, Sat: 3)).GetTextColor();
		var backRect = e.ClipRectangle.Pad(UI.Scale(8));
		using var onBannerBrush = new SolidBrush(Color.FromArgb(banner is null ? 125 : 50, onBannerColor));

		using (var brush = Gradient(Color.FromArgb(FormDesign.Design.IsDarkTheme ? 100 : 70, backColor), backRect, 3.5f))
		{
			e.Graphics.FillRoundedRectangleWithShadow(backRect, borderRadius, UI.Scale(8), BackColor, Color.FromArgb(10, isActive ? FormDesign.Design.GreenColor : backColor));

			e.Graphics.FillRoundedRectangle(brush, backRect, borderRadius);
		}

		if (banner is null)
		{
			using var brush = new SolidBrush(Color.FromArgb(40, onBannerColor));

			e.Graphics.FillRoundedRectangle(brush, Thumbnail, borderRadius, botLeft: false, botRight: false);

			using var icon = customPlayset.Usage.GetIcon().Get(Thumbnail.Width * 3 / 4).Color(onBannerColor);

			e.Graphics.DrawImage(icon, Thumbnail.CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(banner, Thumbnail, borderRadius, botLeft: false, botRight: false);
		}

		if (isActive)
		{
			ActivateButton = ActivateButton.InvertPad(InnerPadding).Pad(UI.Scale(2), Padding.Vertical, UI.Scale(2), UI.Scale(2));

			using var greenBrush = new SolidBrush(FormDesign.Design.GreenColor);
			e.Graphics.FillRoundedRectangle(greenBrush, ActivateButton.Pad(-InnerPadding.Left - UI.Scale(2)), borderRadius, topLeft: false, topRight: false);

			var text = Locale.ActivePlayset.One.ToUpper();
			using var font = UI.Font(10.5F, FontStyle.Bold).FitTo(text, ActivateButton, e.Graphics);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
			using var textBrush2 = new SolidBrush(FormDesign.Design.GreenColor.GetTextColor());

			e.Graphics.DrawString(text, font, textBrush2, ActivateButton, format);
		}

		DrawFavoriteButton(e, customPlayset, onBannerColor, onBannerBrush, Favorite, Thumbnail, CursorLocation);

		using var textBrush = new SolidBrush(backColor.MergeColor(BackColor, (FormDesign.Design.IsDarkTheme ? 100 : 70) * 100 / 255).GetTextColor());
		using var fadedBrush = new SolidBrush(Color.FromArgb(175, textBrush.Color));
		using var textFont = UI.Font(12.5F, FontStyle.Bold).FitTo(Playset.Name, TextRect, e.Graphics);
		using var smallTextFont = UI.Font(8.25F);

		if (!_textBox.Visible)
		{
			e.Graphics.DrawHighResText(Playset.Name, textFont, textBrush, TextRect);

			e.Graphics.DrawString(customPlayset.Usage > 0 ? Locale.UsagePlayset.Format(LocaleHelper.GetGlobalText(customPlayset.Usage.ToString())) : Locale.GenericPlayset, smallTextFont, fadedBrush, new Point(TextRect.X, TextRect.Y + (int)e.Graphics.Measure(Playset.Name, textFont, TextRect.Width).Height));

			TextRect = TextRect.Pad(0, 0, Padding.Left / -2, Padding.Bottom * 3 / -2);

			if (HoverState.HasFlag(HoverState.Hovered) && TextRect.Contains(CursorLocation))
			{
				using var brush = new SolidBrush(Color.FromArgb(35, onBannerColor));

				e.Graphics.FillRoundedRectangle(brush, TextRect, borderRadius / 2);
			}
		}
		else
		{
			TextRect = TextRect.Pad(0, 0, Padding.Left / -2, Padding.Bottom * 3 / -2);

			using var brush = new SolidBrush(_textBox.BackColor);

			e.Graphics.FillRoundedRectangle(brush, TextRect, borderRadius / 2);
		}

		var contentText = $"{Locale.ContainCount.FormatPlural(Playset.ModCount, Locale.Mod.FormatPlural(Playset.ModCount).ToLower())} • {Playset.ModSize.SizeString(0)}";
		using (var contentBrush = Gradient(Color.FromArgb(FormDesign.Design.IsDarkTheme ? 100 : 70, backColor.Tint(Lum: backColor.IsDark() ? 6 : -6)), backRect, 3.5f))
		using (var format = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center })
		{
			e.Graphics.FillRectangle(contentBrush, Content.CenterR(backRect.Width, smallTextFont.Height * 5 / 3));
			e.Graphics.DrawString(contentText, smallTextFont, textBrush, Content, format);
		}

		using var pen = new Pen(isActive ? FormDesign.Design.GreenColor : customPlayset.IsFavorite ? FormDesign.Modern.ActiveColor : backColor, UI.Scale(2.5f)) { Alignment = PenAlignment.Center };
		e.Graphics.DrawRoundedRectangle(pen, backRect.Pad(-1), borderRadius);

		var isHovered = DotsRect.Contains(CursorLocation);
		using var img = IconManager.GetIcon("VertialMore", DotsRect.Height * 3 / 4).Color(isHovered ? FormDesign.Design.ActiveColor : onBannerColor);

		e.Graphics.DrawImage(img, DotsRect.CenterR(img.Size));

#if CS1
		labelRects.Y += e.Graphics.DrawLabel(Locale.ContainCount.FormatPlural(e.Item.AssetCount, Locale.Asset.FormatPlural(e.Item.AssetCount).ToLower()), IconManager.GetSmallIcon("Assets"), FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor, 75), labelRects, ContentAlignment.TopLeft).Height + GridPadding.Top;
#endif

		if (customPlayset.OnlineInfo?.Author?.Name is not null and not "")
		{
			var name = customPlayset.OnlineInfo.Author.Name;

			using var userIcon = IconManager.GetSmallIcon("User");

			var Author = e.Graphics.DrawLabel(name, userIcon, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor, 25), default, ContentAlignment.TopLeft);

			throw new NotImplementedException();
		}

#if CS1
		else if (e.Item.IsMissingItems)
		{
			using var icon = IconManager.GetSmallIcon("MinorIssues");
			e.Graphics.DrawLabel(Locale.IncludesItemsYouDoNotHave, icon, FormDesign.Design.RedColor.MergeColor(FormDesign.Design.BackColor, 50), Content, ContentAlignment.TopRight);
		}
#endif

		if (!isActive)
		{
			SlickButton.Draw(e.Graphics, new ButtonDrawArgs
			{
				Text = Locale.ActivatePlayset.ToString().ToUpper(),
				Icon = "Check",
				Font = smallTextFont,
				Rectangle = ActivateButton.Pad(0, Padding.Top, 0, 0),
				Padding = UI.Scale(new Padding(8, 4, 8, 4)),
				HoverState = HoverState & ~HoverState.Focused,
				Cursor = CursorLocation,
				Control = this,
				BackgroundColor = backColor.MergeColor(onBannerColor, 85).MergeColor(BackColor, 45).Tint(null, -6, 6),
				ActiveColor = FormDesign.Design.GreenColor
			});
		}
		else
		{
			ActivateButton = default;
		}
	}

	private void DrawFavoriteButton(PaintEventArgs e, ICustomPlayset customPlayset, Color onBannerColor, SolidBrush onBannerBrush, Rectangle Favorite, Rectangle Thumbnail, Point CursorLocation)
	{
		if (customPlayset.IsFavorite)
		{
			using var favBrush = new SolidBrush(FormDesign.Modern.ActiveColor);
			e.Graphics.FillRoundedRectangle(favBrush, Favorite, Padding.Left / 2);
		}
		else if (HoverState.HasFlag(HoverState.Hovered) && Favorite.Contains(CursorLocation))
		{
			e.Graphics.FillRoundedRectangle(onBannerBrush, Favorite, Padding.Left / 2);
		}

		var fav = new DynamicIcon(customPlayset.IsFavorite != Favorite.Contains(CursorLocation) ? "StarFilled" : "Star");
		using var favIcon = fav.Get(Favorite.Height * 3 / 4);

		if (customPlayset.IsFavorite || (HoverState.HasFlag(HoverState.Hovered) && Thumbnail.Contains(CursorLocation)))
		{
			e.Graphics.DrawImage(favIcon.Color(customPlayset.IsFavorite ? FormDesign.Modern.ActiveForeColor : Favorite.Contains(CursorLocation) ? FormDesign.Modern.ActiveColor : onBannerColor), Favorite.CenterR(favIcon.Size));
		}
	}
}
