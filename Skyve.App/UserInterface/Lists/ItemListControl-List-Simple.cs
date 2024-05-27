using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	public partial class Simple : ItemListControl
	{
		public Simple(SkyvePage page, IPackageUtil? customPackageUtil = null) : base(page, customPackageUtil)
		{
			GridItemSize = new Size(190, 260);
			DynamicSizing = true;
		}

		private void OnPaintItemCompactList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var package = e.Item.GetPackage();
			var localIdentity = e.Item.GetLocalPackageIdentity();
			var workshopInfo = e.Item.GetWorkshopInfo();
			var isIncluded = _packageUtil.IsIncluded(e.Item, out var partialIncluded, SelectedPlayset) || partialIncluded;
			var isEnabled = _packageUtil.IsEnabled(e.Item, SelectedPlayset);

			var compatibilityReport = e.Item.GetCompatibilityInfo();
			var notificationType = compatibilityReport?.GetNotification();
			var hasStatus = GetStatusDescriptors(e.Item, out var statusText, out var statusIcon, out var statusColor);

			SetBackColorForList(e, notificationType, hasStatus, statusColor);

			base.OnPaintItemList(e);

			if (e.InvalidRects.All(x => x == e.Rects.IncludedRect))
			{
				DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out _);
				return;
			}

			e.Graphics.SetClip(new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y - Padding.Top + 1, e.ClipRectangle.Width, e.ClipRectangle.Height + Padding.Vertical - 2));

			DrawTitleAndTags(e);
			DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, package?.LocalData, out var activeColor);
			DrawCompactVersionAndDate(e, package, localIdentity, workshopInfo);
			DrawButtons(e, localIdentity, workshopInfo);

			if (Width / UI.FontScale >= 600)
			{
				DrawCompactAuthorOrFolder(e, localIdentity, workshopInfo);
			}

			if (Width / UI.FontScale >= 500)
			{
				DrawCompatibilityAndStatusList(e, notificationType, statusText, statusIcon, statusColor);
			}

			if (Width / UI.FontScale >= 965)
			{
				DrawTags(e);
			}

			e.Graphics.ResetClip();

			if (e.DrawableItem.Tag is not null)
			{
				using var brush = new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 175, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
			else if (!isEnabled && isIncluded && !IsPackagePage && _settings.UserSettings.FadeDisabledItems && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(Padding));
			}
		}

		protected override void OnPaintItemList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			if (CompactList)
			{
				OnPaintItemCompactList(e);

				return;
			}

			var package = e.Item.GetPackage();
			var localIdentity = e.Item.GetLocalPackageIdentity();
			var workshopInfo = e.Item.GetWorkshopInfo();
			var isIncluded = _packageUtil.IsIncluded(e.Item, out var partialIncluded, SelectedPlayset) || partialIncluded;
			var isEnabled = _packageUtil.IsEnabled(e.Item, SelectedPlayset);

			var compatibilityReport = e.Item.GetCompatibilityInfo();
			var notificationType = compatibilityReport?.GetNotification();
			var hasStatus = GetStatusDescriptors(e.Item, out var statusText, out var statusIcon, out var statusColor);

			SetBackColorForList(e, notificationType, hasStatus, statusColor);

			base.OnPaintItemList(e);

			if (e.InvalidRects.All(x => x == e.Rects.IncludedRect))
			{
				DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out _);
				return;
			}

			DrawThumbnail(e, localIdentity, workshopInfo);
			DrawTitleAndTags(e);
			DrawVersionAndTags(e, package, localIdentity, workshopInfo);
			DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out _);
			DrawCenterInfo(e, localIdentity, workshopInfo);
			DrawButtons(e, localIdentity, workshopInfo);

			if (!IsPackagePage)
			{
				DrawCompatibilityAndStatusList(e, notificationType, statusText, statusIcon, statusColor);
			}

			e.Graphics.ResetClip();

			if (e.DrawableItem.Tag is not null)
			{
				using var brush = new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 175, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
			else if (!isEnabled && isIncluded && !IsPackagePage && _settings.UserSettings.FadeDisabledItems && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(Padding));
			}
		}

		private void DrawCenterInfo(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageIdentity? localIdentity, IWorkshopInfo? workshopInfo)
		{
			if ((e.ClipRectangle.Width - Padding.Horizontal) / UI.FontScale <= 500)
			{
				return;
			}

			using var fontBase = UI.Font(8.25F);
			using var brush = new SolidBrush(e.BackColor.GetTextColor());
			using var stringFormat = new StringFormat { LineAlignment = StringAlignment.Center };

			var index = 0;
			var itemHeight = (int)e.Graphics.Measure(" ", fontBase).Height;
			var rect = new Rectangle(e.Rects.TextRect.Right, e.Rects.TextRect.Y + Padding.Top, e.ClipRectangle.Width * 2 / 10, itemHeight);
			var author = workshopInfo?.Author;

			if (author?.Name is not null and not "")
			{
				var isHovered = rect.Contains(CursorLocation);

				using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor);
				using var icon = IconManager.GetIcon("Author", itemHeight + Padding.Top).Color(isHovered ? activeBrush.Color : brush.Color);
				using var font = UI.Font(8.25F).FitToWidth(author.Name, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);
				using var fontUnderline = UI.Font(8.25F, FontStyle.Underline).FitToWidth(author.Name, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);

				e.Graphics.DrawImage(icon, rect.Align(icon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(author.Name, isHovered ? fontUnderline : font, isHovered ? activeBrush : brush, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), stringFormat);

				e.Rects.AuthorRect = rect;
			}
			else if (localIdentity is not null)
			{
				using var icon = IconManager.GetIcon("Folder", itemHeight + Padding.Top).Color(brush.Color);
				using var font = UI.Font(8.25F).FitTo(Path.GetFileName(localIdentity.Folder), rect.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);

				e.Graphics.DrawImage(icon, rect.Align(icon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(Path.GetFileName(localIdentity.Folder), font, brush, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), stringFormat);
			}

			tick();

			var date = workshopInfo is null || workshopInfo.ServerTime == default ? (localIdentity?.LocalTime ?? default) : workshopInfo.ServerTime;

			if (date != default)
			{
				var dateText = _settings.UserSettings.ShowDatesRelatively ? date.ToLocalTime().ToRelatedString(true, false) : date.ToLocalTime().ToString("g");
				var isRecent = date > DateTime.UtcNow.AddDays(-7) && e.BackColor != FormDesign.Design.ActiveColor;

				using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor);
				using var icon = IconManager.GetIcon("UpdateTime", itemHeight + Padding.Top).Color(isRecent ? activeBrush.Color : brush.Color);
				using var font = UI.Font(8.25F).FitToWidth(dateText, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);

				e.Graphics.DrawImage(icon, rect.Align(icon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(dateText, font, isRecent ? activeBrush : brush, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), stringFormat);
			}

			tick();

			if (workshopInfo is not null)
			{
				if (workshopInfo.VoteCount >= 0)
				{
					var isHovered = rect.Contains(CursorLocation);
					var text = Locale.VotesCount.FormatPlural(workshopInfo.VoteCount, workshopInfo.VoteCount.ToString("N0"));
					using var fontBold = UI.Font(8.25F, FontStyle.Bold);
					using var fontUnderline = UI.Font(8.25F, workshopInfo.HasVoted ? FontStyle.Bold | FontStyle.Underline : FontStyle.Underline);
					using var greenBrush = new SolidBrush(FormDesign.Design.GreenColor.MergeColor(brush.Color, 75));
					using var icon = IconManager.GetIcon(workshopInfo.HasVoted ? "VoteFilled" : "Vote", itemHeight + Padding.Top).Color(isHovered || workshopInfo.HasVoted ? greenBrush.Color : brush.Color);
					using var font = UI.Font(8.25F).FitToWidth(text, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);

					e.Graphics.DrawImage(icon, rect.Align(icon.Size, ContentAlignment.MiddleLeft));
					e.Graphics.DrawString(text, isHovered ? fontUnderline : workshopInfo.HasVoted ? fontBold : font, isHovered || workshopInfo.HasVoted ? greenBrush : brush, rect.Pad(icon.Width + Padding.Left, 0, 0, 0), stringFormat);

					e.Rects.ScoreRect = rect;
				}

				tick();

				if (workshopInfo.Subscribers >= 0)
				{
					var text2 = Locale.SubscribersCount.FormatPlural(workshopInfo.Subscribers, workshopInfo.Subscribers.ToString("N0"));
					using var subsIcon = IconManager.GetIcon("People", itemHeight + Padding.Top).Color(brush.Color);
					using var font2 = UI.Font(8.25F).FitToWidth(text2, rect.Pad(subsIcon.Width + Padding.Left, 0, 0, 0), e.Graphics);

					e.Graphics.DrawImage(subsIcon, rect.Align(subsIcon.Size, ContentAlignment.MiddleLeft));
					e.Graphics.DrawString(text2, font2, brush, rect.Pad(subsIcon.Width + Padding.Left, 0, 0, 0), stringFormat);
				}
			}

			void tick()
			{
				if (++index % 2 == 0)
				{
					rect.X += e.ClipRectangle.Width / 5;
					rect.Y = e.Rects.TextRect.Y + Padding.Top;
				}
				else
				{
					rect.Y = e.ClipRectangle.Bottom - (Padding.Bottom * 2) - rect.Height;
				}
			}
		}

		private void DrawCompatibilityAndStatusList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, NotificationType? notificationType, string? statusText, DynamicIcon? statusIcon, Color statusColor)
		{
			var height = CompactList ? ((int)(18 * UI.FontScale)) : (Math.Max(e.Rects.WorkshopRect.Y, e.Rects.FolderRect.Y) - e.ClipRectangle.Top - Padding.Vertical);

			if (notificationType > NotificationType.Info)
			{
				var point = CompactList
					? new Point(_columnSizes[Columns.Status].X, e.ClipRectangle.Y + ((e.ClipRectangle.Height - height) / 2))
					: new Point(e.ClipRectangle.Right - Padding.Right, e.ClipRectangle.Top + Padding.Top);

				e.Rects.CompatibilityRect = e.Graphics.DrawLargeLabel(
					point,
					LocaleCR.Get($"{notificationType}"),
					"CompatibilityReport",
					notificationType.Value.GetColor(),
					CompactList ? ContentAlignment.TopLeft : ContentAlignment.TopRight,
					Padding,
					height,
					CursorLocation,
					CompactList);
			}

			if (statusText is not null && statusIcon is not null)
			{
				var point = CompactList
					? new Point(notificationType > NotificationType.Info ? (e.Rects.CompatibilityRect.Right + Padding.Left) : _columnSizes[Columns.Status].X, e.ClipRectangle.Y + ((e.ClipRectangle.Height - height) / 2))
					: new Point(notificationType > NotificationType.Info ? (e.Rects.CompatibilityRect.X - Padding.Left) : e.ClipRectangle.Right - Padding.Right, e.ClipRectangle.Top + Padding.Top);

				e.Rects.DownloadStatusRect = e.Graphics.DrawLargeLabel(
					point,
					notificationType > NotificationType.Info ? "" : statusText,
					statusIcon,
					statusColor,
					CompactList ? ContentAlignment.TopLeft : ContentAlignment.TopRight,
					Padding,
					height,
					null,
					CompactList);
			}

			if (CompactList && Math.Max(e.Rects.CompatibilityRect.Right, e.Rects.DownloadStatusRect.Right) > (_columnSizes[Columns.Status].X + _columnSizes[Columns.Status].Width))
			{
				DrawSeam(e, _columnSizes[Columns.Status].X + _columnSizes[Columns.Status].Width);
			}
		}

		protected override Rectangles GenerateRectangles(IPackageIdentity item, Rectangle rectangle)
		{
			return GridView ? GenerateGridRectangles(item, rectangle) : GenerateListRectangles(item, rectangle);
		}

		private Rectangles GenerateListRectangles(IPackageIdentity item, Rectangle rectangle)
		{
			rectangle = rectangle.Pad(Padding.Left, 0, Padding.Right, 0);

			var rects = new Rectangles(item)
			{
				IconRect = CompactList ? default : rectangle.Align(new Size(rectangle.Height - Padding.Vertical, rectangle.Height - Padding.Vertical), ContentAlignment.MiddleLeft)
			};

			var includedSize = CompactList ? 26 : 30;

			rects.IncludedRect = rectangle.Pad(Padding + new Padding(0, 0, 0, (int)(1.5 * UI.FontScale))).Align(UI.Scale(new Size(includedSize, CompactList ? 18 : includedSize), UI.FontScale), ContentAlignment.MiddleLeft);

			if (CompactList)
			{
				rects.TextRect = new Rectangle(_columnSizes[Columns.PackageName].X, rectangle.Y, _columnSizes[Columns.PackageName].Width, rectangle.Height).Pad(Math.Max(rects.IncludedRect.Right, rects.EnabledRect.Right) + Padding.Horizontal, 0, 0, 0);
			}
			else
			{
				rects.IconRect.X += rects.IncludedRect.Right + Padding.Horizontal;

				using var font = UI.Font(9.75F, FontStyle.Bold);
				rects.TextRect = new Rectangle(rects.IconRect.Right + (Padding.Left * 2), rectangle.Y + Padding.Top, (rectangle.Width / UI.FontScale > 500 ? (rectangle.Width * 5 / 10) : (rectangle.Width * 7 / 10)) - rects.IconRect.Right - Padding.Left, 0).AlignToFontSize(font, ContentAlignment.TopLeft);
			}

			rects.CenterRect = rects.TextRect.Pad(-Padding.Horizontal, 0, 0, 0);

			return rects;
		}
	}
}
