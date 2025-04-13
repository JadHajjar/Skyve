using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	public partial class Complex : ItemListControl
	{
		public Complex(SkyvePage page, IPackageUtil? customPackageUtil = null) : base(page, customPackageUtil)
		{
			GridItemSize = new Size(390, 150);
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
				DrawTags(e, _columnSizes[Columns.Tags].X + _columnSizes[Columns.Tags].Width);
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
			DrawVersionAndDate(e, package, localIdentity, workshopInfo);
			DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out var activeColor);

			if (workshopInfo?.Author is not null)
			{
				DrawAuthor(e, workshopInfo.Author);
			}
			else if (e.Item.IsLocal())
			{
				DrawFolderName(e, localIdentity);
			}

			var maxTagX = DrawButtons(e, package?.LocalData, workshopInfo);

			if (!IsPackagePage)
			{
				DrawCompatibilityAndStatusList(e, notificationType, statusText, statusIcon, statusColor);
			}

			if (e.Rects.DownloadStatusRect.X > 0)
			{
				maxTagX = Math.Min(maxTagX, e.Rects.DownloadStatusRect.X);
			}
			else if (e.Rects.CompatibilityRect.X > 0)
			{
				maxTagX = Math.Min(maxTagX, e.Rects.CompatibilityRect.X);
			}

			if (e.ClipRectangle.Width > 575 * UI.FontScale)
			{
				DrawVoteAndSubscribers(e, workshopInfo);
				DrawTags(e, maxTagX);
			}

			e.Graphics.ResetClip();

			if (e.DrawableItem.Tag is not null)
			{
				using var brush = new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 175, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
			else if (!isIncluded && package?.LocalData is not null && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(Padding));
			}
		}

		private void DrawCompatibilityAndStatusList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, NotificationType? notificationType, string? statusText, DynamicIcon? statusIcon, Color statusColor)
		{
			var height = (int)((CompactList ? 22 : 24) * UI.FontScale);

			if (notificationType > NotificationType.Info)
			{
				var point = CompactList
					? new Point(_columnSizes[Columns.Status].X, e.ClipRectangle.Y + ((e.ClipRectangle.Height - height) / 2))
					: new Point(e.ClipRectangle.Right - Padding.Horizontal, e.ClipRectangle.Top + Padding.Top);

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

		protected override IDrawableItemRectangles<IPackageIdentity> GenerateRectangles(IPackageIdentity item, Rectangle rectangle, IDrawableItemRectangles<IPackageIdentity> current)
		{
			if (GridView)
			{
				return GenerateGridRectangles(item, rectangle);
			}
			else
			{
				return GenerateListRectangles(item, rectangle);
			}
		}

		private Rectangles GenerateListRectangles(IPackageIdentity item, Rectangle rectangle)
		{
			rectangle = rectangle.Pad(Padding.Left, 0, Padding.Right, 0);

			var rects = new Rectangles(item)
			{
				IconRect = CompactList ? default : rectangle.Align(new Size(rectangle.Height - Padding.Vertical, rectangle.Height - Padding.Vertical), ContentAlignment.MiddleLeft)
			};

			var includedSize = 28;

			if (_settings.UserSettings.AdvancedIncludeEnable && item.GetPackage()?.IsCodeMod == true)
			{
				rects.EnabledRect = rects.IncludedRect = rectangle.Pad(Padding).Align(new Size((int)(includedSize * UI.FontScale), CompactList ? UI.Scale(22) : (rects.IconRect.Height / 2)), ContentAlignment.MiddleLeft);

				if (CompactList)
				{
					rects.EnabledRect.X += rects.EnabledRect.Width;
				}
				else
				{
					rects.IncludedRect.Y -= rects.IncludedRect.Height / 2;
					rects.EnabledRect.Y += rects.EnabledRect.Height / 2;
				}
			}
			else
			{
				rects.IncludedRect = rectangle.Pad(Padding).Align(UI.Scale(new Size(includedSize, CompactList ? 22 : includedSize)), ContentAlignment.MiddleLeft);
			}

			if (CompactList)
			{
				rects.TextRect = new Rectangle(_columnSizes[Columns.PackageName].X, rectangle.Y, _columnSizes[Columns.PackageName].Width, rectangle.Height).Pad(Math.Max(rects.IncludedRect.Right, rects.EnabledRect.Right) + Padding.Horizontal, 0, 0, 0);
			}
			else
			{
				rects.IconRect.X += rects.IncludedRect.Right + Padding.Horizontal;

				using var font = UI.Font(9F, FontStyle.Bold);
				rects.TextRect = rectangle.Pad(rects.IconRect.Right + Padding.Left, 0, IsPackagePage ? 0 : UI.Scale(200), rectangle.Height).AlignToFontSize(font, ContentAlignment.TopLeft);
			}

			rects.CenterRect = rects.TextRect.Pad(-Padding.Horizontal, 0, 0, 0);

			return rects;
		}
	}
}