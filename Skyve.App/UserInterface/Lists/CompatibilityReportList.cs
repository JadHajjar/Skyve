using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Panels;
using Skyve.App.Utilities;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

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
	private readonly ICompatibilityActionsHelper _compatibilityActions;

	private PackageSorting sorting;
	private bool headerHovered;
	private readonly Dictionary<IPackageIdentity, int> _modHeights = [];
	private readonly Dictionary<NotificationType, Rectangle> _headerRects = [];

	public bool SortDescending { get; private set; }
	public NotificationType CurrentGroup { get; private set; }

	public event EventHandler? GroupChanged;
	public event EventHandler? FilterRequested;

	public CompatibilityReportList()
	{
		GridView = true;
		DynamicSizing = true;
		AllowDrop = true;

		ServiceCenter.Get(out _subscriptionsManager, out _compatibilityActions, out _compatibilityManager, out _packageUtil, out _dlcManager, out _settings, out _modUtil, out _modLogicManager, out _workshopService, out _packageNameUtil);
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

		Padding = UI.Scale(new Padding(8), UI.FontScale);
		GridPadding = UI.Scale(new Padding(5), UI.FontScale);
		StartHeight = (int)(44 * UI.FontScale);
	}

	protected override void CanDrawItemInternal(CanDrawItemEventArgs<ICompatibilityInfo> args)
	{
		args.DoNotDraw = CurrentGroup != NotificationType.None && args.Item.GetNotification() != CurrentGroup;

		base.CanDrawItemInternal(args);
	}

	public override void SetItems(IEnumerable<ICompatibilityInfo> items)
	{
		if (CurrentGroup != NotificationType.None && items.All(x => x.GetNotification() != CurrentGroup))
		{
			CurrentGroup = NotificationType.None;
		}

		base.SetItems(items);

		DoFilterChanged();

		GroupChanged?.Invoke(this, EventArgs.Empty);
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

	protected override IEnumerable<DrawableItem<ICompatibilityInfo, Rectangles>> OrderItems(IEnumerable<DrawableItem<ICompatibilityInfo, Rectangles>> items)
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

	protected override void DrawHeader(PaintEventArgs e)
	{
		if (ItemCount == 0)
		{
			return;
		}

		var items = Items.GroupBy(x => x.GetNotification()).OrderByDescending(x => x.Key).ToList();

		items.Insert(0, Items.GroupBy(x => NotificationType.None).FirstOrDefault());

		var smaller = Width < items.Count * 100 * UI.FontScale;
		var xpos = 0;

		using var brush = new SolidBrush(FormDesign.Design.AccentBackColor);
		using var accentBrush = new SolidBrush(FormDesign.Design.AccentColor);

		_headerRects.Clear();

		e.Graphics.FillRectangle(brush, new Rectangle(0, 0, Width, StartHeight));

		foreach (var item in items)
		{
			int width;

			if (!smaller)
			{
				width = Math.Min(Width / items.Count, (int)(250 * UI.FontScale));
			}
			else
			{
				width = StartHeight;
			}

			var rectangle = new Rectangle(xpos, 0, width, StartHeight).Pad(Padding);

			_headerRects[item.Key] = rectangle;

			var text = (item.Key == NotificationType.None ? LocaleHelper.GetGlobalText("All") : LocaleCR.Get(item.Key.ToString())) + $" ({item.Count()})";
			using var icon = (item.Key == NotificationType.None ? "CompatibilityReport" : item.Key.GetIcon(true)).Get((StartHeight - Padding.Vertical) * 3 / 4);
			using var font = UI.Font(9.75F, CurrentGroup == item.Key ? FontStyle.Bold : FontStyle.Regular).FitToWidth(text, rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0), e.Graphics);

			var baseColor = item.Key == NotificationType.None ? FormDesign.Design.ActiveColor : item.Key.GetColor();
			using var foreBrush = new SolidBrush(CurrentGroup == item.Key ? baseColor.GetTextColor() : baseColor.MergeColor(ForeColor, 80));
			var textSize = e.Graphics.Measure(text, font, rectangle.Width - icon.Width - Padding.Left);
			var textBounds = rectangle.CenterR(Size.Ceiling(textSize));
			var iconBounds = textBounds;

			if (CurrentGroup == item.Key)
			{
				e.Graphics.FillRoundedRectangleWithShadow(rectangle, GridPadding.Left, Padding.Left, Color.FromArgb(200, baseColor), Color.FromArgb(6, baseColor));
			}
			else if (rectangle.Contains(CursorLocation) && HoverState.HasFlag(HoverState.Hovered))
			{
				using var backBrush = new SolidBrush(Color.FromArgb(50, baseColor));
				e.Graphics.FillRoundedRectangle(backBrush, rectangle, GridPadding.Left);
			}

			if (!smaller || CurrentGroup == item.Key)
			{
				textBounds.X += (icon.Width + Padding.Left) / 2;
				iconBounds.X -= (icon.Width + Padding.Left) / 2;
				iconBounds.Y = (StartHeight - icon.Height) / 2;
				iconBounds.Width = iconBounds.Height = icon.Height;

				e.Graphics.DrawString(text, font, foreBrush, textBounds, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
			}
			else
			{
				iconBounds = rectangle.CenterR(icon.Size);
			}

			e.Graphics.DrawImage(icon.Color(foreBrush.Color), iconBounds);

			xpos += rectangle.Width+Padding.Horizontal;

			e.Graphics.FillRectangle(accentBrush, new Rectangle(xpos - 1, Padding.Top, 2, StartHeight - Padding.Vertical));
		}

		e.Graphics.FillRectangle(accentBrush, new Rectangle(0, StartHeight - (int)UI.FontScale, Width, (int)UI.FontScale));
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		var headerHovered = e.Y.IsWithin(0, StartHeight + 1);

		if (headerHovered || headerHovered != this.headerHovered)
		{
			Invalidate(new Rectangle(0, 0, Width, StartHeight));
		}

		this.headerHovered = headerHovered;

		base.OnMouseMove(e);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		Invalidate(new Rectangle(0, 0, Width, StartHeight));

		base.OnMouseLeave(e);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		foreach (var item in _headerRects)
		{
			if (item.Value.Contains(e.Location))
			{
				CurrentGroup = item.Key;
				DoFilterChanged();
				GroupChanged?.Invoke(this, EventArgs.Empty);
				return;
			}
		}

		base.OnMouseClick(e);
	}

	protected override bool IsHeaderActionHovered(Point location)
	{
		return _headerRects.Values.Any(x => x.Contains(location));
	}

	protected override void OnPaintItemGrid(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e)
	{
		var workshopInfo = e.Item.GetWorkshopInfo();
		var message = e.Item.ReportItems?.Any() == true ? e.Item.ReportItems.OrderBy(x => _compatibilityManager.IsSnoozed(x) ? 0 : x.Status.Notification).LastOrDefault() : null;
		var clip = e.ClipRectangle;

		e.BackColor = BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 3f : -3f);
		e.Graphics.ResetClip();
		e.Graphics.FillRoundedRectangleWithShadow(e.ClipRectangle.InvertPad(GridPadding), GridPadding.Left, Padding.Right / 2, e.BackColor, message is null ? null : Color.FromArgb(6, message.Status.Notification.GetColor()), addOutline: true);
		e.Graphics.SetClip(clip);

		DrawThumbnail(e);
		DrawTitleAndTags(e);

		if (workshopInfo is not null)
		{
			e.Rects.AuthorRect = DrawAuthor(e, workshopInfo, e.Rects.CenterRect.Pad(GridPadding.Left / 2, e.Rects.TextRect.Height + GridPadding.Top, 0, 0), CursorLocation);
		}

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
			using var generic = IconManager.GetIcon(e.Item.IsLocal() ? "Mods" : "Paradox", e.Rects.IconRect.Height).Color(e.BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, e.Rects.IconRect, (int)(5 * UI.FontScale));
			e.Graphics.DrawImage(generic, e.Rects.IconRect.CenterR(generic.Size));
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
			e.Graphics.FillRoundedRectangle(brush, e.Rects.IconRect, (int)(5 * UI.FontScale));
		}

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, e.Rects.IconRect, (int)(5 * UI.FontScale), FormDesign.Design.BackColor);
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

	private void DrawReport(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, ICompatibilityItem Message)
	{
		var color = Message.Status.Notification.GetColor().Tint(Lum: FormDesign.Design.IsDarkTheme ? 6 : -6);
		var isSnoozed = _compatibilityManager.IsSnoozed(Message);
		var text = Message.GetMessage(_workshopService, _packageNameUtil);
		var note = string.IsNullOrWhiteSpace(Message.Status.Note) ? null : LocaleCRNotes.Get(Message.Status.Note!).One;
		var rectangle = e.ClipRectangle.Pad(GridPadding);
		var cursor = PointToClient(Cursor.Position);

		rectangle.Y = e.Rects.IconRect.Bottom + (GridPadding.Top * 3);

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

		e.Graphics.DrawString(text, font, brush, rectangle.Pad(GridPadding.Horizontal, 0, 0, 0));
		rectangle.Y += GridPadding.Top + (int)e.Graphics.Measure(text, font, rectangle.Width - GridPadding.Horizontal).Height;

		e.Graphics.FillRoundedRectangle(barBrush, Rectangle.FromLTRB(e.ClipRectangle.X + GridPadding.Left, e.Rects.IconRect.Bottom + GridPadding.Vertical, e.ClipRectangle.X + GridPadding.Left + GridPadding.Left, rectangle.Y), GridPadding.Left / 2);

		if (note is not null)
		{
			e.Graphics.DrawString(note, smallFont, fadedBrush, rectangle.Pad(GridPadding.Top + GridPadding.Horizontal, -GridPadding.Left, 0, 0));
			rectangle.Y += GridPadding.Top - GridPadding.Left + (int)e.Graphics.Measure(note, smallFont, rectangle.Width - GridPadding.Top - GridPadding.Horizontal).Height;
		}

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
			var bounds = new Rectangle(rectangle.X + (index * columnWidth), rectangle.Y + currentY[index], columnWidth, _modHeights.TryGet(item).If(0, columnWidth * 10 / 8));

			currentY[index] += (e.Rects._modHeights[item] = _modHeights[item] = DrawPackage(e, Message, item, bounds, cursor)) + GridPadding.Top;

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

	private int DrawPackage(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, ICompatibilityItem Message, IPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		e.Graphics.FillRoundedRectangleWithShadow(rectangle.Pad(GridPadding.Left), (int)(5 * UI.FontScale), GridPadding.Left);

		var thumbRect = rectangle.ClipTo(rectangle.Width).Pad(GridPadding.Left + GridPadding.Top);
		var textRect = rectangle.Pad(GridPadding.Left + GridPadding.Top, GridPadding.Vertical + thumbRect.Height + GridPadding.Top, GridPadding.Left + GridPadding.Top, GridPadding.Bottom);

		DrawThumbnail(e, package, thumbRect, cursor);

		rectangle.Height = thumbRect.Width + GridPadding.Vertical;

		rectangle.Height += DrawTitleAndTags(e, package, textRect, cursor) + GridPadding.Top;

		rectangle.Height += DrawAuthor(e, package, new Rectangle(textRect.X, rectangle.Bottom, textRect.Width, textRect.Height), cursor).Height + (GridPadding.Top / 3);

		e.Rects._modRects[package] = thumbRect;

		var action = _compatibilityActions.GetAction(Message, package);

		if (action != null)
		{
			using var buttonArgs = new ButtonDrawArgs
			{
				Cursor = cursor,
				BorderRadius = GridPadding.Left,
				Padding = UI.Scale(new Padding(4, 2, 4, 2), UI.FontScale),
				Rectangle = new Rectangle(0, 0, textRect.Width, (int)(24 * UI.FontScale)),
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

		return rectangle.Height + (GridPadding.Left * 2);
	}

	private int DrawThumbnail(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, IPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		var thumbnail = package.GetThumbnail();

		if (thumbnail is null)
		{
			using var generic = IconManager.GetIcon(package.IsCodeMod() ? "Mods" : "Package", rectangle.Height).Color(BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, rectangle, (int)(5 * UI.FontScale));
			e.Graphics.DrawImage(generic, rectangle.CenterR(generic.Size));
		}
		else if (package.IsLocal())
		{
			using var unsatImg = new Bitmap(thumbnail, rectangle.Size).Tint(Sat: 0);

			drawThumbnail(unsatImg);
		}
		else
		{
			drawThumbnail(thumbnail);
		}

		if (HoverState.HasFlag(HoverState.Hovered) && rectangle.Contains(cursor))
		{
			using var brush = new SolidBrush(Color.FromArgb(75, FormDesign.Design.ForeColor));
			e.Graphics.FillRoundedRectangle(brush, rectangle, (int)(5 * UI.FontScale));
		}

		return rectangle.Height + (int)(16 * UI.FontScale);

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, rectangle, (int)(5 * UI.FontScale), FormDesign.Design.BackColor);
	}

	private int DrawTitleAndTags(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, IPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		var dotsRect = rectangle.Align(UI.Scale(new Size(16, 22), UI.FontScale), ContentAlignment.TopRight);
		e.Rects._modDotsRects[package] = dotsRect;
		DrawDots(e, dotsRect, cursor);

		rectangle = rectangle.Pad(0, 0, dotsRect.Width, 0);

		var text = package.CleanName(out var tags);

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

	private Rectangle DrawAuthor(PaintEventArgs e, IPackageIdentity package, Rectangle rect, Point cursor)
	{
		var author = package.GetWorkshopInfo()?.Author;

		if (author is null)
		{
			return default;
		}

		using var authorFont = UI.Font(6.75F, FontStyle.Regular);
		using var authorFontUnderline = UI.Font(6.75F, FontStyle.Underline);
		using var stringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

		var size = e.Graphics.Measure(author.Name, authorFont).ToSize();

		using var authorIcon = IconManager.GetIcon("Author", size.Height);

		var authorRect = rect.Align(size + new Size(authorIcon.Width, 0), ContentAlignment.TopLeft);

		var isHovered = authorRect.Contains(cursor);
		using var brush = new SolidBrush(isHovered ? FormDesign.Design.ActiveColor : Color.FromArgb(200, ForeColor));

		e.Graphics.DrawImage(authorIcon.Color(brush.Color, brush.Color.A), authorRect.Align(authorIcon.Size, ContentAlignment.MiddleLeft));
		e.Graphics.DrawString(author.Name, isHovered ? authorFontUnderline : authorFont, brush, authorRect, stringFormat);

		return authorRect;
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

		if (rects.CenterRect.Contains(e.Location) || rects.IconRect.Contains(e.Location))
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
				else if (e.Button == MouseButtons.Right)
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

	private async Task Invoke(DrawableItem<ICompatibilityInfo, Rectangles> item, ICompatibilityItem Message, ICompatibilityActionInfo action, IPackageIdentity? package = null)
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

		if (drgevent.Data.GetDataPresent(DataFormats.FileDrop))
		{
			var file = ((string[])drgevent.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();

			if (Path.GetExtension(file).ToLower() is ".zip" or ".json")
			{
				drgevent.Effect = DragDropEffects.Copy;
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

		Invalidate();
	}

	internal void Next()
	{
		CurrentGroup = _headerRects.Keys.Next(CurrentGroup, true);
		DoFilterChanged();
		GroupChanged?.Invoke(this, EventArgs.Empty);
	}

	internal void Previous()
	{
		CurrentGroup = _headerRects.Keys.Previous(CurrentGroup, true);
		DoFilterChanged();
		GroupChanged?.Invoke(this, EventArgs.Empty);
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

	protected override Rectangles GenerateRectangles(ICompatibilityInfo item, Rectangle rectangle)
	{
		var rects = new Rectangles(item)
		{
			IconRect = rectangle.Pad(GridPadding).Align(UI.Scale(new Size(35, 35), UI.FontScale), ContentAlignment.TopLeft)
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

		public readonly Dictionary<IPackageIdentity, (Rectangle Rectangle, ICompatibilityActionInfo Action)> _actionRects = [];
		public readonly Dictionary<IPackageIdentity, Rectangle> _modRects = [];
		public readonly Dictionary<IPackageIdentity, Rectangle> _modDotsRects = [];
		public readonly Dictionary<IPackageIdentity, int> _modHeights = [];
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
