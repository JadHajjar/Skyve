using Skyve.App.UserInterface.Content;
using Skyve.Compatibility.Domain.Enums;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	private List<(string text, int width)?> headers = [];
	public static Bitmap? AssetThumb { get; private set; }
	public static Bitmap? AssetThumbUnsat { get; private set; }
	public static Bitmap? ModThumb { get; private set; }
	public static Bitmap? ModThumbUnsat { get; private set; }
	public static Bitmap? PackageThumb { get; private set; }
	public static Bitmap? PackageThumbUnsat { get; private set; }
	public static Bitmap? WorkshopThumb { get; private set; }
	public static Bitmap? WorkshopThumbUnsat { get; private set; }

	public static void LoadThumbnails()
	{
		WorkshopThumb = Properties.Resources.Thumb_Pdx;
		WorkshopThumbUnsat = Properties.Resources.Thumb_Pdx.ToGrayscale();
		PackageThumb = Properties.Resources.Thumb_Package;
		PackageThumbUnsat = Properties.Resources.Thumb_Package.ToGrayscale();
		AssetThumb = Properties.Resources.Thumb_Asset;
		AssetThumbUnsat = Properties.Resources.Thumb_Asset.ToGrayscale();
		ModThumb = Properties.Resources.Thumb_Mod;
		ModThumbUnsat = Properties.Resources.Thumb_Mod.ToGrayscale();
	}

	private int DrawScore(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IWorkshopInfo? workshopInfo, int xdiff)
	{
		if (workshopInfo is null)
		{
			return 0;
		}

		var score = workshopInfo.VoteCount;
		var padding = GridView ? GridPadding : Padding;
		var height = e.Rects.IconRect.Bottom - Math.Max(e.Rects.TextRect.Bottom, Math.Max(e.Rects.VersionRect.Bottom, e.Rects.DateRect.Bottom)) - padding.Bottom;

		//e.Rects.ScoreRect = DrawLargeLabel(e.Graphics, new Point(e.Rects.TextRect.X + xdiff + (xdiff == 0 ? 0 : padding.Left), e.Rects.IconRect.Bottom), score.ToMagnitudeString(), "VoteFilled", workshopInfo!.HasVoted ? FormDesign.Design.GreenColor : null, alignment: ContentAlignment.BottomLeft, padding: padding, height: height, cursorLocation: CursorLocation);

		return 0;
	}

	private void DrawThumbnail(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageIdentity? localIdentity, IWorkshopInfo? workshopInfo)
	{
		if (!e.InvalidRects.Any(x => x.IntersectsWith(e.Rects.IconRect)))
		{
			return;
		}

		var thumbnail = e.Item.GetThumbnail();

		if (thumbnail is null)
		{
			thumbnail = e.Item.IsLocal()
				? e.Item is IAsset ? AssetThumbUnsat : _page is SkyvePage.Mods ? ModThumbUnsat : _page is SkyvePage.Packages ? PackageThumbUnsat : WorkshopThumbUnsat
				: e.Item is IAsset ? AssetThumb : _page is SkyvePage.Mods ? ModThumb : _page is SkyvePage.Packages ? PackageThumb : WorkshopThumb;

			if (thumbnail is not null)
			{
				drawThumbnail(thumbnail);
			}
		}
		else if (e.Item.IsLocal())
		{
			using var unsatImg = thumbnail.ToGrayscale();

			drawThumbnail(unsatImg);
		}
		else
		{
			drawThumbnail(thumbnail);
		}

		if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.IconRect.Contains(CursorLocation))
		{
			using var brush = new SolidBrush(Color.FromArgb(75, 255, 255, 255));
			e.Graphics.FillRoundedRectangle(brush, e.Rects.IconRect, UI.Scale(5));
		}

		var date = workshopInfo is null || workshopInfo.ServerTime == default ? (localIdentity?.LocalTime ?? default) : workshopInfo.ServerTime;
		var isRecent = date > DateTime.UtcNow.AddDays(-7);

		if (isRecent && !IsPackagePage && _page is not SkyvePage.Workshop)
		{
			using var pen = new Pen(FormDesign.Design.ActiveColor, (float)(2 * UI.FontScale));

			if (!GridView || _settings.UserSettings.ComplexListUI)
			{
				e.Graphics.DrawRoundedRectangle(pen, e.Rects.IconRect, UI.Scale(5));

				return;
			}

			using var font = UI.Font(9F, FontStyle.Bold);
			using var gradientBrush = new LinearGradientBrush(e.Rects.IconRect.Pad(0, e.Rects.IconRect.Height * 2 / 3, 0, 0), Color.Empty, FormDesign.Design.ActiveColor, 90);
			using var dateBrush = new SolidBrush(FormDesign.Design.ActiveForeColor);
			using var stringFormat = new StringFormat { LineAlignment = StringAlignment.Far, Alignment = StringAlignment.Center };

			var cblend = new ColorBlend(4)
			{
				Colors = [default, Color.FromArgb(100, FormDesign.Design.ActiveColor), Color.FromArgb(200, FormDesign.Design.ActiveColor), FormDesign.Design.ActiveColor],
				Positions = [0f, 0.5f, 0.7f, 1f]
			};

			gradientBrush.InterpolationColors = cblend;

			e.Graphics.FillRoundedRectangle(gradientBrush, e.Rects.IconRect.Pad(0, e.Rects.IconRect.Height * 2 / 3, 0, 0), UI.Scale(5));
			e.Graphics.DrawRoundedRectangle(pen, e.Rects.IconRect, UI.Scale(5));

			e.Graphics.DrawString(Locale.RecentlyUpdated, font, dateBrush, e.Rects.IconRect.Pad(GridPadding), stringFormat);
		}

		void drawThumbnail(Bitmap generic)
		{
			e.Graphics.DrawRoundedImage(generic, e.Rects.IconRect, UI.Scale(5));

			using var pen = new Pen(e.BackColor, 1.5f) { Alignment = PenAlignment.Center };
			e.Graphics.DrawRoundedRectangle(pen, e.Rects.IconRect, UI.Scale(5));
		}
	}

	private void DrawAuthorImage(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IUser author, Rectangle rectangle, Color color)
	{
		var image = _workshopService.GetUser(author).GetThumbnail();

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

#if CS2
	private void DrawIncludedButton(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, bool isIncluded, bool isPartialIncluded, bool isEnabled, ILocalPackageIdentity? localIdentity, out Color activeColor)
	{
		activeColor = default;

		if (!IsGenericPage && localIdentity is null && e.Item.IsLocal())
		{
			return; // missing local item
		}

		var required = _modLogicManager.IsRequired(localIdentity, _modUtil);
		var isHovered = !(SelectedPlayset is null && !e.Item.IsLocal()) && !IsGenericPage && (e.DrawableItem.Loading || (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.IncludedRect.Contains(CursorLocation)));

		if (!required && isIncluded && isHovered)
		{
			isPartialIncluded = false;
			isEnabled = !isEnabled;
		}

		if (isIncluded && !required && isHovered && ModifierKeys.HasFlag(Keys.Alt))
		{
			activeColor = FormDesign.Design.RedColor;
		}
		else if (isEnabled)
		{
			activeColor = isPartialIncluded ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
		}
		else if (required)
		{
			activeColor = Color.FromArgb(200, ForeColor.MergeColor(BackColor));
		}

		Color iconColor;

		if (required && activeColor != default)
		{
			iconColor = !FormDesign.Design.IsDarkTheme ? activeColor.MergeColor(ForeColor, 75) : activeColor;
			activeColor = activeColor.MergeColor(BackColor, !FormDesign.Design.IsDarkTheme ? 35 : 20);
		}
		else if (activeColor == default && isHovered)
		{
			iconColor = isIncluded ? isEnabled ? FormDesign.Design.GreenColor : FormDesign.Design.RedColor : FormDesign.Design.ActiveColor;
			activeColor = Color.FromArgb(40, iconColor);
		}
		else
		{
			if (activeColor == default)
			{
				activeColor = Color.FromArgb(20, ForeColor);
			}
			else if (isHovered)
			{
				activeColor = activeColor.MergeColor(ForeColor, 75);
			}

			iconColor = activeColor.GetTextColor();
		}

		using var brush = e.Rects.IncludedRect.Gradient(activeColor);
		e.Graphics.FillRoundedRectangle(brush, e.Rects.IncludedRect, UI.Scale(4));

		if (e.DrawableItem.Loading)
		{
			var rectangle = e.Rects.IncludedRect.CenterR(e.Rects.IncludedRect.Height * 3 / 5, e.Rects.IncludedRect.Height * 3 / 5);
#if CS2
			if (!_subscriptionsManager.TryGetDownloadStatus(e.Item.Id, out var status) || status.Stage > ModDownloadStage.Started)
			{
				DrawLoader(e.Graphics, rectangle, iconColor);
				return;
			}

			var width = Math.Min(Math.Min(rectangle.Width, rectangle.Height), (int)(32 * UI.UIScale));
			var size = (float)Math.Max(2, width / (8D - (Math.Abs(100 - LoaderPercentage) / 50)));
			var drawSize = new SizeF(width - size, width - size);
			var rect = new RectangleF(new PointF(rectangle.X + ((rectangle.Width - drawSize.Width) / 2), rectangle.Y + ((rectangle.Height - drawSize.Height) / 2)), drawSize).Pad(size / 2);
			using var pen = new Pen(iconColor, size) { StartCap = LineCap.Round, EndCap = LineCap.Round };

			e.Graphics.DrawArc(pen, rect, -90, 360 * status.TotalProgress);
#else
			DrawLoader(e.Graphics, rectangle, iconColor);
#endif
			return;
		}

		var icon = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item) ? "Wait" : (isHovered && isIncluded && ModifierKeys.HasFlag(Keys.Alt)) ? "X" : isPartialIncluded ? "Slash" : isEnabled ? "Ok" : !isIncluded ? "Add" : "Enabled");
		using var includedIcon = icon.Get(e.Rects.IncludedRect.Height * 3 / 4).Color(iconColor);

		e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));

		if (SelectedPlayset is null && !e.Item.IsLocal())
		{
			var dimBrush = new SolidBrush(Color.FromArgb(150, e.BackColor));
			e.Graphics.FillRectangle(dimBrush, e.Rects.IncludedRect);
		}
	}
