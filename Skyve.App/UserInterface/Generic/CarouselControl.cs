using Skyve.App.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;

public partial class CarouselControl : SlickControl
{
	private int index = 0;
	private List<IThumbnailObject> thumbnails = [];

	public CarouselControl()
	{
		InitializeComponent();
	}

	public void SetThumbnails(IEnumerable<IThumbnailObject> thumbnails)
	{
		this.thumbnails = thumbnails.ToList();

		index = index.Between(0, this.thumbnails.Count);

		var currentThumb = thumbnails.TryGet(index);

		MainThumb.Invalidate();

		FLP_Thumbs.SuspendDrawing();
		FLP_Thumbs.Controls.Clear(true);
		FLP_Thumbs.Controls.AddRange(thumbnails.Reverse().ToArray(x =>
		{
			var thumb = new MiniThumb(x) { Selected = currentThumb == x, Dock = DockStyle.Left, Size = new Size(P_Thumbs.Height * 16 / 9, P_Thumbs.Height) };

			thumb.MouseClick += Thumb_MouseClick;

			return thumb;
		}));
		FLP_Thumbs.ResumeDrawing();
	}

	private void Thumb_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			foreach (MiniThumb item in FLP_Thumbs.Controls)
			{
				if (item.Selected = sender == item)
				{
					index = thumbnails.IndexOf(item.ThumbnailObject);
				}

				item.Invalidate();
			}

