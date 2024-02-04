using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Forms;
using Skyve.App.UserInterface.Panels;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;
using Skyve.Domain;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class PackageDescriptionControl : SlickImageControl
{
#pragma warning disable IDE1006
#pragma warning disable CS0649
	private Padding GridPadding;
	private readonly bool GridView;
	private readonly bool CompactList;
	private readonly bool IsPackagePage;
#pragma warning restore CS0649
#pragma warning restore IDE1006

	public IPackageIdentity? Package { get; private set; }
	public PC_PackagePage? PackagePage { get; private set; }

	private DrawableItem<IPackageIdentity, Rectangles>? _drawablePackage;

	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageUtil _packageUtil;
	private readonly ISettings _settings;
	private readonly IModLogicManager _modLogicManager;
	private readonly IModUtil _modUtil;
	private readonly IUserService _userService;

	public PackageDescriptionControl()
	{
		ServiceCenter.Get(out _settings, out _packageUtil, out _compatibilityManager, out _subscriptionsManager, out _modUtil, out _modLogicManager, out _userService);
	}

	public void SetPackage(IPackageIdentity package, PC_PackagePage? page)
	{
		PackagePage = page;
		Package = package;
		_drawablePackage = new DrawableItem<IPackageIdentity, Rectangles>(Package);

		var workshopInfo = Package.GetWorkshopInfo();

		if (!string.IsNullOrWhiteSpace(workshopInfo?.Author?.AvatarUrl))
		{
			Image = null;
			LoadImage(workshopInfo?.Author?.AvatarUrl, ServiceCenter.Get<IImageService>().GetImage);
		}

		Invalidate();
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(4), UI.FontScale);
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button != MouseButtons.Left || _drawablePackage is null)
		{
			return;
		}

		var item = _drawablePackage;
		var rects = item.Rectangles;

		if (rects.IncludedRect.Contains(e.Location))
		{
			if (item.Item.GetLocalPackage() is not ILocalPackageData localPackage)
			{
				if (!item.Item.IsLocal())
				{
					_subscriptionsManager.Subscribe(new IPackageIdentity[] { item.Item });
				}

				return;
			}

			{
				_packageUtil.SetIncluded(localPackage, !_packageUtil.IsIncluded(localPackage));
			}

			return;
		}

		if (rects.EnabledRect.Contains(e.Location) && item.Item.GetLocalPackage() is not null)
		{
			{
				_packageUtil.SetEnabled(item.Item.GetLocalPackage()!, !_packageUtil.IsEnabled(item.Item.GetLocalPackage()!));
			}

			return;
		}

		if (rects.FolderRect.Contains(e.Location))
		{
			PlatformUtil.OpenFolder(item.Item.GetLocalPackage()?.FilePath);
			return;
		}

		if (rects.SteamRect.Contains(e.Location) && item.Item.GetWorkshopInfo()?.Url is string url)
		{
			PlatformUtil.OpenUrl(url);
			return;
		}

		if (rects.GithubRect.Contains(e.Location) && item.Item.GetPackageInfo()?.Links?.FirstOrDefault(x => x.Type == LinkType.Github) is ILink gitLink)
		{
			PlatformUtil.OpenUrl(gitLink.Url);
			return;
		}

		if (rects.MoreRect.Contains(e.Location))
		{
			var items = ServiceCenter.Get<ICustomPackageService>().GetRightClickMenuItems(item.Item);

			this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, items));

			return;
		}

		if (rects.AuthorRect.Contains(e.Location) && item.Item.GetWorkshopInfo()?.Author is IUser user)
		{
			{
				var pc = new PC_UserPage(user);

				(FindForm() as BasePanelForm)?.PushPanel(null, pc);
			}

			return;
		}

		if (rects.FolderNameRect.Contains(e.Location) && item.Item.IsLocal())
		{
			{
				Clipboard.SetText(Path.GetFileName(item.Item.GetLocalPackage()?.Folder ?? string.Empty));

			}

			return;
		}

		if (rects.SteamIdRect.Contains(e.Location))
		{
			{
				Clipboard.SetText(item.Item.Id.ToString());
			}

			return;
		}

		if (rects.CompatibilityRect.Contains(e.Location))
		{
			{
				if (PackagePage is not null)
				{
					PackagePage.T_CR.Selected = true;
				}
			}
			return;
		}

		if (rects.VersionRect.Contains(e.Location) && item.Item.GetPackage()?.IsCodeMod == true)
		{
			Clipboard.SetText(item.Item.GetLocalPackage()!.Version);
		}

		if (rects.ScoreRect.Contains(e.Location))
		{
			throw new NotImplementedException();
			return;
		}

		if (rects.DateRect.Contains(e.Location))
		{
			var date = item.Item.GetWorkshopInfo()?.ServerTime ?? item.Item.GetLocalPackage()?.LocalTime;

			if (date.HasValue)
			{
				{
					Clipboard.SetText(date.Value.ToString("g"));
				}
			}

			return;
		}

		foreach (var tag in rects.TagRects)
		{
			if (tag.Value.Contains(e.Location))
			{
				{
					Clipboard.SetText(tag.Key.Value);
				}

				return;
			}
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (_drawablePackage is null)
		{
			Cursor = Cursors.Default;
			return;
		}

		var location = e.Location;

		Cursor = _drawablePackage.Rectangles?.IsHovered(this, location) == true ? Cursors.Hand : Cursors.Default;

		if (_drawablePackage.Rectangles?.GetToolTip(this, location, out var text, out var point) ?? false)
		{
			SlickTip.SetTo(this, text, offset: point);
		}
		else
		{
			SlickTip.SetTo(this, string.Empty);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		if (_drawablePackage is not null)
		{
			_drawablePackage.Rectangles = GenerateListRectangles(_drawablePackage.Item, ClientRectangle);

			OnPaintItemList(new ItemPaintEventArgs<IPackageIdentity, Rectangles>(_drawablePackage, e.Graphics, [ClientRectangle], ClientRectangle, HoverState, false));
		}
	}

	protected void OnPaintItemList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
	{
		var localPackage = e.Item.GetPackage();
		var localParentPackage = localPackage?.GetLocalPackage();
		var workshopInfo = e.Item.GetWorkshopInfo();
		var isPressed = false;
		var isIncluded = e.Item.IsIncluded(out var partialIncluded) || partialIncluded;

		var compatibilityReport = e.Item.GetCompatibilityInfo();
		var notificationType = compatibilityReport?.GetNotification();

		GetStatusDescriptors(localParentPackage, out var statusText, out var statusIcon, out var statusColor);

		e.BackColor = BackColor;

		using var backBrush = new SolidBrush(BackColor == FormDesign.Design.BackColor ? FormDesign.Design.AccentBackColor : FormDesign.Design.BackColor);
		e.Graphics.FillRoundedRectangle(backBrush, e.Rects.BotRect.Pad(0, 0, 0, -Padding.Bottom), Padding.Left);

		DrawTitleAndTagsAndVersionForList(e, localPackage, localParentPackage, workshopInfo);
		DrawIncludedButton(e, isIncluded, partialIncluded, localParentPackage, out var activeColor);

		var scoreX = DrawScore(e, workshopInfo);

		if (scoreX > 0)
		{
			scoreX += Padding.Horizontal;
		}

		if (workshopInfo?.Author is not null)
		{
			DrawAuthor(e, workshopInfo.Author, scoreX);
		}
		else if (e.Item.IsLocal())
		{
			DrawFolderName(e, localParentPackage!, scoreX);
		}

		DrawButtons(e, isPressed, localParentPackage, workshopInfo);

		DrawCompatibilityAndStatusList(e, notificationType, statusText, statusIcon, statusColor);
	}

	private bool GetStatusDescriptors(ILocalPackageIdentity? mod, out string text, out DynamicIcon? icon, out Color color)
	{
		switch (_packageUtil.GetStatus(mod, out text))
		{
			case DownloadStatus.Unknown:
				text = Locale.StatusUnknown;
				icon = "I_Question";
				color = FormDesign.Design.YellowColor;
				return true;
			case DownloadStatus.OutOfDate:
				text = Locale.OutOfDate;
				icon = "I_OutOfDate";
				color = FormDesign.Design.YellowColor;
				return true;
			case DownloadStatus.PartiallyDownloaded:
				text = Locale.PartiallyDownloaded;
				icon = "I_Broken";
				color = FormDesign.Design.RedColor;
				return true;
			case DownloadStatus.Removed:
				text = Locale.RemovedByAuthor;
				icon = "I_ContentRemoved";
				color = FormDesign.Design.RedColor;
				return true;
		}

		icon = null;
		color = Color.White;
		return false;
	}

	private void DrawTitleAndTagsAndVersionForList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IPackage? package, ILocalPackageIdentity? localPackageIdentity, IWorkshopInfo? workshopInfo)
	{
		using var font = UI.Font(14.5F, FontStyle.Bold);
		var mod = e.Item is not IAsset;
		var tags = new List<(Color Color, string Text)>();
		var text = mod ? e.Item.CleanName(out tags) : e.Item.ToString();
		using var brush = new SolidBrush(FormDesign.Design.ForeColor);
		e.Graphics.DrawString(text, font, brush, e.Rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center });

