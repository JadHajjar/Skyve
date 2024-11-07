

using Skyve.Compatibility.Domain.Enums;

using SlickControls;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	public partial class Complex : ItemListControl
	{
		protected override void OnPaintItemGrid(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var package = e.Item.GetPackage();
			var localIdentity = e.Item.GetLocalPackageIdentity();
			var workshopInfo = e.Item.GetWorkshopInfo();
			var isIncluded = _packageUtil.IsIncluded(e.Item, out var partialIncluded, SelectedPlayset) || partialIncluded;
			var isEnabled = _packageUtil.IsEnabled(e.Item, SelectedPlayset);

			if (e.IsSelected)
			{
				e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
			}

			if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)))
			{
				e.BackColor = (e.IsSelected ? e.BackColor : FormDesign.Design.AccentBackColor).MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);
			}

			base.OnPaintItemGrid(e);

			DrawThumbnail(e, localIdentity, workshopInfo);
			DrawTitleAndTags(e);
			DrawVersionAndDate(e, package, localIdentity, workshopInfo);
			DrawVoteAndSubscribers(e, workshopInfo);
			DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, localIdentity, out var activeColor);

			if (workshopInfo?.Author is not null)
			{
				DrawAuthor(e, workshopInfo.Author);
			}
			else if (e.Item.IsLocal())
			{
				DrawFolderName(e, localIdentity);
			}

			DrawDividerLine(e);

			var maxTagX = DrawButtons(e, package?.LocalData, workshopInfo);

			DrawTags(e, maxTagX);

			e.Graphics.ResetClip();

			DrawCompatibilityAndStatus(e, out var outerColor);

			if (e.DrawableItem.Tag is not null)
			{
				using var brush = new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 50 : 175, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
			else if (isEnabled)
			{
				if (outerColor == default)
				{
					outerColor = Color.FromArgb(FormDesign.Design.IsDarkTheme ? 65 : 100, activeColor);
				}

				using var pen = new Pen(outerColor, (float)(1.5 * UI.FontScale));

				e.Graphics.DrawRoundedRectangle(pen, e.ClipRectangle.InvertPad(GridPadding - new Padding((int)pen.Width)), UI.Scale(5));
			}
			else if (isIncluded && !IsPackagePage && _settings.UserSettings.FadeDisabledItems && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
		}

		private void DrawCompatibilityAndStatus(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, out Color outerColor)
		{
			var compatibilityReport = e.Item.GetCompatibilityInfo();
			var notificationType = compatibilityReport?.GetNotification();
			outerColor = default;

			var height = GridView ? e.Rects.IncludedRect.Height : (e.Rects.IconRect.Bottom - Math.Max(e.Rects.TextRect.Bottom, Math.Max(e.Rects.VersionRect.Bottom, e.Rects.DateRect.Bottom)) - GridPadding.Bottom);

			if (notificationType > NotificationType.Info)
			{
				outerColor = notificationType.Value.GetColor();

				e.Rects.CompatibilityRect = e.Graphics.DrawLargeLabel(new(e.ClipRectangle.Right - GridPadding.Right, e.Rects.IconRect.Bottom), LocaleCR.Get($"{notificationType}"), "CompatibilityReport", outerColor, ContentAlignment.BottomRight, padding: GridPadding, height: height, cursorLocation: CursorLocation);
			}

			if (GetStatusDescriptors(e.Item, out var text, out var icon, out var color))
			{
				if (!(notificationType > NotificationType.Info))
				{
					outerColor = color;
				}

				e.Rects.DownloadStatusRect = e.Graphics.DrawLargeLabel(new(notificationType > NotificationType.Info ? (e.Rects.CompatibilityRect.X - GridPadding.Left) : (e.ClipRectangle.Right - GridPadding.Right), e.Rects.IconRect.Bottom), notificationType > NotificationType.Info ? "" : text, icon!, color, ContentAlignment.BottomRight, padding: GridPadding, height: height, cursorLocation: CursorLocation);
			}

			if (e.IsSelected && outerColor == default)
			{
				outerColor = FormDesign.Design.GreenColor;
			}
		}

		private void DrawTags(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, int maxTagX)
		{
			Rectangle tagsRect;

			if (GridView)
			{
				tagsRect = new(e.ClipRectangle.X, e.Rects.IconRect.Bottom + (GridPadding.Top * 3), maxTagX - e.ClipRectangle.X, e.ClipRectangle.Bottom - e.Rects.IconRect.Bottom - (GridPadding.Top * 3));
			}
			else if (CompactList)
			{
				tagsRect = new(_columnSizes[Columns.Tags].X, e.ClipRectangle.Y, _columnSizes[Columns.Tags].Width, e.ClipRectangle.Height);
			}
			else
			{
				tagsRect = new(e.ClipRectangle.Width * 5 / 10, e.Rects.TextRect.Bottom + UI.Scale(20) + (Padding.Bottom * 2), maxTagX - (e.ClipRectangle.Width * 4 / 10), UI.Scale(20));
			}

			var location = tagsRect.Location;

			if (GridView)
			{
				e.Graphics.SetClip(new Rectangle(tagsRect.X, tagsRect.Y, maxTagX - tagsRect.X, e.ClipRectangle.Bottom - tagsRect.Y));
			}
			else
			{
				e.Graphics.SetClip(new Rectangle(tagsRect.X, e.ClipRectangle.Y, maxTagX - tagsRect.X, e.ClipRectangle.Height));
			}

			if (!IsPackagePage && e.Item.Id > 0)
			{
				DrawTag(e, ref location, tagsRect, _tagsService.CreateIdTag(e.Item.Id.ToString()), FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor));

				e.Rects.SteamIdRect = e.Rects.TagRects.First().Value;

				location.X += (GridView ? GridPadding : Padding).Left;
			}

			var endReached = false;

			foreach (var item in e.Item.GetTags(IsPackagePage))
			{
				if (endReached = DrawTag(e, ref location, tagsRect, item))
				{
					break;
				}
			}

			if (!endReached)
			{
				return;
			}

			if (CompactList)
			{
				using var backBrush = new SolidBrush(e.BackColor);
				e.Graphics.FillRectangle(backBrush, e.ClipRectangle.Pad(_columnSizes[Columns.Tags].X + _columnSizes[Columns.Tags].Width, 0, 0, 0));

				DrawSeam(e, _columnSizes[Columns.Tags].X + _columnSizes[Columns.Tags].Width);
			}
			else if (IsPackagePage)
			{
				var seamRectangle = new Rectangle(maxTagX - (int)(40 * UI.UIScale), e.Rects.TextRect.Bottom, (int)(40 * UI.UIScale), e.ClipRectangle.Height);

				using var seamBrush = new LinearGradientBrush(seamRectangle, Color.Empty, e.BackColor, 0F);

				e.Graphics.FillRectangle(seamBrush, seamRectangle);
			}
			else
			{
				DrawSeam(e, maxTagX);
			}
		}

		private void DrawDividerLine(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var lineRect = new Rectangle(e.ClipRectangle.X, e.Rects.IconRect.Bottom + GridPadding.Vertical, e.ClipRectangle.Width, UI.Scale(2));
			using var lineBrush = new LinearGradientBrush(lineRect, default, default, 0F);

			lineBrush.InterpolationColors = new ColorBlend
			{
				Colors = new[] { Color.Empty, FormDesign.Design.AccentColor, FormDesign.Design.AccentColor, Color.Empty, Color.Empty },
				Positions = new[] { 0.0f, 0.15f, 0.6f, 0.75f, 1f }
			};

			e.Graphics.FillRectangle(lineBrush, lineRect);
		}

		private bool DrawTag(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ref Point location, Rectangle tagsRect, ITag item, Color? color = null)
		{
			using var tagIcon = IconManager.GetSmallIcon(item.Icon);

			var tagSize = e.Graphics.MeasureLabel(item.Value, tagIcon);

			if (!tagsRect.Contains(new Rectangle(location, tagSize)))
			{
				if (GridView)
				{
					location.Y += tagSize.Height + GridPadding.Top;
					location.X = e.ClipRectangle.X;
				}
				else
				{
					return true;
				}
			}

			var tagRect = e.Graphics.DrawLabel(item.Value, tagIcon, color ?? Color.FromArgb(200, FormDesign.Design.LabelColor.MergeColor(FormDesign.Design.AccentBackColor, 40)), new Rectangle(location.X, tagsRect.Y, tagsRect.Width, tagsRect.Height), CompactList ? ContentAlignment.MiddleLeft : ContentAlignment.TopLeft, mousePosition: CursorLocation);

			location.X += (GridView ? GridPadding : Padding).Left + tagRect.Width;

			e.Rects.TagRects[item] = tagRect;

			return tagsRect.Right > location.X;
		}

		private int DrawFolderName(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageIdentity? localIdentity)
		{
			if (localIdentity is null)
			{
				return 0;
			}

			var rect = new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Bottom, 0, 0);

			if (GridView)
			{
				rect.Y += UI.Scale(40) + (GridPadding.Bottom / 2);
			}
			else
			{
				rect.Y += UI.Scale(20) + (Padding.Bottom * 2);
			}

			using var authorIcon = IconManager.GetSmallIcon("Folder");

			e.Graphics.DrawLabel(Path.GetFileNameWithoutExtension(localIdentity.FilePath), authorIcon, default, rect, ContentAlignment.TopLeft);

			return 0;
		}

		private int DrawAuthor(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IUser author)
		{
			var padding = GridView ? GridPadding : Padding;
			var authorRect = new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Bottom, 0, 0);
			var authorImg = _workshopService.GetUser(author)?.GetThumbnail();

			if (GridView)
			{
				authorRect.Y += UI.Scale(40) + (GridPadding.Bottom / 2);
			}
			else
			{
				authorRect.Y += UI.Scale(20) + (Padding.Bottom * 2);
			}

			if (authorImg is null)
			{
				using var authorIcon = IconManager.GetSmallIcon("Author");

				authorRect = e.Graphics.DrawLabel(author.Name, authorIcon, default, authorRect, ContentAlignment.TopLeft, mousePosition: CursorLocation);
			}
			else
			{
				using var smallImage = new Bitmap(authorImg, authorImg.Size.GetProportionalDownscaledSize(IconManager.GetSmallScale()));
				authorRect = e.Graphics.DrawLabel(author.Name, smallImage, default, authorRect, ContentAlignment.TopLeft, mousePosition: CursorLocation, recolor: false);
			}

			if (_userService.IsUserVerified(author))
			{
				var avatarRect = authorRect.Pad(padding).Align(CompactList ? UI.Scale(new Size(18, 18)) : new(authorRect.Height * 3 / 4, authorRect.Height * 3 / 4), ContentAlignment.MiddleLeft);
				var checkRect = avatarRect.Align(new Size(avatarRect.Height / 3, avatarRect.Height / 3), ContentAlignment.BottomRight);

				e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect.Pad(-UI.Scale(2)));

				using var img = IconManager.GetIcon("Check", checkRect.Height);

				e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
			}

			e.Rects.AuthorRect = authorRect;

			return authorRect.Width;
		}

		private void DrawVersionAndDate(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IPackage? package, ILocalPackageIdentity? localPackageIdentity, IWorkshopInfo? workshopInfo)
		{
#if CS1
			var isVersion = localParentPackage?.Mod is not null && !e.Item.IsBuiltIn && !IsPackagePage;
			var versionText = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.IsBuiltIn ? Locale.Vanilla : e.Item is ILocalPackageData lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString();
#else
			var isVersion = (package?.IsCodeMod ?? workshopInfo?.IsCodeMod ?? false);
			var versionText = isVersion ? "v" + package?.VersionName ?? (workshopInfo?.Changelog.FirstOrDefault(x => x.VersionId == package?.Version)?.Version) : null;
			versionText = versionText is not null ? $"v{versionText}" : localPackageIdentity != null ? localPackageIdentity.FileSize.SizeString(0) : workshopInfo?.ServerSize.SizeString(0);
#endif
			var tagRect = new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Bottom + (GridView ? GridPadding.Bottom / 2 : (Padding.Bottom * 2)), 0, 0);

			var rect = e.Graphics.DrawLabel(versionText, null, isVersion ? Color.FromArgb(125, FormDesign.Design.YellowColor) : default, tagRect, ContentAlignment.TopLeft, smaller: false);
			tagRect.X += (GridView ? GridPadding.Left : Padding.Left) + rect.Width;

			var date = workshopInfo is null || workshopInfo.ServerTime == default ? (localPackageIdentity?.LocalTime ?? default) : workshopInfo.ServerTime;

			if (date != default)
			{
				var dateText = _settings.UserSettings.ShowDatesRelatively ? date.ToLocalTime().ToRelatedString(true, false) : date.ToLocalTime().ToString("g");
				var isRecent = date > DateTime.UtcNow.AddDays(-7);

				e.Graphics.DrawLabel(dateText, IconManager.GetSmallIcon("UpdateTime"), isRecent ? Color.FromArgb(125, FormDesign.Design.ActiveColor) : default, tagRect, ContentAlignment.TopLeft, smaller: false);
			}
		}

		private void DrawVoteAndSubscribers(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IWorkshopInfo? workshopInfo)
		{
			if (workshopInfo is null)
			{
				return;
			}

			var rect = GridView
				? new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Bottom + UI.Scale(20) + (GridPadding.Bottom / 2), 0, 0)
				: new Rectangle(e.ClipRectangle.Width * 5 / 10, e.Rects.TextRect.Bottom + (Padding.Bottom * 2), 0, 0);

			e.Rects.ScoreRect = e.Graphics.DrawLabel(Locale.VotesCount.FormatPlural(workshopInfo.VoteCount, workshopInfo.VoteCount.ToString("N0"))
					, IconManager.GetSmallIcon(workshopInfo.HasVoted ? "VoteFilled" : "Vote")
					, workshopInfo!.HasVoted ? FormDesign.Design.GreenColor : default
					, rect
					, ContentAlignment.TopLeft
					, mousePosition: CursorLocation);

			rect.X += GridPadding.Left + e.Rects.ScoreRect.Width;

			e.Graphics.DrawLabel(Locale.SubscribersCount.FormatPlural(workshopInfo.Subscribers, workshopInfo.Subscribers.ToString("N0"))
					, IconManager.GetSmallIcon("People")
					, default
					, rect
					, ContentAlignment.TopLeft);
		}

		private Rectangles GenerateGridRectangles(IPackageIdentity item, Rectangle rectangle)
		{
			var rects = new Rectangles(item)
			{
				IconRect = rectangle.Pad(GridPadding).Align(UI.Scale(new Size(80, 80)), ContentAlignment.TopLeft)
			};

			using var font = UI.Font(11.25F, FontStyle.Bold);
			rects.TextRect = rectangle.Pad(rects.IconRect.Width + (GridPadding.Left * 2), GridPadding.Top, GridPadding.Right, rectangle.Height).AlignToFontSize(font, ContentAlignment.TopLeft);

			rects.IncludedRect = rects.TextRect.Align(UI.Scale(new Size(28, 28)), ContentAlignment.TopRight);

			if (_settings.UserSettings.AdvancedIncludeEnable && item.GetPackage()?.IsCodeMod == true)
			{
				rects.EnabledRect = rects.IncludedRect;

				rects.IncludedRect.X -= rects.IncludedRect.Width;
			}

			rects.TextRect.Width = rects.IncludedRect.X - rects.TextRect.X;

			rects.CenterRect = rects.TextRect.Pad(-GridPadding.Horizontal, 0, 0, 0);

			return rects;
		}

		protected override void OnViewChanged()
		{
			base.OnViewChanged();

			if (GridView)
			{
				GridPadding = UI.Scale(new Padding(4), UI.UIScale);
			}
		}
	}
}