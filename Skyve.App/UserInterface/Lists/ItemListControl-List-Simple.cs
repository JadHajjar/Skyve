using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	public partial class Simple : ItemListControl
	{
		public Simple(SkyvePage page) : base(page)
		{
			GridItemSize = new Size(200, 236);
			DynamicSizing = true;
		}

		private void OnPaintItemCompactList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var package = e.Item.GetPackage();
			var localIdentity = e.Item.GetLocalPackageIdentity();
			var workshopInfo = e.Item.GetWorkshopInfo();
			var isIncluded = e.Item.IsIncluded(out var partialIncluded) || partialIncluded;
			var isEnabled = e.Item.IsEnabled();

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

			if (!isEnabled && isIncluded && !IsPackagePage && _settings.UserSettings.FadeDisabledItems && !e.HoverState.HasFlag(HoverState.Hovered))
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
			var isIncluded = e.Item.IsIncluded(out var partialIncluded) || partialIncluded;
			var isEnabled = e.Item.IsEnabled();

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

			if (!isEnabled && isIncluded && !IsPackagePage && _settings.UserSettings.FadeDisabledItems && !e.HoverState.HasFlag(HoverState.Hovered))
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

			using var font = UI.Font(8.25F);
			using var brush = new SolidBrush(e.BackColor.GetTextColor());

			var index = 0;
			var itemHeight = (int)e.Graphics.Measure(" ", font).Height;
			var rect = new Rectangle(e.Rects.TextRect.Right, e.Rects.TextRect.Y + Padding.Top, e.ClipRectangle.Width * 2 / 10, itemHeight);
			var author = workshopInfo?.Author;

			if (author?.Name is not null and not "")
			{
				var isHovered = rect.Contains(CursorLocation);

				using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor);
				using var fontUnderline = UI.Font(8.25F, FontStyle.Underline);
				using var subsIcon = IconManager.GetIcon("I_Author", itemHeight + Padding.Top).Color(isHovered ? activeBrush.Color : brush.Color);

				e.Graphics.DrawImage(subsIcon, rect.Align(subsIcon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(author.Name, isHovered ? fontUnderline : font, isHovered ? activeBrush : brush, rect.Pad(subsIcon.Width + Padding.Left, 0, 0, 0));

				e.Rects.AuthorRect = rect;
			}
			else if (localIdentity is not null)
			{
				using var subsIcon = IconManager.GetIcon("I_Folder", itemHeight + Padding.Top).Color(brush.Color);

				e.Graphics.DrawImage(subsIcon, rect.Align(subsIcon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(Path.GetFileName(localIdentity.Folder), font, brush, rect.Pad(subsIcon.Width + Padding.Left, 0, 0, 0));
			}

			tick();

			var date = workshopInfo is null || workshopInfo.ServerTime == default ? (localIdentity?.LocalTime ?? default) : workshopInfo.ServerTime;

			if (date != default)
			{
				var dateText = _settings.UserSettings.ShowDatesRelatively ? date.ToRelatedString(true, false) : date.ToString("g");
				var isRecent = date > DateTime.UtcNow.AddDays(-7) && !e.HoverState.HasFlag(HoverState.Pressed);

				using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor);
				using var subsIcon = IconManager.GetIcon("I_UpdateTime", itemHeight + Padding.Top).Color(isRecent ? activeBrush.Color : brush.Color);

				e.Graphics.DrawImage(subsIcon, rect.Align(subsIcon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(dateText, font, isRecent ? activeBrush : brush, rect.Pad(subsIcon.Width + Padding.Left, 0, 0, 0));
			}

			tick();

			if (workshopInfo is not null)
			{
				var isHovered = rect.Contains(CursorLocation);

				using var fontBold = UI.Font(8.25F, FontStyle.Bold);
				using var fontUnderline = UI.Font(8.25F, workshopInfo.HasVoted ? FontStyle.Bold | FontStyle.Underline : FontStyle.Underline);
				using var greenBrush = new SolidBrush(FormDesign.Design.GreenColor.MergeColor(brush.Color, 75));
				using var voteIcon = IconManager.GetIcon(workshopInfo.HasVoted ? "I_VoteFilled" : "I_Vote", itemHeight + Padding.Top).Color(isHovered || workshopInfo.HasVoted ? greenBrush.Color : brush.Color);

				e.Graphics.DrawImage(voteIcon, rect.Align(voteIcon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(Locale.VotesCount.FormatPlural(workshopInfo.VoteCount, workshopInfo.VoteCount.ToString("N0")), isHovered ? fontUnderline : workshopInfo.HasVoted ? fontBold : font, isHovered || workshopInfo.HasVoted ? greenBrush : brush, rect.Pad(voteIcon.Width + Padding.Left, 0, 0, 0));

				e.Rects.ScoreRect = rect;

				tick();

				using var subsIcon = IconManager.GetIcon("I_People", itemHeight + Padding.Top).Color(brush.Color);
				e.Graphics.DrawImage(subsIcon, rect.Align(subsIcon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(Locale.SubscribersCount.FormatPlural(workshopInfo.Subscribers, workshopInfo.Subscribers.ToString("N0")), font, brush, rect.Pad(subsIcon.Width + Padding.Left, 0, 0, 0));
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
			var height = CompactList ? ((int)(24 * UI.FontScale) - 4) : (Math.Max(e.Rects.SteamRect.Y, e.Rects.FolderRect.Y) - e.ClipRectangle.Top - Padding.Vertical);

			if (notificationType > NotificationType.Info)
			{
				var point = CompactList
					? new Point(_columnSizes[Columns.Status].X, e.ClipRectangle.Y + ((e.ClipRectangle.Height - height) / 2))
					: new Point(e.ClipRectangle.Right - Padding.Horizontal, e.ClipRectangle.Top + Padding.Top);

				e.Rects.CompatibilityRect = e.Graphics.DrawLargeLabel(
					point,
					LocaleCR.Get($"{notificationType}"),
					"I_CompatibilityReport",
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
					: new Point(notificationType > NotificationType.Info ? (e.Rects.CompatibilityRect.X - Padding.Left) : e.ClipRectangle.Right - Padding.Horizontal, e.ClipRectangle.Top + Padding.Top);

				e.Rects.DownloadStatusRect = e.Graphics.DrawLargeLabel(
					point,
					notificationType > NotificationType.Info ? "" : statusText,
					statusIcon,
					statusColor,
					CompactList ? ContentAlignment.TopLeft : ContentAlignment.TopRight,
					Padding,
					height,
					CursorLocation,
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