#if CS1
		var isVersion = localParentPackage?.Mod is not null && !e.Item.IsBuiltIn && !IsPackagePage;
		var versionText = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.IsBuiltIn ? Locale.Vanilla : e.Item is ILocalPackageData lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString();
#else
		var isVersion = (package?.IsCodeMod ?? false) && !string.IsNullOrEmpty(package?.Version);
		var versionText = isVersion ? "v" + package!.Version : localPackageIdentity != null ? localPackageIdentity.FileSize.SizeString(0) : workshopInfo?.ServerSize.SizeString(0);
#endif
		var date = workshopInfo?.ServerTime ?? e.Item.GetLocalPackage()?.LocalTime;

		var padding = GridView ? GridPadding : Padding;
		var textSize = e.Graphics.Measure(text, font);
		var tagRect = new Rectangle(e.Rects.TextRect.X + (int)textSize.Width, e.Rects.TextRect.Y, 0, e.Rects.TextRect.Height);

		for (var i = 0; i < tags.Count; i++)
		{
			var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, large: true);

			if (i == 0 && !string.IsNullOrEmpty(versionText))
			{
				e.Rects.VersionRect = rect;
			}

			tagRect.X += padding.Left + rect.Width;
		}

		tagRect = e.Rects.BotRect;
		tagRect.X += padding.Left;

		if (e.Item.Id > 0)
		{
			using var icon = IconManager.GetSmallIcon("I_Steam");

			e.Rects.SteamIdRect = e.Graphics.DrawLabel(e.Item.Id.ToString(), icon, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor), tagRect, ContentAlignment.BottomLeft, large: true, mousePosition: CursorLocation);

			tagRect.X += padding.Left + e.Rects.SteamIdRect.Width;
		}

		if (!string.IsNullOrEmpty(versionText))
		{
			e.Rects.VersionRect = e.Graphics.DrawLabel(versionText, null, isVersion ? FormDesign.Design.YellowColor : FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.AccentBackColor, 40), tagRect, ContentAlignment.BottomLeft, large: true, mousePosition: isVersion ? CursorLocation : null);

			tagRect.X += padding.Left + e.Rects.VersionRect.Width;
		}

		if (date.HasValue && !IsPackagePage)
		{
			var dateText = _settings.UserSettings.ShowDatesRelatively ? date.Value.ToRelatedString(true, false) : date.Value.ToString("g");
			using var icon = IconManager.GetSmallIcon("I_UpdateTime");

			e.Rects.DateRect = e.Graphics.DrawLabel(dateText, icon, FormDesign.Design.AccentColor, tagRect, ContentAlignment.BottomLeft, large: true, mousePosition: CursorLocation);
		}
	}

	private void DrawIncludedButton(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, bool isIncluded, bool partialIncluded, ILocalPackageData? package, out Color activeColor)
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

	private int DrawScore(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IWorkshopInfo? workshopInfo)
	{
		var score = workshopInfo?.VoteCount ?? -1;

		if (score != -1)
		{
			var clip = e.Graphics.ClipBounds;
			var padding = GridView ? GridPadding : Padding;
			var height = (int)(24 * UI.FontScale);
			var labelH = height - padding.Bottom;
			var scoreRect = new Rectangle(e.Rects.BotRect.X + padding.Horizontal, e.Rects.BotRect.Top + padding.Vertical + height / 2 - labelH / 2 + 1, labelH, labelH);
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

			return labelH + Padding.Left;
		}

		return 0;
	}

	private void DrawFolderName(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageData package, int scoreX)
	{
		if (package is null)
		{
			return;
		}

		var padding = GridView ? GridPadding : Padding;
		var height = (int)(24 * UI.FontScale);
		var folderPoint = new Point(scoreX == 0 ? e.Rects.BotRect.X + padding.Horizontal : scoreX + padding.Left * 3, e.Rects.BotRect.Y + padding.Vertical);

		e.Rects.FolderNameRect = e.Graphics.DrawLargeLabel(folderPoint, Path.GetFileName(package.Folder), "I_Folder", alignment: ContentAlignment.TopLeft, padding: GridView ? GridPadding : Padding, height: height, cursorLocation: CursorLocation);
	}

	private void DrawAuthor(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IUser author, int scoreX)
	{
		var padding = GridView ? GridPadding : Padding;
		var authorRect = new Rectangle(scoreX + padding.Left * 3, e.Rects.BotRect.Y + padding.Vertical, 0, 0);
		var authorImg = author.GetUserAvatar();

		var height = (int)(24 * UI.FontScale);

		if (authorImg is null)
		{
			using var authorIcon = IconManager.GetIcon("I_Author", height);

			authorRect = e.Graphics.DrawLargeLabel(authorRect.Location, author.Name, authorIcon, alignment: ContentAlignment.TopLeft, padding: padding, height: height, cursorLocation: CursorLocation);
		}
		else
		{
			authorRect = e.Graphics.DrawLargeLabel(authorRect.Location, author.Name, authorImg, alignment: ContentAlignment.TopLeft, padding: padding, height: height, cursorLocation: CursorLocation);
		}

		if (_userService.IsUserVerified(author))
		{
			var avatarRect = authorRect.Pad(padding).Align(CompactList ? UI.Scale(new Size(18, 18), UI.FontScale) : new(authorRect.Height * 3 / 4, authorRect.Height * 3 / 4), ContentAlignment.MiddleLeft);
			var checkRect = avatarRect.Align(new Size(avatarRect.Height / 3, avatarRect.Height / 3), ContentAlignment.BottomRight);

			e.Graphics.FillEllipse(new SolidBrush(FormDesign.Design.GreenColor), checkRect.Pad(-(int)(2 * UI.FontScale)));

			using var img = IconManager.GetIcon("I_Check", checkRect.Height);

			e.Graphics.DrawImage(img.Color(Color.White), checkRect.Pad(0, 0, -1, -1));
		}

		e.Rects.AuthorRect = authorRect;
	}

	private int DrawButtons(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, bool isPressed, ILocalPackageData? parentPackage, IWorkshopInfo? workshopInfo)
	{
		var padding = GridView ? GridPadding : Padding;
		var size = UI.Scale(CompactList ? new Size(24, 24) : new Size(28, 28), UI.FontScale);
		var rect = e.Rects.TopRect.Pad(Padding).Align(size, ContentAlignment.BottomRight);
		var backColor = Color.FromArgb(175, GridView ? FormDesign.Design.BackColor : FormDesign.Design.ButtonColor);

		{
			using var icon = IconManager.GetIcon("I_More", size.Height * 3 / 4);

			SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

			e.Rects.MoreRect = rect;

			rect.X -= rect.Width + padding.Left;
		}

		if (parentPackage is not null)
		{
			using var icon = IconManager.GetIcon("I_Folder", size.Height * 3 / 4);

			SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

			e.Rects.FolderRect = rect;

			rect.X -= rect.Width + padding.Left;
		}

		if (workshopInfo?.Url is not null)
		{
			using var icon = IconManager.GetIcon("I_Steam", rect.Height * 3 / 4);

			SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

			e.Rects.SteamRect = rect;

			rect.X -= rect.Width + padding.Left;
		}

		if (e.Item.GetPackageInfo()?.Links?.FirstOrDefault(x => x.Type == LinkType.Github) is ILink gitLink)
		{
			using var icon = IconManager.GetIcon("I_Github", rect.Height * 3 / 4);

			SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

			e.Rects.GithubRect = rect;

			rect.X -= rect.Width + padding.Left;
		}

		return rect.X + rect.Width;
	}

	private void DrawCompatibilityAndStatusList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, NotificationType? notificationType, string? statusText, DynamicIcon? statusIcon, Color statusColor)
	{
		var height = (int)(24 * UI.FontScale);

		if (notificationType > NotificationType.Info)
		{
			var point = new Point(e.ClipRectangle.Right - Padding.Horizontal * 2, e.Rects.BotRect.Y + Padding.Vertical);

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
			var point = new Point(e.ClipRectangle.Right - Padding.Horizontal * 2, e.Rects.BotRect.Bottom - Padding.Bottom);

			e.Rects.DownloadStatusRect = e.Graphics.DrawLargeLabel(
				point,
				statusText,
				statusIcon,
				statusColor,
				CompactList ? ContentAlignment.TopLeft : ContentAlignment.BottomRight,
				Padding,
				height,
				CursorLocation,
				CompactList);
		}
	}

	private static void DrawSeam(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, int x)
	{
		var seamRectangle = new Rectangle(x - (int)(40 * UI.UIScale), e.ClipRectangle.Y, (int)(40 * UI.UIScale), e.ClipRectangle.Height);

		using var seamBrush = new LinearGradientBrush(seamRectangle, Color.Empty, e.BackColor, 0F);

		e.Graphics.FillRectangle(seamBrush, seamRectangle);
	}

	private Rectangles GenerateListRectangles(IPackageIdentity item, Rectangle rectangle)
	{
		rectangle = rectangle.Pad(Padding.Left, 0, Padding.Right, 0);

		var rects = new Rectangles(item)
		{
			TopRect = rectangle.Pad(0, 0, 0, rectangle.Height * 6 / 10),
			BotRect = rectangle.Pad(Padding.Left, rectangle.Height * 4 / 10 + Padding.Top, Padding.Right, Padding.Bottom),
			IconRect = rectangle.Pad(0, 0, rectangle.Width, rectangle.Height * 6 / 10),
		};

		var includedSize = 28;

		rects.IncludedRect = rects.TopRect.Pad(Padding).Align(UI.Scale(new Size(includedSize, CompactList ? 22 : includedSize), UI.FontScale), ContentAlignment.BottomLeft);

		if (_settings.UserSettings.AdvancedIncludeEnable && item.GetLocalPackage() is not null)
		{
			rects.EnabledRect = rects.IncludedRect;
			rects.EnabledRect.X += rects.EnabledRect.Width;
		}

		rects.TextRect = rects.IncludedRect.AlignToFontSize(UI.Font(14.5F, FontStyle.Bold), ContentAlignment.MiddleLeft);

		rects.TextRect.X = Math.Max(rects.IncludedRect.Right, rects.EnabledRect.Right) + Padding.Horizontal;
		rects.TextRect.Width = rectangle.Width;

		return rects;
	}

	public class Rectangles : IDrawableItemRectangles<IPackageIdentity>
	{
		public Dictionary<ITag, Rectangle> TagRects = new();
		public Rectangle IncludedRect;
		public Rectangle EnabledRect;
		public Rectangle FolderRect;
		public Rectangle IconRect;
		public Rectangle TextRect;
		public Rectangle SteamRect;
		public Rectangle SteamIdRect;
		public Rectangle AuthorRect;
		public Rectangle VersionRect;
		public Rectangle CompatibilityRect;
		public Rectangle DownloadStatusRect;
		public Rectangle DateRect;
		public Rectangle ScoreRect;
		public Rectangle GithubRect;
		public Rectangle FolderNameRect;
		public Rectangle TopRect;
		public Rectangle BotRect;
		public Rectangle MoreRect;

		public IPackageIdentity	 Item { get; set; }

		public Rectangles(IPackageIdentity item)
		{
			Item = item;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return
				IncludedRect.Contains(location) ||
				EnabledRect.Contains(location) ||
				FolderRect.Contains(location) ||
				SteamRect.Contains(location) ||
				AuthorRect.Contains(location) ||
				FolderNameRect.Contains(location) ||
				DownloadStatusRect.Contains(location) ||
				ScoreRect.Contains(location) ||
				CompatibilityRect.Contains(location) ||
				DateRect.Contains(location) ||
				MoreRect.Contains(location) ||
				GithubRect.Contains(location) ||
				VersionRect.Contains(location) && Item?.GetLocalPackage() is not null ||
				TagRects.Any(x => x.Value.Contains(location)) ||
				SteamIdRect.Contains(location);
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			if (IncludedRect.Contains(location))
			{
				if (Item.GetLocalPackage() is null)
				{
					if (!Item.IsLocal())
					{
						text = Locale.SubscribeToItem;
						point = IncludedRect.Location;
						return true;
					}
					else
					{
						text = string.Empty;
						point = default;
						return false;
					}
				}

				if (Item.IsIncluded())
				{
					text = $"{Locale.ExcludeItem.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByIncluded.ToString().ToLower())}";
				}
				else
				{
					text = $"{Locale.IncludeItem.Format(Item.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByExcluded.ToString().ToLower())}";
				}

				point = IncludedRect.Location;
				return true;
			}

			if (EnabledRect.Contains(location) && Item.GetLocalPackage() is ILocalPackageData localPackageData)
			{
				if (localPackageData.IsEnabled())
				{
					text = $"{Locale.DisableItem.Format(localPackageData.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByEnabled.ToString().ToLower())}";
				}
				else
				{
					text = $"{Locale.EnableItem.Format(localPackageData.CleanName())}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByDisabled.ToString().ToLower())}";
				}

				point = EnabledRect.Location;
				return true;
			}

			if (SteamRect.Contains(location))
			{
				text = Locale.ViewOnWorkshop;
				point = SteamRect.Location;
				return true;
			}

			if (GithubRect.Contains(location))
			{
				text = Locale.ViewOnGithub;
				point = GithubRect.Location;
				return true;
			}

			if (SteamIdRect.Contains(location))
			{
				text = getFilterTip(string.Format(Locale.CopyToClipboard, Item.Id), string.Format(Locale.AddToSearch, Item.Id));
				point = SteamIdRect.Location;
				return true;
			}

			if (FolderNameRect.Contains(location))
			{
				var folder = Path.GetFileName(Item.GetLocalPackage()?.Folder ?? string.Empty);
				text = getFilterTip(string.Format(Locale.CopyToClipboard, folder), string.Format(Locale.AddToSearch, folder));
				point = FolderNameRect.Location;
				return true;
			}

			if (AuthorRect.Contains(location))
			{
				text = getFilterTip(Locale.OpenAuthorPage, Locale.FilterByThisAuthor.Format(Item.GetWorkshopInfo()?.Author?.Name ?? "this author"));
				point = AuthorRect.Location;
				return true;
			}

			if (FolderRect.Contains(location))
			{
				text = Locale.OpenLocalFolder;
				point = FolderRect.Location;
				return true;
			}

			if (CompatibilityRect.Contains(location))
			{
				text = getFilterTip(Locale.ViewPackageCR, Locale.FilterByThisReportStatus);
				point = CompatibilityRect.Location;
				return true;
			}

			if (DownloadStatusRect.Contains(location))
			{
				var packageUtil = ServiceCenter.Get<IPackageUtil>();
				packageUtil.GetStatus(Item, out var reason);

				if (ServiceCenter.Get<ISettings>().UserSettings.FlipItemCopyFilterAction)
				{
					text = Locale.FilterByThisPackageStatus + "\r\n\r\n" + reason;
					point = DownloadStatusRect.Location;
					return true;
				}
				else
				{
					text = reason + "\r\n\r\n" + string.Format(Locale.AltClickTo, Locale.FilterByThisPackageStatus.ToString().ToLower());
					point = DownloadStatusRect.Location;
					return true;
				}
			}

			if (ScoreRect.Contains(location))
			{
				var workshopInfo = Item.GetWorkshopInfo();
				if (workshopInfo is not null)
				{
					text = "";// string.Format(Locale.RatingCount, workshopInfo.VoteCount.ToString("N0"), $"({workshopInfo.VoteCount}%)") + "\r\n" + string.Format(Locale.SubscribersCount, workshopInfo.Subscribers.ToString("N0"));
					point = ScoreRect.Location;
					return true;
				}
			}

			if (VersionRect.Contains(location) && Item.GetLocalPackage() is not null)
			{
				text = Locale.CopyVersionNumber;
				point = VersionRect.Location;
				return true;
			}

			if (DateRect.Contains(location))
			{
				var date = Item.GetWorkshopInfo()?.ServerTime ?? Item.GetLocalPackage()?.LocalTime;
				if (date.HasValue)
				{
					text = getFilterTip(string.Format(Locale.CopyToClipboard, date.Value.ToString("g")), Locale.FilterSinceThisDate);
					point = DateRect.Location;
					return true;
				}
			}

			foreach (var tag in TagRects)
			{
				if (tag.Value.Contains(location))
				{
					text = getFilterTip(string.Format(Locale.CopyToClipboard, tag.Key), string.Format(Locale.FilterByThisTag, tag.Key));
					point = tag.Value.Location;
					return true;
				}
			}

			text = string.Empty;
			point = default;
			return false;

			static string getFilterTip(string? text, string? _)
			{
				if (text is not null)
				{
					return text;
				}

				return string.Empty;
			}
		}
	}
}
