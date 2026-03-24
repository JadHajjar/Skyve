using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.CompatibilityReport;

public class CompatibilityMessageControl : SlickControl
{
	private readonly List<ulong> _subscribingTo = new();
	private readonly Dictionary<IPackage, Rectangle> _buttonRects = new();
	private readonly Dictionary<IPackage, Rectangle> _modRects = new();
	private readonly Dictionary<IPackage, Rectangle> _modDotsRects = [];
	private readonly Dictionary<IPackage, int> _modHeights = [];
	private Rectangle allButtonRect;
	private Rectangle snoozeRect;

	private readonly ICompatibilityManager _compatibilityManager;
	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly IPackageManager _packageManager;
	private readonly IBulkUtil _bulkUtil;
	private readonly IPackageUtil _packageUtil;
	private readonly INotifier _notifier;
	private readonly IDlcManager _dlcManager;

	public ReportType Type { get; }
	public ICompatibilityItem Message { get; }
	public PackageCompatibilityReportControl PackageCompatibilityReportControl { get; }

	public CompatibilityMessageControl(PackageCompatibilityReportControl packageCompatibilityReportControl, ReportType type, ICompatibilityItem message)
	{
		ServiceCenter.Get(out _notifier, out _compatibilityManager, out _subscriptionsManager, out _packageManager, out _bulkUtil, out _packageUtil, out _dlcManager);

		Dock = DockStyle.Top;
		Type = type;
		Message = message;
		PackageCompatibilityReportControl = packageCompatibilityReportControl;

		if (message.Packages?.Length != 0 && !message.Packages.All(x => x.GetWorkshopInfo() is not null))
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
			|| snoozeRect.Contains(e.Location)
			|| allButtonRect.Contains(e.Location)
			|| _buttonRects.Any(x => x.Value.Contains(e.Location))
			|| _modRects.Any(x => x.Value.Contains(e.Location))
			|| _modDotsRects.Any(x => x.Value.Contains(e.Location));

		Cursor = hovered ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		try
		{
			e.Graphics.SetUp(BackColor);

			var color = Message.Status.Notification.GetColor().Tint(Lum: FormDesign.Design.IsDarkTheme ? 6 : -6);
			var isSnoozed = _compatibilityManager.IsSnoozed(Message);
			var text = Message.Message;
			var note = string.IsNullOrWhiteSpace(Message.Status.Note) ? null : LocaleCRNotes.Get(Message.Status.Note!).One;
			var rectangle = ClientRectangle.Pad(Padding.Vertical + IconManager.GetNormalScale(), Padding.Top, Padding.Right, Padding.Bottom);
			var cursor = PointToClient(Cursor.Position);

			using var barBrush = Gradient(color, 1.25f);
			using var icon = Message.Status.Notification.GetIcon(false).Default;
			using var brush = new SolidBrush(FormDesign.Design.ForeColor);
			using var fadedBrush = new SolidBrush(Color.FromArgb(200, FormDesign.Design.ForeColor));
			using var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.ForeColor, 85));
			using var font = Message.Status.Action is not StatusAction.RequestReview ? UI.Font(9F) : UI.Font(10.5F);
			using var smallFont = UI.Font(8.25F);
			using var tinyFont = UI.Font(7.5F, FontStyle.Bold);
			using var tinyUnderlineFont = UI.Font(7.5F, FontStyle.Bold | FontStyle.Underline);
		
			GetAllButton(out var allText, out var allIcon, out var colorStyle);

			e.Graphics.FillRoundedRectangle(barBrush, ClientRectangle.Align(new Size(Padding.Left, Height - Padding.Horizontal), ContentAlignment.MiddleLeft), Padding.Left / 2);

			e.Graphics.DrawImage(icon.Color(FormDesign.Design.ForeColor), new Rectangle(ClientRectangle.X + (Padding.Left * 2), 0, ClientRectangle.Width, Height).Align(icon.Size, ContentAlignment.MiddleLeft));

