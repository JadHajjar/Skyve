using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class TagListControl : SlickControl
{
	private readonly ITagsService _tagsService;
	private readonly List<(Rectangle rectangle, string tag)> _tagRects = [];

	public List<ITag> Tags { get; } = [];
	public List<ITag> AllTags { get; } = [];
	public string? CurrentSearch { get; set; }

	public TagListControl()
	{
		ServiceCenter.Get(out _tagsService);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(7, 7, 7, 32));
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var cursorLocation = PointToClient(Cursor.Position);
		var tagsRect = new Rectangle(Padding.Left, Padding.Top, 0, 0);
		var hovered = false;
		var autoTags = true;
		var padding = UI.Scale(new Padding(3, 2, 3, 2));
		using var font = UI.Font(7.75F);

		_tagRects.Clear();

		using var fadeBrush = new SolidBrush(Color.FromArgb(150, BackColor));

		foreach (var item in AllTags.OrderBy(x => !x.IsCustom))
		{
			if (!autoTags == !item.IsCustom)
			{
				using var brush = new SolidBrush(FormDesign.Design.ActiveColor);
				using var titleFont = UI.Font(7.75F, FontStyle.Bold);

				if (tagsRect.Y != Padding.Top || tagsRect.X != Padding.Left)
				{
					tagsRect.X = Padding.Left;
					tagsRect.Y += tagsRect.Height + (Padding.Top * 5 / 2);
				}

				var text = (autoTags ? Locale.CustomTags : Locale.WorkshopAndGameTags).One.ToUpper();
				e.Graphics.DrawString(text, titleFont, brush, tagsRect.Location);

				var textSize = e.Graphics.Measure(text, titleFont);

				using var activePen = new Pen(FormDesign.Design.ActiveColor, (int)(UI.FontScale * 2)) { DashStyle = DashStyle.Dot, DashCap = DashCap.Round };
				e.Graphics.DrawLine(activePen, tagsRect.X + textSize.Width, tagsRect.Y + ((int)textSize.Height / 2), Width - Padding.Right, tagsRect.Y + ((int)textSize.Height / 2));

				tagsRect.Y += (int)textSize.Height + Padding.Top;

				autoTags = !autoTags;
			}

			using var buttonArgs = new ButtonDrawArgs
			{
				Cursor = cursorLocation,
				HoverState = HoverState & ~HoverState.Focused,
				Font = font,
				Icon = item.Icon,
				Text = item.ToString(),
				Padding = padding,
				Enabled = string.IsNullOrEmpty(CurrentSearch) || CurrentSearch.SearchCheck(item.ToString()),
				ButtonType = Tags.Any(t => t.Value == item.Value) ? ButtonType.Active : ButtonType.Normal
			};

			SlickButton.PrepareLayout(e.Graphics, buttonArgs);

			if (tagsRect.X + buttonArgs.Rectangle.Width + Padding.Right > Width)
			{
				tagsRect.X = Padding.Left;
				tagsRect.Y += buttonArgs.Rectangle.Height + Padding.Top;
			}

			buttonArgs.Rectangle = new Rectangle(tagsRect.Location, buttonArgs.Rectangle.Size);

			SlickButton.SetUpColors(buttonArgs);

			SlickButton.DrawButton(e.Graphics, buttonArgs);

			_tagRects.Add((buttonArgs.Rectangle, item.Value));

			hovered |= buttonArgs.Rectangle.Contains(cursorLocation);
			tagsRect.Height = buttonArgs.Rectangle.Height;
			tagsRect.X += buttonArgs.Rectangle.Width + Padding.Right;
		}

		if (Height > Parent.Height)
		{
			var rect = new Rectangle(0, Parent.Height - Top - Padding.Bottom, Width, Padding.Bottom);
			using var brush = new LinearGradientBrush(rect, Color.Empty, BackColor, 90);

			e.Graphics.FillRectangle(brush, rect);
		}

		//using var pen = new Pen(FormDesign.Design.AccentColor, (float)(UI.FontScale * 1.5));
		//e.Graphics.DrawLine(pen, Padding.Left, -Top, Width - Padding.Horizontal, -Top);

		Height = tagsRect.Bottom + Padding.Bottom;

		Cursor = hovered ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		base.OnLocationChanged(e);

		Invalidate();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		foreach (var item in _tagRects)
		{
			if (item.rectangle.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Right || (e.Button == MouseButtons.Left && Tags.Any(t => t.Value == item.tag)))
				{
					Tags.RemoveAll(t => t.Value == item.tag);
				}
				else if (e.Button == MouseButtons.Left)
				{
					Tags.Add(_tagsService.CreateCustomTag(item.tag));
				}
			}
		}
	}
}
