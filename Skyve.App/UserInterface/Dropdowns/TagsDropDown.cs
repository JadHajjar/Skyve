using SkyveApp.Systems.CS1.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Dropdowns;
internal class TagsDropDown : SlickMultiSelectionDropDown<ITag>
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

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

		e.Graphics.DrawString(Locale.ItemsCount.FormatPlural(_tagsService.GetTagUsage(item)), Font, new SolidBrush(Color.FromArgb(200, foreColor)), rectangle.Pad(0, 0, (int)(5 * UI.FontScale), 0).AlignToFontSize(Font), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
	}

	protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<ITag> items)
	{
		var text = items.Count() switch { 0 => Locale.AnyTags.ToString(), <= 2 => items.ListStrings(", "), _ => $"{items.Take(2).ListStrings(", ")} +{items.Count() - 2}" };

		using var icon = IconManager.GetIcon(!items.Any() ? "I_Slash" : "I_Tag", rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}
}
