using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Panels;
using Skyve.App.Utilities;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Skyve.App.UserInterface.Lists.ItemListControl;

namespace Skyve.App.UserInterface.Lists;
public class CompatibilityReportList : SlickStackedListControl<ICompatibilityInfo, CompatibilityReportList.Rectangles>
{
	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IDlcManager _dlcManager;
	private readonly ISettings _settings;
	private readonly IModLogicManager _modLogicManager;
	private readonly IModUtil _modUtil;
	private readonly IWorkshopService _workshopService;
	private readonly IPackageNameUtil _packageNameUtil;
	private readonly IImageService _imageService;
	private readonly IUserService _userService;
	private readonly ICompatibilityActionsHelper _compatibilityActions;

	private PackageSorting sorting;
	private bool dragActive;

	public bool AllStatuses { get; set; }
	public bool SortDescending { get; private set; }

	public event EventHandler? FilterRequested;

	public CompatibilityReportList()
	{
		GridView = true;
		DynamicSizing = true;
		AllowDrop = true;

		ServiceCenter.Get(out _subscriptionsManager, out _compatibilityActions, out _compatibilityManager, out _packageUtil, out _dlcManager, out _settings, out _modUtil, out _modLogicManager, out _workshopService, out _packageNameUtil, out _imageService, out _userService);
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (Live && _settings.UserSettings.PageSettings.ContainsKey(SkyvePage.CompatibilityReport))
		{
			sorting = (PackageSorting)_settings.UserSettings.PageSettings[SkyvePage.CompatibilityReport].Sorting;
			SortDescending = _settings.UserSettings.PageSettings[SkyvePage.CompatibilityReport].DescendingSort;
		}
	}

	protected override void UIChanged()
	{
		GridItemSize = new Size(400, 380);

		base.UIChanged();

		Padding = UI.Scale(new Padding(8));
		GridPadding = UI.Scale(new Padding(5));
	}

	public override void SetItems(IEnumerable<ICompatibilityInfo> items)
	{
		base.SetItems(items);

		DoFilterChanged();
	}

	public void SetSorting(PackageSorting packageSorting, bool desc)
	{
		if (sorting == packageSorting && SortDescending == desc)
		{
			return;
		}

		SortDescending = desc;
		sorting = packageSorting;

		if (!IsHandleCreated)
		{
			SortingChanged();
		}
		else
		{
			new BackgroundAction(() => SortingChanged()).Run();
		}
	}

	protected override IEnumerable<IDrawableItem<ICompatibilityInfo>> OrderItems(IEnumerable<IDrawableItem<ICompatibilityInfo>> items)
	{
		items = sorting switch
		{
			PackageSorting.FileSize => items
				.OrderBy(x => x.Item.GetLocalPackage()?.FileSize),

			PackageSorting.Name => items
				.OrderBy(x => x.Item.ToString()),

			PackageSorting.Author => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.Author?.Name ?? string.Empty),

			PackageSorting.Status => items
				.OrderBy(x => _packageUtil.GetStatus(x.Item, out _)),

			PackageSorting.UpdateTime => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.ServerTime ?? x.Item.GetLocalPackage()?.LocalTime ?? default),

			PackageSorting.SubscribeTime => items
				.OrderBy(x => x.Item.GetLocalPackage()?.LocalTime),

			PackageSorting.Mod => items
				.OrderBy(x => Path.GetFileName(x.Item.GetLocalPackage()?.FilePath ?? string.Empty)),

			PackageSorting.None => items,

			PackageSorting.CompatibilityReport => items
				.OrderByDescending(x => x.Item.GetNotification())
				.ThenBy(x => x.Item.ToString()),

