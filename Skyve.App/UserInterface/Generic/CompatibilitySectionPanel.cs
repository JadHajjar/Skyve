using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class CompatibilitySectionPanel : SmartPanel
{
	public ReportType ReportType { get; }
	public List<ICompatibilityItem> ReportItems { get; set; }

	public CompatibilitySectionPanel(ReportType reportType)
	{
		ReportType = reportType;
		ReportItems = [];
		DoubleBuffered = true;
		ResizeRedraw = true;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		Padding = UI.Scale(new Padding(12, 12 + 36, 12, 12));
		Margin = UI.Scale(new Padding(6));
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(Parent.BackColor);

		BackColor = FormDesign.Design.AccentBackColor;

		var margin = new Padding(Padding.Left / 2);
		var rectangle = ClientRectangle.Pad(margin.Left);

		e.Graphics.FillRoundedRectangleWithShadow(rectangle, margin.Left, Padding.Right / 2, FormDesign.Design.AccentBackColor, Color.FromArgb(8, ReportItems.Count > 0 ? ReportItems.Max(x => x.Status.Notification).GetColor().Tint(Lum: FormDesign.Design.IsDarkTheme ? 6 : -6) : BackColor));

		rectangle = rectangle.Pad(Margin.Left / 2);

		using var icon = GetTypeIcon().Get(UI.Scale(24));
		var text = LocaleHelper.GetGlobalText($"CRT_{ReportType}");
		var subText = (string?)null;
		var iconRectangle = new Rectangle(rectangle.Right - margin.Right - icon?.Width ?? 0, rectangle.Y, icon?.Width ?? 0, icon?.Height ?? 0);
		var textRect = new Rectangle(rectangle.X + margin.Left, rectangle.Y, rectangle.Right - margin.Horizontal - margin.Left - iconRectangle.Width, UI.Scale(26));
		using var font = UI.Font(9.75F, FontStyle.Bold).FitTo(text, textRect, e.Graphics);
		var titleHeight = Math.Max(icon?.Height ?? 0, (int)e.Graphics.Measure(text, font, rectangle.Right - margin.Horizontal - iconRectangle.Right).Height);
		textRect.Height = titleHeight + margin.Top;

		if (subText is not null)
		{
			textRect.Y += margin.Top / 2;

			using var smallFont = UI.Font(6.75F).FitToWidth(subText, textRect, e.Graphics);
			var textHeight = (int)e.Graphics.Measure(subText, smallFont, rectangle.Right - margin.Horizontal - iconRectangle.Right).Height;

			using var subTextBrush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.ForeColor));
			e.Graphics.DrawString(subText, smallFont, subTextBrush, textRect.Pad(UI.Scale(2), 0, 0, 0));

			titleHeight += textHeight - (margin.Top / 2);
			textRect.Y += textHeight - (margin.Top / 2);
		}
		else
		{
			textRect.Y += margin.Top + ((titleHeight - textRect.Height) / 2);
		}

		iconRectangle.Y += margin.Top + ((titleHeight - icon?.Height ?? 0) / 2);

		if (icon is not null)
		{
			try
			{
				e.Graphics.DrawImage(icon.Color(FormDesign.Design.ForeColor), iconRectangle);
			}
			catch { }
		}

		using var textBrush = new SolidBrush(FormDesign.Design.ForeColor);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, textBrush, textRect, format);
	}

	private DynamicIcon GetTypeIcon()
	{
		return ReportType switch
		{
			ReportType.Stability => "Stability",
			ReportType.DlcMissing or ReportType.RequiredPackages => "MissingMod",
			ReportType.Ambiguous => "Malicious",
			ReportType.Successors => "Upgrade",
			ReportType.Alternatives => "Alternatives",
			ReportType.Status => "Statuses",
			ReportType.OptionalPackages => "Recommendations",
			ReportType.Compatibility => "Compatibilities",
			ReportType.RequiredItem => "Important",
			ReportType.RequestReview => "RequestReview",
			_ => "CompatibilityReport",
		};
	}
}
