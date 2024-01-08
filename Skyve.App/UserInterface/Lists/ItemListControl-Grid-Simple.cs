using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	public partial class Simple : ItemListControl
	{
		protected override void OnPaintItemGrid(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var package = e.Item.GetPackage();
			var workshopInfo = e.Item.GetWorkshopInfo();
			var isPressed = false;
			var isIncluded = e.Item.IsIncluded(out var partialIncluded) || partialIncluded;
			var isEnabled = e.Item.IsEnabled();

			e.BackColor = BackColor;

			if (e.IsSelected)
			{
				e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
			}

			if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)))
			{
				//e.BackColor = (e.IsSelected ? e.BackColor : FormDesign.Design.AccentBackColor).MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);

				isPressed = e.HoverState.HasFlag(HoverState.Pressed);
			}

			base.OnPaintItemGrid(e);

			e.DrawableItem.CachedHeight = e.Rects.IncludedRect.Bottom - e.ClipRectangle.Y + GridPadding.Vertical + Padding.Vertical;

			DrawThumbnail(e);
			DrawTitleAndTagsAndVersion(e, package, e.Item.GetLocalPackageIdentity(), workshopInfo, isPressed);
			DrawIncludedButton(e, isIncluded, partialIncluded, isEnabled, package?.LocalData, out var activeColor);
			DrawDots(e);
			DrawCompatibilityAndStatus(e, out var outerColor);

			if (outerColor != default)
			{
				using var pen = new Pen(outerColor, (float)(1.5 * UI.FontScale));

				e.Graphics.DrawRoundedRectangle(pen, e.ClipRectangle.InvertPad(GridPadding - new Padding((int)pen.Width)), (int)(5 * UI.FontScale));
			}

			if (!isEnabled && isIncluded && !IsPackagePage && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
				e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
			}
		}

		private void DrawDots(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
		{
			var isHovered = e.Rects.DotsRect.Contains(CursorLocation);
			using var img = IconManager.GetIcon("I_VertialMore", e.Rects.DotsRect.Height).Color(isHovered?FormDesign.Design.ActiveColor:FormDesign.Design.IconColor);

			e.Graphics.DrawImage(img, e.Rects.DotsRect.CenterR(img.Size));
		}

		private void DrawCompatibilityAndStatus(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, out Color outerColor)
		{
			var compatibilityReport = e.Item.GetCompatibilityInfo();
			var notificationType = compatibilityReport?.GetNotification();
			outerColor = default;

			var height = (int)(20 * UI.FontScale);

			if (GetStatusDescriptors(e.Item, out var text, out var icon, out var color))
			{
				var rect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y + e.DrawableItem.CachedHeight - Padding.Vertical, e.ClipRectangle.Width, height);
				
				outerColor = Color.FromArgb(rect.Contains(CursorLocation) ? 200 : 255, color);

				using var brush = new SolidBrush(outerColor);
				using var textBrush = new SolidBrush(outerColor.GetTextColor());
				using var font = UI.Font(9F, FontStyle.Bold);
				using var image = icon!.Get(height * 3 / 4).Color(textBrush.Color);

				e.Graphics.FillRoundedRectangle(brush, rect.Pad(-GridPadding.Left + (int)(1.5 * UI.FontScale)), (int)(5 * UI.FontScale), false, false, notificationType <= NotificationType.Info, notificationType <= NotificationType.Info);

				e.Graphics.DrawString(text, font, textBrush, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

				e.Graphics.DrawImage(image, rect.Pad(GridPadding).Align(image.Size, ContentAlignment.MiddleLeft));

				e.Rects.DownloadStatusRect = rect;

				e.DrawableItem.CachedHeight += height + GridPadding.Vertical;

				if (notificationType > NotificationType.Info)
					e.DrawableItem.CachedHeight -= GridPadding.Top;
			}

			if (notificationType > NotificationType.Info)
			{
				var rect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y + e.DrawableItem.CachedHeight - Padding.Vertical, e.ClipRectangle.Width, height);

				outerColor = Color.FromArgb(rect.Contains(CursorLocation) ? 200 : 255, notificationType.Value.GetColor());

				using var brush = new SolidBrush(outerColor);
				using var textBrush = new SolidBrush(outerColor.GetTextColor());
				using var font = UI.Font(9F, FontStyle.Bold);
				using var image = notificationType.Value.GetIcon(false).Get(height * 3 / 4).Color(textBrush.Color);

				e.Graphics.FillRoundedRectangle(brush, rect.Pad(-GridPadding.Left + (int)(1.5 * UI.FontScale)), (int)(5 * UI.FontScale), false, false);

				e.Graphics.DrawString(LocaleCR.Get($"{notificationType}"), font, textBrush, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

				e.Graphics.DrawImage(image, rect.Pad(GridPadding).Align(image.Size, ContentAlignment.MiddleLeft));

				e.Rects.CompatibilityRect = rect;

				e.DrawableItem.CachedHeight += height + GridPadding.Vertical;
			}

			if (e.IsSelected && outerColor == default)
			{
				outerColor = FormDesign.Design.GreenColor;
			}
		}

		private void DrawTags(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, int maxTagX)
		{
			var startLocation = GridView
				? new Point(e.ClipRectangle.X, e.Rects.IconRect.Bottom + (GridPadding.Vertical * 2))
				: IsPackagePage ? new Point(e.Rects.TextRect.X - Padding.Left, e.ClipRectangle.Bottom)
				: new Point(CompactList ? _columnSizes[Columns.Tags].X : (e.ClipRectangle.X + (int)(375 * UI.UIScale)), e.ClipRectangle.Bottom - (CompactList ? 0 : Padding.Bottom));
			var tagsRect = new Rectangle(startLocation, default);

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
				e.Rects.SteamIdRect = DrawTag(e, maxTagX, startLocation, ref tagsRect, _tagsService.CreateWorkshopTag(e.Item.Id.ToString()), FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor));

				tagsRect.X += Padding.Left;
			}

			foreach (var item in e.Item.GetTags(IsPackagePage))
			{
				DrawTag(e, maxTagX, startLocation, ref tagsRect, item);
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
			var lineRect = new Rectangle(e.ClipRectangle.X, e.Rects.IconRect.Bottom + GridPadding.Vertical, e.ClipRectangle.Width, (int)(2 * UI.FontScale));
			using var lineBrush = new LinearGradientBrush(lineRect, default, default, 0F);

			lineBrush.InterpolationColors = new ColorBlend
			{
				Colors = new[] { Color.Empty, FormDesign.Design.AccentColor, FormDesign.Design.AccentColor, Color.Empty, Color.Empty },
				Positions = new[] { 0.0f, 0.15f, 0.6f, 0.75f, 1f }
			};

			e.Graphics.FillRectangle(lineBrush, lineRect);
		}

		private Rectangle DrawTag(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, int maxTagX, Point startLocation, ref Rectangle tagsRect, ITag item, Color? color = null)
		{
			using var tagIcon = IconManager.GetSmallIcon(item.Icon);

			var padding = GridView ? GridPadding : Padding;
			var tagSize = e.Graphics.MeasureLabel(item.Value, tagIcon, large: GridView);
			var tagRect = e.Graphics.DrawLabel(item.Value, tagIcon, color ?? Color.FromArgb(200, FormDesign.Design.LabelColor.MergeColor(FormDesign.Design.AccentBackColor, 40)), tagsRect, GridView ? ContentAlignment.TopLeft : ContentAlignment.BottomLeft, smaller: CompactList, large: GridView, mousePosition: CursorLocation);

			e.Rects.TagRects[item] = tagRect;

			tagsRect.X += padding.Left + tagRect.Width;

			if (tagsRect.X + tagSize.Width + (int)(25 * UI.UIScale) > maxTagX)
			{
				if (IsPackagePage)
				{
					return tagRect;
				}

				tagsRect.X = startLocation.X;
				tagsRect.Y += (GridView ? 1 : -1) * (tagRect.Height + padding.Top);
			}

			return tagRect;
		}

		private int DrawButtons(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, bool isPressed, ILocalPackageData? parentPackage, IWorkshopInfo? workshopInfo)
		{
			var padding = GridView ? GridPadding : Padding;
			var size = UI.Scale(CompactList ? new Size(24, 24) : new Size(28, 28), UI.FontScale);
			var rect = new Rectangle(e.ClipRectangle.Right - size.Width - (GridView ? 0 : Padding.Right), CompactList ? (e.ClipRectangle.Y + ((e.ClipRectangle.Height - size.Height) / 2)) : (e.ClipRectangle.Bottom - size.Height), size.Width, size.Height);
			var backColor = Color.FromArgb(175, GridView ? FormDesign.Design.BackColor : FormDesign.Design.ButtonColor);

			if (parentPackage is not null)
			{
				using var icon = IconManager.GetIcon("I_Folder", size.Height * 3 / 4);

				SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

				e.Rects.FolderRect = rect;

				rect.X -= rect.Width + padding.Left;
			}

			if (!IsPackagePage && workshopInfo?.Url is not null)
			{
#if CS2
				using var icon = IconManager.GetIcon("I_Paradox", rect.Height * 3 / 4);
#else
				using var icon = IconManager.GetIcon("I_Steam", rect.Height * 3 / 4);
#endif

				SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

				e.Rects.SteamRect = rect;

				rect.X -= rect.Width + padding.Left;
			}

			if (!IsPackagePage && _compatibilityManager.GetPackageInfo(e.Item)?.Links?.FirstOrDefault(x => x.Type == LinkType.Github) is ILink gitLink)
			{
				using var icon = IconManager.GetIcon("I_Github", rect.Height * 3 / 4);

				SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

				e.Rects.GithubRect = rect;

				rect.X -= rect.Width + padding.Left;
			}

			return rect.X + rect.Width;
		}

		private int DrawFolderName(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageData? package)
		{
			if (package is null)
			{
				return 0;
			}

			if (CompactList)
			{
				e.Rects.FolderNameRect = DrawCell(e, Columns.Author, Path.GetFileName(package.Folder), "I_Folder", font: UI.Font(8.25F, FontStyle.Bold));
				return 0;
			}

			var padding = GridView ? GridPadding : Padding;
			var height = e.Rects.IconRect.Bottom - Math.Max(e.Rects.TextRect.Bottom, Math.Max(e.Rects.VersionRect.Bottom, e.Rects.DateRect.Bottom)) - padding.Bottom;
			var folderPoint = CompactList ? new Point(_columnSizes[Columns.Author].X, e.ClipRectangle.Y) : new Point(e.Rects.TextRect.X, e.Rects.IconRect.Bottom);

			e.Rects.FolderNameRect = e.Graphics.DrawLargeLabel(folderPoint, Path.GetFileName(package.Folder), "I_Folder", alignment: ContentAlignment.BottomLeft, padding: GridView ? GridPadding : Padding, height: height, cursorLocation: CursorLocation);

			return e.Rects.FolderNameRect.Width;
		}

		private int DrawAuthor(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IUser author)
		{
			var padding = GridView ? GridPadding : Padding;
			var authorRect = new Rectangle(e.Rects.TextRect.X, e.Rects.IconRect.Bottom, 0, 0);
			var authorImg = author.GetUserAvatar();

			var height = CompactList ? authorRect.Height : e.Rects.IconRect.Bottom - Math.Max(e.Rects.TextRect.Bottom, Math.Max(e.Rects.VersionRect.Bottom, e.Rects.DateRect.Bottom)) - padding.Bottom;

			if (CompactList)
			{
				if (authorImg is null)
				{
					authorRect = DrawCell(e, Columns.Author, author.Name, "I_Author", font: UI.Font(8.25F, FontStyle.Bold));
				}
				else
				{
					authorRect = DrawCell(e, Columns.Author, author.Name, null, font: UI.Font(8.25F, FontStyle.Bold), padding: new Padding((int)(20 * UI.FontScale), 0, 0, 0));

					e.Graphics.DrawRoundImage(authorImg, authorRect.Pad(Padding).Align(UI.Scale(new Size(18, 18), UI.FontScale), ContentAlignment.MiddleLeft));
				}
			}
			else if (authorImg is null)
			{
				using var authorIcon = IconManager.GetIcon("I_Author", height);

				authorRect = e.Graphics.DrawLargeLabel(authorRect.Location, author.Name, authorIcon, alignment: ContentAlignment.BottomLeft, padding: padding, height: height, cursorLocation: CursorLocation);
			}
			else
			{
				authorRect = e.Graphics.DrawLargeLabel(authorRect.Location, author.Name, authorImg, alignment: ContentAlignment.BottomLeft, padding: padding, height: height, cursorLocation: CursorLocation);
			}

			if (_compatibilityManager.IsUserVerified(author))
			{
				var avatarRect = authorRect.Pad(padding).Align(CompactList ? UI.Scale(new Size(18, 18), UI.FontScale) : new(authorRect.Height * 3 / 4, authorRect.Height * 3 / 4), ContentAlignment.MiddleLeft);
				var checkRect = avatarRect.Align(new Size(avatarRect.Height / 3, avatarRect.Height / 3), ContentAlignment.BottomRight);

				e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect.Pad(-(int)(2 * UI.FontScale)));

				using var img = IconManager.GetIcon("I_Check", checkRect.Height);

				e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
			}

			e.Rects.AuthorRect = authorRect;

			return authorRect.Width;
		}

		private void DrawTitleAndTagsAndVersionForList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageData? localParentPackage, IWorkshopInfo? workshopInfo, bool isPressed)
		{
			using var font = UI.Font(GridView ? 10.5F : CompactList ? 8.25F : 9F, FontStyle.Bold);
			var mod = e.Item is not IAsset;
			var tags = new List<(Color Color, string Text)>();
			var text = mod ? e.Item.CleanName(out tags) : e.Item.ToString();
			using var brush = new SolidBrush(isPressed ? FormDesign.Design.ActiveForeColor : (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)) && e.HoverState.HasFlag(HoverState.Hovered) && !IsPackagePage ? FormDesign.Design.ActiveColor : ForeColor);
			e.Graphics.DrawString(text, font, brush, e.Rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = CompactList ? StringAlignment.Center : StringAlignment.Near });

