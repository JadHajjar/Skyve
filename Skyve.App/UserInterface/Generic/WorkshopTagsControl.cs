using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class WorkshopTagsControl : SlickControl
{
	private readonly Dictionary<ITag, Rectangle> _tagRects = [];
	public ITag[] Tags { get; set; } = [];
	public List<ITag> SelectedTags { get; } = [];

	public event EventHandler? SelectedTagChanged;

	protected override void UIChanged()
	{
		Width = UI.Scale(150);
		Padding = UI.Scale(new Padding(6));
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		foreach (var tag in _tagRects)
		{
			if (!tag.Value.Contains(e.X, e.Y))
			{
				continue;
			}

			if (SelectedTags.Contains(tag.Key))
			{
				SelectedTags.Remove(tag.Key);
			}
			else
			{
				SelectedTags.Add(tag.Key);
			}

			SelectedTagChanged?.Invoke(this, EventArgs.Empty);

			return;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(FormDesign.Design.AccentBackColor);

		using var pen = new Pen(FormDesign.Design.AccentColor, UI.Scale(1.5f));
		using var font = UI.Font(6.75F, FontStyle.Bold);
		using var brush = new SolidBrush(FormDesign.Design.LabelColor);
		using var format = new StringFormat { Alignment = StringAlignment.Center };

		e.Graphics.DrawString(LocaleSlickUI.Tags.Plural.ToUpper(), font, brush, ClientRectangle.Pad(Padding), format);
		e.Graphics.DrawLine(pen, pen.Width, -5, pen.Width, Height + 5);

		if (Loading)
		{
			DrawLoader(e.Graphics, new Rectangle(0, 0, Width, Width).CenterR(UI.Scale(new Size(32, 32))));
		}

		var cursor = PointToClient(Cursor.Position);
		var y = Padding.Top * 3;

		foreach (var tag in Tags)
		{
			var args = new ButtonDrawArgs
			{
				Font = Font,
				Icon = SelectedTags.Contains(tag) ? "Checked_ON" : "Checked_OFF",
				Text = tag.Value,
				ButtonType = ButtonType.Hidden,
				HoverState = HoverState & ~HoverState.Focused,
				LeftAlign = true,
				AvailableSize = new Size(Width - Padding.Horizontal, int.MaxValue),
				Cursor = cursor
			};

			SlickButton.AlignAndDraw(e.Graphics, ClientRectangle.Pad(Padding.Left, y, 0, 0), ContentAlignment.TopLeft, args);

			y += args.Rectangle.Height + Padding.Top;

			_tagRects[tag] = args.Rectangle;
		}

		Cursor = _tagRects.Values.Any(x => x.Contains(cursor)) ? Cursors.Hand : Cursors.Default;
	}
}
