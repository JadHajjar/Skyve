using Skyve.App.UserInterface.Lists;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class NotificationFilterControl : SlickControl
{
	private readonly Dictionary<NotificationType, Rectangle> _groupRects = [];
	public CompatibilityReportList? ListControl { get; set; }
	public Func<ICompatibilityInfo, bool>? Filter { get; set; }
	public NotificationType CurrentGroup { get; set; }

	public event EventHandler? SelectedGroupChanged;

	protected override void UIChanged()
	{
		Width = UI.Scale(165);
		Padding = UI.Scale(new Padding(6));
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		foreach (var item in _groupRects)
		{
			if (!item.Value.Contains(e.X, e.Y))
			{
				continue;
			}

			CurrentGroup = item.Key;

			if (ListControl != null)
			{
				ListControl.AllStatuses = CurrentGroup is NotificationType.None;
			}

			SelectedGroupChanged?.Invoke(this, EventArgs.Empty);

			return;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(FormDesign.Design.AccentBackColor);

		using var pen = new Pen(FormDesign.Design.AccentColor, UI.Scale(1.5f));
		using var font = UI.Font(9F);
		using var smallFont = UI.Font(6.75F);
		using var headerFont = UI.Font(6.75F, FontStyle.Bold);
		using var brush = new SolidBrush(FormDesign.Design.LabelColor);
		using var format = new StringFormat { Alignment = StringAlignment.Center };

		e.Graphics.DrawString(Locale.CompatibilityStatus.Plural.ToUpper(), headerFont, brush, ClientRectangle.Pad(Padding), format);
		e.Graphics.DrawLine(pen, pen.Width, -5, pen.Width, Height + 5);

		if (ListControl is null || Filter is null)
		{
			return;
		}

		var items = ListControl.Items.Where(Filter).ToList();

		if (items.Count == 0)
		{
			return;
		}

		var compatibilityItems = items.GroupBy(x => x.GetNotification().If(x => x <= NotificationType.Info, NotificationType.Snoozed)).OrderByDescending(x => x.Key).ToList();
		compatibilityItems.Insert(0, items.Where(x => x.GetNotification() > NotificationType.Info).GroupBy(x => NotificationType.None).FirstOrDefault());

		if (Loading)
		{
			DrawLoader(e.Graphics, new Rectangle(0, 0, Width, Width).CenterR(UI.Scale(new Size(32, 32))));
		}

		var cursor = PointToClient(Cursor.Position);
		var y = Padding.Top * 4;

		_groupRects.Clear();

		foreach (var item in compatibilityItems)
		{
			if (item is null)
				continue;

			var rectangle = new Rectangle(Padding.Left, y, Width - Padding.Horizontal, UI.Scale(50)).Pad(Padding);

			var text = (item.Key == NotificationType.None ? LocaleSlickUI.All : LocaleCR.Get(item.Key.ToString())).ToString();
			var subText = Locale.ItemsCount.FormatPlural(item.Count());
			using var icon = (item.Key == NotificationType.None ? "CompatibilityReport" : item.Key.GetIcon(true)).Default;

			var baseColor = item.Key == NotificationType.None ? FormDesign.Design.ActiveColor : item.Key.GetColor();
			using var foreBrush = new SolidBrush(CurrentGroup == item.Key ? baseColor.GetTextColor() : baseColor.MergeColor(ForeColor, 80));
			var textSize = e.Graphics.Measure(text, font, rectangle.Width - icon.Width - Padding.Left);
			var textBounds = rectangle.Pad(Padding.Left).Pad(icon.Width + (Padding.Left / 2), 0, 0, 0).Align(Size.Ceiling(textSize), ContentAlignment.TopLeft);
			var iconBounds = rectangle.Pad(Padding.Left).ClipTo(textBounds.Height + UI.Scale(2)).Align(icon.Size, ContentAlignment.MiddleLeft);

			rectangle.Height = textBounds.Height + smallFont.Height + (Padding.Top / 2) + Padding.Vertical;

			if (CurrentGroup == item.Key)
			{
				e.Graphics.FillRoundedRectangleWithShadow(rectangle, Padding.Left, Padding.Left, baseColor.MergeColor(BackColor, 75), Color.FromArgb(6, baseColor));
			}
			else if (rectangle.Contains(cursor) && HoverState.HasFlag(HoverState.Hovered))
			{
				e.Graphics.FillRoundedRectangleWithShadow(rectangle, Padding.Left, Padding.Left, baseColor.MergeColor(BackColor, 5), Color.FromArgb(3, baseColor));
			}

			e.Graphics.DrawString(text, font, foreBrush, textBounds);

			e.Graphics.DrawString(subText, smallFont, CurrentGroup == item.Key ? foreBrush : brush, Rectangle.FromLTRB(textBounds.X, textBounds.Bottom, rectangle.Right, rectangle.Bottom));

			e.Graphics.DrawImage(icon.Color(foreBrush.Color), iconBounds);

			_groupRects[item.Key] = rectangle;

			y += rectangle.Height + Padding.Vertical;
		}

		Cursor = _groupRects.Values.Any(x => x.Contains(cursor)) ? Cursors.Hand : Cursors.Default;
	}
}
