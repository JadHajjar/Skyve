﻿using SkyveApp.Systems.CS1.Utilities;

using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Dropdowns;

public enum CompatibilityNotificationFilter
{
	Any = -2,
	AnyIssue = -1,
	NoIssues,
	Caution = 2,
	MissingDependency,
	Warning,
	AttentionRequired,
	Exclude,
	Unsubscribe,
	Switch,
}

internal class ReportSeverityDropDown : SlickSelectionDropDown<CompatibilityNotificationFilter>
{
	public override void ResetValue()
	{
		SelectedItem = CompatibilityNotificationFilter.Any;
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(CompatibilityNotificationFilter)).Cast<CompatibilityNotificationFilter>().ToArray();

			selectedItem = CompatibilityNotificationFilter.Any;
		}
	}

	protected override IEnumerable<CompatibilityNotificationFilter> OrderItems(IEnumerable<CompatibilityNotificationFilter> items)
	{
		return items.OrderBy(x => (int)x);
	}

	protected override bool SearchMatch(string searchText, CompatibilityNotificationFilter item)
	{
		var text = item == CompatibilityNotificationFilter.Any ? Locale.AnyCompatibilityStatus : LocaleCR.Get($"{item}");

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, CompatibilityNotificationFilter item)
	{
		var text = item switch { CompatibilityNotificationFilter.Any => Locale.AnyCompatibilityStatus, CompatibilityNotificationFilter.AnyIssue => Locale.AnyIssue, _ => LocaleCR.Get($"{item}") };
		var color = item switch { CompatibilityNotificationFilter.Any => foreColor, CompatibilityNotificationFilter.AnyIssue => FormDesign.Design.RedColor, _ => ((NotificationType)(int)item).GetColor() };
		using var icon = (item switch { CompatibilityNotificationFilter.Any => new DynamicIcon("I_Slash"), CompatibilityNotificationFilter.AnyIssue => new DynamicIcon("I_Warning"), _ => ((NotificationType)(int)item).GetIcon(true) }).Get(rectangle.Height - 2).Color(color);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}
}
