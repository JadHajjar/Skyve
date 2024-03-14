using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
public class AuthorDropDown : SlickMultiSelectionDropDown<IUser>
{
	private readonly Dictionary<IUser, int> _counts = [];
	private readonly IImageService _imageManager;
	private readonly IUserService _userService;

	public AuthorDropDown()
	{
		_imageManager = ServiceCenter.Get<IImageService>();
		_userService = ServiceCenter.Get<IUserService>();
	}

	public void SetItems<T>(IEnumerable<T?> enumerable) where T : IPackageIdentity
	{
		_counts.Clear();

		foreach (var item in enumerable.SelectWhereNotNull(x => x?.GetWorkshopInfo()?.Author))
		{
			if (_counts.ContainsKey(item!))
			{
				_counts[item!]++;
			}
			else
			{
				_counts[item!] = 1;
			}
		}

		Items = [.. _counts.Keys];
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

#if CS1
		var icon = _imageManager.GetImage(item.AvatarUrl, true).Result;
		var avatarRect = rectangle.Align(new Size(rectangle.Height - 2, rectangle.Height - 2), ContentAlignment.MiddleLeft);

		if (icon != null)
		{
			e.Graphics.DrawRoundedImage(icon, avatarRect, (int)(4 * UI.FontScale));
		}

		if (_userService.IsUserVerified(item))
		{
			var checkRect = avatarRect.Align(new Size(avatarRect.Height / 3, avatarRect.Height / 3), ContentAlignment.BottomRight);

			e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect.Pad(-(int)(2 * UI.FontScale)));

			using var img = IconManager.GetIcon("Check", checkRect.Height);
			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}

		rectangle = rectangle.Pad(rectangle.Height + Padding.Left, 0, 0, 0);
#endif

		if (_counts.ContainsKey(item!))
		{
			using var brush2 = new SolidBrush(Color.FromArgb(200, foreColor));
			e.Graphics.DrawString(Locale.ItemsCount.FormatPlural(_counts[item!]), Font, brush2, rectangle.Pad(0, 0, (int)(5 * UI.FontScale), 0).AlignToFontSize(Font), new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });

			rectangle.Width -= (int)e.Graphics.Measure(Locale.ItemsCount.FormatPlural(_counts[item!]), Font).Width;
		}

		using var brush = new SolidBrush(foreColor);
		using var font = UI.Font(8.25F).FitTo(text, rectangle, e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };
		e.Graphics.DrawString(text, font, brush, rectangle, format);
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
				e.Graphics.DrawRoundedImage(icon, iconRect, (int)(4 * UI.FontScale));
			}

			iconRect.X += iconRect.Width * 9 / 10;
		}*/

		e.Graphics.DrawString(Locale.AuthorsSelected.FormatPlural(items.Count()), Font, new SolidBrush(foreColor), rectangle.Pad(iconRect.Right - iconRect.Width, 0, 0, 0).AlignToFontSize(Font), new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });
	}
}