#if CS1
		var isVersion = localParentPackage?.Mod is not null && !e.Item.IsBuiltIn && !IsPackagePage;
		var versionText = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.IsBuiltIn ? Locale.Vanilla : e.Item is ILocalPackageData lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString();
#else
			var isVersion = !string.IsNullOrWhiteSpace(localParentPackage?.Version);
			var versionText = isVersion ? "v" + localParentPackage!.Version : e.Item is ILocalPackageData lp ? lp.FileSize.SizeString() : workshopInfo?.ServerSize.SizeString();
#endif
			var date = workshopInfo?.ServerTime ?? e.Item.GetLocalPackage()?.LocalTime;

			var padding = GridView ? GridPadding : Padding;
			var textSize = e.Graphics.Measure(text, font);
			var tagRect = new Rectangle(e.Rects.TextRect.X + (int)textSize.Width, e.Rects.TextRect.Y, 0, e.Rects.TextRect.Height);

			for (var i = 0; i < tags.Count; i++)
			{
				var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, smaller: true);

				if (i == 0 && !string.IsNullOrEmpty(versionText))
				{
					e.Rects.VersionRect = rect;
				}

				tagRect.X += padding.Left + rect.Width;
			}

			if (CompactList)
			{
				var packageCol = _columnSizes[Columns.PackageName];
				using var backBrush = new SolidBrush(e.BackColor);
				e.Graphics.FillRectangle(backBrush, e.ClipRectangle.Pad(packageCol.X + packageCol.Width, 0, 0, 0));

				if (tagRect.X > packageCol.X + packageCol.Width)
				{
					DrawSeam(e, packageCol.X + packageCol.Width);
				}

				e.Rects.TextRect.Width = packageCol.Width - e.Rects.TextRect.X;
				e.Rects.CenterRect = e.Rects.TextRect;

				if (!string.IsNullOrEmpty(versionText))
				{
					e.Rects.VersionRect = DrawCell(e, Columns.Version, versionText!, null, isVersion ? FormDesign.Design.YellowColor : FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.AccentBackColor, 40), active: isVersion);
				}

				if (date.HasValue && !IsPackagePage)
				{
					var dateText = _settings.UserSettings.ShowDatesRelatively ? date.Value.ToRelatedString(true, false) : date.Value.ToString("g");

					e.Rects.DateRect = DrawCell(e, Columns.UpdateTime, dateText, null);
				}

				return;
			}

			tagRect = new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Bottom + padding.Bottom, 0, 0);

			if (!string.IsNullOrEmpty(versionText))
			{
				e.Rects.VersionRect = e.Graphics.DrawLabel(versionText, null, isVersion ? FormDesign.Design.YellowColor : FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.AccentBackColor, 40), tagRect, ContentAlignment.TopLeft, smaller: true, mousePosition: isVersion ? CursorLocation : null);

				tagRect.X += padding.Left + e.Rects.VersionRect.Width;
			}

			if (date.HasValue && !IsPackagePage)
			{
				var dateText = _settings.UserSettings.ShowDatesRelatively ? date.Value.ToRelatedString(true, false) : date.Value.ToString("g");

				e.Rects.DateRect = e.Graphics.DrawLabel(dateText, IconManager.GetSmallIcon("I_UpdateTime"), FormDesign.Design.AccentColor, tagRect, ContentAlignment.TopLeft, smaller: true, mousePosition: CursorLocation);
			}
		}

		private Rectangle DrawCell(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, Columns column, string text, DynamicIcon? dIcon, Color? backColor = null, Font? font = null, bool active = true, Padding padding = default)
		{
			var cell = _columnSizes[column];
			var rect = new Rectangle(cell.X, e.ClipRectangle.Y, cell.Width, e.ClipRectangle.Height).Pad(0, -Padding.Top, 0, -Padding.Bottom);

			if (active && rect.Contains(CursorLocation))
			{
				using var brush = new SolidBrush(Color.FromArgb(200, backColor ??= FormDesign.Design.ActiveColor));
				e.Graphics.FillRectangle(brush, rect);
			}

			var textColor = backColor?.GetTextColor() ?? e.BackColor.GetTextColor();

			if (dIcon != null)
			{
				using var icon = dIcon.Small.Color(textColor);

				e.Graphics.DrawImage(icon, rect.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleLeft));

				rect = rect.Pad(icon.Width + Padding.Left, 0, 0, 0);
			}

			using (var brush = new SolidBrush(textColor))
			using (font ??= UI.Font(7.5F))
			{
				var textRect = rect.Pad(padding + Padding).AlignToFontSize(font, ContentAlignment.MiddleLeft, e.Graphics);

				e.Graphics.DrawString(text, font, brush, textRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

				if (e.Graphics.Measure(text, font).Width <= rect.Right - textRect.X)
				{
					return rect;
				}
			}

			using var backBrush = new SolidBrush(e.BackColor);
			e.Graphics.FillRectangle(backBrush, e.ClipRectangle.Pad(rect.Right, 0, 0, 0));

			DrawSeam(e, rect.Right);

			return rect;
		}

		private static void DrawSeam(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, int x)
		{
			var seamRectangle = new Rectangle(x - (int)(40 * UI.UIScale), e.ClipRectangle.Y, (int)(40 * UI.UIScale), e.ClipRectangle.Height);

			using var seamBrush = new LinearGradientBrush(seamRectangle, Color.Empty, e.BackColor, 0F);

			e.Graphics.FillRectangle(seamBrush, seamRectangle);
		}

		private void DrawTitleAndTagsAndVersion(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IPackage? package, ILocalPackageIdentity? localParentPackage, IWorkshopInfo? workshopInfo, bool isPressed)
		{
			using var font = UI.Font(GridView ? 9F : CompactList ? 8.25F : 9F, FontStyle.Bold);
			var mod = e.Item is not IAsset;
			var tags = new List<(Color Color, string Text)>();
			var text = mod ? e.Item.CleanName(out tags) : e.Item.ToString();
			using var brushTitle = new SolidBrush((e.Rects.CenterRect.Contains(CursorLocation)) && e.HoverState.HasFlag(HoverState.Hovered) && !IsPackagePage ? FormDesign.Design.ActiveColor : ForeColor);
			using var brush = new SolidBrush(ForeColor);
			e.Graphics.DrawString(text, font, brushTitle, e.Rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = CompactList ? StringAlignment.Center : StringAlignment.Near });

