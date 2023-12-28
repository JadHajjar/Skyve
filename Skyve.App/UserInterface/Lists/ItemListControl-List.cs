using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl<T>
{
	private void OnPaintItemCompactList(ItemPaintEventArgs<T, Rectangles> e)
	{
		var package = e.Item.GetPackage();
		var workshopInfo = e.Item.GetWorkshopInfo();
		var isPressed = false;
		var isIncluded = e.Item.IsIncluded(out _, out var partialIncluded) || partialIncluded;

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

		DrawTitleAndTagsAndVersionForList(e, package?.LocalData, workshopInfo, isPressed);
		DrawIncludedButton(e, isIncluded, partialIncluded, package?.LocalData, out var activeColor);

		if (workshopInfo?.Author is not null)
		{
			DrawAuthor(e, workshopInfo.Author, 0);
		}
		else if (e.Item.IsLocal())
		{
			DrawFolderName(e, package!.LocalData!, 0);
		}

		DrawButtons(e, isPressed, package?.LocalData, workshopInfo);

		DrawCompatibilityAndStatusList(e, notificationType, statusText, statusIcon, statusColor);

		DrawTags(e, _columnSizes[Columns.Tags].X + _columnSizes[Columns.Tags].Width);

		e.Graphics.ResetClip();

		if (!isIncluded && package?.LocalData is not null && !e.HoverState.HasFlag(HoverState.Hovered))
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

		var package = e.Item.GetPackage();
		var workshopInfo = e.Item.GetWorkshopInfo();
		var isPressed = false;
		var isIncluded = e.Item.IsIncluded(out _, out var partialIncluded) || partialIncluded;

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
		DrawTitleAndTagsAndVersionForList(e, package?.LocalData, workshopInfo, isPressed);
		DrawIncludedButton(e, isIncluded, partialIncluded, package?.LocalData, out var activeColor);

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
			else if (e.Item.IsLocal())
			{
				DrawFolderName(e, package!.LocalData!, scoreX);
			}
		}

		var maxTagX = DrawButtons(e, isPressed, package?.LocalData, workshopInfo);

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

		if (!isIncluded && package?.LocalData is not null && !e.HoverState.HasFlag(HoverState.Hovered))
		{
			using var brush = new SolidBrush(Color.FromArgb(85, BackColor));
			e.Graphics.FillRectangle(brush, e.ClipRectangle.InvertPad(Padding));
		}
	}

	private void DrawCompatibilityAndStatusList(ItemPaintEventArgs<T, ItemListControl<T>.Rectangles> e, NotificationType? notificationType, string? statusText, DynamicIcon? statusIcon, Color statusColor)
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

	protected override void DrawHeader(PaintEventArgs e)
	{
		var headers = new (string text, int width)[]
		{
			(Locale.Package, 0),
			(Locale.Version, 65),
			(Locale.UpdateTime, 120),
			(Locale.Author, 120),
			(Locale.IDAndTags, 0),
			(Locale.Status, 160),
			("", 80)
		};

		var remainingWidth = Width - (int)(headers.Sum(x => x.width) * UI.FontScale);
		var autoColumns = headers.Count(x => x.width == 0);
		var xPos = 0;

		using var font = UI.Font(7.5F, FontStyle.Bold);
		using var brush = new SolidBrush(FormDesign.Design.LabelColor);
		using var lineBrush = new SolidBrush(FormDesign.Design.AccentColor);

		e.Graphics.Clear(FormDesign.Design.AccentBackColor);

		e.Graphics.FillRectangle(lineBrush, new Rectangle(0, StartHeight - 2, Width, 2));

		for (var i = 0; i < headers.Length; i++)
		{
			var header = headers[i];

			var width = header.width == 0 ? (remainingWidth / autoColumns) : (int)(header.width * UI.FontScale);

			e.Graphics.DrawString(header.text.ToUpper(), font, brush, new Rectangle(xPos, 1, width, StartHeight).Pad(Padding).AlignToFontSize(font, ContentAlignment.MiddleLeft), new StringFormat { LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter });

			_columnSizes[(Columns)i] = (xPos, width);

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
			var height = e.Rects.IconRect.Bottom - Math.Max(e.Rects.TextRect.Bottom, Math.Max(e.Rects.VersionRect.Bottom, e.Rects.DateRect.Bottom)) - padding.Bottom;
			var labelH = e.Rects.IconRect.Bottom - Math.Max(e.Rects.TextRect.Bottom, Math.Max(e.Rects.VersionRect.Bottom, e.Rects.DateRect.Bottom)) - padding.Horizontal;
			var scoreRect = new Rectangle(e.Rects.TextRect.X + (GridView ? 0 : padding.Left), e.Rects.IconRect.Bottom - height + (height / 2) - (labelH / 2) + 1, labelH, labelH);
			var small = UI.FontScale < 1.25;
			var backColor = score > 90 && workshopInfo!.Subscribers >= 50000 ? FormDesign.Modern.ActiveColor : FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.RedColor, score).MergeColor(FormDesign.Design.BackColor, 75);
			e.Rects.ScoreRect = scoreRect;

			if (!small)
			{
				e.Graphics.FillEllipse(new SolidBrush(backColor), scoreRect);
			}
			else
			{
				scoreRect.Y--;
			}

			using var scoreFilled = IconManager.GetIcon("I_VoteFilled", scoreRect.Width * 3 / 4);

			if (score < 75)
			{
				using var scoreIcon = IconManager.GetIcon("I_Vote", scoreRect.Width * 3 / 4);

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
					using var scoreIcon = IconManager.GetIcon("I_Vote", scoreRect.Width * 3 / 4);

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

			return labelH + Padding.Left;
		}

		return 0;
	}

	private void DrawIncludedButton(ItemPaintEventArgs<T, Rectangles> e, bool isIncluded, bool partialIncluded, ILocalPackageData? package, out Color activeColor)
	{
		activeColor = default;

		if (package is null && e.Item.IsLocal())
		{
			return; // missing local item
		}

		var inclEnableRect = e.Rects.EnabledRect == Rectangle.Empty ? e.Rects.IncludedRect : Rectangle.Union(e.Rects.IncludedRect, e.Rects.EnabledRect);
		var incl = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item) ? "I_Wait" : partialIncluded ? "I_Slash" : isIncluded ? "I_Ok" : package is null ? "I_Add" : "I_Enabled");
		var required = package is not null && _modLogicManager.IsRequired(package, _modUtil);

		DynamicIcon? enabl = null;
		if (_settings.UserSettings.AdvancedIncludeEnable && package is not null)
		{
			enabl = new DynamicIcon(package.IsEnabled() ? "I_Checked" : "I_Checked_OFF");

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
			activeColor = Color.FromArgb(20, ForeColor);
			iconColor = FormDesign.Design.ForeColor;
		}
		else
		{
			iconColor = activeColor.GetTextColor();
		}

		using var brush = inclEnableRect.Gradient(activeColor);

		e.Graphics.FillRoundedRectangle(brush, inclEnableRect, (int)(4 * UI.FontScale));

		using var includedIcon = incl.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);
		using var enabledIcon = enabl?.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);

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

		if (_settings.UserSettings.AdvancedIncludeEnable && item.GetPackage()?.IsCodeMod == true)
		{
			rects.EnabledRect = rects.IncludedRect = rectangle.Pad(Padding).Align(new Size((int)(includedSize * UI.FontScale), CompactList ? (int)(22 * UI.FontScale) : (rects.IconRect.Height / 2)), ContentAlignment.MiddleLeft);

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
			rects.IncludedRect = rectangle.Pad(Padding).Align(UI.Scale(new Size(includedSize, CompactList ? 22 : includedSize), UI.FontScale), ContentAlignment.MiddleLeft);
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
}
