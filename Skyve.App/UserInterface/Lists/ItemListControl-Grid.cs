using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl<T>
{
	protected override void OnPaintItemGrid(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e)
	{
		var localPackage = e.Item.LocalPackage;
		var localParentPackage = localPackage?.LocalParentPackage;
		var workshopInfo = e.Item.GetWorkshopInfo();
		var partialIncluded = false;
		var isPressed = false;
		var isIncluded = (localPackage is not null && _packageUtil.IsIncluded(e.Item.LocalPackage!, out partialIncluded)) || partialIncluded;

		var compatibilityReport = e.Item.GetCompatibilityInfo();
		var notificationType = compatibilityReport?.GetNotification();
		var hasStatus = GetStatusDescriptors(e.Item, out var statusText, out var statusIcon, out var statusColor);

		var clip = e.ClipRectangle;
		var borderColor = Color.Empty;
		e.BackColor = BackColor;

		if (!IsPackagePage && notificationType > NotificationType.Info)
		{
			borderColor = notificationType.Value.GetColor().MergeColor(FormDesign.Design.BackColor);
		}
		else if (hasStatus)
		{
			borderColor = statusColor.MergeColor(FormDesign.Design.BackColor);
		}

		if (e.IsSelected)
		{
			e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
		}

		if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)))
		{
			isPressed = e.HoverState.HasFlag(HoverState.Pressed);

			e.BackColor = isPressed ? FormDesign.Design.ActiveColor : e.BackColor.MergeColor(FormDesign.Design.ActiveColor, 90);
			borderColor = isPressed && borderColor == Color.Empty ? FormDesign.Design.ActiveColor : borderColor;
		}

		e.BackColor = e.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 3f : -3f);
		e.Graphics.ResetClip();
		e.Graphics.FillRoundedRectangleWithShadow(e.ClipRectangle.InvertPad(GridPadding), GridPadding.Left, UI.Scale(6), e.BackColor, borderColor == Color.Empty ? null : Color.FromArgb(7, borderColor), addOutline: true);
		e.Graphics.SetClip(clip);

		DrawThumbnail(e);
		DrawTitleAndTagsAndVersion(e, localParentPackage, workshopInfo, isPressed);
		DrawIncludedButton(e, isIncluded, partialIncluded, localParentPackage, out var activeColor);

		var scoreX = DrawScore(e, workshopInfo);

		if (workshopInfo?.Author is not null)
		{
			DrawAuthor(e, workshopInfo.Author, scoreX);
		}
		else if (e.Item.IsLocal)
		{
			DrawFolderName(e, localParentPackage!, scoreX);
		}

		DrawDividerLine(e);

		var maxTagX = DrawButtons(e, isPressed, localParentPackage, workshopInfo);

		DrawTags(e, maxTagX);

		e.Graphics.ResetClip();

		DrawCompatibilityAndStatus(e, notificationType, statusText, statusIcon, statusColor);

		if (!isIncluded && !IsPackagePage && localPackage is not null && !e.HoverState.HasFlag(HoverState.Hovered))
		{
			using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
			e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(GridPadding));
		}
	}

	private void DrawCompatibilityAndStatus(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, NotificationType? notificationType, string? statusText, DynamicIcon? statusIcon, Color statusColor)
	{
		var compatibilityReport = e.Item.GetCompatibilityInfo();

		var height = UI.Scale(22);

		if (notificationType > NotificationType.Info)
		{
			var point = new Rectangle(e.Rects.IncludedRect.Right, e.Rects.IconRect.Bottom, 0, 0);

			e.Rects.CompatibilityRect = DrawLabel(e.Graphics,
				point,
				LocaleCR.Get(notificationType.ToString()),
				notificationType.Value.GetIcon(false),
				notificationType.Value.GetColor(),
				ContentAlignment.BottomRight,
				CursorLocation);
		}

		if (statusText is not null && statusIcon is not null)
		{
			var point = new Rectangle(notificationType > NotificationType.Info ? (e.Rects.CompatibilityRect.X - Padding.Left) : e.Rects.IncludedRect.Right, e.Rects.IconRect.Bottom, 0, 0);

			e.Rects.DownloadStatusRect = DrawLabel(e.Graphics,
				point,
				notificationType > NotificationType.Info ? "" : statusText,
				statusIcon,
				statusColor,
				ContentAlignment.BottomRight,
				null);
		}
	}

	private void DrawTags(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, int maxTagX)
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

	private void DrawDividerLine(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e)
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

	private Rectangle DrawTag(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, int maxTagX, Point startLocation, ref Rectangle tagsRect, ITag item, Color? color = null)
	{
		using var tagIcon = IconManager.GetSmallIcon(item.Icon);

		var padding = GridView ? GridPadding : Padding;
		var tagSize = e.Graphics.MeasureLabel(item.Value, tagIcon, large: GridView);
		var tagRect = e.Graphics.DrawLabel(item.Value, tagIcon, color ?? Color.FromArgb(200, FormDesign.Design.LabelColor.MergeColor(FormDesign.Design.AccentBackColor, 40)), tagsRect, GridView ? ContentAlignment.TopLeft : ContentAlignment.BottomLeft, smaller: CompactList, mousePosition: CursorLocation);

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

	private int DrawButtons(ItemPaintEventArgs<T, Rectangles> e, bool isPressed, ILocalPackageWithContents? parentPackage, IWorkshopInfo? workshopInfo)
	{
		var padding = GridView ? GridPadding : Padding;
		var size = UI.Scale(CompactList ? new Size(20, 20) : new Size(24, 24));
		var rect = new Rectangle(e.ClipRectangle.Right - size.Width - (GridView ? 0 : Padding.Right), CompactList ? (e.ClipRectangle.Y + ((e.ClipRectangle.Height - size.Height) / 2)) : (e.ClipRectangle.Bottom - size.Height), size.Width, size.Height);
		var backColor = Color.FromArgb(175, GridView ? FormDesign.Design.BackColor : FormDesign.Design.ButtonColor);

		if (parentPackage is not null)
		{
			e.Rects.FolderRect = SlickButton.AlignAndDraw(e.Graphics, rect, CompactList ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
			{
				Size = size,
				Icon = "Folder",
				Font = Font,
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				BorderRadius = CompactList ? 0 : null,
				BackgroundColor = e.BackColor
			}).Rectangle;

			rect.X -= e.Rects.FolderRect.Width + padding.Left;
		}

		if (!IsPackagePage && workshopInfo?.Url is not null)
		{
			e.Rects.SteamRect = SlickButton.AlignAndDraw(e.Graphics, rect, CompactList ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
			{
				Size = size,
				Icon = "Steam",
				Font = Font,
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				BorderRadius = CompactList ? 0 : null,
				BackgroundColor = e.BackColor
			}).Rectangle;

			rect.X -= e.Rects.SteamRect.Width + padding.Left;
		}

		if (!IsPackagePage && _compatibilityManager.GetPackageInfo(e.Item)?.Links?.FirstOrDefault(x => x.Type == LinkType.Github) is ILink gitLink)
		{
			e.Rects.GithubRect = SlickButton.AlignAndDraw(e.Graphics, rect, CompactList ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
			{
				Size = size,
				Icon = "Github",
				Font = Font,
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				BorderRadius = CompactList ? 0 : null,
				BackgroundColor = e.BackColor
			}).Rectangle;

			rect.X -= e.Rects.GithubRect.Width + padding.Left;
		}

		return rect.X + rect.Width;
	}

	private void DrawFolderName(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, ILocalPackageWithContents package, int scoreX)
	{
		if (package is null)
		{
			return;
		}

		if (CompactList)
		{
			e.Rects.FolderNameRect = DrawCell(e, Columns.Author, Path.GetFileName(package.Folder), "Folder", font: UI.Font(8.25F));
			return;
		}

		using var folderFont = UI.Font(7.5F, FontStyle.Regular);
		using var stringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

		var padding = GridView ? GridPadding : Padding;
		var height = UI.Scale(GridView ? 22 : 16) - padding.Bottom;
		var labelH = height - (GridView ? padding.Vertical : padding.Top) - 1;
		var rect = new Rectangle(e.Rects.TextRect.X + scoreX, e.Rects.IconRect.Bottom - height + 2, labelH, labelH);
		var size = e.Graphics.Measure(Path.GetFileName(package.Folder), folderFont).ToSize();

		rect = rect.Align(size + new Size(size.Height + (GridView ? padding.Left : 0), 0), ContentAlignment.TopLeft);
		e.DrawableItem.CachedHeight = rect.Bottom + (GridPadding.Top / 3);

		using var brush = new SolidBrush(Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 255 : 200, e.BackColor.GetTextColor()));

		using var authorIcon = IconManager.GetIcon("Folder", height * 3 / 4);

		e.Graphics.DrawImage(authorIcon.Color(brush.Color, brush.Color.A), rect.Align(new(height, height), ContentAlignment.MiddleLeft).CenterR(authorIcon.Size));

		e.Graphics.DrawString(Path.GetFileName(package.Folder), folderFont, brush, rect, stringFormat);
	}

	private void DrawAuthor(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, IUser author, int scoreX)
	{
		using var authorFont = UI.Font(7.5F, FontStyle.Regular);
		using var authorFontUnderline = UI.Font(7.5F, FontStyle.Underline);
		using var stringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

		var padding = GridView ? GridPadding : Padding;
		var height = UI.Scale(GridView ? 22 : 16) - padding.Bottom;
		var labelH = height - (GridView ? padding.Vertical : padding.Top) - 1;
		var rect = new Rectangle(e.Rects.TextRect.X + scoreX, e.Rects.IconRect.Bottom - height + 2, labelH, labelH);
		var size = e.Graphics.Measure(author.Name, authorFont).ToSize();

		e.Rects.AuthorRect = rect.Align(size + new Size(size.Height + (GridView ? padding.Left : 0), 0), ContentAlignment.TopLeft);
		e.DrawableItem.CachedHeight = e.Rects.AuthorRect.Bottom + (GridPadding.Top / 3);

		var isHovered = e.Rects.AuthorRect.Contains(CursorLocation);
		using var brush = new SolidBrush(isHovered ? FormDesign.Design.ActiveColor : Color.FromArgb(e.HoverState.HasFlag(HoverState.Hovered) ? 255 : 200, e.BackColor.GetTextColor()));

		DrawAuthorImage(e, author, e.Rects.AuthorRect.Align(new(height, height), ContentAlignment.MiddleLeft), brush.Color);

		e.Graphics.DrawString(author.Name, isHovered ? authorFontUnderline : authorFont, brush, e.Rects.AuthorRect, stringFormat);
	}

	private void DrawAuthorImage(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, IUser author, Rectangle rectangle, Color color)
	{
		var image = author.GetUserAvatar();

		if (image != null)
		{
			e.Graphics.DrawRoundImage(image, rectangle.Pad((int)(1.5 * UI.FontScale)));

			if (e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var pen = new Pen(color, 1.5f);

				e.Graphics.DrawEllipse(pen, rectangle.Pad((int)(1.5 * UI.FontScale)));
			}
		}
		else
		{
			using var authorIcon = IconManager.GetIcon("Author", rectangle.Height * 3 / 4);

			e.Graphics.DrawImage(authorIcon.Color(color, color.A), rectangle.CenterR(authorIcon.Size));
		}

		if (_compatibilityManager.IsUserVerified(author))
		{
			var checkRect = rectangle.Align(new Size(rectangle.Height / 3, rectangle.Height / 3), ContentAlignment.BottomRight);

			using var greenBrush = new SolidBrush(FormDesign.Design.GreenColor);
			e.Graphics.FillEllipse(greenBrush, checkRect.Pad(-UI.Scale(2)));

			using var img = IconManager.GetIcon("Check", checkRect.Height);
			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}
	}

	private void DrawTitleAndTagsAndVersion(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, ILocalPackageWithContents? localParentPackage, IWorkshopInfo? workshopInfo, bool isPressed)
	{
		using var font = UI.Font(GridView ? 10.5F : CompactList ? 7.5F : 9F, FontStyle.Bold);
		var mod = e.Item is not IAsset;
		var tags = new List<(Color Color, string Text)>();
		var text = mod ? e.Item.CleanName(out tags) : e.Item.ToString();
		using var brush = new SolidBrush(isPressed ? FormDesign.Design.ActiveForeColor : (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)) && e.HoverState.HasFlag(HoverState.Hovered) && !IsPackagePage ? FormDesign.Design.ActiveColor : ForeColor);
		e.Graphics.DrawString(text, font, brush, e.Rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = CompactList ? StringAlignment.Center : StringAlignment.Near });

		var isVersion = localParentPackage?.Mod is not null && !e.Item.IsBuiltIn && !IsPackagePage;
		var versionText = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.IsBuiltIn ? Locale.Vanilla : (e.Item is ILocalPackage lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString());
		var date = workshopInfo?.ServerTime ?? e.Item.LocalParentPackage?.LocalTime;

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
				e.Rects.VersionRect = DrawCell(e, Columns.Version, versionText!, null, isVersion ? FormDesign.Design.YellowColor : FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.AccentBackColor, 40), active: localParentPackage?.Mod is not null);
			}

			if (date.HasValue && !IsPackagePage)
			{
				var dateText = _settings.UserSettings.ShowDatesRelatively ? date.Value.ToRelatedString(true, false) : date.Value.ToString("g");

				e.Rects.DateRect = DrawCell(e, Columns.UpdateTime, dateText, null);
			}

			return;
		}

		tagRect = new Rectangle(e.Rects.TextRect.X + padding.Left / 2, e.Rects.TextRect.Bottom + (GridView ? padding.Top / 2 : 0), 0, 0);

		if (date.HasValue && !IsPackagePage)
			versionText = (string.IsNullOrEmpty(versionText) ? "" : $"{versionText} • ") + (_settings.UserSettings.ShowDatesRelatively ? date.Value.ToRelatedString(true, false) : date.Value.ToString("g"));

		if (!string.IsNullOrEmpty(versionText))
		{
			using var smallFont = UI.Font(GridView ? 8F : 7F);
			using var fadedBrush = new SolidBrush(Color.FromArgb(200, e.BackColor.GetTextColor()));

			e.Graphics.DrawString(versionText, smallFont, fadedBrush, tagRect);
		}
	}

	private Rectangle DrawCell(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, Columns column, string text, DynamicIcon? dIcon, Color? backColor = null, Font? font = null, bool active = true, Padding padding = default)
	{
		var cell = _columnSizes[column];
		var rect = new Rectangle(cell.X, e.ClipRectangle.Y, cell.Width, e.ClipRectangle.Height).Pad(0, -Padding.Top, 0, -Padding.Bottom);

		if (active && rect.Contains(CursorLocation))
		{
			using var brush = new SolidBrush(Color.FromArgb(200, backColor ??= FormDesign.Design.ActiveColor));
			e.Graphics.FillRectangle(brush, rect);
		}

		var textColor = (active && rect.Contains(CursorLocation) ? backColor?.GetTextColor() : null) ?? e.BackColor.GetTextColor();

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

	private static void DrawSeam(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, int x)
	{
		var seamRectangle = new Rectangle(x - (int)(40 * UI.UIScale), e.ClipRectangle.Y, (int)(40 * UI.UIScale), e.ClipRectangle.Height);

		using var seamBrush = new LinearGradientBrush(seamRectangle, Color.Empty, e.BackColor, 0F);

		e.Graphics.FillRectangle(seamBrush, seamRectangle);
	}

	private void DrawThumbnail(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e)
	{
		var thumbnail = e.Item.GetThumbnail();

		if (thumbnail is null)
		{
			using var generic = (e.Item is ILocalPackageWithContents ? Properties.Resources.I_CollectionIcon : e.Item.IsMod ? Properties.Resources.I_ModIcon : Properties.Resources.I_AssetIcon).Color(FormDesign.Design.IconColor);

			drawThumbnail(generic);
		}
		else if (e.Item.IsLocal)
		{
			using var unsatImg = new Bitmap(thumbnail, e.Rects.IconRect.Size).Tint(Sat: 0);

			drawThumbnail(unsatImg);
		}
		else
		{
			drawThumbnail(thumbnail);
		}

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, e.Rects.IconRect, (int)(5 * UI.FontScale), e.BackColor);
	}

	private ItemListControl<T>.Rectangles GenerateGridRectangles(T item, Rectangle rectangle)
	{
		var rects = new Rectangles(item)
		{
			IconRect = rectangle.Align(UI.Scale(new Size(64, 64), UI.UIScale), ContentAlignment.TopLeft)
		};

		using var font = UI.Font(10.5F, FontStyle.Bold);
		rects.TextRect = rectangle.Pad(rects.IconRect.Width + GridPadding.Left, 0, 0, rectangle.Height).AlignToFontSize(font, ContentAlignment.TopLeft);

		rects.IncludedRect = rects.TextRect.Align(UI.Scale(new Size(28, 28), UI.FontScale), ContentAlignment.TopRight);

		if (_settings.UserSettings.AdvancedIncludeEnable && item.LocalParentPackage?.Mod is not null)
		{
			rects.EnabledRect = rects.IncludedRect;

			rects.IncludedRect.X -= rects.IncludedRect.Width;
		}

		rects.TextRect.Width = rects.IncludedRect.X - rects.TextRect.X;

		rects.CenterRect = rects.TextRect.Pad(-GridPadding.Horizontal, 0, 0, 0);

		return rects;
	}
}
