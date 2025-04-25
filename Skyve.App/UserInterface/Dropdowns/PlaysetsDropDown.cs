using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class PlaysetsDropDown : SlickSelectionDropDown<IPlayset?>
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
#if CS1
		return items.OrderByDescending(x => x?.Temporary ?? true).ThenByDescending(x => x?.DateUpdated);
#else
		return items.OrderByDescending(x => x?.DateUpdated);
#endif
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IPlayset? item)
	{
		var text = item?.Name;
#if CS1
		var nullOrTemp = item?.Temporary ?? true;
#else
		var nullOrTemp = item is null;
#endif

		if (nullOrTemp)
		{
			text = Locale.Unfiltered;
		}

		using var icon = (nullOrTemp ? new DynamicIcon("Slash") : item!.GetCustomPlayset().GetIcon()).Get(rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
