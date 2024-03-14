using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class PackageStatusDropDown : SlickSelectionDropDown<DownloadStatusFilter>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(DownloadStatusFilter)).Cast<DownloadStatusFilter>().ToArray();
		}
	}

	protected override bool SearchMatch(string searchText, DownloadStatusFilter item)
	{
		GetStatusDescriptors(item, out var text, out _, out _);

		return searchText.SearchCheck(text);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, DownloadStatusFilter item)
	{
		GetStatusDescriptors(item, out var text, out var di, out var color);

		using var icon = di.Get(rectangle.Height - 2).Color(hoverState.HasFlag(HoverState.Pressed) || item == DownloadStatusFilter.Any ? foreColor : color);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), format);
	}

	private void GetStatusDescriptors(DownloadStatusFilter status, out string text, out DynamicIcon icon, out Color color)
	{
		switch (status)
		{
			case DownloadStatusFilter.Any:
				text = Locale.AnyStatus;
				icon = new DynamicIcon("Slash");
				color = FormDesign.Design.ForeColor;
				return;

			case DownloadStatusFilter.AnyIssue:
				text = Locale.AnyIssue;
				icon = new DynamicIcon("Warning");
				color = FormDesign.Design.RedColor;
				return;

			case DownloadStatusFilter.OK:
				text = Locale.UpToDate;
				icon = new DynamicIcon("Ok");
				color = FormDesign.Design.GreenColor;
				return;
			case DownloadStatusFilter.Unknown:
				text = Locale.StatusUnknown;
				icon = new DynamicIcon("Question");
				color = FormDesign.Design.YellowColor;
				return;
			case DownloadStatusFilter.OutOfDate:
				text = Locale.OutOfDate;
				icon = new DynamicIcon("OutOfDate");
				color = FormDesign.Design.YellowColor;
				return;
			case DownloadStatusFilter.PartiallyDownloaded:
				text = Locale.PartiallyDownloaded;
				icon = new DynamicIcon("Broken");
				color = FormDesign.Design.RedColor;
				return;
			case DownloadStatusFilter.Removed:
				text = Locale.RemovedByAuthor;
				icon = new DynamicIcon("ContentRemoved");
				color = FormDesign.Design.RedColor;
				return;
		}

		text = Locale.Local;
		icon = new DynamicIcon("PC");
		color = FormDesign.Design.ForeColor;
	}
}
