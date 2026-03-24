using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl<T>
{
	private List<(string text, int width)?> headers = [];
	private void OnPaintItemCompactList(ItemPaintEventArgs<T, Rectangles> e)
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

		if (e.IsSelected)
		{
			e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
		}
		else if (!IsPackagePage && notificationType > NotificationType.Info)
		{
			e.BackColor = notificationType.Value.GetColor().MergeColor(FormDesign.Design.BackColor, 25);
		}
		else if (hasStatus)
		{
			e.BackColor = statusColor.MergeColor(FormDesign.Design.BackColor).MergeColor(FormDesign.Design.BackColor, 25);
		}
		else if (e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.BackColor = FormDesign.Design.AccentBackColor;
		}
		else
		{
			e.BackColor = BackColor;
		}

		if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)))
		{
			e.BackColor = e.BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);

			isPressed = e.HoverState.HasFlag(HoverState.Pressed);
		}

		base.OnPaintItemList(e);

		e.Graphics.SetClip(new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y - Padding.Top + 1, e.ClipRectangle.Width, e.ClipRectangle.Height + Padding.Vertical - 2));

		DrawTitleAndTagsAndVersion(e, localParentPackage, workshopInfo, isPressed);
		DrawIncludedButton(e, isIncluded, partialIncluded, localParentPackage, out var activeColor);
		DrawButtons(e, isPressed, localParentPackage, workshopInfo);

		if (Width / UI.FontScale >= 600)
		{
			if (workshopInfo?.Author is not null)
			{
				DrawAuthor(e, workshopInfo.Author, 0);
			}
			else if (e.Item.IsLocal)
			{
				DrawFolderName(e, localParentPackage!, 0);
			}

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

		if (!isIncluded && localPackage is not null && !e.HoverState.HasFlag(HoverState.Hovered))
		{
			using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
			e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(Padding));
		}
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<T, Rectangles> e)
	{
		if (CompactList)
		{
			OnPaintItemCompactList(e);

			return;
		}

		var localPackage = e.Item.LocalPackage;
		var localParentPackage = localPackage?.LocalParentPackage;
		var workshopInfo = e.Item.GetWorkshopInfo();
		var partialIncluded = false;
		var isPressed = false;
		var isIncluded = (localPackage is not null && _packageUtil.IsIncluded(e.Item.LocalPackage!, out partialIncluded)) || partialIncluded;

		var compatibilityReport = e.Item.GetCompatibilityInfo();
		var notificationType = compatibilityReport?.GetNotification();
		var hasStatus = GetStatusDescriptors(e.Item, out var statusText, out var statusIcon, out var statusColor);

		if (e.IsSelected)
		{
			e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
		}
		else if (!IsPackagePage && notificationType > NotificationType.Info)
		{
			e.BackColor = notificationType.Value.GetColor().MergeColor(FormDesign.Design.BackColor, 25);
		}
		else if (!IsPackagePage && hasStatus)
		{
			e.BackColor = statusColor.MergeColor(FormDesign.Design.BackColor).MergeColor(FormDesign.Design.BackColor, 25);
		}
		else if (e.HoverState.HasFlag(HoverState.Hovered))
		{
			e.BackColor = FormDesign.Design.AccentBackColor;
		}
		else
		{
			e.BackColor = BackColor;
		}

		if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)))
		{
			e.BackColor = e.BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);

			isPressed = e.HoverState.HasFlag(HoverState.Pressed);
		}

		base.OnPaintItemList(e);

		DrawThumbnail(e);
		DrawTitleAndTagsAndVersion(e, localParentPackage, workshopInfo, isPressed);
		DrawIncludedButton(e, isIncluded, partialIncluded, localParentPackage, out var activeColor);

		var scoreX = IsPackagePage ? 0 : DrawScore(e, workshopInfo);

		if (scoreX > 0)
		{
			scoreX += Padding.Horizontal;
		}

		if (!IsPackagePage)
		{
			if (workshopInfo?.Author is not null)
			{
				DrawAuthor(e, workshopInfo.Author, scoreX);
			}
			else if (e.Item.IsLocal)
			{
				DrawFolderName(e, localParentPackage!, scoreX);
			}
		}

		var maxTagX = DrawButtons(e, isPressed, localParentPackage, workshopInfo);

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

		DrawTags(e, maxTagX);

		e.Graphics.ResetClip();

		if (!isIncluded && localPackage is not null && !e.HoverState.HasFlag(HoverState.Hovered))
		{
			using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
			e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(Padding));
		}
	}

	private void DrawCompatibilityAndStatusList(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, NotificationType? notificationType, string? statusText, DynamicIcon? statusIcon, Color statusColor)
	{
		var height = CompactList ? (UI.Scale(18)) : (Math.Max(e.Rects.SteamRect.Y, e.Rects.FolderRect.Y) - e.ClipRectangle.Top - Padding.Vertical);

		if (notificationType > NotificationType.Info)
		{
			var point = CompactList
				? new Rectangle(_columnSizes[Columns.Status].X, e.ClipRectangle.Y, _columnSizes[Columns.Status].Width, height)
				: new Rectangle(e.ClipRectangle.Right - Padding.Right, e.ClipRectangle.Top + Padding.Top, 0, 0);

			e.Rects.CompatibilityRect = DrawLabel(e.Graphics,
				point,
				LocaleCR.Get(notificationType.ToString()),
				notificationType.Value.GetIcon(false),
				notificationType.Value.GetColor(),
				CompactList ? ContentAlignment.MiddleLeft : ContentAlignment.TopRight,
				CursorLocation);
		}

		if (statusText is not null && statusIcon is not null)
		{
			var point = CompactList
				? new Rectangle(notificationType > NotificationType.Info ? (e.Rects.CompatibilityRect.Right + Padding.Left) : _columnSizes[Columns.Status].X, e.ClipRectangle.Y + ((e.ClipRectangle.Height - height) / 2), _columnSizes[Columns.Status].Width, height)
				: new Rectangle(notificationType > NotificationType.Info ? (e.Rects.CompatibilityRect.X - Padding.Left) : e.ClipRectangle.Right - Padding.Right, e.ClipRectangle.Top + Padding.Top, 0, 0);

			e.Rects.DownloadStatusRect = DrawLabel(e.Graphics,
				point,
				notificationType > NotificationType.Info ? "" : statusText,
				statusIcon,
				statusColor,
				CompactList ? ContentAlignment.MiddleLeft : ContentAlignment.TopRight,
				null);
		}

		if (CompactList && Math.Max(e.Rects.CompatibilityRect.Right, e.Rects.DownloadStatusRect.Right) > (_columnSizes[Columns.Status].X + _columnSizes[Columns.Status].Width))
		{
			DrawSeam(e, _columnSizes[Columns.Status].X + _columnSizes[Columns.Status].Width);
		}
	}

	private enum Columns
	{
		PackageName,
		Version,
		UpdateTime,
		Author,
		Tags,
		Status,
		Buttons
	}

	private readonly Dictionary<Columns, (int X, int Width)> _columnSizes = new();


	protected override void OnPrePaint(PaintEventArgs e)
	{
		headers =
		[
			(Locale.Package, 0),
			(Locale.Version, 65),
			(Locale.UpdateTime, 120),
			(Locale.Author, 120),
			(Locale.IDAndTags, 0),
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

	private int DrawScore(ItemPaintEventArgs<T, Rectangles> e, IWorkshopInfo? workshopInfo)
	{
		var score = workshopInfo?.Score ?? -1;

		if (score != -1)
		{
			var clip = e.Graphics.ClipBounds;
			var padding = GridView ? GridPadding : Padding;
			var height = UI.Scale(GridView ? 22 : 16) - padding.Bottom;
			var labelH = height - padding.Top;
			var scoreRect = new Rectangle(e.Rects.TextRect.X + padding.Left, e.Rects.IconRect.Bottom - (GridView ? height - 2 : labelH), labelH, labelH);
			var small = UI.FontScale < 1.25;
			var backColor = score > 90 && workshopInfo!.Subscribers >= 50000 ? FormDesign.Modern.ActiveColor : FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.RedColor, score).MergeColor(FormDesign.Design.BackColor, 75);
			e.Rects.ScoreRect = scoreRect;

			if (!small)
			{
				e.Graphics.FillEllipse(new SolidBrush(backColor), scoreRect.Pad(score > 90 && workshopInfo!.Subscribers >= 50000 ? (int)UI.Scale(-1.5f) : 0));
			}
			else
			{
				scoreRect.Y--;
			}

			using var scoreFilled = IconManager.GetIcon("VoteFilled", scoreRect.Width * 3 / 4);

			if (score < 75)
			{
				using var scoreIcon = IconManager.GetIcon("Vote", scoreRect.Width * 3 / 4);

				e.Graphics.DrawImage(scoreIcon.Color(small ? backColor : backColor.GetTextColor()), scoreRect.CenterR(scoreIcon.Size));

				e.Graphics.SetClip(scoreRect.CenterR(scoreFilled.Size).Pad(0, scoreFilled.Height - (scoreFilled.Height * score / 105), 0, 0));
				e.Graphics.DrawImage(scoreFilled.Color(small ? backColor : backColor.GetTextColor()), scoreRect.CenterR(scoreFilled.Size));
				e.Graphics.SetClip(clip);
			}
			else
			{
				e.Graphics.DrawImage(scoreFilled.Color(small ? backColor : backColor.GetTextColor()), scoreRect.CenterR(scoreFilled.Size));
			}

			if (workshopInfo!.Subscribers < 50000 || score <= 90)
			{
				if (small)
				{
					using var scoreIcon = IconManager.GetIcon("Vote", scoreRect.Width * 3 / 4);

					e.Graphics.SetClip(scoreRect.CenterR(scoreIcon.Size).Pad(0, scoreIcon.Height - (scoreIcon.Height * workshopInfo!.Subscribers / 15000), 0, 0));
					e.Graphics.DrawImage(scoreIcon.Color(FormDesign.Modern.ActiveColor), scoreRect.CenterR(scoreIcon.Size));
					e.Graphics.SetClip(clip);
				}
				else
				{
					using var pen = new Pen(Color.FromArgb(score >= 75 ? 255 : 200, FormDesign.Modern.ActiveColor), (float)(1.5 * UI.FontScale)) { EndCap = LineCap.Round, StartCap = LineCap.Round };
					e.Graphics.DrawArc(pen, scoreRect.Pad(-1), 90 - (Math.Min(360, 360F * workshopInfo!.Subscribers / 15000) / 2), Math.Min(360, 360F * workshopInfo!.Subscribers / 15000));
				}
			}

			return scoreRect.Right - e.Rects.TextRect.X + Padding.Left;
		}

		return 0;
	}

	private void DrawIncludedButton(ItemPaintEventArgs<T, Rectangles> e, bool isIncluded, bool partialIncluded, ILocalPackageWithContents? package, out Color activeColor)
	{
		activeColor = default;

		if (package is null && e.Item.IsLocal)
		{
			return; // missing local item
		}

		var inclEnableRect = e.Rects.EnabledRect == Rectangle.Empty ? e.Rects.IncludedRect : Rectangle.Union(e.Rects.IncludedRect, e.Rects.EnabledRect);
		var incl = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item) ? "Wait" : partialIncluded ? "Slash" : isIncluded ? "Ok" : package is null ? "Add" : "Enabled");
		var mod = package?.Mod;
		var required = mod is not null && _modLogicManager.IsRequired(mod, _modUtil);
		var isHovered = !IsGenericPage && !required && isIncluded && (e.DrawableItem.Loading || (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.IncludedRect.Contains(CursorLocation)));

		DynamicIcon? enabl = null;
		if (_settings.UserSettings.AdvancedIncludeEnable && mod is not null)
		{
			enabl = new DynamicIcon(mod.IsEnabled() ? "Checked" : "Checked_OFF");

			if (isIncluded)
			{
				activeColor = partialIncluded ? FormDesign.Design.YellowColor : mod.IsEnabled() ? FormDesign.Design.GreenColor : FormDesign.Design.RedColor;
			}
			else if (mod.IsEnabled())
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
			iconColor = !FormDesign.Design.IsDarkTheme ? activeColor.MergeColor(ForeColor, 75) : activeColor;
			activeColor = activeColor.MergeColor(BackColor, !FormDesign.Design.IsDarkTheme ? 35 : 20);
		}
		else if (activeColor == default && inclEnableRect.Contains(CursorLocation))
		{
			activeColor = Color.FromArgb(20, ForeColor);
			iconColor = FormDesign.Design.ForeColor;
		}
		else
		{
			iconColor = activeColor.GetTextColor();
		}

		if (isHovered)
			activeColor = activeColor.MergeColor(e.BackColor);

		using var brush = inclEnableRect.Gradient(activeColor);

		e.Graphics.FillRoundedRectangle(brush, inclEnableRect, (int)(4 * UI.FontScale));

		using var includedIcon = incl.Get(e.Rects.IncludedRect.Height * 3 / 4).Color(iconColor);
		using var enabledIcon = enabl?.Get(e.Rects.IncludedRect.Height * 3 / 4).Color(iconColor);

		e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
		if (enabledIcon is not null)
		{
			e.Graphics.DrawImage(enabledIcon, e.Rects.EnabledRect.CenterR(includedIcon.Size));
		}
	}

	private ItemListControl<T>.Rectangles GenerateListRectangles(T item, Rectangle rectangle)
	{
		rectangle = rectangle.Pad(Padding.Left, 0, Padding.Right, 0);

		var rects = new Rectangles(item)
		{
			IconRect = CompactList ? default : rectangle.Align(new Size(rectangle.Height - Padding.Vertical, rectangle.Height - Padding.Vertical), ContentAlignment.MiddleLeft)
		};

		var includedSize = 28;

		if (_settings.UserSettings.AdvancedIncludeEnable && item.LocalParentPackage?.Mod is not null)
		{
			rects.EnabledRect = rects.IncludedRect = rectangle.Pad(Padding).Align(new Size((int)(includedSize * UI.FontScale), CompactList ? (int)(18 * UI.FontScale) : (rects.IconRect.Height / 2)), ContentAlignment.MiddleLeft);

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
			rects.IncludedRect = rectangle.Pad(Padding).Align(UI.Scale(new Size(includedSize, CompactList ? 18 : includedSize), UI.FontScale), ContentAlignment.MiddleLeft);
		}

		if (CompactList)
		{
			rects.TextRect = new Rectangle(_columnSizes[Columns.PackageName].X, rectangle.Y, _columnSizes[Columns.PackageName].Width, rectangle.Height).Pad(Math.Max(rects.IncludedRect.Right, rects.EnabledRect.Right) + Padding.Horizontal, 0, 0, 0);
		}
		else
		{
			rects.IconRect.X += rects.IncludedRect.Right + Padding.Horizontal;

			rects.TextRect = rectangle.Pad(rects.IconRect.Right + Padding.Left, 0, IsPackagePage ? 0 : (int)(200 * UI.FontScale), rectangle.Height).AlignToFontSize(UI.Font(CompactList ? 8.25F : 9F, FontStyle.Bold), ContentAlignment.TopLeft);
		}

		rects.CenterRect = rects.TextRect.Pad(-Padding.Horizontal, 0, 0, 0);

		return rects;
	}

	public static Rectangle DrawLabel(
		Graphics graphics, Rectangle container, string text, DynamicIcon icon, Color? color = null,
		ContentAlignment alignment = ContentAlignment.TopLeft, Point? cursorLocation = null, bool large = false)
	{
		text = text.ToUpper();

		using var font = large ? UI.Font(8.25F) : UI.Font(7F);
		using var image = icon.Get(font.Height + UI.Scale(2));
		var padding = UI.Scale(new Padding(2));
		var textSize = Size.Ceiling(graphics.Measure(text.IfEmpty("A"), font)) + (large ? new Size(0, padding.Vertical) : Size.Empty);
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
