using Skyve.App.Interfaces;
using Skyve.App.Utilities;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Skyve.App.UserInterface.Lists.ItemListControl;

namespace Skyve.App.UserInterface.CompatibilityReport;

public class CompatibilityMessageControl : SlickControl
{
	private readonly Dictionary<ICompatibilityPackageIdentity, (Rectangle Rectangle, ICompatibilityActionInfo Action)> _actionRects = [];
	private readonly Dictionary<ICompatibilityPackageIdentity, Rectangle> _modRects = [];
	private readonly Dictionary<ICompatibilityPackageIdentity, Rectangle> _modDotsRects = [];
	private readonly Dictionary<ICompatibilityPackageIdentity, int> _modHeights = [];
	private Rectangle bulkActionRect;
	private Rectangle dismissActionRect;
	private Rectangle linkActionRect;
	private Rectangle recommendedActionRect;
	private Rectangle snoozeRect;

	private readonly INotifier _notifier;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IWorkshopService _workshopService;
	private readonly IPackageNameUtil _packageNameUtil;
	private readonly IImageService _imageService;
	private readonly ICompatibilityActionsHelper _compatibilityActions;
	private readonly IDlcManager _dlcManager;

	public ReportType Type { get; }
	public ICompatibilityItem Message { get; }
	public PackageCompatibilityReportControl PackageCompatibilityReportControl { get; }
	public string? LinkToFollow { get; set; }

	public CompatibilityMessageControl(PackageCompatibilityReportControl packageCompatibilityReportControl, ReportType type, ICompatibilityItem message)
	{
		ServiceCenter.Get(out _notifier, out _compatibilityManager, out _compatibilityActions, out _workshopService, out _packageNameUtil, out _imageService, out _dlcManager);

		Dock = DockStyle.Top;
		Type = type;
		Message = message;
		PackageCompatibilityReportControl = packageCompatibilityReportControl;

		if (message.Packages?.Count() != 0 && !message.Packages.All(x => x.IsDlc || x.GetWorkshopInfo() is not null))
		{
			_notifier.WorkshopInfoUpdated += Invalidate;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			_notifier.WorkshopInfoUpdated -= Invalidate;
		}

		base.Dispose(disposing);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(5, 8, 5, 8));
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var hovered = snoozeRect.Contains(e.Location)
			|| bulkActionRect.Contains(e.Location)
			|| linkActionRect.Contains(e.Location)
			|| dismissActionRect.Contains(e.Location)
			|| recommendedActionRect.Contains(e.Location)
			|| _actionRects.Any(x => x.Value.Rectangle.Contains(e.Location))
			|| _modRects.Any(x => x.Value.Contains(e.Location))
			|| _modDotsRects.Any(x => x.Value.Contains(e.Location));

