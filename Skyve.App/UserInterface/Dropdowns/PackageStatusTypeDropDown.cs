﻿using Skyve.Compatibility.Domain.Enums;

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
			Items = Enum.GetValues(typeof(T)).Cast<T>().Where(x => CRNAttribute.GetAttribute(x).Browsable && (!_restricted || CRNAttribute.GetAttribute(x).AllowedChange != CRNAttribute.ChangeType.Deny)).ToArray();
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = (int)(175 * UI.FontScale);
	}

	protected override bool SearchMatch(string searchText, T item)
	{
		var text = LocaleCR.Get($"{item}");

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, T item)
	{
		var text = LocaleCR.Get($"{item}");
		var color = CRNAttribute.GetNotification(item).GetColor();

		using var icon = IconManager.GetIcon("I_Stability", rectangle.Height - 2).Color(color);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
