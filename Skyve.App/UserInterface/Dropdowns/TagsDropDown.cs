using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class TagsDropDown : SlickMultiSelectionDropDown<ITag>
{
	private readonly ITagsService _tagsService = ServiceCenter.Get<ITagsService>();

	protected override IEnumerable<ITag> OrderItems(IEnumerable<ITag> items)
	{
		return items.OrderByDescending(x => SelectedItems.Contains(x)).ThenBy(x => x.Value);
	}

	protected override bool SearchMatch(string searchText, ITag item)
	{
		return searchText.SearchCheck(item.Value);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, ITag item, bool selected)
	{
		var text = item.Value;

		using var icon = IconManager.GetIcon(text == Locale.AnyTags ? "I_Slash" : item.Icon, rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);

		var text2 = Locale.ItemsCount.FormatPlural(_tagsService.GetTagUsage(item));
		using var brush2 = new SolidBrush(Color.FromArgb(200, foreColor));
		using var font2 = UI.Font(8.25F).FitTo(text, rectangle.Pad(0, 0, (int)(5 * UI.FontScale), 0), e.Graphics);
		using var format2 = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text2, font2, brush2, rectangle.Pad(0, 0, (int)(5 * UI.FontScale), 0), format);
	}

	protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<ITag> items)
	{
		var text = items.Count() switch { 0 => Locale.AnyTags.ToString(), <= 2 => items.ListStrings(", "), _ => $"{items.Take(2).ListStrings(", ")} +{items.Count() - 2}" };

		using var icon = IconManager.GetIcon(!items.Any() ? "I_Slash" : "I_Tag", rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
