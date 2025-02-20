using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class WorkshopPaginationControl : SlickControl
{
	private Dictionary<Rectangle, int> _pages = [];

	public event EventHandler<int>? PageSelected;

	public int Page { get; set; }
	public int TotalCount { get; set; }

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(6));
		Height = UI.Scale(32) + Padding.Vertical;
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (TotalCount == 0)
		{
			return;
		}

		foreach (var item in _pages)
		{
			if (item.Key.Contains(e.X, e.Y))
			{
				Page = item.Value - 1;
				Enabled = false;
				PageSelected?.Invoke(this, item.Value - 1);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		if (TotalCount == 0)
		{
			_pages = [];
			return;
		}

		const int range = 3;

		var maxPages = (int)Math.Ceiling(TotalCount / 30f);
		var startPage = Page + 1 - range;
		var endPage = Page + 1 + range;
		var buttonSize = UI.Scale(new Size(26, 26));
		var x = (Width - ((buttonSize.Width + Padding.Left) * 13)) / 2;
		var cursor = PointToClient(Cursor.Position);
		var pages = new Dictionary<Rectangle, int>();

		using var font = UI.Font(9.75F, FontStyle.Bold);

		if (Page > range)
		{
			drawButton(1);

			drawButton(null, "More");
		}
		else
		{
			x += 2 * (buttonSize.Width + Padding.Left);
		}

		drawButton(Page, "ArrowLeft");

		for (var i = startPage; i <= endPage; i++)
		{
			drawButton(i > maxPages ? -1 : i);
		}

		drawButton(Page + 2, "ArrowRight");

		if (Page + range < maxPages)
		{
			drawButton(null, "More");

			drawButton(maxPages);
		}

		_pages = pages;

		if (!Enabled)
		{
			using var brush = new SolidBrush(Color.FromArgb(50, BackColor));

			e.Graphics.FillRectangle(brush, ClientRectangle);
		}

		Cursor = pages.Any(x => x.Key.Contains(cursor)) ? Cursors.Hand : Cursors.Default;

		void drawButton(int? page, string? icon = null)
		{
			if (page is null || (page > 0 && page <= maxPages))
			{
				var args = new ButtonDrawArgs
				{
					Font = font,
					Rectangle = new Rectangle(x, Padding.Top * 3 / 2, buttonSize.Width, buttonSize.Height),
					Text = icon is null ? page?.ToString() : null,
					Image = icon is null ? null : IconManager.GetIcon(icon, buttonSize.Width * 2 / 3),
					Padding = icon is not null ? UI.Scale(new Padding(3)) : UI.Scale(new Padding(0, 3, 5, 3)),
					HoverState = page is null ? default : HoverState & ~HoverState.Focused,
					ButtonType = page - 1 == Page ? ButtonType.Active : page is null ? ButtonType.Hidden : icon is null ? ButtonType.Dimmed : ButtonType.Normal,
					Cursor = cursor
				};

				SlickButton.Draw(e.Graphics, args);

				if (page is not null)
				{
					pages[args.Rectangle] = page.Value;
				}
			}

			x += buttonSize.Width + Padding.Left;
		}
	}

	internal void SetTotalCount(int totalCount)
	{
		this.TryInvoke(() =>
		{
			TotalCount = totalCount;
			Enabled = true;
			Invalidate();
		});
	}
}
