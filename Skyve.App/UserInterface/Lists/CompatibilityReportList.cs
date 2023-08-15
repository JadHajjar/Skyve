﻿using SkyveApp.Systems.CS1.Utilities;
using SkyveApp.UserInterface.Panels;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Lists;
internal class CompatibilityReportList : SlickStackedListControl<ICompatibilityInfo, CompatibilityReportList.Rectangles>
{
	private readonly ISubscriptionsManager _subscriptionsManager;
	private readonly ICompatibilityManager _compatibilityManager;
	private readonly IPackageUtil _packageUtil;
	private readonly IDlcManager _dlcManager;
	private readonly IBulkUtil _bulkUtil;
	private readonly ISettings _settings;
	private readonly IModLogicManager _modLogicManager;
	private readonly IModUtil _modUtil;

#pragma warning disable IDE1006
#pragma warning disable CS0649
	private readonly bool CompactList;
	private readonly bool IsPackagePage;
#pragma warning restore CS0649
#pragma warning restore IDE1006

	public CompatibilityReportList()
	{
		GridView = true;
		DynamicSizing = true;
		AllowDrop = true;

		ServiceCenter.Get(out _subscriptionsManager, out _compatibilityManager, out _packageUtil, out _dlcManager, out _bulkUtil, out _settings, out _modUtil, out _modLogicManager);

		CanDrawItem += CompatibilityReportList_CanDrawItem;
	}

	private void CompatibilityReportList_CanDrawItem(object sender, CanDrawItemEventArgs<ICompatibilityInfo> e)
	{
		if (e.Item.Package is null)
		{
			e.DoNotDraw = true;
		}
	}

	protected override void UIChanged()
	{
		GridItemSize = new Size(380, 380);

		base.UIChanged();

		Padding = UI.Scale(new Padding(5), UI.FontScale);
		GridPadding = UI.Scale(new Padding(3), UI.FontScale);
	}

	protected override IEnumerable<DrawableItem<ICompatibilityInfo, Rectangles>> OrderItems(IEnumerable<DrawableItem<ICompatibilityInfo, Rectangles>> items)
	{
		return items.OrderByDescending(x => x.Item.Package.CleanName());
	}

	protected override void OnPaintItemGrid(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e)
	{
		if (e.Item.Package is null)
		{
			return;
		}

		var package = e.Item.Package;
		var localPackage = package.LocalPackage;
		var localParentPackage = localPackage?.LocalParentPackage;
		var workshopInfo = package.GetWorkshopInfo();
		var partialIncluded = false;
		var isPressed = false;
		var isIncluded = (localPackage is not null && _packageUtil.IsIncluded(package.LocalPackage!, out partialIncluded)) || partialIncluded;

		if (e.IsSelected)
		{
			e.BackColor = FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor);
		}
		else
		{
			e.BackColor = BackColor.Tint(Lum: FormDesign.Design.Type.If(FormDesignType.Dark, 5, -5));
		}

