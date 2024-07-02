using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;

public enum CompatibilityNotificationFilter
{
	Any = -2,
	AnyIssue = -1,
	NoIssues,
	RequiredItem = 20,

	Caution = 30,
	MissingDependency = 40,
	Warning = 50,

	AttentionRequired = 60,
	Exclude = 70,
	Unsubscribe = 80,
	Switch = 90,
}

public class ReportSeverityDropDown : SlickSelectionDropDown<CompatibilityNotificationFilter>
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
		var text = item == CompatibilityNotificationFilter.Any ? Locale.AnyStatus : LocaleCR.Get($"{item}");

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, CompatibilityNotificationFilter item)
	{
		var text = item switch { CompatibilityNotificationFilter.Any => Locale.AnyStatus, CompatibilityNotificationFilter.AnyIssue => Locale.AnyIssue, _ => LocaleCR.Get($"{item}") };
		var color = item switch { CompatibilityNotificationFilter.Any => foreColor, CompatibilityNotificationFilter.AnyIssue => FormDesign.Design.RedColor, _ => ((NotificationType)(int)item).GetColor() };
		using var icon = (item switch { CompatibilityNotificationFilter.Any => new DynamicIcon("Slash"), CompatibilityNotificationFilter.AnyIssue => new DynamicIcon("Warning"), _ => ((NotificationType)(int)item).GetIcon(true) }).Get(rectangle.Height - 2).Color(color);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}
}