#if CS1
		var isVersion = localParentPackage?.Mod is not null && !e.Item.IsBuiltIn && !IsPackagePage;
		var versionText = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.IsBuiltIn ? Locale.Vanilla : e.Item is ILocalPackageData lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString();
#else
			var isVersion = package?.IsCodeMod ?? false && !string.IsNullOrEmpty(package!.Version);
			var versionText = isVersion ? "v" + package!.Version : localParentPackage?.FileSize.SizeString(0) ?? workshopInfo?.ServerSize.SizeString(0);
#endif
			var date = localParentPackage?.LocalTime ?? default;
			var isRecent = date > DateTime.UtcNow.AddDays(-7);

			var padding = GridView ? GridPadding : Padding;
			var textSize = e.Graphics.Measure(text, font);
			var tagRect = new Rectangle(e.Rects.TextRect.X + (int)textSize.Width, e.Rects.TextRect.Y, 0, e.Rects.TextRect.Height);

			for (var i = 0; i < tags.Count; i++)
			{
				var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, smaller: true);

				tagRect.X += padding.Left + rect.Width;
			}

			var packageTags = e.Item.GetTags(IsPackagePage).ToList();

			if (packageTags.Count > 0)
			{
				if (!string.IsNullOrEmpty(versionText))
					versionText += " • ";
				else
					versionText = string.Empty;

				versionText += packageTags.ListStrings(", ");
			}

			tagRect = new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Bottom + padding.Bottom, 0, 0);

			var author = workshopInfo?.Author;

			if (author?.Name is not null and not "")
			{
				using var authorFont = UI.Font(7.5F);
				using var authorFontUnderline = UI.Font(7.5F, FontStyle.Underline);
				using var fadedBrush = new SolidBrush(Color.FromArgb(200, brush.Color));

				var rect = new Rectangle(e.Rects.TextRect.X, e.Rects.IncludedRect.Y, e.Rects.TextRect.Width, e.Rects.IncludedRect.Height);
				var size = e.Graphics.Measure(author.Name, authorFont).ToSize();

				using var authorIcon = IconManager.GetIcon("I_Author", size.Height);

				e.Rects.AuthorRect = rect.Align(size + new Size(authorIcon.Width, 0), ContentAlignment.BottomLeft);

				var isHovered = e.Rects.AuthorRect.Contains(CursorLocation);

				e.Graphics.DrawImage(authorIcon.Color(brush.Color, (byte)(isHovered ? 255 : 185)), e.Rects.AuthorRect.Align(authorIcon.Size, ContentAlignment.MiddleLeft));
				e.Graphics.DrawString(author.Name, isHovered ? authorFontUnderline : authorFont, isHovered ? brush : fadedBrush, e.Rects.AuthorRect, new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center });
			}

			if (!string.IsNullOrEmpty(versionText))
			{
				using var versionFont = UI.Font(7.5F);
				using var fadedBrush = new SolidBrush(Color.FromArgb(150, brush.Color));

				var rect = new Rectangle(e.Rects.TextRect.X, e.Rects.IncludedRect.Y, e.Rects.TextRect.Width, e.Rects.IncludedRect.Height);
				var size = e.Graphics.Measure(versionText, versionFont).ToSize();

				if (author is not null)
				{
					rect.Y += e.Rects.AuthorRect.Height;

					e.DrawableItem.CachedHeight += e.Rects.AuthorRect.Height;
				}

				e.Graphics.DrawString(versionText, versionFont, fadedBrush, rect, new StringFormat { LineAlignment = StringAlignment.Far });
			}

			if (isRecent && !IsPackagePage)
			{
				using var pen = new Pen(FormDesign.Modern.ActiveColor, (float)(UI.FontScale));
				using var gradientBrush = new LinearGradientBrush(e.Rects.IconRect.Pad(0, e.Rects.IconRect.Height*2/3, 0,0), Color.Empty, FormDesign.Modern.ActiveColor, 90);
				using var dateBrush = new SolidBrush(FormDesign.Modern.ActiveForeColor);

				e.Graphics.FillRoundedRectangle(gradientBrush, e.Rects.IconRect.Pad(0, e.Rects.IconRect.Height * 2 / 3, 0, 0), (int)(5 * UI.FontScale));
				e.Graphics.DrawRoundedRectangle(pen, e.Rects.IconRect, (int)(5 * UI.FontScale));

				e.Graphics.DrawString(Locale.RecentlyUpdated, font, dateBrush, e.Rects.IconRect.Pad(GridPadding), new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center });
			}
		}

		private Rectangles GenerateGridRectangles(IPackageIdentity item, Rectangle rectangle)
		{
			var rects = new Rectangles(item)
			{
				IconRect = rectangle.Align(new Size(rectangle.Width, rectangle.Width), ContentAlignment.TopCenter),
				DotsRect = new Rectangle(rectangle.X, rectangle.Y + rectangle.Width + GridPadding.Top, rectangle.Width, 0).Align(UI.Scale(new Size(16, 24), UI.FontScale), ContentAlignment.TopRight)
			};

			rects.IncludedRect = new Rectangle(new Point(rectangle.X, rectangle.Y + rectangle.Width + GridPadding.Top), UI.Scale(new Size(32, 32), UI.FontScale));

			using var titleFont = UI.Font(9F, FontStyle.Bold);
			rects.TextRect = new Rectangle(rectangle.X + rects.IncludedRect.Width + GridPadding.Left, rectangle.Y + rectangle.Width + GridPadding.Top, rectangle.Width - rects.IncludedRect.Width - GridPadding.Horizontal - rects.DotsRect.Width, 0).AlignToFontSize(titleFont, ContentAlignment.TopLeft);
			rects.CenterRect = rects.TextRect.Pad(0, -GridPadding.Vertical, 0, 0);

			if (_settings.UserSettings.AdvancedIncludeEnable && item.GetPackage()?.IsCodeMod == true)
			{
				rects.EnabledRect = rects.IncludedRect;

				rects.IncludedRect.X -= rects.IncludedRect.Width;
			}

			return rects;
		}

		protected override void OnViewChanged()
		{
			base.OnViewChanged();

			if (GridView)
			{
				Padding =  UI.Scale(new Padding(4), UI.UIScale);
				GridPadding = UI.Scale(new Padding(6), UI.UIScale);
			}
		}
	}
}