			PackageSorting.Subscribers => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.Subscribers),

			PackageSorting.Votes => items
				.OrderBy(x => x.Item.GetWorkshopInfo()?.VoteCount),

			PackageSorting.LoadOrder => items
				.OrderBy(x => !x.Item.IsIncluded())
				.ThenByDescending(x => x.Item.GetPackage() is null ? 0 : _modUtil.GetLoadOrder(x.Item.GetPackage()!))
				.ThenBy(x => x.Item.ToString()),

			_ => items
				.OrderByDescending(x => x.Item.GetNotification())
				.ThenBy(x => !(x.Item.IsIncluded(out var partial) || partial))
				.ThenBy(x => x.Item.IsLocal())
				.ThenBy(x => !x.Item.GetLocalPackage()?.IsCodeMod)
				.ThenBy(x => x.Item.CleanName() ?? x.Item.CleanName())
		};

		if (SortDescending)
		{
			return items.Reverse();
		}

		return items;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);

		if (ItemCount == 0 && !Loading)
		{
			using var font = UI.Font(9.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.LabelColor);
			using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

			e.Graphics.ResetClip();
			e.Graphics.DrawString(Locale.NoCompatibilityIssues, font, brush, ClientRectangle, stringFormat);
		}

		if (dragActive)
		{
			var border = UI.Scale(16);
			var rectangle = ClientRectangle.Pad(border);
			using var font = UI.Font(11.75F, FontStyle.Italic);
			using var brush = new SolidBrush(FormDesign.Design.ForeColor);
			using var stringFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
			using var backBrush = new SolidBrush(Color.FromArgb(200, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor)));
			using var pen = new Pen(FormDesign.Design.ActiveColor, (float)(1.5 * UI.FontScale)) { DashStyle = DashStyle.Dash };

			e.Graphics.ResetClip();
			e.Graphics.FillRoundedRectangle(backBrush, rectangle, border);
			e.Graphics.DrawRoundedRectangle(pen, rectangle, border);
			e.Graphics.DrawString(Locale.DropImportFile.Format(Locale.CompatibilityReport), font, brush, ClientRectangle, stringFormat);
		}
	}

	protected override void OnPaintItemGrid(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e)
	{
		var workshopInfo = e.Item.GetWorkshopInfo();
		var message = e.Item.ReportItems?.Any() == true ? e.Item.ReportItems.OrderBy(x => _compatibilityManager.IsSnoozed(x) ? 0 : x.Status.Notification).LastOrDefault() : null;
		var clip = e.ClipRectangle;

		e.BackColor = BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 3f : -3f);
		e.Graphics.ResetClip();
		e.Graphics.FillRoundedRectangleWithShadow(e.ClipRectangle.InvertPad(GridPadding), GridPadding.Left, UI.Scale(6), e.BackColor, message is null ? null : Color.FromArgb(7, message.Status.Notification.GetColor().MergeColor(BackColor, AllStatuses ? 100 : 25)), addOutline: true);
		e.Graphics.SetClip(clip);

		DrawThumbnail(e);
		DrawTitleAndTags(e);

		if (workshopInfo is not null)
		{
			e.Rects.AuthorRect = DrawAuthor(e, workshopInfo, e.Rects.CenterRect.Pad(GridPadding.Left / 2, 0, 0, 0), CursorLocation);

			e.Rects.CenterRect.Y += e.Rects.AuthorRect.Height;
		}

		e.Rects.CenterRect.Y = Math.Max(e.Rects.CenterRect.Y, e.Rects.IconRect.Bottom);

		if (message is not null)
		{
			DrawReport(e, message);
		}
	}

	private void DrawThumbnail(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e)
	{
		if (!e.InvalidRects.Any(x => x.IntersectsWith(e.Rects.IconRect)))
		{
			return;
		}

		var thumbnail = e.Item.GetThumbnail();

		if (thumbnail is null)
		{
			thumbnail = e.Item.IsLocal()
				? e.Item is IAsset ? AssetThumbUnsat : e.Item.IsLocal() ? ModThumbUnsat : WorkshopThumbUnsat
				: e.Item is IAsset ? AssetThumb : e.Item.IsLocal() ? ModThumb : WorkshopThumb;

			if (thumbnail is not null)
			{
				drawThumbnail(thumbnail);
			}
		}
		else if (e.Item.IsLocal())
		{
			using var unsatImg = new Bitmap(thumbnail, e.Rects.IconRect.Size).Tint(Sat: 0);

			drawThumbnail(unsatImg);
		}
		else
		{
			drawThumbnail(thumbnail);
		}

		if (e.HoverState.HasFlag(HoverState.Hovered) && e.Rects.IconRect.Contains(CursorLocation))
		{
			using var brush = new SolidBrush(Color.FromArgb(75, 255, 255, 255));
			e.Graphics.FillRoundedRectangle(brush, e.Rects.IconRect, UI.Scale(5));
		}

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, e.Rects.IconRect, UI.Scale(5), FormDesign.Design.BackColor);
	}

	private void DrawTitleAndTags(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e)
	{
		var padding = GridView ? GridPadding : Padding;
		var text = e.Item.CleanName(out var tags);

		using var font = UI.Font(9.25F, FontStyle.Bold);
		var textRect = new Rectangle(e.Rects.TextRect.X, e.Rects.TextRect.Y, e.Rects.TextRect.Width, Height);

		var textSize = e.Graphics.Measure(text, font, textRect.Width);
		var oneLineSize = e.Graphics.Measure(text, font);
		var oneLine = textSize.Height == oneLineSize.Height;
		var tagRect = new Rectangle(e.Rects.TextRect.X + (oneLine ? (int)textSize.Width : 0), textRect.Y + (oneLine ? 0 : (int)textSize.Height), 0, (int)oneLineSize.Height);

		e.Rects.TextRect.Height = (int)textSize.Height + (GridPadding.Top / 3);
		e.Rects.CenterRect = e.Rects.TextRect.Pad(0, -GridPadding.Top, 0, 0);
		e.DrawableItem.CachedHeight = e.Rects.TextRect.Bottom;

		using var brushTitle = new SolidBrush(e.Rects.CenterRect.Contains(CursorLocation) && e.HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor : e.BackColor.GetTextColor());

		e.Graphics.DrawString(text, font, brushTitle, textRect);

		for (var i = 0; i < tags.Count; i++)
		{
			var tagSize = e.Graphics.MeasureLabel(tags[i].Text, null, smaller: !_settings.UserSettings.ComplexListUI);

			if (tagRect.X + tagSize.Width > e.Rects.TextRect.Right)
			{
				tagRect.Y += tagRect.Height;
				tagRect.X = e.Rects.TextRect.X;
				e.Rects.CenterRect.Y += tagRect.Height;
				e.DrawableItem.CachedHeight += tagRect.Height;
			}

			var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, smaller: !_settings.UserSettings.ComplexListUI);

			tagRect.X += padding.Left + rect.Width;
		}

		if (!oneLine && tags.Count > 0)
		{
			e.DrawableItem.CachedHeight += tagRect.Height;
			e.Rects.CenterRect.Y += tagRect.Height;
		}

		e.Rects.CenterRect.Y += e.Rects.TextRect.Height + GridPadding.Top;
	}

	private void DrawReport(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, ICompatibilityItem Message)
	{
		var color = Message.Status.Notification.GetColor().Tint(Lum: FormDesign.Design.IsDarkTheme ? 6 : -6);
		var isSnoozed = _compatibilityManager.IsSnoozed(Message);
		var text = Message.GetMessage(_workshopService, _packageNameUtil);
		var note = string.IsNullOrWhiteSpace(Message.Status.Note) ? null : LocaleCRNotes.Get(Message.Status.Note!).One;
		var rectangle = e.ClipRectangle.Pad(GridPadding);
		var cursor = PointToClient(Cursor.Position);

		rectangle.Y = e.Rects.CenterRect.Y + (GridPadding.Top * 3);

		using var barBrush = Gradient(color, e.ClipRectangle, 1.25f);
		using var icon = Message.Status.Notification.GetIcon(false).Default;
		using var brush = new SolidBrush(FormDesign.Design.ForeColor);
		using var fadedBrush = new SolidBrush(Color.FromArgb(200, FormDesign.Design.ForeColor));
		using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.ForeColor, 85));
		using var font = UI.Font(8.25F);
		using var smallFont = UI.Font(7.5F);
		using var tinyFont = UI.Font(7F, FontStyle.Bold);
		using var tinyUnderlineFont = UI.Font(7F, FontStyle.Bold | FontStyle.Underline);

		e.Graphics.DrawImage(icon.Color(color), e.ClipRectangle.Pad(GridPadding).Align(icon.Size, ContentAlignment.TopRight));

		if (string.IsNullOrEmpty(Message.Status.Header))
		{
			e.Graphics.DrawString(text, font, brush, rectangle.Pad(GridPadding.Horizontal, 0, 0, 0));
			rectangle.Y += GridPadding.Top + (int)e.Graphics.Measure(text, font, rectangle.Width - GridPadding.Horizontal).Height;
		}
		else
		{
			using var fontBold = UI.Font(9F, FontStyle.Bold);
			using var tinyFontNotBold = UI.Font(7F);

			e.Graphics.DrawString(LocaleHelper.GetGlobalText(Message.Status.Header).ToUpper(), tinyFontNotBold, fadedBrush, rectangle.Pad(GridPadding.Horizontal, 0, 0, 0));
			rectangle.Y += (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Message.Status.Header).ToUpper(), tinyFontNotBold, rectangle.Width - GridPadding.Horizontal).Height;

			e.Graphics.DrawString(text, fontBold, brush, rectangle.Pad(GridPadding.Horizontal, 0, 0, 0));
			rectangle.Y += GridPadding.Top + (int)e.Graphics.Measure(text, fontBold, rectangle.Width - GridPadding.Horizontal).Height;
		}

		if (note is not null)
		{
			e.Graphics.DrawString(note, smallFont, fadedBrush, rectangle.Pad(GridPadding.Top + GridPadding.Horizontal, -GridPadding.Left, 0, 0));
			rectangle.Y += GridPadding.Top - GridPadding.Left + (int)e.Graphics.Measure(note, smallFont, rectangle.Width - GridPadding.Top - GridPadding.Horizontal).Height;
		}

		e.Graphics.FillRoundedRectangle(barBrush, Rectangle.FromLTRB(e.ClipRectangle.X + GridPadding.Left, e.Rects.CenterRect.Y + GridPadding.Vertical, e.ClipRectangle.X + GridPadding.Left + GridPadding.Left, rectangle.Y), GridPadding.Left / 2);

		rectangle.Y += GridPadding.Top;

		if (Message.Status.Action is StatusAction.RequestReview)
		{
			e.Rects.bulkActionRect = SlickButton.AlignAndDraw(e.Graphics, rectangle, ContentAlignment.TopLeft, new ButtonDrawArgs
			{
				Text = LocaleCR.RequestReview,
				Icon = "RequestReview",
				Font = font,
				BackgroundColor = FormDesign.Design.BackColor,
				Cursor = cursor,
				HoverState = HoverState & ~HoverState.Focused,
			}).Rectangle;

			rectangle.Y += e.Rects.bulkActionRect.Height;
		}

		var bulkAction = _compatibilityActions.GetBulkAction(Message);

		if (bulkAction is not null)
		{
			e.Rects.bulkActionRect = SlickButton.AlignAndDraw(e.Graphics, rectangle.Pad(GridPadding.Top), ContentAlignment.TopCenter, new ButtonDrawArgs
			{
				Text = bulkAction.Text,
				Icon = bulkAction.Icon,
				Font = font,
				ActiveColor = bulkAction.Color,
				ButtonType = ButtonType.Active,
				Cursor = cursor,
				HoverState = HoverState & ~HoverState.Focused,
			}).Rectangle;

			rectangle.Y += e.Rects.bulkActionRect.Height + GridPadding.Vertical;
		}

		var preferredSize = Message.Packages.Count() > 3 ? 100 : 115;
		var columns = (int)Math.Max(1, Math.Floor(rectangle.Width / (preferredSize * UI.FontScale)));
		var columnWidth = rectangle.Width / columns;
		var currentY = new int[columns];
		var index = 0;

		foreach (var item in Message.Packages)
		{
			var bounds = new Rectangle(rectangle.X + (index * columnWidth), rectangle.Y + currentY[index], columnWidth, e.Rects._modHeights.TryGet(item).If(0, columnWidth * 10 / 8));

			currentY[index] += (e.Rects._modHeights[item] = DrawPackage(e, Message, item, bounds, cursor)) + GridPadding.Top;

			index = (index + 1) % columns;
		}

		rectangle.Y += currentY.Max() + GridPadding.Top;

		var bottomText = 0;
		var hasRecommendedAction = _compatibilityActions.HasRecommendedAction(Message);

		if (hasRecommendedAction)
		{
			e.Rects.recommendedActionRect = new Rectangle(rectangle.Location, e.Graphics.Measure(LocaleCR.ApplyRecommendedAction, tinyFont).ToSize());

			e.Graphics.DrawString(LocaleCR.ApplyRecommendedAction, e.Rects.recommendedActionRect.Contains(cursor) ? tinyUnderlineFont : tinyFont, activeBrush, e.Rects.recommendedActionRect.X, e.Rects.recommendedActionRect.Y - (GridPadding.Top / 2));

			rectangle.X += e.Rects.recommendedActionRect.Width + GridPadding.Horizontal;
			bottomText = e.Rects.recommendedActionRect.Height;
		}

		if (_compatibilityActions.CanSnooze(Message))
		{
			if (isSnoozed && Enabled)
			{
				using var fadeBrush = new SolidBrush(Color.FromArgb(125, BackColor));

				e.Graphics.FillRectangle(fadeBrush, e.ClipRectangle);
			}

			e.Rects.snoozeRect = new Rectangle(rectangle.Location, e.Graphics.Measure(isSnoozed ? Locale.UnSnooze : Locale.Snooze, tinyFont).ToSize());

			rectangle.X += e.Rects.snoozeRect.Width + GridPadding.Horizontal;
			bottomText = Math.Max(bottomText, e.Rects.snoozeRect.Height);

			using var purpleBrush = new SolidBrush(Color.FromArgb(isSnoozed ? 255 : 200, 100, 60, 220));
			e.Graphics.DrawString(isSnoozed ? Locale.UnSnooze : Locale.Snooze, e.Rects.snoozeRect.Contains(cursor) ? tinyUnderlineFont : tinyFont, purpleBrush, e.Rects.snoozeRect.X, e.Rects.snoozeRect.Y - (GridPadding.Top / 2));
		}

		{
			e.Rects.compatibilityReport = new Rectangle(rectangle.Location, e.Graphics.Measure(Locale.ViewCompatibilityReport, tinyFont).ToSize());

			bottomText = Math.Max(bottomText, e.Rects.compatibilityReport.Height);

			using var labelBrush = new SolidBrush(FormDesign.Design.LabelColor);
			e.Graphics.DrawString(Locale.ViewCompatibilityReport, e.Rects.compatibilityReport.Contains(cursor) ? tinyUnderlineFont : tinyFont, labelBrush, e.Rects.compatibilityReport.X, e.Rects.compatibilityReport.Y - (GridPadding.Top / 2));
		}

		if (e.DrawableItem.Loading)
		{
			using var fadeBrush = new SolidBrush(Color.FromArgb(125, e.BackColor));

			e.Graphics.FillRectangle(fadeBrush, e.ClipRectangle);

			DrawLoader(e.Graphics, e.ClipRectangle);
		}

		e.DrawableItem.CachedHeight = rectangle.Y - e.ClipRectangle.Y + bottomText + Padding.Vertical + GridPadding.Vertical;
	}

	private int DrawPackage(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, ICompatibilityItem Message, ICompatibilityPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		e.Graphics.FillRoundedRectangleWithShadow(rectangle.Pad(GridPadding.Left), UI.Scale(5), GridPadding.Left);

		var thumbRect = rectangle.ClipTo(package.IsDlc ? ((rectangle.Width * 215 / 460) + GridPadding.Left + GridPadding.Top) : rectangle.Width).Pad(GridPadding.Left + GridPadding.Top);
		var textRect = rectangle.Pad(GridPadding.Left + GridPadding.Top, GridPadding.Vertical + thumbRect.Height + GridPadding.Top, GridPadding.Left + GridPadding.Top, GridPadding.Bottom);

		DrawThumbnail(e, package, thumbRect, cursor);

		rectangle.Height = thumbRect.Height + GridPadding.Vertical;

		rectangle.Height += DrawTitleAndTags(e, package, textRect, cursor) + GridPadding.Top;

		rectangle.Height += DrawAuthor(e, package, new Rectangle(textRect.X, rectangle.Bottom, textRect.Width, textRect.Height), default).Height + (GridPadding.Top / 3);

		e.Rects._modRects[package] = thumbRect;

		var action = _compatibilityActions.GetAction(Message, package);

		if (action is null)
		{
			return rectangle.Height + (GridPadding.Left * 2);
		}

		using var buttonArgs = new ButtonDrawArgs
		{
			Cursor = cursor,
			BorderRadius = GridPadding.Left,
			Padding = UI.Scale(new Padding(4, 2, 4, 2)),
			Rectangle = new Rectangle(0, 0, textRect.Width, UI.Scale(24)),
			HoverState = HoverState & ~HoverState.Focused,
			Text = action.Text,
			Icon = action.Icon,
		};

		if (Message.Status.Action is StatusAction.ExcludeOther or StatusAction.UnsubscribeOther)
		{
			buttonArgs.ButtonType = ButtonType.Active;
		}
		else
		{
			buttonArgs.BackgroundColor = BackColor;
			buttonArgs.ButtonType = ButtonType.Dimmed;
		}

		SlickButton.PrepareLayout(e.Graphics, buttonArgs);

		buttonArgs.Rectangle = new Rectangle(textRect.X, rectangle.Bottom + GridPadding.Top, textRect.Width, buttonArgs.Rectangle.Height);

		e.Rects._actionRects[package] = (buttonArgs, action);

		SlickButton.SetUpColors(buttonArgs);

		SlickButton.DrawButton(e.Graphics, buttonArgs);

		return rectangle.Height + buttonArgs.Rectangle.Height + GridPadding.Vertical + GridPadding.Left;
	}

	private int DrawThumbnail(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, ICompatibilityPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		Bitmap? thumbnail;

		if (package.IsDlc)
		{
			thumbnail = _dlcManager.TryGetDlc(package.Id) is IThumbnailObject thumbnailObject ? thumbnailObject.GetThumbnail() : null;
		}
		else
		{
			thumbnail = package.GetThumbnail();
		}

		thumbnail ??= package.IsLocal()
			? package is IAsset ? AssetThumbUnsat : package.IsCodeMod() ? ModThumbUnsat : package.IsLocal() ? PackageThumbUnsat : WorkshopThumbUnsat
			: package is IAsset ? AssetThumb : package.IsCodeMod() ? ModThumb : package.IsLocal() ? PackageThumb : WorkshopThumb;

		if (thumbnail is not null)
		{
			e.Graphics.DrawRoundedImage(thumbnail, rectangle, UI.Scale(5), FormDesign.Design.BackColor);
		}

		if (HoverState.HasFlag(HoverState.Hovered) && rectangle.Contains(cursor))
		{
			using var brush = new SolidBrush(Color.FromArgb(75, FormDesign.Design.ForeColor));
			e.Graphics.FillRoundedRectangle(brush, rectangle, UI.Scale(5));
		}

		return rectangle.Height + UI.Scale(16);
	}

	private int DrawTitleAndTags(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, ICompatibilityPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		if (!package.IsDlc)
		{
			var dotsRect = rectangle.Align(UI.Scale(new Size(16, 22)), ContentAlignment.TopRight);
			e.Rects._modDotsRects[package] = dotsRect;
			DrawDots(e, dotsRect, cursor);

			rectangle = rectangle.Pad(0, 0, dotsRect.Width, 0);
		}

		var text = package.CleanName(out var tags);

		if (package.IsDlc)
		{
			tags.Clear();
			text = text.RegexRemove("^.+?- ").RegexRemove("(Content )?Creator Pack: ");
		}

		using var font = UI.Font(7.25F, FontStyle.Bold);
		var textRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

		var textSize = e.Graphics.Measure(text, font, textRect.Width);
		var oneLineSize = e.Graphics.Measure(text, font);
		var oneLine = textSize.Height == oneLineSize.Height;
		var tagRect = new Rectangle(rectangle.X + (oneLine ? (int)textSize.Width : 0), textRect.Y + (oneLine ? 0 : (int)textSize.Height), 0, (int)oneLineSize.Height);

		textRect.Height = rectangle.Height = (int)textSize.Height + (GridPadding.Top / 3);
		var finalY = rectangle.Height;

		using var brushTitle = new SolidBrush(FormDesign.Design.ForeColor);

		e.Graphics.DrawString(text, font, brushTitle, textRect);

		for (var i = 0; i < tags.Count; i++)
		{
			var tagSize = e.Graphics.MeasureLabel(tags[i].Text, null, smaller: true);

			if (tagRect.X + tagSize.Width > rectangle.Right)
			{
				tagRect.Y += tagRect.Height;
				tagRect.X = rectangle.X;
				finalY += tagRect.Height;
			}

			var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, smaller: true);

			tagRect.X += GridPadding.Left + rect.Width;
		}

		if (!oneLine && tags.Count > 0)
		{
			finalY += tagRect.Height;
		}

		return finalY;
	}

	private Rectangle DrawAuthor(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, IPackageIdentity package, Rectangle rect, Point cursor)
	{
		var author = package.GetWorkshopInfo()?.Author;
		var padding = GridView ? GridPadding : Padding;
		var authorRect = Rectangle.Empty;
		var authorImg = _workshopService.GetUser(author)?.GetThumbnail();

		if (author?.Name is not null and not "")
		{
			using var authorFont = UI.Font(7.5F, FontStyle.Regular);
			using var authorFontUnderline = UI.Font(7.5F, FontStyle.Underline);
			using var stringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

			var size = e.Graphics.Measure(author.Name, authorFont).ToSize();

			authorRect = rect.Align(size + new Size(size.Height, 0), ContentAlignment.TopLeft);

			var isHovered = authorRect.Contains(cursor) && e.HoverState.HasFlag(HoverState.Hovered);
			using var brush = new SolidBrush(isHovered ? FormDesign.Design.ActiveColor : Color.FromArgb(200, UserIcon.GetUserColor(author.Id?.ToString() ?? string.Empty, true)));

			DrawAuthorImage(e, author, authorRect.Align(new(size.Height, size.Height), ContentAlignment.MiddleLeft), brush.Color, isHovered);

			e.Graphics.DrawString(author.Name, isHovered ? authorFontUnderline : authorFont, brush, authorRect, stringFormat);
		}

		return authorRect;
	}

	private void DrawAuthorImage(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, IUser author, Rectangle rectangle, Color color, bool isHovered)
	{
		var image = _workshopService.GetUser(author).GetThumbnail();

		if (image != null)
		{
			e.Graphics.DrawRoundImage(image, rectangle.Pad((int)(1.5 * UI.FontScale)));

			if (isHovered)
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

	private void DrawDots(PaintEventArgs e, Rectangle rectangle, Point cursor)
	{
		var isHovered = rectangle.Contains(cursor);
		using var img = IconManager.GetIcon("VertialMore", rectangle.Height * 3 / 4).Color(isHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor);

		e.Graphics.DrawImage(img, rectangle.CenterR(img.Size));
	}

	protected override async void OnItemMouseClick(DrawableItem<ICompatibilityInfo, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		if (e.Button == MouseButtons.Right)
		{
			ShowRightClickMenu(item.Item);
			return;
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		var rects = item.Rectangles;

		if (rects.TextRect.Contains(e.Location) || rects.IconRect.Contains(e.Location))
		{
			if (e.Button == MouseButtons.Right)
			{
				ShowRightClickMenu(item.Item);
				return;
			}

			if (e.Button == MouseButtons.Left)
			{
				ServiceCenter.Get<IInterfaceService>().OpenPackagePage(item.Item.GetPackage() ?? (IPackageIdentity)item.Item, false);
				return;
			}
		}

		if (rects.DotsRect.Contains(e.Location))
		{
			ShowRightClickMenu(item.Item);
			return;
		}

		if (rects.AuthorRect.Contains(e.Location) && item.Item.GetWorkshopInfo()?.Author is IUser user)
		{
			Program.MainForm.PushPanel(new PC_UserPage(user));

			return;
		}

		var Message = item.Item.ReportItems?.Any() == true ? item.Item.ReportItems.OrderBy(x => _compatibilityManager.IsSnoozed(x) ? 0 : x.Status.Notification).LastOrDefault() : null;

		if (Message is null)
		{
			return;
		}

		foreach (var rect in rects._modRects)
		{
			if (rect.Value.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left)
				{
					if (Message.Type is ReportType.DlcMissing)
					{
						PlatformUtil.OpenUrl(rect.Key.Url);
					}
					else
					{
						ServiceCenter.Get<IInterfaceService>().OpenPackagePage(rect.Key, false);
					}
				}
				else if (e.Button == MouseButtons.Right && !rect.Key.IsDlc)
				{
					var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(rect.Key);

					SlickToolStrip.Show(Program.MainForm, items);
				}

				return;
			}
		}

		foreach (var rect in rects._modDotsRects)
		{
			if (rect.Value.Contains(e.Location))
			{
				var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(rect.Key);

				SlickToolStrip.Show(Program.MainForm, items);

				return;
			}
		}

		if (e.Button == MouseButtons.Left && rects.compatibilityReport.Contains(e.Location))
		{
			ServiceCenter.Get<IInterfaceService>().OpenPackagePage(item.Item, true);

			return;
		}

		if (e.Button == MouseButtons.Left && rects.snoozeRect.Contains(e.Location))
		{
			_compatibilityManager.ToggleSnoozed(Message);

			return;
		}

		if (e.Button == MouseButtons.Left)
		{
			foreach (var rect in rects._actionRects)
			{
				if (rect.Value.Rectangle.Contains(e.Location))
				{
					await Invoke(item, Message, rect.Value.Action, rect.Key);

					return;
				}
			}
		}

		if (e.Button == MouseButtons.Left && rects.recommendedActionRect.Contains(e.Location))
		{
			var action = _compatibilityActions.GetRecommendedAction(Message);

			if (action != null)
			{
				await Invoke(item, Message, action);
			}

			return;
		}

		if (e.Button == MouseButtons.Left && rects.bulkActionRect.Contains(e.Location))
		{
			if (Message.Status.Action is StatusAction.RequestReview)
			{
				return;
			}

			var action = _compatibilityActions.GetBulkAction(Message);

			if (action != null)
			{
				await Invoke(item, Message, action);
			}
		}
	}

	private async Task Invoke(IDrawableItem<ICompatibilityInfo> item, ICompatibilityItem Message, ICompatibilityActionInfo action, IPackageIdentity? package = null)
	{
		Loading = item.Loading = true;
		Enabled = false;

		try
		{
			await action.Invoke(Message, package);

			//_compatibilityManager.QuickUpdate(Message);
		}
		catch (Exception ex)
		{
			MessagePrompt.Show(ex, Locale.FailedToApplyChanges, form: Program.MainForm);
		}

		await Task.Delay(2000);

		Loading = item.Loading = false;
		Enabled = true;
	}

	public void ShowRightClickMenu(IPackageIdentity item)
	{
		var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(item);

		this.TryBeginInvoke(() => SlickToolStrip.Show(FindForm() as SlickForm, items));
	}

	protected override void OnDragEnter(DragEventArgs drgevent)
	{
		base.OnDragEnter(drgevent);

		dragActive = false;
		if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
		{
			var file = ((string[])drgevent.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();

			if (Path.GetExtension(file).ToLower() is ".zip" or ".json")
			{
				drgevent.Effect = DragDropEffects.Copy;
				dragActive = true;
				Invalidate();
			}

			return;
		}

		drgevent.Effect = DragDropEffects.None;
		Invalidate();
	}

	protected override void OnDragLeave(EventArgs e)
	{
		base.OnDragLeave(e);

		dragActive = false;
		Invalidate();
	}

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		base.OnDragDrop(drgevent);

		var file = ((string[])drgevent.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();

		if (file != null)
		{
			if (CrossIO.CurrentPlatform is not Platform.Windows)
			{
				var realPath = ServiceCenter.Get<IIOUtil>().ToRealPath(file);

				if (CrossIO.FileExists(realPath))
				{
					file = realPath!;
				}
			}

			(PanelContent.GetParentPanel(this) as PC_CompatibilityReport)!.Import(file);
		}

		dragActive = false;
		Invalidate();
	}

	public void DoFilterChanged()
	{
		base.FilterChanged();

		AutoInvalidate = !Loading && Items.Any() && !SafeGetItems().Any();
	}

	public override void FilterChanged()
	{
		if (!IsHandleCreated)
		{
			base.FilterChanged();
		}
		else
		{
			FilterRequested?.Invoke(this, EventArgs.Empty);
		}
	}

	protected override IDrawableItemRectangles<ICompatibilityInfo> GenerateRectangles(ICompatibilityInfo item, Rectangle rectangle, IDrawableItemRectangles<ICompatibilityInfo> current)
	{
		var rects = new Rectangles(item)
		{
			IconRect = rectangle.Pad(GridPadding).Align(UI.Scale(new Size(35, 35)), ContentAlignment.TopLeft),
			_modHeights = (current as Rectangles)?._modHeights ?? []
		};

		rectangle = new Rectangle(rects.IconRect.X, rects.IconRect.Y, rectangle.Width - GridPadding.Horizontal, rects.IconRect.Height);

		rects.TextRect = rectangle.Pad(rects.IconRect.Right - rectangle.X + GridPadding.Left, 0, rects.IconRect.Width, rectangle.Height);

		return rects;
	}

	public class Rectangles : IDrawableItemRectangles<ICompatibilityInfo>
	{
		public Rectangle IconRect;
		public Rectangle TextRect;
		public Rectangle CenterRect;
		public Rectangle AuthorRect;
		public Rectangle DotsRect;

		public readonly Dictionary<ICompatibilityPackageIdentity, (Rectangle Rectangle, ICompatibilityActionInfo Action)> _actionRects = [];
		public readonly Dictionary<ICompatibilityPackageIdentity, Rectangle> _modRects = [];
		public readonly Dictionary<ICompatibilityPackageIdentity, Rectangle> _modDotsRects = [];
		public Dictionary<ICompatibilityPackageIdentity, int> _modHeights = [];
		public Rectangle bulkActionRect;
		public Rectangle recommendedActionRect;
		public Rectangle snoozeRect;
		public Rectangle compatibilityReport;

		public bool Enabled;

		public ICompatibilityInfo Item { get; set; }

		public Rectangles(ICompatibilityInfo item)
		{
			Item = item;
			Enabled = true;
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			text = string.Empty;
			point = default;
			return false;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return
				IconRect.Contains(location) ||
				TextRect.Contains(location) ||
				AuthorRect.Contains(location) ||
				snoozeRect.Contains(location) ||
				bulkActionRect.Contains(location) ||
				recommendedActionRect.Contains(location) ||
				compatibilityReport.Contains(location) ||
				_actionRects.Any(x => x.Value.Rectangle.Contains(location)) ||
				_modRects.Any(x => x.Value.Contains(location)) ||
				_modDotsRects.Any(x => x.Value.Contains(location));
		}
	}
}
