using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class WorkshopTagsControl : SlickControl
{
	private readonly Dictionary<IWorkshopTag, Rectangle> _tagRects = [];
	private static readonly Dictionary<IWorkshopTag, bool> _tagOpened = [];
	public static List<IWorkshopTag> Tags { get; } = [];
	public static List<string> SelectedTags { get; } = [];

	public event EventHandler? SelectedTagChanged;

	public void ClearTags()
	{
		SelectedTags.Clear();
		SelectedTagChanged?.Invoke(this, EventArgs.Empty);
		Invalidate();
	}

	public void SetTags(IEnumerable<IWorkshopTag> tags)
	{
		Tags.AddRange(tags);

		foreach (var tag in Tags)
		{
			_tagOpened[tag] = true;
		}
	}

	protected override void UIChanged()
	{
		Font = UI.Font(7.5f);
		Padding = UI.Scale(new Padding(3));
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

			var isOpenable = tag.Key.Children.Any(x => x.UsageCount is null or not 0 || !x.IsSelectable);
			var leftHover = (!isOpenable || e.X < tag.Value.X + (tag.Value.Width / 2)) && tag.Key.IsSelectable;

			if (leftHover)
			{
				if (SelectedTags.Contains(tag.Key.Key))
				{
					SelectedTags.Remove(tag.Key.Key);
				}
				else
				{
					SelectedTags.Add(tag.Key.Key);
				}

				SelectedTagChanged?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				_tagOpened[tag.Key] = !_tagOpened.ContainsKey(tag.Key) || !_tagOpened[tag.Key];
			}

			return;
		}
	}

	protected override void OnMouseDoubleClick(MouseEventArgs e)
	{
		base.OnMouseDoubleClick(e);

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

			var isOpenable = tag.Key.Children.Any(x => x.UsageCount is null or not 0 || !x.IsSelectable);
			var leftHover = (!isOpenable || e.X < tag.Value.X + (tag.Value.Width / 2)) && tag.Key.IsSelectable;

			if (leftHover && isOpenable)
			{
				foreach (var item in tag.Key.Children.Where(x => x.UsageCount is null or not 0 || !x.IsSelectable))
				{
					if (SelectedTags.Contains(tag.Key.Key) == SelectedTags.Contains(item.Key))
					{
						continue;
					}

					if (SelectedTags.Contains(item.Key))
					{
						SelectedTags.Remove(item.Key);
					}
					else
					{
						SelectedTags.Add(item.Key);
					}
				}

				SelectedTagChanged?.Invoke(this, EventArgs.Empty);
			}

			return;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		Cursor = _tagRects.Values.Any(x => x.Contains(e.Location)) ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		using var font = UI.Font(6.75F, FontStyle.Bold);
		using var brush = new SolidBrush(FormDesign.Design.LabelColor);
		using var format = new StringFormat { Alignment = StringAlignment.Center };

		if (Loading)
		{
			DrawLoader(e.Graphics, new Rectangle(0, 0, Width, Width).CenterR(UI.Scale(new Size(32, 32))));
		}

		var cursor = PointToClient(Cursor.Position);
		var y = Padding.Top / 2;

		_tagRects.Clear();
		y = DrawTags(e, cursor, y, 0, Tags);

		Height = y;
	}

	private int DrawTags(PaintEventArgs e, Point cursor, int y, int x, IEnumerable<IWorkshopTag> tags)
	{
		foreach (var tag in tags.Where(x => x.UsageCount is null or not 0 || !x.IsSelectable))
		{
			var tagRect = Rectangle.FromLTRB(Padding.Left + x, y, Width - Padding.Right, y + UI.Scale(22));
			var hovered = HoverState.HasFlag(HoverState.Hovered) && tagRect.Contains(cursor);
			var selected = SelectedTags.Contains(tag.Key);
			var isOpenable = tag.Children.Any(x => x.UsageCount is null or not 0 || !x.IsSelectable);
			var leftHover = tag.IsSelectable && (!isOpenable || cursor.X < tagRect.X + (tagRect.Width / 2));
			var childHovered = false;

			_tagOpened.TryGetValue(tag, out var open);

			if (selected || (!open &&IsRecursiveSelected(tag)))
			{
				e.Graphics.FillRoundedRectangleWithShadow(tagRect, UI.Scale(4), UI.Scale(6), (hovered ? FormDesign.Design.ButtonColor : BackColor).MergeColor(FormDesign.Design.ActiveColor, selected ? 90 : 100), Color.FromArgb(selected ? 8 : 4, FormDesign.Design.ActiveColor));
			}

			if (hovered)
			{
				if (!selected)
				{
					using var pen = new Pen(Color.FromArgb(75, ForeColor), 1.5f);
					e.Graphics.DrawRoundedRectangle(pen, tagRect, UI.Scale(4));
				}

				using var back = new LinearGradientBrush(tagRect, Color.FromArgb(100, FormDesign.Design.ActiveColor), Color.FromArgb(200, FormDesign.Design.ButtonColor), leftHover ? 0f : 180f);

				e.Graphics.FillRoundedRectangle(back, tagRect, UI.Scale(4));
			}

			if (tag.IsSelectable)
			{
				using var img = IconManager.GetIcon(selected ? "Checked_ON" : "Checked_OFF", UI.Scale(16)).Color(selected ? FormDesign.Design.ActiveColor : (hovered && leftHover) ? FormDesign.Design.ActiveColor.MergeColor(ForeColor, 75) : FormDesign.Design.IconColor);

				e.Graphics.DrawImage(img, tagRect.ClipWidthTo(UI.Scale(18)).CenterR(img.Size));
			}

			if (tag.UsageCount.HasValue && tag.IsSelectable)
			{
				var rect = tagRect.Align(UI.Scale(new Size(30, 14)), ContentAlignment.MiddleRight);
				using var tinyFont = UI.Font(6F, FontStyle.Bold).FitTo(tag.UsageCount.ToString(), rect, e.Graphics);
				using var tinyFormat = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center };
				using var tinyBrush = new SolidBrush(Color.FromArgb(200, ForeColor));

				if (!hovered && !selected)
				{
					using var back = new SolidBrush(Color.FromArgb(150, FormDesign.Design.ButtonColor));
					e.Graphics.FillRoundedRectangle(back, rect, UI.Scale(2));
				}

				e.Graphics.DrawString(tag.UsageCount.ToString(), tinyFont, tinyBrush, rect, tinyFormat);
			}

			if (tag.IsSelectable || isOpenable)
			{
				_tagRects[tag] = tagRect;
			}

			var textRect = tagRect.Pad(tag.IsSelectable ? UI.Scale(18) : Padding.Left, 0, tag.IsSelectable ? UI.Scale(32) : Padding.Right, 0);

			y += tagRect.Height + Padding.Top;

			if (isOpenable)
			{
				using var chevron = IconManager.GetIcon(open ? "DropChevronUp" : "DropChevron", UI.Scale(10)).Color(hovered && !leftHover ? FormDesign.Design.ActiveColor.MergeColor(ForeColor, 75) : FormDesign.Design.IconColor);

				e.Graphics.DrawImage(chevron, textRect.Pad(Padding).Align(chevron.Size, ContentAlignment.MiddleRight));

				textRect.Width -= chevron.Width + Padding.Vertical;

				if (open)
				{
					var startY = y;
					y = DrawTags(e, cursor, y, x + UI.Scale(15), tag.Children);

					childHovered = HoverState.HasFlag(HoverState.Hovered) && cursor.Y > startY && cursor.Y < y;
					using var pen = new Pen(FormDesign.Design.AccentColor.MergeColor(FormDesign.Design.ActiveColor, childHovered ? 25 : 100), UI.Scale(2.5f)) { EndCap = LineCap.Round, StartCap = LineCap.Round };
					e.Graphics.DrawLine(pen, x + UI.Scale(11), startY - UI.Scale(1), x + UI.Scale(11), y - UI.Scale(3));

					y += Padding.Bottom;
				}
			}

			using var font = UI.Font(7.5f).FitToWidth(tag.Value, textRect, e.Graphics);
			using var brush = new SolidBrush(childHovered ? ForeColor.MergeColor(FormDesign.Design.ActiveColor, 25) : ForeColor);
			using var format = new StringFormat { LineAlignment = StringAlignment.Center };

			e.Graphics.DrawString(tag.Value, font, brush, textRect, format);
		}

		return y;
	}

	private bool IsRecursiveSelected(IWorkshopTag tag)
	{
		if (SelectedTags.Contains(tag.Key))
		{
			return true;
		}

		return tag.Children?.Any(IsRecursiveSelected) ?? false;
	}
}
