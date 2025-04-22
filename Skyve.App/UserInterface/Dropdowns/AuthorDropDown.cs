using Skyve.App.UserInterface.Content;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class AuthorDropDown : SlickMultiSelectionDropDown<IUser>
{
	private readonly Dictionary<IUser, int> _counts = [];
	private readonly IImageService _imageManager;
	private readonly IUserService _userService;
	private readonly IWorkshopService _workshopService;

	[DefaultValue(false)]
	public bool HideUsage { get; set; }

	public AuthorDropDown()
	{
		_imageManager = ServiceCenter.Get<IImageService>();
		_userService = ServiceCenter.Get<IUserService>();
		_workshopService = ServiceCenter.Get<IWorkshopService>();
	}

	public void RefreshItems()
	{
		Items = _workshopService.GetKnownUsers().ToArray();
	}

	protected override IEnumerable<IUser> OrderItems(IEnumerable<IUser> items)
	{
		return items.OrderByDescending(x => SelectedItems.Contains(x)).ThenBy(x => x.Name);
	}

	protected override bool SearchMatch(string searchText, IUser item)
	{
		return searchText.SearchCheck(item.Name) || (item.Id?.ToString().Contains(searchText) ?? false);
	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IUser item, bool selected)
	{
		if (item is null)
		{
			return;
		}

		var text = item.Name;

		using var brush = new SolidBrush(hoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : Color.FromArgb(hoverState.HasFlag(HoverState.Hovered) ? 255 : 200, UserIcon.GetUserColor(item.Id?.ToString() ?? string.Empty, true)));
	
		DrawAuthorImage(e, item, rectangle.Align(new Size(rectangle.Height + UI.Scale(2), rectangle.Height + UI.Scale(2)), ContentAlignment.MiddleLeft), brush.Color, hoverState);

		rectangle = rectangle.Pad(rectangle.Height + Padding.Left, 0, 0, 0);

		if (!HideUsage && _counts.ContainsKey(item!))
		{
			using var brush2 = new SolidBrush(Color.FromArgb(200, foreColor));
			e.Graphics.DrawString(Locale.ItemsCount.FormatPlural(_counts[item!]), Font, brush2, rectangle.Pad(0, 0, UI.Scale(5), 0).AlignToFontSize(Font), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });

			rectangle.Width -= (int)e.Graphics.Measure(Locale.ItemsCount.FormatPlural(_counts[item!]), Font).Width;
		}

		using var font = UI.Font(8.25F).FitTo(text, rectangle, e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle, format);
	}

	private void DrawAuthorImage(PaintEventArgs e, IUser author, Rectangle rectangle, Color color, HoverState hoverState)
	{
		var image = _workshopService.GetUser(author).GetThumbnail();

		if (image != null)
		{
			e.Graphics.DrawRoundImage(image, rectangle.Pad((int)(1.5 * UI.FontScale)));

			if (hoverState.HasFlag(HoverState.Hovered))
			{
				using var pen = new Pen(color, 1.5f);

				e.Graphics.DrawEllipse(pen, rectangle.Pad((int)(1.5 * UI.FontScale)));
			}
		}
		else
		{
			using var authorIcon = IconManager.GetIcon("Author", rectangle.Height);

			e.Graphics.DrawImage(authorIcon.Color(color, color.A), rectangle.CenterR(authorIcon.Size));
		}

		if (_userService.IsUserVerified(author))
		{
			var checkRect = rectangle.Align(new Size(rectangle.Height / 3, rectangle.Height / 3), ContentAlignment.BottomRight);

			using var greenBrush = new SolidBrush(FormDesign.Design.GreenColor);
			e.Graphics.FillEllipse(greenBrush, checkRect.Pad(-UI.Scale(2)));

			using var img = IconManager.GetIcon("Check", checkRect.Height);
			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}
	}

	protected override void PaintSelectedItems(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, IEnumerable<IUser> items)
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

			e.Graphics.DrawString(Locale.AnyAuthor, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

			return;
		}

		var iconRect = Rectangle.Empty;/* rectangle.Align(new Size(rectangle.Height - 2, rectangle.Height - 2), ContentAlignment.MiddleLeft);
		foreach (var item in items)
		{
			var icon = _imageManager.GetImage(item?.AvatarUrl, true).Result;
			if (icon is not null)
			{
				e.Graphics.DrawRoundedImage(icon, iconRect, UI.Scale(4));
			}

			iconRect.X += iconRect.Width * 9 / 10;
		}*/

		e.Graphics.DrawString(Locale.AuthorsSelected.FormatPlural(items.Count()), Font, new SolidBrush(foreColor), rectangle.Pad(iconRect.Right - iconRect.Width, 0, 0, 0).AlignToFontSize(Font), new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
	}
}
