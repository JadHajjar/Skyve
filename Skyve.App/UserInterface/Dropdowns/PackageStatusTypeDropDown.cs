using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;


namespace Skyve.App.UserInterface.Dropdowns;

public class PackageStatusTypeDropDown<T> : SlickSelectionDropDown<T> where T : struct, Enum
{
	private readonly bool _restricted;

	public PackageStatusTypeDropDown(bool restricted)
	{
		_restricted = restricted;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(T)).Cast<T>()
				.Where(x => CRNAttribute.GetAttribute(x).Browsable && (!_restricted || CRNAttribute.GetAttribute(x).AllowedChange != CRNAttribute.ChangeType.Deny))
				.OrderBy(CRNAttribute.GetNotification)
				.ToArray();
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(175);
	}

	protected override bool SearchMatch(string searchText, T item)
	{
		var text = GetText(item);

		return searchText.SearchCheck(text);
	}

	protected virtual LocaleHelper.Translation GetText(T item)
	{
		return LocaleCR.Get(item.ToString());
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item)
	{
		var text = GetText(item);
		var notification = CRNAttribute.GetNotification(item);
		var color = notification.GetColor();

		using var icon = notification.GetIcon(true).Get(rectangle.Height - 2).Color(color);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