		if (!IsPackagePage && e.HoverState.HasFlag(HoverState.Hovered) && (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)))
		{
			e.BackColor = e.BackColor.MergeColor(FormDesign.Design.ActiveColor, e.HoverState.HasFlag(HoverState.Pressed) ? 0 : 90);

			isPressed = e.HoverState.HasFlag(HoverState.Pressed);
		}

		base.OnPaintItemGrid(e);

		DrawThumbnail(e);
		DrawTitleAndTagsAndVersionForList(e, localParentPackage, workshopInfo, isPressed);
		DrawIncludedButton(e, isIncluded, partialIncluded, localParentPackage, out _);
		DrawButtons(e, isPressed, localParentPackage, workshopInfo);

		DrawReport(e, DrawDividerLine(e, e.Rects.IconRect.Bottom), isPressed);
	}

	private void DrawReport(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, int baseY, bool isPressed)
	{
		var Message = e.Item.ReportItems.FirstOrDefault(x => x.Status.Notification == e.Item.GetNotification() && !_compatibilityManager.IsSnoozed(x));

		var backColor = Color.FromArgb(175, GridView ? FormDesign.Design.BackColor : FormDesign.Design.ButtonColor);
		var reportRect = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, 9999).Pad(0, baseY - e.ClipRectangle.Y, 0, 0);
		var cursor = PointToClient(Cursor.Position);
		var pad = (int)(6 * UI.FontScale);
		var note = string.IsNullOrWhiteSpace(Message.Status.Note) ? null : LocaleCRNotes.Get(Message.Status.Note!).One;
		var color = Message.Status.Notification.GetColor().MergeColor(BackColor, 60);
		using var font = UI.Font(8.25F);
		using var smallFont = UI.Font(7.5F);
		var iconRect = new Rectangle(e.Rects.IncludedRect.X, baseY, e.Rects.IncludedRect.Width, e.Rects.IncludedRect.Height);
		var messageSize = e.Graphics.Measure(Message.Message, font, reportRect.Width - (iconRect.Width * 2) - pad);
		var noteSize = e.Graphics.Measure(note, UI.Font(7.5F), reportRect.Width - (iconRect.Width * 2) - pad);
		using var brush = new SolidBrush(color);
		using var icon = Message.Status.Notification.GetIcon(false).Get(e.Rects.IncludedRect.Width * 3 / 4);
		var y = baseY + (int)(messageSize.Height + noteSize.Height + (noteSize.Height == 0 ? 0 : pad * 2));

		GetAllButton(Message, out var allText, out var allIcon, out var colorStyle);

		iconRect.Height = y - baseY;
		iconRect = iconRect.CenterR(iconRect.Width, iconRect.Width);

		e.Graphics.FillRoundedRectangle(brush, iconRect, pad);

		e.Graphics.DrawImage(icon.Color(color.GetTextColor()), iconRect.CenterR(icon.Size));

		if (Message.Status.Notification > NotificationType.Info && Message.PackageId != 0)
		{
			e.Rects.SnoozeRect = iconRect;
			e.Rects.SnoozeRect.X = e.ClipRectangle.Right - e.Rects.SnoozeRect.Width;

			var purple = Color.FromArgb(100, 60, 220);
			var isSnoozed = _compatibilityManager.IsSnoozed(Message);

			if (HoverState.HasFlag(HoverState.Hovered) && !HoverState.HasFlag(HoverState.Pressed) && e.Rects.SnoozeRect.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(125, purple)), e.Rects.SnoozeRect, pad);
			}
			else if (isSnoozed || (HoverState.HasFlag(HoverState.Pressed) && e.Rects.SnoozeRect.Contains(cursor)))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(purple), e.Rects.SnoozeRect, pad);
			}

			using var snoozeIcon = IconManager.GetLargeIcon("I_Snooze");
			e.Graphics.DrawImage(snoozeIcon.Color((isSnoozed || (HoverState.HasFlag(HoverState.Pressed) && e.Rects.SnoozeRect.Contains(cursor))) ? purple.GetTextColor() : FormDesign.Design.IconColor), e.Rects.SnoozeRect.CenterR(icon.Size));
		}

		using var textBrush = new SolidBrush(FormDesign.Design.ForeColor);
		e.Graphics.DrawString(Message.Message, font, textBrush, reportRect.Pad(iconRect.Width + pad, 0, iconRect.Width, 0));

		if (note is not null)
		{
			using var smallTextBrush = new SolidBrush(Color.FromArgb(200, ForeColor));
			e.Graphics.DrawString(note, smallFont, smallTextBrush, reportRect.Pad(iconRect.Width + pad, string.IsNullOrWhiteSpace(Message.Message) ? 0 : ((int)messageSize.Height + pad), iconRect.Width, 0));
		}

		if (allText is not null || Message.Packages.Length > 0)
		{
			y = DrawDividerLine(e, y);
		}

		if (allText is not null)
		{
			e.Rects.AllButtonRect = new Rectangle(e.ClipRectangle.X + iconRect.Width, y, e.ClipRectangle.Width - (iconRect.Width * 2), (int)(26 * UI.FontScale));

			using var buttonIcon = IconManager.GetIcon(allIcon, e.Rects.AllButtonRect.Height * 3 / 4);

			SlickButton.DrawButton(e, e.Rects.AllButtonRect, allText, UI.Font(9F), buttonIcon, UI.Scale(new Padding(4), UI.FontScale), e.Rects.AllButtonRect.Contains(cursor) ? HoverState & ~HoverState.Focused : HoverState.Normal, colorStyle, buttonType: ButtonType.Active);

			y += e.Rects.AllButtonRect.Height + GridPadding.Vertical;
		}

		if (Message.Packages.Length > 0)
		{
			var isDlc = Message.Type == ReportType.DlcMissing;
			var rect = new Rectangle(reportRect.X, y, reportRect.Width, (int)(32 * UI.FontScale) + GridPadding.Vertical).Pad(GridPadding);

			{
				foreach (var packageID in Message.Packages)
				{
					var fore = ForeColor;

					e.Rects.modRects[packageID] = rect;

					if (rect.Contains(cursor) && (!e.Rects.buttonRects.ContainsKey(packageID) || !e.Rects.buttonRects[packageID].Contains(cursor)))
					{
						if (HoverState.HasFlag(HoverState.Pressed))
						{
							fore = FormDesign.Design.ActiveColor;
						}

						using var gradientbrush = new LinearGradientBrush(reportRect.Pad(rect.Height / 2, 0, (int)(100 * UI.FontScale), 0), Color.FromArgb(50, fore), Color.Empty, LinearGradientMode.Horizontal);

						e.Graphics.FillRectangle(gradientbrush, rect.Pad(rect.Height / 2, 0, (int)(100 * UI.FontScale), 0));
					}

					var dlc = isDlc ? _dlcManager.Dlcs.FirstOrDefault(x => x.Id == packageID.Id) : null;
					var package = dlc is null ? packageID : null;
					var packageThumbnail = dlc?.GetThumbnail() ?? package.GetThumbnail();

					if ((package?.IsLocal ?? false) && packageThumbnail is not null)
					{
						using var unsatImg = new Bitmap(packageThumbnail, new Size(rect.Height, rect.Height)).Tint(Sat: 0);
						e.Graphics.DrawRoundedImage(unsatImg, rect.Align(new Size(rect.Height, rect.Height), ContentAlignment.TopLeft), (int)(4 * UI.FontScale), FormDesign.Design.AccentBackColor);
					}
					else
					{
						e.Graphics.DrawRoundedImage(packageThumbnail ?? (dlc is null ? Properties.Resources.I_ModIcon : Properties.Resources.I_DlcIcon).Color(fore), rect.Align(new Size(isDlc ? (rect.Height * 460 / 215) : rect.Height, rect.Height), ContentAlignment.TopLeft), pad, FormDesign.Design.AccentBackColor);
					}

					List<(Color Color, string Text)>? tags = null;

					var textRect = rect.Pad((isDlc ? rect.Height * 460 / 215 : rect.Height) + GridPadding.Horizontal, 0, 0, 0).AlignToFontSize(UI.Font(7.5F, FontStyle.Bold), ContentAlignment.TopLeft);

					e.Graphics.DrawString(dlc?.Name.Remove("Cities: Skylines - ").Replace("Content Creator Pack", "CCP") ?? package?.CleanName(out tags) ?? Locale.UnknownPackage, UI.Font(7.5F, FontStyle.Bold), new SolidBrush(fore), textRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

					var tagRect = new Rectangle(textRect.Left, textRect.Y, 0, textRect.Height);

					if (tags is not null)
					{
						foreach (var item in tags)
						{
							tagRect.X += Padding.Left + e.Graphics.DrawLabel(item.Text, null, item.Color, tagRect, ContentAlignment.BottomLeft, smaller: true).Width;
						}
					}

					string? buttonText = null;
					string? iconName = null;

					switch (Message.Status.Action)
					{
						case StatusAction.SubscribeToPackages:
							var p = package?.LocalParentPackage;

							if (p is null)
							{
								buttonText = Locale.Subscribe;
								iconName = "I_Add";
							}
							else if (!p.IsIncluded())
							{
								buttonText = Locale.Include;
								iconName = "I_Check";
							}
							else if (!(p.Mod?.IsEnabled() ?? true))
							{
								buttonText = Locale.Enable;
								iconName = "I_Enabled";
							}
							break;
						case StatusAction.SelectOne:
							buttonText = Locale.SelectThisPackage;
							iconName = "I_Ok";
							break;
						case StatusAction.Switch:
							buttonText = Locale.Switch;
							iconName = "I_Switch";
							break;
					}

					if (buttonText is null || package?.GetWorkshopInfo()?.IsCollection == true)
					{
						rect.Y += e.Rects.modRects[packageID].Height + GridPadding.Vertical;
						continue;
					}

					var buttonIcon = IconManager.GetIcon(iconName);
					var buttonSize = SlickButton.GetSize(e.Graphics, buttonIcon, buttonText, UI.Font(7.5F), UI.Scale(new Padding(3), UI.FontScale));

					e.Rects.buttonRects[packageID] = rect.Align(buttonSize, ContentAlignment.MiddleRight);

					SlickButton.DrawButton(e, e.Rects.buttonRects[packageID], buttonText, UI.Font(7.5F), buttonIcon, UI.Scale(new Padding(3), UI.FontScale), e.Rects.buttonRects[packageID].Contains(cursor) ? HoverState & ~HoverState.Focused : HoverState.Normal, Message.Status.Action is StatusAction.SelectOne ? ColorStyle.Active : ColorStyle.Green, backColor);

					rect.Y += e.Rects.modRects[packageID].Height + GridPadding.Vertical;
				}
			}

			y = rect.Y;
		}

		var otherWarnings = e.Item.ReportItems.Count(x => x.Status.Notification >= NotificationType.Caution) - 1;
		var finalY = y + (int)e.Graphics.Measure(otherWarnings > 0 ? LocaleCR.OtherCompatibilityWarnings.FormatPlural(otherWarnings) : Locale.ViewPackageCR, font, reportRect.Width - (2 * iconRect.Width) - GridPadding.Horizontal).Height + GridPadding.Vertical;
		var bottomRect = new Rectangle(e.ClipRectangle.X, y + (GridPadding.Vertical * 3), e.ClipRectangle.Width, finalY - y).Pad(GridPadding);


		//if (bottomRect.Y > y + GridPadding.Vertical * 3)
		//{
		//	bottomRect.Y =  y + GridPadding.Vertical * 3;

		//	e.Graphics.FillRectangle(new SolidBrush(BackColor), new Rectangle(e.ClipRectangle.X, bottomRect.Y, e.ClipRectangle.Width, e.ClipRectangle.Bottom - bottomRect.Y + GridPadding.Vertical * 2).InvertPad(GridPadding));
		//}

		//{
		//	using var fillBrush = new SolidBrush(e.BackColor);

		//	e.Graphics.FillRoundedRectangle(fillBrush, bottomRect.InvertPad(GridPadding + GridPadding).Pad(0, -GridPadding.Vertical, 0, -GridPadding.Vertical), (int)(5 * UI.FontScale), false, false);

		//	var maxY = bottomRect.Y - GridPadding.Vertical * 2;

		//	foreach (var key in e.Rects.modRects.Keys.ToList())
		//	{
		//		if (e.Rects.modRects[key].Y > maxY)
		//			e.Rects.modRects[key] = default;
		//		else if (e.Rects.modRects[key].Bottom > maxY)
		//			e.Rects.modRects[key] = e.Rects.modRects[key].Pad(0, 0, 0, e.Rects.modRects[key].Bottom - maxY);
		//	}
		//	foreach (var key in e.Rects.buttonRects.Keys.ToList())
		//	{
		//		if (e.Rects.buttonRects[key].Y > maxY)
		//			e.Rects.buttonRects[key] = default;
		//		else if (e.Rects.buttonRects[key].Bottom > maxY)
		//			e.Rects.buttonRects[key] = e.Rects.buttonRects[key].Pad(0, 0, 0, e.Rects.buttonRects[key].Bottom - maxY);
		//	}
		//}

		DrawDividerLine(e, bottomRect.Y - (GridPadding.Vertical * 4));

		e.Graphics.DrawString(otherWarnings > 0 ? LocaleCR.OtherCompatibilityWarnings.FormatPlural(otherWarnings) : Locale.ViewPackageCR, font, textBrush, bottomRect.Pad(iconRect.Width + GridPadding.Left, 0, iconRect.Width + GridPadding.Left, 0), new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

		if (otherWarnings > 0)
		{
			var iconRect2 = bottomRect.Align(iconRect.Size, ContentAlignment.MiddleLeft);

			e.Graphics.FillRoundedRectangle(brush, iconRect2, pad);

			using var icon2 = IconManager.GetIcon("I_Info", e.Rects.IncludedRect.Width * 3 / 4);
			e.Graphics.DrawImage(icon2.Color(color.GetTextColor()), iconRect2.CenterR(icon2.Size));
		}

		var rect3 = bottomRect.Align(iconRect.Size, ContentAlignment.MiddleRight);
		using var cricon = IconManager.GetIcon("I_CompatibilityReport", iconRect.Height * 3 / 4);

		SlickButton.DrawButton(e, rect3, string.Empty, Font, cricon, null, rect3.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

		e.Rects.CompatibilityRect = rect3;

		e.DrawableItem.CachedHeight = finalY - e.ClipRectangle.Y + (GridPadding.Vertical * 5) + Padding.Vertical;
	}

	private void GetAllButton(ICompatibilityItem Message, out string? allText, out string? allIcon, out ColorStyle colorStyle)
	{
		allText = null;
		allIcon = null;
		colorStyle = ColorStyle.Red;

		switch (Message.Status.Action)
		{
			case StatusAction.SubscribeToPackages:
				{
					var max = Message.Packages.Max(x =>
					{
						var p = x.GetLocalPackage();

						if (p is null)
						{
							return 3;
						}
						else if (!p.IsIncluded())
						{
							return 2;
						}
						else if (!p.IsEnabled())
						{
							return 1;
						}

						return 0;
					});

					colorStyle = ColorStyle.Green;
					allText = max switch { 3 => Locale.SubscribeAll, 2 => Locale.IncludeAll, 1 => Locale.EnableAll, _ => null };
					allIcon = max switch { 3 => "I_Add", 2 => "I_Check", 1 => "I_Enabled", _ => null };
				}
				break;
			case StatusAction.RequiresConfiguration:
				allText = _compatibilityManager.IsSnoozed(Message) ? Locale.UnSnooze : Locale.Snooze;
				allIcon = "I_Snooze";
				colorStyle = ColorStyle.Active;
				break;
			case StatusAction.UnsubscribeThis:
				allText = Locale.Unsubscribe;
				allIcon = "I_RemoveSteam";
				break;
			case StatusAction.UnsubscribeOther:
				allText = Message.Packages?.Length switch { 0 => null, _ => Locale.UnsubscribeAll };
				allIcon = "I_RemoveSteam";
				break;
			case StatusAction.ExcludeThis:
				allText = Locale.Exclude;
				allIcon = "I_X";
				break;
			case StatusAction.ExcludeOther:
				allText = Message.Packages?.Length switch { 0 => null, 1 => Locale.Exclude, _ => Locale.ExcludeAll };
				allIcon = "I_X";
				break;
			case StatusAction.RequestReview:
				allText = LocaleCR.RequestReview;
				allIcon = "I_RequestReview";
				colorStyle = ColorStyle.Active;
				break;
		}
	}

	private void DrawThumbnail(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e)
	{
		var thumbnail = e.Item.Package.GetThumbnail();

		if (thumbnail is null)
		{
			using var generic = (e.Item is ILocalPackageWithContents ? Properties.Resources.I_CollectionIcon : e.Item.Package!.IsMod ? Properties.Resources.I_ModIcon : Properties.Resources.I_AssetIcon).Color(FormDesign.Design.IconColor);

			drawThumbnail(generic);
		}
		else if (e.Item.Package!.IsLocal)
		{
			using var unsatImg = new Bitmap(thumbnail, e.Rects.IconRect.Size).Tint(Sat: 0);

			drawThumbnail(unsatImg);
		}
		else
		{
			drawThumbnail(thumbnail);
		}

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, e.Rects.IconRect, (int)(5 * UI.FontScale), FormDesign.Design.BackColor);
	}

	private int DrawDividerLine(PaintEventArgs e, int y)
	{
		var lineRect = new Rectangle(e.ClipRectangle.X + GridPadding.Horizontal, y + (GridPadding.Vertical * 2), e.ClipRectangle.Width - (GridPadding.Horizontal * 2), (int)(2 * UI.FontScale));
		using var lineBrush = new LinearGradientBrush(lineRect, default, default, 0F);

		lineBrush.InterpolationColors = new ColorBlend
		{
			Colors = new[] { Color.Empty, FormDesign.Design.AccentColor, FormDesign.Design.AccentColor, Color.Empty },
			Positions = new[] { 0.0f, 0.2f, 0.8f, 1f }
		};

		e.Graphics.FillRectangle(lineBrush, lineRect);

		return y + (GridPadding.Vertical * 4);
	}

	private void DrawTitleAndTagsAndVersionForList(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, ILocalPackageWithContents? localParentPackage, IWorkshopInfo? workshopInfo, bool isPressed)
	{
		using var font = UI.Font(9F, FontStyle.Bold);
		var mod = e.Item is not IAsset;
		var tags = new List<(Color Color, string Text)>();
		var text = mod ? e.Item.Package!.CleanName(out tags) : e.Item.ToString();
		using var brush = new SolidBrush(isPressed ? FormDesign.Design.ActiveForeColor : (e.Rects.CenterRect.Contains(CursorLocation) || e.Rects.IconRect.Contains(CursorLocation)) && e.HoverState.HasFlag(HoverState.Hovered) && !IsPackagePage ? FormDesign.Design.ActiveColor : ForeColor);
		e.Graphics.DrawString(text, font, brush, e.Rects.TextRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = CompactList ? StringAlignment.Center : StringAlignment.Near });

		var isVersion = localParentPackage?.Mod is not null && !e.Item.Package!.IsBuiltIn && !IsPackagePage;
		var versionText = isVersion ? "v" + localParentPackage!.Mod!.Version.GetString() : e.Item.Package!.IsBuiltIn ? Locale.Vanilla : (e.Item is ILocalPackage lp ? lp.LocalSize.SizeString() : workshopInfo?.ServerSize.SizeString());
		var date = workshopInfo?.ServerTime ?? e.Item.Package!.LocalParentPackage?.LocalTime;

		var padding = GridView ? GridPadding : GridPadding;
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

		tagRect = new Rectangle(e.Rects.TextRect.X, e.Rects.IconRect.Y, 0, e.Rects.IconRect.Height);

		if (e.Item.Package!.Id > 0)
		{
			using var icon = IconManager.GetSmallIcon("I_Steam");

			e.Rects.SteamIdRect = e.Graphics.DrawLabel(e.Item.Package.Id.ToString(), icon, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor), tagRect, ContentAlignment.BottomLeft, smaller: true, mousePosition: CursorLocation);

			tagRect.X += padding.Left + e.Rects.SteamIdRect.Width;
		}

		if (workshopInfo?.Author is not null)
		{
			using var icon = IconManager.GetSmallIcon("I_Developer");

			e.Rects.AuthorRect = e.Graphics.DrawLabel(workshopInfo?.Author.Name, icon, FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.BackColor), tagRect, ContentAlignment.BottomLeft, smaller: true, mousePosition: CursorLocation);

			tagRect.X += padding.Left + e.Rects.AuthorRect.Width;
		}

		tagRect.X = e.Rects.TextRect.X;
		tagRect.Y -= (int)(14 * UI.FontScale);

		if (!string.IsNullOrEmpty(versionText))
		{
			e.Rects.VersionRect = e.Graphics.DrawLabel(versionText, null, isVersion ? FormDesign.Design.YellowColor : FormDesign.Design.YellowColor.MergeColor(FormDesign.Design.AccentBackColor, 40), tagRect, ContentAlignment.BottomLeft, smaller: true, mousePosition: localParentPackage?.Mod is not null ? CursorLocation : null);

			tagRect.X += padding.Left + e.Rects.VersionRect.Width;
		}

		if (date.HasValue && !IsPackagePage)
		{
			var dateText = _settings.UserSettings.ShowDatesRelatively ? date.Value.ToRelatedString(true, false) : date.Value.ToString("g");

			e.Rects.DateRect = e.Graphics.DrawLabel(dateText, IconManager.GetSmallIcon("I_UpdateTime"), FormDesign.Design.AccentColor, tagRect, ContentAlignment.BottomLeft, smaller: true, mousePosition: CursorLocation);
		}
	}

	private void DrawIncludedButton(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, bool isIncluded, bool partialIncluded, ILocalPackageWithContents? package, out Color activeColor)
	{
		activeColor = default;

		if (package is null && e.Item.Package!.IsLocal)
		{
			return; // missing local item
		}

		var inclEnableRect = e.Rects.EnabledRect == Rectangle.Empty ? e.Rects.IncludedRect : Rectangle.Union(e.Rects.IncludedRect, e.Rects.EnabledRect);
		var incl = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item.Package!) ? "I_Wait" : partialIncluded ? "I_Slash" : isIncluded ? "I_Ok" : package is null ? "I_Add" : "I_Enabled");
		var mod = package?.Mod;
		var required = mod is not null && _modLogicManager.IsRequired(mod, _modUtil);

		DynamicIcon? enabl = null;
		if (_settings.UserSettings.AdvancedIncludeEnable && mod is not null)
		{
			enabl = new DynamicIcon(mod.IsEnabled() ? "I_Checked" : "I_Checked_OFF");

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

	private void DrawButtons(ItemPaintEventArgs<ICompatibilityInfo, Rectangles> e, bool isPressed, ILocalPackageWithContents? parentPackage, IWorkshopInfo? workshopInfo)
	{
		var padding = GridView ? GridPadding : GridPadding;
		var size = UI.Scale(CompactList ? new Size(24, 24) : new Size(28, 28), UI.FontScale);
		var rect = new Rectangle(e.ClipRectangle.Right - size.Width - (GridView ? 0 : GridPadding.Right), e.ClipRectangle.Y + ((e.Rects.IconRect.Height - size.Height) / 2), size.Width, size.Height);
		var backColor = Color.FromArgb(175, GridView ? FormDesign.Design.BackColor : FormDesign.Design.ButtonColor);

		if (parentPackage is not null)
		{
			rect.X -= rect.Width + padding.Left;
		}

		if (!IsPackagePage && workshopInfo?.Url is not null)
		{
			rect.X -= rect.Width + padding.Left;
		}

		using var brush = new SolidBrush(e.BackColor);
		e.Graphics.FillRectangle(brush, new Rectangle(rect.X + rect.Width, e.Rects.IconRect.Y, e.ClipRectangle.Right - rect.X - padding.Left, e.Rects.IconRect.Height));

		rect = new Rectangle(e.ClipRectangle.Right - size.Width - (GridView ? 0 : GridPadding.Right), e.ClipRectangle.Y + ((e.Rects.IconRect.Height - size.Height) / 2), size.Width, size.Height);

		if (parentPackage is not null)
		{
			using var icon = IconManager.GetIcon("I_Folder", size.Height * 3 / 4);

			SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

			e.Rects.FolderRect = rect;

			rect.X -= rect.Width + padding.Left;
		}

		if (!IsPackagePage && workshopInfo?.Url is not null)
		{
			using var icon = IconManager.GetIcon("I_Steam", rect.Height * 3 / 4);

			SlickButton.DrawButton(e, rect, string.Empty, Font, icon, null, rect.Contains(CursorLocation) ? e.HoverState | (isPressed ? HoverState.Pressed : 0) : HoverState.Normal, backColor: backColor);

			e.Rects.SteamRect = rect;

			rect.X -= rect.Width + padding.Left;
		}

		var seamRectangle = new Rectangle(rect.X + rect.Width - (int)(40 * UI.UIScale), e.ClipRectangle.Y, (int)(40 * UI.UIScale), e.ClipRectangle.Height);

		using var seamBrush = new LinearGradientBrush(seamRectangle, Color.Empty, e.BackColor, 0F);

		e.Graphics.FillRectangle(seamBrush, seamRectangle);
	}

	protected override void OnItemMouseClick(DrawableItem<ICompatibilityInfo, Rectangles> item, MouseEventArgs e)
	{
		base.OnItemMouseClick(item, e);

		if (e.Button == MouseButtons.Right)
		{
			ShowRightClickMenu(item.Item.Package!);
			return;
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		var rects = item.Rectangles;

		if (rects.IncludedRect.Contains(e.Location))
		{
			if (item.Item.Package!.LocalPackage is not ILocalPackage localPackage)
			{
				if (!item.Item.Package!.IsLocal)
				{
					_subscriptionsManager.Subscribe(new IPackage[] { item.Item.Package! });
				}

				return;
			}

			{
				_packageUtil.SetIncluded(localPackage, !_packageUtil.IsIncluded(localPackage));
			}

			return;
		}

		if (rects.EnabledRect.Contains(e.Location) && item.Item.Package!.LocalPackage is not null)
		{
			{
				_packageUtil.SetEnabled(item.Item.Package!.LocalPackage, !_packageUtil.IsEnabled(item.Item.Package!.LocalPackage));
			}

			return;
		}

		if (rects.FolderRect.Contains(e.Location))
		{
			PlatformUtil.OpenFolder(item.Item.Package!.LocalPackage?.FilePath);
			return;
		}

		if (rects.SteamRect.Contains(e.Location) && item.Item.Package!.GetWorkshopInfo()?.Url is string url)
		{
			PlatformUtil.OpenUrl(url);
			return;
		}

		if (rects.AuthorRect.Contains(e.Location) && item.Item.Package!.GetWorkshopInfo()?.Author is IUser user)
		{
			{
				var pc = new PC_UserPage(user);

				(FindForm() as BasePanelForm)?.PushPanel(null, pc);
			}

			return;
		}

		if (rects.FolderNameRect.Contains(e.Location) && item.Item.Package!.IsLocal)
		{
			{
				Clipboard.SetText(Path.GetFileName(item.Item.Package.LocalPackage?.Folder ?? string.Empty));

			}

			return;
		}

		if (rects.SteamIdRect.Contains(e.Location))
		{
			{
				Clipboard.SetText(item.Item.Package!.Id.ToString());
			}

			return;
		}

		if (rects.CompatibilityRect.Contains(e.Location))
		{
			{
				var pc = new PC_PackagePage((IPackage?)item.Item.Package!.GetLocalPackage() ?? item.Item.Package!, true);

				(FindForm() as BasePanelForm)?.PushPanel(null, pc);

				if (_settings.UserSettings.ResetScrollOnPackageClick)
				{
					ScrollTo(item.Item);
				}
			}
			return;
		}

		if (rects.VersionRect.Contains(e.Location) && item.Item.Package!.LocalParentPackage?.Mod is IMod mod)
		{
			Clipboard.SetText(mod.Version.GetString());
		}

		if (rects.CenterRect.Contains(e.Location) || rects.IconRect.Contains(e.Location))
		{
			Program.MainForm.PushPanel(null, item.Item.Package!.GetWorkshopInfo()?.IsCollection == true ? new PC_ViewCollection(item.Item.Package!) : new PC_PackagePage((IPackage?)item.Item.Package!.GetLocalPackage() ?? item.Item.Package!));

			if (_settings.UserSettings.ResetScrollOnPackageClick)
			{
				ScrollTo(item.Item);
			}

			return;
		}

		if (rects.DateRect.Contains(e.Location))
		{
			var date = item.Item.Package!.GetWorkshopInfo()?.ServerTime ?? item.Item.Package!.LocalParentPackage?.LocalTime;

			if (date.HasValue)
			{
				{
					Clipboard.SetText(date.Value.ToString("g"));
				}
			}

			return;
		}

		var Message = item.Item.ReportItems.FirstOrDefault(x => x.Status.Notification == item.Item.GetNotification() && !_compatibilityManager.IsSnoozed(x));

		foreach (var rect in rects.buttonRects)
		{
			if (rect.Value.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left)
				{
					Clicked(item.Item, Message, rect.Key, true);
				}

				return;
			}
		}

		foreach (var rect in rects.modRects)
		{
			if (rect.Value.Contains(e.Location))
			{
				if (e.Button == MouseButtons.Left)
				{
					Clicked(item.Item, Message, rect.Key, false);
				}
				else if (e.Button == MouseButtons.Right && rect.Key.GetLocalPackage() is not null)
				{
					var items = PC_PackagePage.GetRightClickMenuItems(rect.Key.GetLocalPackage()!);

					this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, items));
				}

				return;
			}
		}

		if (e.Button == MouseButtons.Left && rects.SnoozeRect.Contains(e.Location))
		{
			_compatibilityManager.ToggleSnoozed(Message);
			FilterChanged();
		}

		if (e.Button == MouseButtons.Left && rects.AllButtonRect.Contains(e.Location))
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
					FilterChanged();
					break;
				case StatusAction.UnsubscribeThis:
					_subscriptionsManager.UnSubscribe(new[] { item.Item.Package! });
					break;
				case StatusAction.UnsubscribeOther:
					_subscriptionsManager.UnSubscribe(Message.Packages!);
					break;
				case StatusAction.ExcludeThis:
				{
					var pp = item.Item.Package!.GetLocalPackage();
					if (pp is not null)
					{
						_packageUtil.SetIncluded(pp, false);
					}
				}
				break;
				case StatusAction.ExcludeOther:
					foreach (var p in Message.Packages!)
					{
						var pp = p.GetLocalPackage();
						if (pp is not null)
						{
							_packageUtil.SetIncluded(pp, false);
						}
					}
					break;
				case StatusAction.RequestReview:
					Program.MainForm.PushPanel(null, new PC_RequestReview(item.Item.Package!));
					break;
			}
		}
	}

	public void ShowRightClickMenu(IPackage item)
	{
		var items = PC_PackagePage.GetRightClickMenuItems(item);

		this.TryBeginInvoke(() => SlickToolStrip.Show(FindForm() as SlickForm, items));
	}

	private void Clicked(ICompatibilityInfo info, ICompatibilityItem Message, IPackageIdentity item, bool button)
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

		var p = package?.LocalParentPackage;

		if (p is null)
		{
			//_subscribingTo.Add(item);

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
				foreach (var id in Message.Packages!)
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
				_packageUtil.SetIncluded(info.Package!.LocalParentPackage!, false);
				break;
		}
	}

	protected override Rectangles GenerateRectangles(ICompatibilityInfo item, Rectangle rectangle)
	{
		var rects = new Rectangles(item)
		{
			IconRect = rectangle.Align(UI.Scale(new Size(48, 48), UI.FontScale), ContentAlignment.TopLeft)
		};

		rectangle.Height = rects.IconRect.Height;

		var includedSize = 28;

		if (_settings.UserSettings.AdvancedIncludeEnable && item.Package?.LocalParentPackage?.Mod is not null)
		{
			rects.EnabledRect = rects.IncludedRect = rectangle.Pad(GridPadding).Align(UI.Scale(new Size(includedSize, includedSize), UI.FontScale), ContentAlignment.MiddleLeft);

			rects.EnabledRect.X += rects.EnabledRect.Width;
		}
		else
		{
			rects.IncludedRect = rectangle.Pad(GridPadding).Align(UI.Scale(new Size(includedSize, includedSize), UI.FontScale), ContentAlignment.MiddleLeft);
		}

		{
			rects.IconRect.X = Math.Max(rects.IncludedRect.Right, rects.EnabledRect.Right) + GridPadding.Horizontal;

			rects.TextRect = rectangle.Pad(rects.IconRect.Right - rectangle.X + GridPadding.Left, 0, 0, rectangle.Height).AlignToFontSize(UI.Font(9F, FontStyle.Bold), ContentAlignment.TopLeft);
		}

		rects.CenterRect = rects.TextRect.Pad(-GridPadding.Horizontal, 0, 0, 0);

		return rects;
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

	public class Rectangles : IDrawableItemRectangles<ICompatibilityInfo>
	{
		internal Rectangle IncludedRect;
		internal Rectangle EnabledRect;
		internal Rectangle FolderRect;
		internal Rectangle IconRect;
		internal Rectangle TextRect;
		internal Rectangle SteamRect;
		internal Rectangle CenterRect;
		internal Rectangle AuthorRect;
		internal Rectangle VersionRect;
		internal Rectangle DateRect;
		internal Dictionary<IPackageIdentity, Rectangle> buttonRects = new();
		internal Dictionary<IPackageIdentity, Rectangle> modRects = new();
		internal Rectangle SteamIdRect;
		internal Rectangle SnoozeRect;
		internal Rectangle AllButtonRect;
		internal Rectangle CompatibilityRect;

		public ICompatibilityInfo Item { get; set; }
		public Rectangle FolderNameRect { get; internal set; }

		public Rectangles(ICompatibilityInfo item)
		{
			Item = item;
		}

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			if (IncludedRect.Contains(location))
			{
				if (Item.Package?.LocalPackage is null)
				{
					if (Item.Package?.IsLocal == false)
					{
						text = Locale.SubscribeToItem;
						point = IncludedRect.Location;
						return true;
					}
				}

				text = $"{Locale.ExcludeInclude}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByThisIncludedStatus.ToString().ToLower())}";
				point = IncludedRect.Location;
				return true;
			}

			if (EnabledRect.Contains(location) && Item.Package?.LocalPackage is not null)
			{
				text = $"{Locale.EnableDisable}\r\n\r\n{string.Format(Locale.AltClickTo, Locale.FilterByThisEnabledStatus.ToString().ToLower())}";
				point = EnabledRect.Location;
				return true;
			}

			if (SteamRect.Contains(location))
			{
				text = Locale.ViewOnSteam;
				point = SteamRect.Location;
				return true;
			}

			if (SteamIdRect.Contains(location))
			{
				text = getFilterTip(string.Format(Locale.CopyToClipboard, Item.Package?.Id), string.Format(Locale.AddToSearch, Item.Package?.Id));
				point = SteamIdRect.Location;
				return true;
			}

			if (FolderNameRect.Contains(location))
			{
				var folder = Path.GetFileName(Item.Package?.LocalPackage?.Folder ?? string.Empty);
				text = getFilterTip(string.Format(Locale.CopyToClipboard, folder), string.Format(Locale.AddToSearch, folder));
				point = FolderNameRect.Location;
				return true;
			}

			if (AuthorRect.Contains(location))
			{
				text = getFilterTip(Locale.OpenAuthorPage, Locale.FilterByThisAuthor.Format(Item.Package?.GetWorkshopInfo()?.Author?.Name ?? "this author"));
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

			if (VersionRect.Contains(location) && Item.Package?.LocalParentPackage?.Mod is IMod mod)
			{
				text = Locale.CopyVersionNumber;
				point = VersionRect.Location;
				return true;
			}

			if (CenterRect.Contains(location) || IconRect.Contains(location))
			{
				{
					text = Locale.OpenPackagePage;
				}

				point = CenterRect.Location;
				return true;
			}

			if (DateRect.Contains(location))
			{
				var date = Item.Package?.GetWorkshopInfo()?.ServerTime ?? Item.Package?.LocalParentPackage?.LocalTime;
				if (date.HasValue)
				{
					text = getFilterTip(string.Format(Locale.CopyToClipboard, date.Value.ToString("g")), Locale.FilterSinceThisDate);
					point = DateRect.Location;
					return true;
				}
			}

			if (SnoozeRect.Contains(location))
			{
				text = Locale.Snooze;
				point = SnoozeRect.Location;
				return true;
			}

			if (AllButtonRect.Contains(location))
			{
				text = Locale.Snooze;
				point = AllButtonRect.Location;
				return true;
			}

			text = string.Empty;
			point = default;
			return false;

			static string getFilterTip(string? text, string? _) => text ?? "";
		}

		public bool IsHovered(Control instance, Point location)
		{
			return
				SnoozeRect.Contains(location) ||
				AllButtonRect.Contains(location) ||
				CompatibilityRect.Contains(location) ||
				IncludedRect.Contains(location) ||
				EnabledRect.Contains(location) ||
				FolderRect.Contains(location) ||
				SteamIdRect.Contains(location) ||
				SteamRect.Contains(location) ||
				AuthorRect.Contains(location) ||
				IconRect.Contains(location) ||
				CenterRect.Contains(location) ||
				DateRect.Contains(location) ||
				buttonRects.Values.Any(x => x.Contains(location)) ||
				modRects.Values.Any(x => x.Contains(location)) ||
				(VersionRect.Contains(location) && Item?.Package?.LocalParentPackage?.Mod is not null);
		}
	}
}
