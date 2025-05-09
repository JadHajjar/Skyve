﻿using Skyve.App.UserInterface.Content;
using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	public partial class Simple : ItemListControl
	{
		protected override void OnPaintItemGrid(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var package = e.Item.GetPackage();
			var localIdentity = e.Item.GetLocalPackageIdentity();
			var workshopInfo = e.Item.GetWorkshopInfo();
			var isIncluded = _packageUtil.IsIncluded(e.Item, out var partialIncluded, SelectedPlayset) || partialIncluded;
			var isEnabled = _packageUtil.IsEnabled(e.Item, SelectedPlayset);

			e.BackColor = BackColor;

			if (e.IsSelected)
			{
				e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
			}

			base.OnPaintItemGrid(e);

			if (e.InvalidRects.All(x => x == e.Rects.IncludedRect))
			{
				DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out _);
				return;
			}

			e.DrawableItem.CachedHeight = int.MinValue;

			DrawThumbnail(e, localIdentity, workshopInfo);
			DrawTitleAndTags(e);
			DrawAuthor(e, workshopInfo);
			DrawVersionAndTags(e, package, localIdentity, workshopInfo);
			DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, package?.LocalData, out var activeColor);
			DrawDots(e);

			e.DrawableItem.CachedHeight = Math.Max(e.Rects.IncludedRect.Bottom, e.DrawableItem.CachedHeight) - e.ClipRectangle.Y + GridPadding.Vertical + Padding.Vertical;

			DrawCompatibilityAndStatus(e, out var outerColor);

			if (outerColor != default)
			{
				using var pen = new Pen(outerColor, (float)(1.5 * UI.FontScale));

				e.Graphics.DrawRoundedRectangle(pen, e.ClipRectangle.ClipTo(e.DrawableItem.CachedHeight - Padding.Vertical - GridPadding.Vertical).InvertPad(GridPadding - new Padding((int)pen.Width)), UI.Scale(5));
			}

			if (e.DrawableItem.Tag is not null)
			{
				using var brush = new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 175, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
			else if (!isEnabled && isIncluded && !IsPackagePage && _settings.UserSettings.FadeDisabledItems && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
		}

		private void DrawDots(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var isHovered = e.Rects.DotsRect.Contains(CursorLocation);
			using var img = IconManager.GetIcon("VertialMore", e.Rects.IncludedRect.Height * 3 / 4).Color(isHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor);

			e.Graphics.DrawImage(img, e.Rects.DotsRect.CenterR(img.Size));
		}

		private void DrawCompatibilityAndStatus(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, out Color outerColor)
		{
			var compatibilityReport = e.Item.GetCompatibilityInfo();
			var notificationType = compatibilityReport?.GetNotification();
			outerColor = default;

			var height = UI.Scale(20);
			using var baseFont = UI.Font(9F, FontStyle.Bold);

			if (GetStatusDescriptors(e.Item, out var text, out var icon, out var color))
			{
				var rect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y + e.DrawableItem.CachedHeight - Padding.Vertical, e.ClipRectangle.Width, height);

				outerColor = Color.FromArgb(200, color);

				using var brush = new SolidBrush(outerColor);
				using var textBrush = new SolidBrush(outerColor.GetTextColor());
				using var image = icon!.Get(baseFont.Height * 4 / 3).Color(textBrush.Color);
				using var font = UI.Font(9F, FontStyle.Bold).FitTo(text, rect.Pad(image.Width * 2, GridPadding.Top, image.Width * 2, GridPadding.Bottom), e.Graphics);
				using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

				e.Graphics.FillRoundedRectangle(brush, rect.Pad(-GridPadding.Left + (int)(1.5 * UI.FontScale)), UI.Scale(5), false, false, notificationType <= NotificationType.Info, notificationType <= NotificationType.Info);

				e.Graphics.DrawString(text, font, textBrush, rect.Pad(image.Width * 2, 0, image.Width * 2, 0), format);

				e.Graphics.DrawImage(image, rect.Pad(GridPadding).Align(image.Size, ContentAlignment.MiddleLeft));

				e.Rects.DownloadStatusRect = rect;

				e.DrawableItem.CachedHeight += height + GridPadding.Vertical;

				if (notificationType > NotificationType.Info)
				{
					e.DrawableItem.CachedHeight -= GridPadding.Top / 2;
				}
			}

			if (notificationType > NotificationType.Info)
			{
				var rect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y + e.DrawableItem.CachedHeight - Padding.Vertical, e.ClipRectangle.Width, height);
				var text2 = LocaleCR.Get(notificationType.ToString());

				outerColor = Color.FromArgb(rect.Contains(CursorLocation) ? 200 : 255, notificationType.Value.GetColor());

				using var brush = new SolidBrush(outerColor);
				using var textBrush = new SolidBrush(outerColor.GetTextColor());
				using var image = notificationType.Value.GetIcon(false).Get(baseFont.Height * 4 / 3).Color(textBrush.Color);
				using var font = UI.Font(9F, FontStyle.Bold).FitTo(text2, rect.Pad(image.Width * 2, GridPadding.Top, image.Width * 2, GridPadding.Bottom), e.Graphics);
				using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

				e.Graphics.FillRoundedRectangle(brush, rect.Pad(-GridPadding.Left + (int)(1.5 * UI.FontScale)), UI.Scale(5), false, false);

				e.Graphics.DrawString(text2, font, textBrush, rect.Pad(image.Width * 2, 0, image.Width * 2, 0), format);

				e.Graphics.DrawImage(image, rect.Pad(GridPadding).Align(image.Size, ContentAlignment.MiddleLeft));

				e.Rects.CompatibilityRect = rect;

				e.DrawableItem.CachedHeight += height + GridPadding.Vertical;
			}

			if (e.IsSelected && outerColor == default)
			{
				outerColor = FormDesign.Design.GreenColor;
			}
		}

		private void DrawTags(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var packageTags = e.Item.GetTags(IsPackagePage).ToList();

			if (packageTags.Count == 0)
			{
				return;
			}

			var text = packageTags.ListStrings(", ");

			DrawCell(e, Columns.Tags, packageTags.ListStrings(", "), "Tag", active: false);
		}

		private void DrawAuthor(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IWorkshopInfo? workshopInfo)
		{
			var author = workshopInfo?.Author;

			if (author?.Name is not null and not "")
			{
				using var authorFont = UI.Font(7.5F, FontStyle.Regular);
				using var authorFontUnderline = UI.Font(7.5F, FontStyle.Underline);
				using var stringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

				var rect = new Rectangle(e.Rects.TextRect.X, e.DrawableItem.CachedHeight, e.Rects.TextRect.Width, 0);
				var size = e.Graphics.Measure(author.Name, authorFont).ToSize();

				e.Rects.AuthorRect = rect.Align(size + new Size(size.Height, 0), ContentAlignment.TopLeft);
				e.DrawableItem.CachedHeight = e.Rects.AuthorRect.Bottom + (GridPadding.Top / 3);

				var isHovered = e.Rects.AuthorRect.Contains(CursorLocation);
				using var brush = new SolidBrush(isHovered ? FormDesign.Design.ActiveColor : Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 255 : 200, UserIcon.GetUserColor(author.Id?.ToString() ?? string.Empty, true)));

				DrawAuthorImage(e, author, e.Rects.AuthorRect.Align(new(size.Height, size.Height), ContentAlignment.MiddleLeft), brush.Color);

				e.Graphics.DrawString(author.Name, isHovered ? authorFontUnderline : authorFont, brush, e.Rects.AuthorRect, stringFormat);
			}
		}

		private void DrawVersionAndTags(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IPackage? package, ILocalPackageIdentity? localPackageIdentity, IWorkshopInfo? workshopInfo)
		{
#if CS1
			var isVersion = localParentPackage?.Mod is not null && !e.Item.IsBuiltIn && !IsPackagePage;
			var text = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.IsBuiltIn ? Locale.Vanilla : e.Item is ILocalPackageData lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString();
#else
			var isVersion = package?.IsCodeMod() ?? false;
			var versionText = isVersion ? package?.VersionName ?? (workshopInfo?.Changelog.FirstOrDefault(x => x.VersionId == package?.Version)?.Version) : null;
			versionText = versionText is not null ? $"v{versionText}" : localPackageIdentity != null ? localPackageIdentity.FileSize.SizeString(0) : workshopInfo?.ServerSize.SizeString(0);
#endif

			var packageTags = e.Item.GetTags(IsPackagePage).ToList();

			if (packageTags.Count > 0)
			{
				if (!string.IsNullOrEmpty(versionText))
				{
					versionText += " • ";
				}
				else
				{
					versionText = string.Empty;
				}

				versionText += packageTags.ListStrings(", ");
			}

			if (!string.IsNullOrEmpty(versionText))
			{
				using var fadedBrush = new SolidBrush(Color.FromArgb(GridView ? 150 : 200, e.BackColor.GetTextColor()));

				var rect = GridView
					? new Rectangle(e.Rects.TextRect.X, e.DrawableItem.CachedHeight, e.Rects.TextRect.Width, Height)
					: new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Bottom, e.Rects.TextRect.Width, e.Rects.IconRect.Bottom - e.Rects.TextRect.Bottom - Padding.Bottom);

				using var versionFont = GridView ? UI.Font(7.5F) : UI.Font(8.25F).FitToHeight(versionText, rect, e.Graphics);
				using var format = GridView ? new() : new StringFormat { LineAlignment = StringAlignment.Far };

				e.Graphics.DrawString(versionText, versionFont, fadedBrush, rect, format);

				if (GridView)
				{
					e.DrawableItem.CachedHeight += (int)e.Graphics.Measure(versionText, versionFont, rect.Width).Height;
				}
			}
		}

		private Rectangles GenerateGridRectangles(IPackageIdentity item, Rectangle rectangle)
		{
			var rects = new Rectangles(item)
			{
				IconRect = rectangle.Align(new Size(rectangle.Width, rectangle.Width), ContentAlignment.TopCenter),
				IncludedRect = new Rectangle(new Point(rectangle.X, rectangle.Y + rectangle.Width + GridPadding.Top), UI.Scale(new Size(32, 32)))
			};

			rects.DotsRect = new Rectangle(rectangle.X, rects.IncludedRect.Y, rectangle.Width, rects.IncludedRect.Height).Align(UI.Scale(new Size(16, 24)), ContentAlignment.MiddleRight);

			using var titleFont = UI.Font(CompactList ? 8.25F : 10.5F, FontStyle.Bold);
			rects.TextRect = new Rectangle(rectangle.X + rects.IncludedRect.Width + GridPadding.Left, rectangle.Y + rectangle.Width + GridPadding.Top, rectangle.Width - rects.IncludedRect.Width - GridPadding.Horizontal - rects.DotsRect.Width, 0).AlignToFontSize(titleFont, ContentAlignment.TopLeft);
			rects.CenterRect = rects.TextRect.Pad(0, -GridPadding.Vertical, 0, 0);

			if (_settings.UserSettings.AdvancedIncludeEnable && item.IsCodeMod())
			{
				rects.EnabledRect = rects.IncludedRect;

				rects.IncludedRect.X -= rects.IncludedRect.Width;
			}

			return rects;
		}

		protected override void OnViewChanged()
		{
			base.OnViewChanged();

			DynamicSizing = GridView;

			if (GridView)
			{
				Padding = UI.Scale(new Padding(4), UI.UIScale);
				GridPadding = UI.Scale(new Padding(6), UI.UIScale);
			}
		}
	}
}