				e.Graphics.DrawString(text, font, brush, rectangle);
				rectangle.Y += Padding.Top + (int)e.Graphics.Measure(text, font, rectangle.Width).Height;

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

			if (Message.Status.Action is StatusAction.RequestReview)
			{
#if !DEBUG
				var isManager = ServiceCenter.Get<IUserService>().User.Manager;
#else
				var isManager = false;
#endif

				allButtonRect = SlickButton.AlignAndDraw(e.Graphics, rectangle, ContentAlignment.TopLeft, new ButtonDrawArgs
				{
					Text = isManager ? Locale.EditCompatibility : LocaleCR.RequestReview,
					Icon = isManager ? "CompatibilityReport" : "RequestReview",
					Font = font,
					Cursor = cursor,
					HoverState = HoverState & ~HoverState.Focused,
				}).Rectangle;

				rectangle.Y += allButtonRect.Height + Padding.Vertical;
			}
			else if (allText is not null)
			{
				allButtonRect = SlickButton.AlignAndDraw(e.Graphics, rectangle.Pad(Padding.Top), ContentAlignment.TopCenter, new ButtonDrawArgs
				{
					Text = allText,
					Icon = allIcon,
					Font = font,
					ColorStyle = colorStyle,
					ButtonType = ButtonType.Active,
					Cursor = cursor,
					HoverState = HoverState & ~HoverState.Focused,
				}).Rectangle;

				rectangle.Y += allButtonRect.Height + Padding.Vertical;
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

			if (Message.Status.Notification > NotificationType.Info && Message.PackageId != 0)
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

	private int DrawPackage(PaintEventArgs e, IPackage package, Rectangle rectangle, Point cursor)
	{
		e.Graphics.FillRoundedRectangleWithShadow(rectangle.Pad(Padding.Left), UI.Scale(5), Padding.Left);

		var thumbRect = rectangle.ClipTo(rectangle.Width).Pad(Padding.Left + Padding.Top);
		var textRect = rectangle.Pad(Padding.Left + Padding.Top, Padding.Vertical + thumbRect.Height, Padding.Left + Padding.Top, Padding.Bottom);

		DrawThumbnail(e, package, thumbRect, cursor);

		rectangle.Height = thumbRect.Height + Padding.Vertical;

		rectangle.Height += DrawTitleAndTags(e, package, textRect, cursor);

			rectangle.Height += DrawAuthor(e, package, new Rectangle(textRect.X, rectangle.Bottom, textRect.Width, textRect.Height), cursor);

		_modRects[package] = thumbRect;


		string? buttonText = null;
		string? iconName = null;

		switch (Message.Status.Action)
		{
			case StatusAction.SubscribeToPackages:
				var p = package.LocalParentPackage;

				if (p is null)
				{
					buttonText = Locale.Subscribe;
					iconName = "Add";
				}
				else if (!p.IsIncluded())
				{
					buttonText = Locale.Include;
					iconName = "Check";
				}
				else if (!(p.Mod?.IsEnabled() ?? true))
				{
					buttonText = Locale.Enable;
					iconName = "Enabled";
				}
				break;
			case StatusAction.SelectOne:
				buttonText = Locale.SelectThisPackage;
				iconName = "Ok";
				break;
			case StatusAction.Switch:
				buttonText = Locale.Switch;
				iconName = "Switch";
				break;
		}

		if (buttonText is not null && (package.GetWorkshopInfo()?.IsCollection) != true)
		{
			using var buttonArgs = new ButtonDrawArgs
			{
				Cursor = cursor,
				BorderRadius = Padding.Left,
				Padding = UI.Scale(new Padding(4, 2, 4, 2)),
				Rectangle = new Rectangle(0, 0, textRect.Width, UI.Scale(24)),
				HoverState = HoverState & ~HoverState.Focused,
				Text = buttonText,
				Icon = iconName,
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

			_buttonRects[package] = buttonArgs.Rectangle;

			SlickButton.SetUpColors(buttonArgs);

			SlickButton.DrawButton(e.Graphics, buttonArgs);

			return rectangle.Height + buttonArgs.Rectangle.Height + Padding.Vertical + Padding.Left;
		}

		return rectangle.Height + (Padding.Left * 2);
	}

	private int DrawThumbnail(PaintEventArgs e, IPackage package, Rectangle rectangle, Point cursor)
	{
		Bitmap?thumbnail = package.GetThumbnail();

		if (thumbnail is not null &&  package.IsLocal)
		{
			using var unsatImg = thumbnail.ToGrayscale();

			drawThumbnail(unsatImg);
		}
		else
		{
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

	private int DrawTitleAndTags(PaintEventArgs e, IPackage package, Rectangle rectangle, Point cursor)
	{
			var dotsRect = rectangle.Align(UI.Scale(new Size(16, 22)), ContentAlignment.TopRight);
			_modDotsRects[package] = dotsRect;
			DrawDots(e, dotsRect, cursor);

			rectangle = rectangle.Pad(0, 0, dotsRect.Width, 0);

		var text = package.CleanName(out var tags);

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

		var isHovered = false && authorRect.Contains(cursor);
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

	private void GetAllButton(out string? allText, out string? allIcon, out ColorStyle colorStyle)
	{
		allText = null;
		allIcon = null;
		colorStyle = ColorStyle.Red;

		switch (Message.Status.Action)
		{
			case StatusAction.SubscribeToPackages:
				if (Message.Packages.Length > 1)
				{
					var max = Message.Packages.Max(x =>
					{
						var p = x?.GetLocalPackage();

						if (p is null)
						{
							return 3;
						}
						else if (!p.IsIncluded())
						{
							return 2;
						}
						else if (!(p.LocalParentPackage.Mod?.IsEnabled() ?? true))
						{
							return 1;
						}

						return 0;
					});

					colorStyle = ColorStyle.Green;
					allText = max switch { 3 => Locale.SubscribeAll, 2 => Locale.IncludeAll, 1 => Locale.EnableAll, _ => null };
					allIcon = max switch { 3 => "Add", 2 => "Check", 1 => "Enabled", _ => null };
				}
				break;
			case StatusAction.RequiresConfiguration:
				allText = _compatibilityManager.IsSnoozed(Message) ? Locale.UnSnooze : Locale.Snooze;
				allIcon = "Snooze";
				colorStyle = ColorStyle.Active;
				break;
			case StatusAction.UnsubscribeThis:
				allText = Locale.Unsubscribe;
				allIcon = "RemoveSteam";
				break;
			case StatusAction.UnsubscribeOther:
				allText = Message.Packages.Length switch { 0 => null, 1 => Locale.Unsubscribe, _ => Locale.UnsubscribeAll };
				allIcon = "RemoveSteam";
				break;
			case StatusAction.ExcludeThis:
				allText = Locale.Exclude;
				allIcon = "X";
				break;
			case StatusAction.ExcludeOther:
				allText = Message.Packages.Length switch { 0 => null, 1 => Locale.Exclude, _ => Locale.ExcludeAll };
				allIcon = "X";
				break;
			case StatusAction.RequestReview:
				allText = LocaleCR.RequestReview;
				allIcon = "RequestReview";
				colorStyle = ColorStyle.Active;
				break;
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		foreach (var item in _buttonRects)
		{
			if (item.Value.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left)
				{
					Clicked(item.Key, true);
				}

				return;
			}
		}

		foreach (var item in _modDotsRects)
		{
			if (item.Value.Contains(e.Location))
			{
				var items = ServiceCenter.Get<ICustomPackageService>().GetRightClickMenuItems(item.Key);

				SlickToolStrip.Show(Program.MainForm, items);

				return;
			}
		}

		foreach (var item in _modRects)
		{
			if (item.Value.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left)
				{
					Clicked(item.Key, false);
				}
				else if (e.Button == MouseButtons.Right && item.Key.GetWorkshopPackage() is not null)
				{
					var items = ServiceCenter.Get<ICustomPackageService>().GetRightClickMenuItems(item.Key.GetWorkshopPackage()!);

					this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, items));
				}

				return;
			}
		}

		if (e.Button == MouseButtons.Left && snoozeRect.Contains(e.Location))
		{
			_compatibilityManager.ToggleSnoozed(Message);
		}

		if (e.Button == MouseButtons.Left && allButtonRect.Contains(e.Location))
		{
			switch (Message.Status.Action)
			{
				case StatusAction.SubscribeToPackages:
					_subscriptionsManager.Subscribe(Message.Packages.Where(x => x.GetLocalPackage() is null));
					_bulkUtil.SetBulkIncluded(Message.Packages.SelectWhereNotNull(x => x.GetLocalPackage())!, true);
					_bulkUtil.SetBulkEnabled(Message.Packages.SelectWhereNotNull(x => x.GetLocalPackage())!, true);
					break;
				case StatusAction.RequiresConfiguration:
					_compatibilityManager.ToggleSnoozed(Message);
					break;
				case StatusAction.UnsubscribeThis:
					_subscriptionsManager.UnSubscribe(new[] { PackageCompatibilityReportControl.Package });
					break;
				case StatusAction.UnsubscribeOther:
					_subscriptionsManager.UnSubscribe(Message.Packages);
					break;
				case StatusAction.ExcludeThis:
				{
					var package = PackageCompatibilityReportControl.Package.GetLocalPackage();
					if (package is not null)
					{
						_packageUtil.SetIncluded(package, false);
					}
				}
				break;
				case StatusAction.ExcludeOther:
					foreach (var item in Message.Packages)
					{
						var package = PackageCompatibilityReportControl.Package.GetLocalPackage();
						if (package is not null)
						{
							_packageUtil.SetIncluded(package, false);
						}
					}
					break;
				case StatusAction.RequestReview:
					Program.MainForm.PushPanel(null, new PC_RequestReview(PackageCompatibilityReportControl.Package));
					break;
			}
			_compatibilityManager.QuickUpdate(Message);
		}
	}

	private void Clicked(IPackageIdentity item, bool button)
	{
		var package = item.GetWorkshopPackage();

		if (!button)
		{
			if (Message.Type is ReportType.DlcMissing)
			{
				PlatformUtil.OpenUrl($"https://store.steampowered.com/app/{item.Id}");
			}
			else if (package is not null)
			{
				Program.MainForm.PushPanel(null, package.GetWorkshopInfo()?.IsCollection == true ? new PC_ViewCollection(package) : new PC_PackagePage(package));
			}
			else
			{
				PlatformUtil.OpenUrl($"https://steamcommunity.com/workshop/filedetails/?id={item.Id}");
			}

			return;
		}

		var p = package?.GetLocalPackage();

		if (p is null)
		{
			_subscribingTo.Add(item.Id);

			Loading = true;

			_subscriptionsManager.Subscribe(new[] { item });
		}
		else
		{
			_packageUtil.SetIncluded(p, true);
			_packageUtil.SetEnabled(p, true);
		}

		switch (Message.Status.Action)
		{
			case StatusAction.SelectOne:
				foreach (var id in Message.Packages)
				{
					if (id != item)
					{
						var pp = id.GetLocalPackage();

						if (pp is not null)
						{
							_packageUtil.SetIncluded(pp, false);
						}
					}
				}
				break;
			case StatusAction.Switch:
			{
				var pp = PackageCompatibilityReportControl.Package.GetLocalPackage();
				if (pp is not null)
				{
					_packageUtil.SetIncluded(pp, false);
					_packageUtil.SetEnabled(pp, false);
				}
			}
			break;
		}

		_compatibilityManager.QuickUpdate(Message);
	}
}