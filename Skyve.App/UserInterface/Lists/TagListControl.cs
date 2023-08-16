using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class TagListControl : SlickControl
{
	private readonly ITagsService _tagsService;
	private readonly List<(Rectangle rectangle, string tag)> _tagRects = new();

	public List<ITag> Tags { get; } = new();
	public List<ITag> AllTags { get; } = new();
	public string? CurrentSearch { get; set; }

    public TagListControl()
    {
		ServiceCenter.Get(out _tagsService);
    }

    protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(7, 7, 7, 32), UI.FontScale);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var cursorLocation = PointToClient(Cursor.Position);
		var tagsRect = new Rectangle(Padding.Left, Padding.Top, 0, 0);
		var hovered = false;
		var autoTags = true;

		_tagRects.Clear();

		using var fadeBrush = new SolidBrush(Color.FromArgb(150, BackColor));

		foreach (var item in AllTags)
		{
			if (!autoTags == !item.IsCustom && !Tags.Any(t => t.Value == item.Value))
			{
				using var brush = new SolidBrush(FormDesign.Design.ActiveColor);
				using var font = UI.Font(9F, FontStyle.Bold);

				if (tagsRect.Y != Padding.Top || tagsRect.X != Padding.Left)
				{
					tagsRect.X = Padding.Left;
					tagsRect.Y += tagsRect.Height + Padding.Top * 3 / 2;
				}

				e.Graphics.DrawString(autoTags ? Locale.CustomTags : Locale.WorkshopAndGameTags, font, brush, tagsRect.Location);

				var textSize = e.Graphics.Measure(autoTags ? Locale.CustomTags : Locale.WorkshopAndGameTags, font);

				using var activePen = new Pen(FormDesign.Design.ActiveColor, (int)(UI.FontScale * 2)) { DashStyle = DashStyle.Dot, DashCap = DashCap.Round };
				e.Graphics.DrawLine(activePen, tagsRect.X + textSize.Width, tagsRect.Y + (int)textSize.Height / 2, Width - Padding.Right, tagsRect.Y + (int)textSize.Height / 2);

				tagsRect.Y += (int)textSize.Height + Padding.Top / 2;

				autoTags = !autoTags;
			}

			using var tagIcon = IconManager.GetSmallIcon(item.Icon);
			var size = e.Graphics.MeasureLabel(item.Value, tagIcon, large: true);

			if (tagsRect.X + size.Width + Padding.Right > Width)
			{
				tagsRect.X = Padding.Left;
				tagsRect.Y += size.Height + Padding.Top;
			}

			var rect = e.Graphics.DrawLabel(item.Value, tagIcon, Tags.Any(t => t.Value == item.Value) ? FormDesign.Design.ActiveColor : Color.FromArgb(200, FormDesign.Design.LabelColor.MergeColor(FormDesign.Design.AccentBackColor, 40)), tagsRect, ContentAlignment.TopLeft, large: true, mousePosition: cursorLocation);

			if (!string.IsNullOrEmpty(CurrentSearch) && !CurrentSearch.SearchCheck(item.Value))
			{
				e.Graphics.FillRectangle(fadeBrush, rect);
			}

			_tagRects.Add((rect, item.Value));

			hovered |= rect.Contains(cursorLocation);

			tagsRect.Height = rect.Height;

			tagsRect.X += size.Width + Padding.Right;
		}

		if (Height > Parent.Height)
		{
			var rect = new Rectangle(0, Parent.Height - Top - Padding.Bottom, Width, Padding.Bottom);
			using var brush = new LinearGradientBrush(rect, Color.Empty, BackColor, 90);

			e.Graphics.FillRectangle(brush, rect);
		}

		using var pen = new Pen(FormDesign.Design.AccentColor, (float)(UI.FontScale * 1.5));
		e.Graphics.DrawLine(pen, Padding.Left, -Top, Width - Padding.Horizontal, -Top);

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
				if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Left && Tags.Any(t => t.Value == item.tag))
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