			MainThumb.Invalidate();
		}
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(3, 10, 3, 10), UI.FontScale);
		roundedPanel.Padding = UI.Scale(new Padding(5), UI.FontScale);
		slickScroll.Padding = UI.Scale(new Padding(0, 5, 0, 0), UI.FontScale);

		var rectangle = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (Width - Padding.Horizontal) * 9 / 16);
		var bottomSize = (int)(64 * UI.FontScale);

		MainThumb.Size = new Size(Width - Padding.Horizontal, Math.Min((Width - Padding.Horizontal) * 9 / 16, Height - Padding.Vertical - bottomSize));
		P_Thumbs.Height = bottomSize;

		foreach (MiniThumb item in FLP_Thumbs.Controls)
		{
			item.Size = new Size(P_Thumbs.Height * 16 / 9, P_Thumbs.Height);
			item.cachedImage?.Dispose();
			item.cachedImage = null;
		}
	}

	protected override void DesignChanged(FormDesign design)
	{
		roundedPanel.BackColor = design.BackColor.Tint(Lum: design.IsDarkTheme ? 6 : -6);
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		base.OnSizeChanged(e);

		var rectangle = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (Width - Padding.Horizontal) * 9 / 16);

		MainThumb.Size = new Size(Width - Padding.Horizontal, Math.Min((Width - Padding.Horizontal) * 9 / 16, Height - Padding.Vertical - P_Thumbs.Height));
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData is Keys.Left or Keys.Right)
		{
			index += keyData is Keys.Left ? -1 : 1;

			if (index < 0)
			{
				index = thumbnails.Count - 1;
			}
			else if (index >= thumbnails.Count)
			{
				index = 0;
			}

			var thumbnail = thumbnails.TryGet(index);

			foreach (MiniThumb item in FLP_Thumbs.Controls)
			{
				item.Selected = item.ThumbnailObject == thumbnail;
				item.Invalidate();
			}

			MainThumb.Invalidate();

			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
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

	private void MainThumb_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		if (thumbnails.Count == 0)
		{
			return;
		}

		index = index.Between(0, thumbnails.Count - 1);

		var thumbnail = thumbnails[index];
		var image = thumbnail.GetThumbnail();
		var rectangle = MainThumb.ClientRectangle;
		var imageRect = rectangle;

		rectangle.Height -= Padding.Bottom;

		if (MainThumb.Loading = image is null)
		{
			MainThumb.DrawLoader(e.Graphics, rectangle);
		}
		else
		{
			e.Graphics.DrawRoundedImage(image, imageRect = GetRectangle(rectangle, image!.Size), (int)(10 * UI.FontScale));
		}

		if (thumbnails.Count <= 1 || !MainThumb.HoverState.HasFlag(HoverState.Hovered))
		{
			return;
		}

		var cursor = MainThumb.PointToClient(Cursor.Position);

		if (!imageRect.Contains(cursor) || cursor.X.IsWithin(MainThumb.Width / 3, MainThumb.Width * 2 / 3))
		{
			return;
		}

		var gap = (int)(64 * UI.FontScale);

		var rect1 = new Rectangle(0, imageRect.Y, gap * 2, imageRect.Height).CenterR(gap, gap);
		var rect2 = new Rectangle(MainThumb.Width - (gap * 2), imageRect.Y, gap * 2, imageRect.Height).CenterR(gap, gap);
		using var brush1 = new SolidBrush(Color.FromArgb(rect1.Contains(cursor) ? 255 : 125, BackColor.Tint(Lum: BackColor.IsDark() ? 6 : -6)));
		using var brush2 = new SolidBrush(Color.FromArgb(rect2.Contains(cursor) ? 255 : 125, BackColor.Tint(Lum: BackColor.IsDark() ? 6 : -6)));

		e.Graphics.FillEllipse(brush1, rect1);
		e.Graphics.FillEllipse(brush2, rect2);

		var icon1 = IconManager.GetIcon("ArrowLeft", gap * 3 / 4).Color(rect1.Contains(cursor) ? FormDesign.Design.ActiveColor : ForeColor);
		var icon2 = IconManager.GetIcon("ArrowRight", gap * 3 / 4).Color(rect2.Contains(cursor) ? FormDesign.Design.ActiveColor : ForeColor);

		e.Graphics.DrawImage(icon1, rect1.Pad(0, 0, gap / 10, 0).CenterR(icon1.Size));
		e.Graphics.DrawImage(icon2, rect2.Pad(gap / 10, 0, 0, 0).CenterR(icon2.Size));

		MainThumb.Cursor = rect1.Contains(cursor) || rect2.Contains(cursor) ? Cursors.Hand : Cursors.Default;
	}

	private void MainThumb_MouseClick(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Middle)
		{
			if (thumbnails.Count == 0)
			{
				return;
			}

			index = index.Between(0, thumbnails.Count - 1);

			thumbnails[index].GetThumbnail(ServiceCenter.Get<IImageService>(), out _, out var url);

			PlatformUtil.OpenUrl(url);
		}
		else if (e.Button == MouseButtons.Left)
		{
			var gap = (int)(64 * UI.FontScale);

			var rect1 = new Rectangle(0, 0, gap * 2, MainThumb.Height).CenterR(gap, gap);
			var rect2 = new Rectangle(MainThumb.Width - (gap * 2), 0, gap * 2, MainThumb.Height).CenterR(gap, gap);

			if (rect1.Contains(e.Location))
			{
				index--;
			}
			else if (rect2.Contains(e.Location))
			{
				index++;
			}
			else
			{
				return;
			}

			if (index < 0)
			{
				index = thumbnails.Count - 1;
			}
			else if (index >= thumbnails.Count)
			{
				index = 0;
			}

			var thumbnail = thumbnails.TryGet(index);

			foreach (MiniThumb item in FLP_Thumbs.Controls)
			{
				item.Selected = item.ThumbnailObject == thumbnail;
				item.Invalidate();
			}

			MainThumb.Invalidate();
		}
	}

	private class MiniThumb : SlickControl
	{
		public Bitmap? cachedImage;
		private bool _selected;

		public MiniThumb(IThumbnailObject thumbnailObject)
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
				cachedImage = image = new Bitmap(image, GetRectangle(ClientRectangle.Pad((int)(5 * UI.FontScale)), image.Size).Size);
			}

			if (Selected || HoverState.HasFlag(HoverState.Hovered))
			{
				using var brush = new SolidBrush(Color.FromArgb(Selected ? 220 : 100, FormDesign.Design.ActiveColor));

				e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad(1), (int)(5 * UI.FontScale));
			}

			if (image != null)
			{
				e.Graphics.DrawRoundedImage(image, ClientRectangle.CenterR(image.Size), (int)(5 * UI.FontScale));
			}
			else
			{
				using var icon = IconManager.GetIcon("Gallery", Height / 2).Color(FormDesign.Design.IconColor);
				e.Graphics.DrawImage(icon, ClientRectangle.CenterR(icon.Size));
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
	}
}