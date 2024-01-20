using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class ProfilesDropDown : SlickSelectionDropDown<IPlayset?>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = [null, .. ServiceCenter.Get<IPlaysetManager>().Playsets];

			selectedItem = null;
		}
	}

	protected override IEnumerable<IPlayset?> OrderItems(IEnumerable<IPlayset?> items)
	{
		return items.OrderByDescending(x => x?.Temporary ?? true).ThenByDescending(x => x?.DateUpdated);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IPlayset? item)
	{
		var text = item?.Name;

		if (item?.Temporary ?? true)
		{
			text = Locale.Unfiltered;
		}

		using var icon = (item?.Temporary ?? true ? new DynamicIcon("I_Slash") : item.GetIcon()).Get(rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}
}