#else
		private void DrawIncludedButton(ItemPaintEventArgs<T, Rectangles> e, bool isIncluded, bool partialIncluded, ILocalPackageData? package, out Color activeColor)
		{
			activeColor = default;

			if (package is null && e.Item.IsLocal())
			{
				return; // missing local item
			}

			var inclEnableRect = e.Rects.EnabledRect == Rectangle.Empty ? e.Rects.IncludedRect : Rectangle.Union(e.Rects.IncludedRect, e.Rects.EnabledRect);
			var incl = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item) ? "Wait" : partialIncluded ? "Slash" : isIncluded ? "Ok" : package is null ? "Add" : "Enabled");
			var required = package is not null && _modLogicManager.IsRequired(package, _modUtil);

			DynamicIcon? enabl = null;

			if (_settings.UserSettings.AdvancedIncludeEnable && package is not null)
			{
				enabl = new DynamicIcon(package.IsEnabled() ? "Checked" : "Checked_OFF");

				if (isIncluded)
				{
					activeColor = partialIncluded ? FormDesign.Design.YellowColor : package.IsEnabled() ? FormDesign.Design.GreenColor : FormDesign.Design.RedColor;
				}
				else if (package.IsEnabled())
				{
					activeColor = FormDesign.Design.YellowColor;
				}
			}
			else if (isIncluded)
			{
				activeColor = partialIncluded ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
			}

			Color iconColor;

			if (required && activeColor != default)
			{
				iconColor = FormDesign.Design.Type is FormDesignType.Light ? activeColor.MergeColor(ForeColor, 75) : activeColor;
				activeColor = activeColor.MergeColor(BackColor, FormDesign.Design.Type is FormDesignType.Light ? 35 : 20);
			}
			else if (activeColor == default && inclEnableRect.Contains(CursorLocation))
{
				activeColor = Color.FromArgb(40, FormDesign.Design.ActiveColor);
				iconColor = FormDesign.Design.ActiveColor;
			}
			else
			{
				if (activeColor == default)
					activeColor = Color.FromArgb(20, ForeColor);
				else if (inclEnableRect.Contains(CursorLocation))
					activeColor = activeColor.MergeColor(ForeColor, 75);

				iconColor = activeColor.GetTextColor();
			}

			using var brush = inclEnableRect.Gradient(activeColor);

			e.Graphics.FillRoundedRectangle(brush, inclEnableRect, UI.Scale(4));

			using var includedIcon = incl.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);
			using var enabledIcon = enabl?.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);

			e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
			if (enabledIcon is not null)
			{
				e.Graphics.DrawImage(enabledIcon, e.Rects.EnabledRect.CenterR(includedIcon.Size));
			}
		}
