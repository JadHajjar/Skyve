using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class DlcDropDown : SlickMultiSelectionDropDown<IDlcInfo>
{
	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = ServiceCenter.Get<IDlcManager>().Dlcs.ToArray();
		}
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Width = UI.Scale(200);
	}

	protected override IEnumerable<IDlcInfo> OrderItems(IEnumerable<IDlcInfo> items)
	{
		return items.OrderByDescending(x => SelectedItems.Contains(x)).ThenByDescending(x =>
		{
			if (x.ReleaseDate.Year > 1)
			{
				return x.ReleaseDate;
			}

			if (int.TryParse(x.ExpectedRelease, out var year))
			{
				return new DateTime(year, 12, 32);
			}

			var match = Regex.Match(x.ExpectedRelease ?? string.Empty, @"Q(\d) (\d+)");
			if (match.Success)
			{
				return new DateTime(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[1].Value) * 3, 1);
			}

			return DateTime.MaxValue;
		});
	}

	protected override bool SearchMatch(string searchText, IDlcInfo item)
	{
		return searchText.SearchCheck(item.Name.RegexRemove("^.+?- ").RegexRemove("(Content )?Creator Pack: "));
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IDlcInfo item, bool selected)
	{
		if (item is null)
		{
			return;
		}

		var text = item.Name.RegexRemove("^.+?- ").RegexRemove("(Content )?Creator Pack: ");
		var icon = item is IThumbnailObject thumbnailObject ? thumbnailObject.GetThumbnail() : null;

		icon ??= (item.Id == 2427731 ? Properties.Resources.Cities2Landmark : Properties.Resources.Cities2Dlc);

		if (icon != null)
		{
			e.Graphics.DrawRoundedImage(icon, rectangle.Align(new Size(rectangle.Height * 460 / 215, rectangle.Height), ContentAlignment.MiddleLeft), UI.Scale(3), hoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : BackColor);
		}

		rectangle = rectangle.Pad((rectangle.Height * 460 / 215) + Padding.Left, 0, 0, 0);

		using var format = new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
		using var brush = new SolidBrush(foreColor);
		e.Graphics.DrawString(text, base.Font, brush, rectangle.AlignToFontSize(base.Font), format);
	}

	protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<IDlcInfo> items)
	{
		if (items.Count() == 1)
		{
			PaintItem(e, rectangle, foreColor, hoverState, items.First(), false);

			return;
		}

		if (!items.Any())
		{
			using var icon = IconManager.GetIcon("Slash", rectangle.Height - 2).Color(foreColor);

			e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

			e.Graphics.DrawString(LocaleCR.NoRequiredDlcs, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

			return;
		}

		e.Graphics.DrawString(LocaleCR.DlcsSelected.FormatPlural(items.Count()), Font, new SolidBrush(foreColor), rectangle.AlignToFontSize(Font), new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
	}
}
