﻿using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;

public class MiniThumbControl : SlickControl
{
	public Bitmap? cachedImage;
	private bool _selected;

	public MiniThumbControl(IThumbnailObject thumbnailObject)
	{
		Cursor = Cursors.Hand;
		AutoInvalidate = false;
		ThumbnailObject = thumbnailObject;
		Margin = default;
	}

	public IThumbnailObject ThumbnailObject { get; }
	public bool Selected
	{
		get => _selected; set
		{
			_selected = value;

			if (value && Parent?.Parent is not null)
			{
				var screenPosition = new Rectangle(PointToScreen(default), Size);
				var visibleBounds = new Rectangle(Parent.Parent.PointToScreen(default), Parent.Parent.Size);

				if (!visibleBounds.Contains(screenPosition))
				{
					SlickScroll.GlobalScrollTo(this, 5);
				}
			}
		}
	}

	[DefaultValue(null)]
	public string? Label { get; set; }

	protected override void OnHoverStateChanged()
	{
		base.OnHoverStateChanged();

		Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var image = cachedImage ?? ThumbnailObject.GetThumbnail();

		if (cachedImage is null && image is not null)
		{
			cachedImage = image = new Bitmap(image, GetRectangle(ClientRectangle.Pad(UI.Scale(5)), image.Size).Size);
		}

		if (Selected || HoverState.HasFlag(HoverState.Hovered))
		{
			using var brush = new SolidBrush(Color.FromArgb(Selected ? 220 : 100, FormDesign.Design.ActiveColor));

			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1), UI.Scale(5));
		}

		if (image != null)
		{
			e.Graphics.DrawRoundedImage(image, ClientRectangle.CenterR(image.Size), UI.Scale(5));
		}
		else
		{
			using var icon = IconManager.GetIcon("Gallery", Height / 2).Color(FormDesign.Design.IconColor);
			e.Graphics.DrawImage(icon, ClientRectangle.CenterR(icon.Size));
		}

		if (Label is not null and not "")
		{
			var padding = UI.Scale(new Padding(3));
			using var smallFont = UI.Font(6.5F);
			using var backBrush = new SolidBrush(Color.FromArgb(HoverState.HasFlag(HoverState.Hovered) ? 100 : 255, FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.BackColor)));
			using var foreBrush = new SolidBrush(Color.FromArgb(HoverState.HasFlag(HoverState.Hovered) ? 100 : 255, backBrush.Color.GetTextColor()));
			using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			var size = Size.Round(e.Graphics.Measure(Label, smallFont, Width - (padding.Horizontal * 2))) + padding.Size;
			var rectangle = ClientRectangle.Pad(padding).Align(size, ContentAlignment.BottomCenter);

			e.Graphics.FillRoundedRectangle(backBrush, rectangle, padding.Left);
			e.Graphics.DrawString(Label, smallFont, foreBrush, rectangle, format);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			cachedImage?.Dispose();
		}

		base.Dispose(disposing);

	}

	private static Rectangle GetRectangle(Rectangle rectangle, Size imageSize)
	{
		var widthRatio = (double)rectangle.Width / imageSize.Width;
		var heightRatio = (double)rectangle.Height / imageSize.Height;

		var maxRatio = Math.Max(widthRatio, heightRatio);
		var minRatio = Math.Min(widthRatio, heightRatio);

		if (minRatio < 1 || imageSize.Width <= imageSize.Height)
		{
			maxRatio = minRatio;
		}

		var newWidth = (int)(imageSize.Width * maxRatio);
		var newHeight = (int)(imageSize.Height * maxRatio);

		return rectangle.CenterR(newWidth, newHeight);
	}
}