#endif
	private void DrawTitleAndTags(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
	{
		var padding = GridView ? GridPadding : Padding;
		var text = e.Item.CleanName(out var tags);
		using var stringFormat = CompactList ? new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center } : new();

		if (GridView && !_settings.UserSettings.ComplexListUI)
		{
			using var font = UI.Font(9F, FontStyle.Bold);
			var textRect = new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Y, e.Rects.TextRect.Width, Height);

			var textSize = e.Graphics.Measure(text, font, textRect.Width);
			var oneLineSize = e.Graphics.Measure(text, font);
			var oneLine = textSize.Height == oneLineSize.Height;
			var tagRect = new Rectangle(e.Rects.TextRect.X + (oneLine ? (int)textSize.Width : 0), textRect.Y + (oneLine ? 0 : (int)textSize.Height), 0, (int)oneLineSize.Height);

			e.Rects.TextRect.Height = (int)textSize.Height + (GridPadding.Top / 3);
			e.Rects.CenterRect = e.Rects.TextRect.Pad(0, -GridPadding.Top, 0, 0);
			e.DrawableItem.CachedHeight = e.Rects.TextRect.Bottom;

			using var brushTitle = new SolidBrush(e.Rects.CenterRect.Contains(CursorLocation) && e.HoverState == HoverState.Hovered && !IsPackagePage ? FormDesign.Design.ActiveColor : e.BackColor.GetTextColor());

			e.Graphics.DrawString(text, font, brushTitle, textRect, stringFormat);

			for (var i = 0; i < tags.Count; i++)
			{
				var tagSize = e.Graphics.MeasureLabel(tags[i].Text, null, smaller: !_settings.UserSettings.ComplexListUI);

				if (tagRect.X + tagSize.Width > e.Rects.TextRect.Right)
				{
					tagRect.Y += tagRect.Height;
					tagRect.X = e.Rects.TextRect.X;
					e.DrawableItem.CachedHeight += tagRect.Height;
				}

				var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, smaller: !_settings.UserSettings.ComplexListUI);

				tagRect.X += padding.Left + rect.Width;
			}

			if (!oneLine && tags.Count > 0)
			{
				e.DrawableItem.CachedHeight += tagRect.Height;
			}
		}
		else
		{
			var tagSizes = 0;

			for (var i = 0; i < tags.Count; i++)
			{
				var size = e.Graphics.MeasureLabel(tags[i].Text, null, smaller: !_settings.UserSettings.ComplexListUI);

				tagSizes += padding.Left + size.Width;
			}

			using var brushTitle = new SolidBrush(e.Rects.CenterRect.Contains(CursorLocation) && e.HoverState == HoverState.Hovered && !IsPackagePage ? FormDesign.Design.ActiveColor : e.BackColor.GetTextColor());
			using var font = UI.Font(GridView ? 11.25F : CompactList ? 8.25F : 10.5F, FontStyle.Bold).FitTo(text, e.Rects.TextRect.Pad(0, 0, tagSizes + Padding.Right, 0), e.Graphics);
			var textRect = e.Rects.TextRect.Pad(0, 0, tagSizes, 0).AlignToFontSize(font, CompactList ? ContentAlignment.MiddleLeft : ContentAlignment.TopLeft, e.Graphics);

			e.Graphics.DrawString(text, font, brushTitle, textRect, stringFormat);

			var textSize = e.Graphics.Measure(text, font, e.Rects.TextRect.Width - tagSizes - Padding.Right);
			var tagRect = new Rectangle(e.Rects.TextRect.X + (int)textSize.Width, textRect.Y, 0, textRect.Height);

			for (var i = 0; i < tags.Count; i++)
			{
				var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, smaller: !_settings.UserSettings.ComplexListUI);

				tagRect.X += padding.Left + rect.Width;
			}
		}
	}

	private void DrawCompactVersionAndDate(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IPackage? package, ILocalPackageIdentity? localPackageIdentity, IWorkshopInfo? workshopInfo)
	{
#if CS1
		var isVersion = localParentPackage?.Mod is not null && !e.Item.IsBuiltIn && !IsPackagePage;
		var text = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.IsBuiltIn ? Locale.Vanilla : e.Item is ILocalPackageData lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString();
#else
		var isVersion = package?.IsCodeMod() ?? false;
		var versionText = isVersion ? package?.VersionName ?? (workshopInfo?.Changelog.FirstOrDefault(x => x.VersionId == package?.Version)?.Version) : null;
		versionText = versionText is not null ? $"v{versionText}" : localPackageIdentity != null ? localPackageIdentity.FileSize.SizeString(0) : workshopInfo?.ServerSize.SizeString(0);
#endif
		var date = workshopInfo is null || workshopInfo.ServerTime == default ? (localPackageIdentity?.LocalTime ?? default) : workshopInfo.ServerTime;

		if (versionText is not null and not "")
		{
			DrawCell(e, Columns.Version, versionText, null, active: false);
		}

		if (Width / UI.FontScale >= 800 && date != default)
		{
			var dateText = _settings.UserSettings.ShowDatesRelatively ? date.ToLocalTime().ToRelatedString(true, false) : date.ToLocalTime().ToString("g");
			DrawCell(e, Columns.UpdateTime, dateText, "UpdateTime", active: false);
		}
	}

	private Rectangle DrawCell(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, Columns column, string text, DynamicIcon? dIcon, Color? backColor = null, Font? font = null, bool active = true, Padding padding = default, Color? textColor = null)
	{
		var cell = _columnSizes[column];
		var rect = new Rectangle(cell.X, e.ClipRectangle.Y, cell.Width, e.ClipRectangle.Height).Pad(0, -Padding.Top, 0, -Padding.Bottom);

		textColor ??= backColor?.GetTextColor() ?? e.BackColor.GetTextColor();

		if (active && rect.Contains(CursorLocation))
		{
			textColor = FormDesign.Design.ActiveColor;
		}

		if (dIcon != null)
		{
			using var icon = dIcon.Small.Color(textColor.Value);

			e.Graphics.DrawImage(icon, rect.Pad(Padding).Align(icon.Size, ContentAlignment.MiddleLeft));

			rect = rect.Pad(icon.Width + Padding.Left, 0, 0, 0);
		}

		using (var brush = new SolidBrush(textColor.Value))
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

	protected override void OnPrePaint(PaintEventArgs e)
	{
		headers =
		[
			(Locale.Package, 0),
			(Locale.Version, 65),
			(Locale.UpdateTime, 140),
			(Locale.Author, 150),
			(LocaleSlickUI.Tags, 0),
			(Locale.Status, 160),
			("", 80)
		];

		if (Width / UI.FontScale < 500)
		{
			headers[5] = null;
		}

		if (Width / UI.FontScale < 965)
		{
			headers[4] = null;
		}

		if (Width / UI.FontScale < 600)
		{
			headers[3] = null;
		}

		if (Width / UI.FontScale < 800)
		{
			headers[2] = null;
		}

		var remainingWidth = Width - (int)(headers.Sum(x => x?.width ?? 0) * UI.FontScale);
		var autoColumns = headers.Count(x => x?.width == 0);
		var xPos = 0;

		for (var i = 0; i < headers.Count; i++)
		{
			var header = headers[i];

			if (header == null)
			{
				continue;
			}

			var width = header.Value.width == 0 ? (remainingWidth / autoColumns) : (int)(header.Value.width * UI.FontScale);

			_columnSizes[(Columns)i] = (xPos, width);

			xPos += width;
		}
	}

	protected override void DrawHeader(PaintEventArgs e)
	{
		var remainingWidth = Width - (int)(headers.Sum(x => x?.width ?? 0) * UI.FontScale);
		var autoColumns = headers.Count(x => x?.width == 0);
		var xPos = 0;

		using var font = UI.Font(7.5F, FontStyle.Bold);
		using var brush = new SolidBrush(FormDesign.Design.LabelColor);
		using var lineBrush = new SolidBrush(FormDesign.Design.AccentColor);

		e.Graphics.Clear(FormDesign.Design.AccentBackColor);

		e.Graphics.FillRectangle(lineBrush, new Rectangle(0, StartHeight - 2, Width, 2));

		for (var i = 0; i < headers.Count; i++)
		{
			var header = headers[i];

			if (header == null)
			{
				continue;
			}

			var width = header.Value.width == 0 ? (remainingWidth / autoColumns) : (int)(header.Value.width * UI.FontScale);

			e.Graphics.DrawString(header.Value.text.ToUpper(), font, brush, new Rectangle(xPos, 1, width, StartHeight).Pad(Padding).AlignToFontSize(font, ContentAlignment.MiddleLeft), new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });

			xPos += width;
		}
	}

	private void DrawCompactAuthorOrFolder(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageIdentity? localIdentity, IWorkshopInfo? workshopInfo)
	{
		var author = workshopInfo?.Author;

		if (author?.Name is not null and not "")
		{
			e.Rects.AuthorRect = DrawCell(e, Columns.Author, author.Name, "Author", font: UI.Font(8.25F), textColor: UserIcon.GetUserColor(author.Id?.ToString() ?? string.Empty, true));
		}
		else if (localIdentity is not null)
		{
			DrawCell(e, Columns.Author, Path.GetFileName(localIdentity.Folder), "Folder", active: false, font: UI.Font(8.25F));
		}
	}

	private int DrawButtons(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageIdentity? localIdentity, IWorkshopInfo? workshopInfo)
	{
		var padding = GridView ? GridPadding : Padding;
		var size = _settings.UserSettings.ComplexListUI ?
			UI.Scale(CompactList ? new Size(24, 24) : new Size(28, 28)) :
			UI.Scale(CompactList ? new Size(18, 18) : new Size(22, 22));
		var rect = new Rectangle(e.ClipRectangle.Right, e.ClipRectangle.Y, 0, e.ClipRectangle.Height).Pad(padding);

		if (localIdentity is not null)
		{
			e.Rects.FolderRect = SlickButton.AlignAndDraw(e.Graphics, rect, CompactList ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
			{
				Size = size,
				Icon = "Folder",
				Font = Font,
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				BackgroundColor = e.BackColor
			}).Rectangle;

			rect.X -= e.Rects.FolderRect.Width + padding.Left;
		}

		if (!IsPackagePage && workshopInfo?.Url is not null)
		{
			e.Rects.WorkshopRect = SlickButton.AlignAndDraw(e.Graphics, rect, CompactList ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
			{
				Size = size,
#if CS2
				Icon = "Paradox",
#else
				Icon = "Steam",
#endif
				Font = Font,
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				BackgroundColor = e.BackColor
			}).Rectangle;

			rect.X -= e.Rects.WorkshopRect.Width + padding.Left;
		}

		if (!IsPackagePage && _settings.UserSettings.ComplexListUI && e.Item.GetPackageInfo()?.Links?.FirstOrDefault(x => x.Type == LinkType.Github) is ILink gitLink)
		{
			e.Rects.GithubRect = SlickButton.AlignAndDraw(e.Graphics, rect, CompactList ? ContentAlignment.MiddleRight : ContentAlignment.BottomRight, new ButtonDrawArgs
			{
				Size = size,
				Icon = "Github",
				Font = Font,
				HoverState = e.HoverState,
				Cursor = CursorLocation,
				BackgroundColor = e.BackColor
			}).Rectangle;

			rect.X -= e.Rects.GithubRect.Width + padding.Left;
		}

		return rect.X + rect.Width;
	}

	private void SetBackColorForList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, NotificationType? notificationType, bool hasStatus, Color statusColor)
	{
		if (e.IsSelected)
		{
			e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
		}
		else if (!IsPackagePage && notificationType > NotificationType.Info)
		{
			e.BackColor = notificationType.Value.GetColor().MergeColor(FormDesign.Design.BackColor, 25);
		}
		else
		{
			e.BackColor = !IsPackagePage && hasStatus
				? statusColor.MergeColor(FormDesign.Design.BackColor).MergeColor(FormDesign.Design.BackColor, 25)
				: e.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.AccentBackColor : BackColor;
		}

		if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)))
		{
			e.BackColor = e.BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);
		}
	}

	public static Rectangle DrawLabel(
		Graphics graphics, Rectangle container, string text, DynamicIcon icon, Color? color = null,
		ContentAlignment alignment = ContentAlignment.TopLeft, Point? cursorLocation = null, bool large = false)
	{
		text = text.ToUpper();

		using var font = large ? UI.Font(8.25F) : UI.Font(7F);
		using var image = icon.Get(font.Height + UI.Scale(2));
		var padding = UI.Scale(new Padding(2));
		var textSize = Size.Ceiling(graphics.Measure(text.IfEmpty("A"), font)) + (large ? new Size(0, padding.Vertical):Size.Empty);
		var size = string.IsNullOrEmpty(text) ? new Size(textSize.Height, textSize.Height) : new Size(textSize.Width + image.Width + padding.Left + padding.Horizontal, textSize.Height);
		var rect = container.Align(size, alignment);

		using var brush = new SolidBrush(color.HasValue ? Color.FromArgb(cursorLocation.HasValue && rect.Contains(cursorLocation.Value) ? 160 : 255, color.Value) : Color.FromArgb(120, !cursorLocation.HasValue || rect.Contains(cursorLocation.Value) ? FormDesign.Design.ActiveColor : FormDesign.Design.LabelColor.MergeColor(FormDesign.Design.AccentBackColor, 40)));
		using var textBrush = new SolidBrush(brush.Color.GetTextColor());
		using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		graphics.FillRoundedRectangle(brush, rect, (int)UI.Scale(2.5));
		graphics.DrawString(text, font, textBrush, rect.Pad(image.Width + padding.Left * 2, padding.Top, padding.Right, padding.Bottom), stringFormat);

		graphics.DrawImage(image.Color(textBrush.Color), string.IsNullOrEmpty(text) ? rect.CenterR(image.Size) : rect.Pad(padding.Horizontal).Align(image.Size, ContentAlignment.MiddleLeft));

		return rect;
	}
}