		Cursor = hovered ? Cursors.Hand : Cursors.Default;
	}

	protected override async void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		foreach (var item in _modRects)
		{
			if (item.Value.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left)
				{
					if (Message.Type is ReportType.DlcMissing)
					{
						PlatformUtil.OpenUrl(item.Key.Url);
					}
					else
					{
						ServiceCenter.Get<IInterfaceService>().OpenPackagePage(item.Key, false);
					}
				}
				else if (e.Button == MouseButtons.Right && !item.Key.IsDlc)
				{
					var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(item.Key);

					SlickToolStrip.Show(Program.MainForm, items);
				}

				return;
			}
		}

		foreach (var item in _modDotsRects)
		{
			if (item.Value.Contains(e.Location) && !item.Key.IsDlc)
			{
				var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(item.Key);

				SlickToolStrip.Show(Program.MainForm, items);

				return;
			}
		}

		if (e.Button == MouseButtons.Left && snoozeRect.Contains(e.Location))
		{
			_compatibilityManager.ToggleSnoozed(Message);

			return;
		}

		if (e.Button == MouseButtons.Left)
		{
			foreach (var item in _actionRects)
			{
				if (item.Value.Rectangle.Contains(e.Location))
				{
					await Invoke(item.Value.Action, item.Key);

					return;
				}
			}
		}

		if (e.Button == MouseButtons.Left && recommendedActionRect.Contains(e.Location))
		{
			var action = _compatibilityActions.GetRecommendedAction(Message);

			if (action != null)
			{
				await Invoke(action);
			}

			return;
		}

		if (e.Button == MouseButtons.Left && bulkActionRect.Contains(e.Location))
		{
			if (Message.Status.Action is StatusAction.RequestReview)
			{
#if !DEBUG
				if (ServiceCenter.Get<IUserService>().User.Manager)
				{
					Program.MainForm.PushPanel(ServiceCenter.Get<IAppInterfaceService>().CompatibilityManagementPanel(PackageCompatibilityReportControl.Package));
				}
				else
#endif
				{
					Program.MainForm.PushPanel(ServiceCenter.Get<IAppInterfaceService>().RequestReviewPanel(PackageCompatibilityReportControl.Package));
				}

				return;
			}

			if (Message.Status.Action is StatusAction.MarkAsRead)
			{
				PackageCompatibilityReportControl.RemoveReviewInfo();

				await ServiceCenter.Get<IUpdateManager>().MarkReviewReplyAsRead(PackageCompatibilityReportControl.Package);

				return;
			}

			var action = _compatibilityActions.GetBulkAction(Message);

			if (action != null)
			{
				await Invoke(action);
			}
		}

		if (e.Button == MouseButtons.Left && dismissActionRect.Contains(e.Location))
		{
			PackageCompatibilityReportControl.RemoveReviewInfo();

			await ServiceCenter.Get<IUpdateManager>().MarkReviewReplyAsRead(PackageCompatibilityReportControl.Package);
		}

		if (e.Button == MouseButtons.Left && linkActionRect.Contains(e.Location))
		{
			PlatformUtil.OpenUrl(LinkToFollow);
		}
	}

	private async Task Invoke(ICompatibilityActionInfo action, IPackageIdentity? package = null)
	{
		Loading = true;
		Enabled = false;

		try
		{
			await action.Invoke(Message, package);

			_compatibilityManager.QuickUpdate(Message);
		}
		catch (Exception ex)
		{
			MessagePrompt.Show(ex, Locale.FailedToApplyChanges, form: Program.MainForm);
		}

		await Task.Delay(2000);

		Loading = false;
		Enabled = true;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		try
		{
			e.Graphics.SetUp(BackColor);

			var color = Message.Status.Notification.GetColor().Tint(Lum: FormDesign.Design.IsDarkTheme ? 6 : -6);
			var isSnoozed = _compatibilityManager.IsSnoozed(Message);
			var text = Message.GetMessage(_workshopService, _packageNameUtil);
			var note = string.IsNullOrWhiteSpace(Message.Status.Note) ? null : LocaleCRNotes.Get(Message.Status.Note!).One;
			var rectangle = ClientRectangle.Pad(Padding.Vertical + IconManager.GetNormalScale(), Padding.Top, Padding.Right, Padding.Bottom);
			var cursor = PointToClient(Cursor.Position);

			using var barBrush = Gradient(color, 1.25f);
			using var icon = Message.Status.Notification.GetIcon(false).Default;
			using var brush = new SolidBrush(FormDesign.Design.ForeColor);
			using var fadedBrush = new SolidBrush(Color.FromArgb(200, FormDesign.Design.ForeColor));
			using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.ForeColor, 85));
			using var font = Message.Type is not ReportType.RequestReview ? UI.Font(9F) : UI.Font(10.5F);
			using var smallFont = UI.Font(8.25F);
			using var tinyFont = UI.Font(7.5F, FontStyle.Bold);
			using var tinyUnderlineFont = UI.Font(7.5F, FontStyle.Bold | FontStyle.Underline);

			e.Graphics.FillRoundedRectangle(barBrush, ClientRectangle.Align(new Size(Padding.Left, Height - Padding.Horizontal), ContentAlignment.MiddleLeft), Padding.Left / 2);

			e.Graphics.DrawImage(icon.Color(FormDesign.Design.ForeColor), new Rectangle(ClientRectangle.X + (Padding.Left * 2), 0, ClientRectangle.Width, Height).Align(icon.Size, ContentAlignment.MiddleLeft));

			if (Message.Status.Action is StatusAction.RequestReview && Message.Type is not ReportType.RequestReview)
			{
				DrawPackageInfo(e, brush, fadedBrush, ref rectangle);
			}

			if (string.IsNullOrEmpty(Message.Status.Header))
			{
				e.Graphics.DrawString(text, font, brush, rectangle);
				rectangle.Y += Padding.Top + (int)e.Graphics.Measure(text, font, rectangle.Width).Height;
			}
			else
			{
				e.Graphics.DrawString(LocaleHelper.GetGlobalText(Message.Status.Header).ToUpper(), tinyFont, brush, rectangle);
				rectangle.Y += (int)e.Graphics.Measure(LocaleHelper.GetGlobalText(Message.Status.Header).ToUpper(), tinyFont, rectangle.Width).Height;

				e.Graphics.DrawString(text, font, brush, rectangle);
				rectangle.Y += Padding.Top + (int)e.Graphics.Measure(text, font, rectangle.Width).Height;
			}

			if (note is not null)
			{
				if (string.IsNullOrEmpty(text))
				{
					e.Graphics.DrawString(note, font, brush, rectangle.Pad(0, -Padding.Left, 0, 0));
					rectangle.Y += Padding.Top - Margin.Left + (int)e.Graphics.Measure(note, font, rectangle.Width).Height;
				}
				else
				{
					e.Graphics.DrawString(note, smallFont, fadedBrush, rectangle.Pad(Padding.Top, -Margin.Left, 0, 0));
					rectangle.Y += Padding.Top - Margin.Left + (int)e.Graphics.Measure(note, smallFont, rectangle.Width - Padding.Top).Height;
				}
			}

			var buttonRectangle = rectangle;
			var buttonHeight = 0;

			if (!string.IsNullOrEmpty(LinkToFollow))
			{
				linkActionRect = SlickButton.AlignAndDraw(e.Graphics, buttonRectangle, ContentAlignment.TopLeft, new ButtonDrawArgs
				{
					Text = $"Go to {new Uri(LinkToFollow).Host}",
					Icon = "Share",
					Font = font,
					ButtonType = ButtonType.Active,
					Cursor = cursor,
					HoverState = HoverState & ~HoverState.Focused,
				}).Rectangle;

				buttonRectangle = buttonRectangle.Pad(linkActionRect.Width + Padding.Horizontal, 0, 0, 0);
				buttonHeight = linkActionRect.Height;
			}

			if (Message.Status.Action is StatusAction.RequestReview)
			{
#if !DEBUG
				var isManager = ServiceCenter.Get<IUserService>().User.Manager;
#else
				var isManager = false;
#endif

				bulkActionRect = SlickButton.AlignAndDraw(e.Graphics, buttonRectangle, ContentAlignment.TopLeft, new ButtonDrawArgs
				{
					Text = isManager ? Locale.EditCompatibility : Message.Type is ReportType.RequestReview ? LocaleCR.RequestReviewUpdate : LocaleCR.RequestReview,
					Icon = isManager ? "CompatibilityReport" : "RequestReview",
					Font = font,
					BackgroundColor = Message.Type is ReportType.RequestReview ? default : FormDesign.Design.BackColor,
					ButtonType = Message.Type is ReportType.RequestReview ? ButtonType.Active : ButtonType.Normal,
					Cursor = cursor,
					HoverState = HoverState & ~HoverState.Focused,
				}).Rectangle;

				buttonRectangle = buttonRectangle.Pad(bulkActionRect.Width + Padding.Horizontal, 0, 0, 0);
				buttonHeight = bulkActionRect.Height;
			}

			if (Message.Type is ReportType.RequestReview || Message.Status.Action is StatusAction.MarkAsRead)
			{
				dismissActionRect = SlickButton.AlignAndDraw(e.Graphics, buttonRectangle, ContentAlignment.TopLeft, new ButtonDrawArgs
				{
					Text = Locale.Dismiss,
					Icon = "Ok",
					Font = font,
					ButtonType = ButtonType.Dimmed,
					BackgroundColor = FormDesign.Design.BackColor,
					Cursor = cursor,
					HoverState = HoverState & ~HoverState.Focused,
				}).Rectangle;

				buttonRectangle = buttonRectangle.Pad(dismissActionRect.Width + Padding.Horizontal, 0, 0, 0);
				buttonHeight = dismissActionRect.Height;
			}

			rectangle.Y += buttonHeight;

			var bulkAction = _compatibilityActions.GetBulkAction(Message);

			if (bulkAction is not null)
			{
				bulkActionRect = SlickButton.AlignAndDraw(e.Graphics, rectangle.Pad(Padding.Top), ContentAlignment.TopCenter, new ButtonDrawArgs
				{
					Text = bulkAction.Text,
					Icon = bulkAction.Icon,
					Font = font,
					ActiveColor = bulkAction.Color,
					ButtonType = ButtonType.Active,
					Cursor = cursor,
					HoverState = HoverState & ~HoverState.Focused,
				}).Rectangle;

				rectangle.Y += bulkActionRect.Height + Padding.Vertical;
			}

			var preferredSize = Message.Packages.Count() > 3 ? 125 : 150;
			var columns = (int)Math.Max(1, Math.Floor(rectangle.Width / (preferredSize * UI.FontScale)));
			var columnWidth = Math.Min(rectangle.Width, (int)(preferredSize * UI.FontScale));//rectangle.Width / columns;
			var currentY = new int[columns];
			var index = 0;

			foreach (var item in Message.Packages)
			{
				var bounds = new Rectangle(rectangle.X + (index * columnWidth), rectangle.Y + currentY[index], columnWidth, _modHeights.TryGet(item).If(0, columnWidth * 10 / 8));

				currentY[index] += (_modHeights[item] = DrawPackage(e, item, bounds, cursor)) + Padding.Top;

				index = (index + 1) % columns;
			}

			rectangle.Y += currentY.Max();

			var bottomText = 0;
			var hasRecommendedAction = _compatibilityActions.HasRecommendedAction(Message);

			if (hasRecommendedAction)
			{
				recommendedActionRect = new Rectangle(rectangle.Location, e.Graphics.Measure(LocaleCR.ApplyRecommendedAction, tinyFont).ToSize());

				e.Graphics.DrawString(LocaleCR.ApplyRecommendedAction, recommendedActionRect.Contains(cursor) ? tinyUnderlineFont : tinyFont, activeBrush, recommendedActionRect.X, recommendedActionRect.Y - (Padding.Top / 2));

				rectangle.X += recommendedActionRect.Width + Padding.Horizontal;
				bottomText = recommendedActionRect.Height;
			}

			if (_compatibilityActions.CanSnooze(Message))
			{
				if (isSnoozed && Enabled)
				{
					using var fadeBrush = new SolidBrush(Color.FromArgb(125, BackColor));

					e.Graphics.FillRectangle(fadeBrush, ClientRectangle);
				}

				snoozeRect = new Rectangle(rectangle.Location, e.Graphics.Measure(isSnoozed ? Locale.UnSnooze : Locale.Snooze, tinyFont).ToSize());

				bottomText = Math.Max(bottomText, snoozeRect.Height);

				using var purpleBrush = new SolidBrush(Color.FromArgb(isSnoozed ? 255 : 200, 100, 60, 220));
				e.Graphics.DrawString(isSnoozed ? Locale.UnSnooze : Locale.Snooze, snoozeRect.Contains(cursor) ? tinyUnderlineFont : tinyFont, purpleBrush, snoozeRect.X, snoozeRect.Y - (Padding.Top / 2));
			}

			if (!Enabled)
			{
				using var fadeBrush = new SolidBrush(Color.FromArgb(125, BackColor));

				e.Graphics.FillRectangle(fadeBrush, ClientRectangle);
			}

			if (Loading)
			{
				DrawLoader(e.Graphics, ClientRectangle);
			}

			Height = rectangle.Y + bottomText;
		}
		catch { }
	}

	private void DrawPackageInfo(PaintEventArgs e, SolidBrush brush, SolidBrush fadedBrush, ref Rectangle rectangle)
	{
		var packageData = Message.GetPackageInfo();

		if (packageData is null)
		{
			return;
		}

		using var font = UI.Font(9F, FontStyle.Bold);
		using var tinyFont = UI.Font(7.5F);

		var startPos = rectangle.Location;

		e.Graphics.DrawString(LocaleCR.LastReview.ToUpper(), tinyFont, fadedBrush, rectangle);
		rectangle.Y += (int)e.Graphics.Measure(LocaleCR.LastReview.ToUpper(), tinyFont, rectangle.Width).Height;

		var dateText = packageData.ReviewDate == DateTime.MinValue ? LocaleCR.NotReviewed.One : packageData.ReviewDate.ToReadableString(packageData.ReviewDate.Year != DateTime.Now.Year, ExtensionClass.DateFormat.MDY);
		e.Graphics.DrawString(dateText, font, brush, rectangle);
		rectangle.Y += (int)e.Graphics.Measure(dateText, font, rectangle.Width).Height;

		if (rectangle.Width > UI.Scale(250))
		{
			var rect = new Rectangle(rectangle.X + (rectangle.Width / 2), startPos.Y, rectangle.Width / 2, rectangle.Height);

			e.Graphics.DrawString(LocaleCR.ActiveReviewRequests.ToUpper(), tinyFont, fadedBrush, rect);
			rect.Y += (int)e.Graphics.Measure(LocaleCR.ActiveReviewRequests.ToUpper(), tinyFont, rect.Width).Height;

			var text = LocaleCR.ActiveReportsCount.FormatPlural(packageData.ActiveReports);
			e.Graphics.DrawString(text, font, brush, rect);
			rect.Y += (int)e.Graphics.Measure(text, font, rect.Width).Height;

			rectangle.Y = Math.Max(rectangle.Y, rect.Y);
		}
		else
		{
			rectangle.Y += Padding.Top;

			e.Graphics.DrawString(LocaleCR.ActiveReviewRequests.ToUpper(), tinyFont, fadedBrush, rectangle);
			rectangle.Y += (int)e.Graphics.Measure(LocaleCR.ActiveReviewRequests.ToUpper(), tinyFont, rectangle.Width).Height;

			var text = LocaleCR.ActiveReportsCount.FormatPlural(packageData.ActiveReports);
			e.Graphics.DrawString(text, font, brush, rectangle);
			rectangle.Y += (int)e.Graphics.Measure(text, font, rectangle.Width).Height;
		}

		rectangle.Y += Padding.Top;
	}

	private int DrawPackage(PaintEventArgs e, ICompatibilityPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		e.Graphics.FillRoundedRectangleWithShadow(rectangle.Pad(Padding.Left), UI.Scale(5), Padding.Left);

		var thumbRect = rectangle.ClipTo(package.IsDlc ? ((rectangle.Width * 215 / 460) + Padding.Left + Padding.Top) : rectangle.Width).Pad(Padding.Left + Padding.Top);
		var textRect = rectangle.Pad(Padding.Left + Padding.Top, Padding.Vertical + thumbRect.Height, Padding.Left + Padding.Top, Padding.Bottom);

		DrawThumbnail(e, package, thumbRect, cursor);

		rectangle.Height = thumbRect.Height + Padding.Vertical;

		rectangle.Height += DrawTitleAndTags(e, package, textRect, cursor);

		if (!package.IsDlc)
		{
			rectangle.Height += DrawAuthor(e, package, new Rectangle(textRect.X, rectangle.Bottom, textRect.Width, textRect.Height), cursor);
		}

		_modRects[package] = thumbRect;

		var action = _compatibilityActions.GetAction(Message, package);

		if (action != null)
		{
			using var buttonArgs = new ButtonDrawArgs
			{
				Cursor = cursor,
				BorderRadius = Padding.Left,
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

			buttonArgs.Rectangle = new Rectangle(textRect.X, rectangle.Bottom + Padding.Top, textRect.Width, buttonArgs.Rectangle.Height);

			_actionRects[package] = (buttonArgs, action);

			SlickButton.SetUpColors(buttonArgs);

			SlickButton.DrawButton(e.Graphics, buttonArgs);

			return rectangle.Height + buttonArgs.Rectangle.Height + Padding.Vertical + Padding.Left;
		}

		return rectangle.Height + (Padding.Left * 2);
	}

	private int DrawThumbnail(PaintEventArgs e, ICompatibilityPackageIdentity package, Rectangle rectangle, Point cursor)
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

		if (thumbnail is not null && !package.IsDlc && package.IsLocal())
		{
			using var unsatImg = thumbnail.ToGrayscale();

			drawThumbnail(unsatImg);
		}
		else
		{
			thumbnail ??= package.IsLocal()
				? package is IAsset ? AssetThumbUnsat : package.IsCodeMod() ? ModThumbUnsat : package.IsLocal() ? PackageThumbUnsat : WorkshopThumbUnsat
				: package is IAsset ? AssetThumb : package.IsCodeMod() ? ModThumb : package.IsLocal() ? PackageThumb : WorkshopThumb;

			if (thumbnail is not null)
			{
				drawThumbnail(thumbnail);
			}
		}

		if (HoverState.HasFlag(HoverState.Hovered) && rectangle.Contains(cursor))
		{
			using var brush = new SolidBrush(Color.FromArgb(75, FormDesign.Design.ForeColor));
			e.Graphics.FillRoundedRectangle(brush, rectangle, UI.Scale(5));
		}

		return rectangle.Height + UI.Scale(16);

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, rectangle, UI.Scale(5), FormDesign.Design.BackColor);
	}

	private int DrawTitleAndTags(PaintEventArgs e, ICompatibilityPackageIdentity package, Rectangle rectangle, Point cursor)
	{
		if (!package.IsDlc)
		{
			var dotsRect = rectangle.Align(UI.Scale(new Size(16, 22)), ContentAlignment.TopRight);
			_modDotsRects[package] = dotsRect;
			DrawDots(e, dotsRect, cursor);

			rectangle = rectangle.Pad(0, 0, dotsRect.Width, 0);
		}

		var text = package.CleanName(out var tags);

		if (package.IsDlc)
		{
			tags.Clear();
			text = text.RegexRemove("^.+?- ").RegexRemove("(Content )?Creator Pack: ");
		}

		using var font = UI.Font(8.25F, FontStyle.Bold);
		var textRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

		var textSize = e.Graphics.Measure(text, font, textRect.Width);
		var oneLineSize = e.Graphics.Measure(text, font);
		var oneLine = textSize.Height == oneLineSize.Height;
		var tagRect = new Rectangle(rectangle.X + (oneLine ? (int)textSize.Width : 0), textRect.Y + (oneLine ? 0 : (int)textSize.Height), 0, (int)oneLineSize.Height);

		textRect.Height = rectangle.Height = (int)textSize.Height + (Margin.Top / 3);
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

			tagRect.X += Margin.Left + rect.Width;
		}

		if (!oneLine && tags.Count > 0)
		{
			finalY += tagRect.Height;
		}

		return finalY;
	}

	private int DrawAuthor(PaintEventArgs e, IPackageIdentity package, Rectangle rect, Point cursor)
	{
		var author = package.GetWorkshopInfo()?.Author;

		if (author is null)
		{
			return 0;
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

		return authorRect.Height + (Margin.Top / 3);
	}

	private void DrawDots(PaintEventArgs e, Rectangle rectangle, Point cursor)
	{
		var isHovered = rectangle.Contains(cursor);
		using var img = IconManager.GetIcon("VertialMore", rectangle.Height * 3 / 4).Color(isHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.IconColor);

		e.Graphics.DrawImage(img, rectangle.CenterR(img.Size));
	}
}