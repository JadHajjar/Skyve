using System.Drawing;
using System.Drawing.Drawing2D;

namespace Skyve.App.UserInterface.Lists;

public partial class ItemListControl
{
	private int DrawScore(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, IWorkshopInfo? workshopInfo, int xdiff)
	{
		if (workshopInfo is null)
		{
			return 0;
		}

		var score = workshopInfo.VoteCount;
		var padding = GridView ? GridPadding : Padding;
		var height = e.Rects.IconRect.Bottom - Math.Max(e.Rects.TextRect.Bottom, Math.Max(e.Rects.VersionRect.Bottom, e.Rects.DateRect.Bottom)) - padding.Bottom;

		e.Rects.ScoreRect = e.Graphics.DrawLargeLabel(new Point(e.Rects.TextRect.X + xdiff + (xdiff == 0 ? 0 : padding.Left), e.Rects.IconRect.Bottom), score.ToMagnitudeString(), "I_VoteFilled", workshopInfo!.HasVoted ? FormDesign.Design.GreenColor : null, alignment: ContentAlignment.BottomLeft, padding: padding, height: height, cursorLocation: CursorLocation);

		return 0;
	}

	private void DrawThumbnail(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, ILocalPackageIdentity? localIdentity, IWorkshopInfo? workshopInfo)
	{
		var thumbnail = e.Item.GetThumbnail();

		if (thumbnail is null)
		{
			using var generic = IconManager.GetIcon(e.Item is IAsset ? "I_Assets" : _page is SkyvePage.Mods ? "I_Mods" : _page is SkyvePage.Packages ? "I_Package" : "I_Paradox", e.Rects.IconRect.Height).Color(e.BackColor);
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

		var date = workshopInfo is null || workshopInfo.ServerTime == default ? (localIdentity?.LocalTime ?? default) : workshopInfo.ServerTime;
		var isRecent = date > DateTime.UtcNow.AddDays(-7);

		if (isRecent && !IsPackagePage)
		{
			using var pen = new Pen(FormDesign.Design.ActiveColor, (float)(2 * UI.FontScale));

			if (!GridView)
			{
				e.Graphics.DrawRoundedRectangle(pen, e.Rects.IconRect, (int)(5 * UI.FontScale));

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

			e.Graphics.FillRoundedRectangle(gradientBrush, e.Rects.IconRect.Pad(0, e.Rects.IconRect.Height * 2 / 3, 0, 0), (int)(5 * UI.FontScale));
			e.Graphics.DrawRoundedRectangle(pen, e.Rects.IconRect, (int)(5 * UI.FontScale));

			e.Graphics.DrawString(Locale.RecentlyUpdated, font, dateBrush, e.Rects.IconRect.Pad(GridPadding), stringFormat);
		}

		void drawThumbnail(Bitmap generic) => e.Graphics.DrawRoundedImage(generic, e.Rects.IconRect, (int)(5 * UI.FontScale), FormDesign.Design.BackColor/*, blur: e.Rects.IconRect.Contains(CursorLocation)*/);
	}

#if CS2
	private void DrawIncludedButton(ItemPaintEventArgs<IPackageIdentity, Rectangles> e, bool isIncluded, bool isPartialIncluded, bool isEnabled, ILocalPackageIdentity? localIdentity, out Color activeColor)
	{
		activeColor = default;

		if (localIdentity is null && e.Item.IsLocal())
		{
			return; // missing local item
		}

		var required = _modLogicManager.IsRequired(localIdentity, _modUtil);

		if (!required && isIncluded && e.Rects.IncludedRect.Contains(CursorLocation))
		{
			isPartialIncluded = false;
			isEnabled = !isEnabled;
		}

		if (isEnabled)
		{
			activeColor = isPartialIncluded ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
		}

		Color iconColor;

		if (required && activeColor != default)
		{
			iconColor = FormDesign.Design.Type is FormDesignType.Light ? activeColor.MergeColor(ForeColor, 75) : activeColor;
			activeColor = activeColor.MergeColor(BackColor, FormDesign.Design.Type is FormDesignType.Light ? 35 : 20);
		}
		else if (activeColor == default && e.Rects.IncludedRect.Contains(CursorLocation))
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
			else if (e.Rects.IncludedRect.Contains(CursorLocation))
			{
				activeColor = activeColor.MergeColor(ForeColor, 75);
			}

			iconColor = activeColor.GetTextColor();
		}

		using var brush = e.Rects.IncludedRect.Gradient(activeColor);
		e.Graphics.FillRoundedRectangle(brush, e.Rects.IncludedRect, (int)(4 * UI.FontScale));

		if (e.DrawableItem.Loading)
		{
			DrawLoader(e.Graphics, e.Rects.IncludedRect.CenterR(e.Rects.IncludedRect.Height / 2, e.Rects.IncludedRect.Height / 2), iconColor);
			return;
		}

		var icon = new DynamicIcon(_subscriptionsManager.IsSubscribing(e.Item) ? "I_Wait" : isPartialIncluded ? "I_Slash" : isEnabled ? "I_Ok" : !isIncluded ? "I_Add" : "I_Enabled");
		using var includedIcon = icon.Get(e.Rects.IncludedRect.Height * 3 / 4).Color(iconColor);

		e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
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

			e.Graphics.FillRoundedRectangle(brush, inclEnableRect, (int)(4 * UI.FontScale));

			using var includedIcon = incl.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);
			using var enabledIcon = enabl?.Get(e.Rects.IncludedRect.Width * 3 / 4).Color(iconColor);

			e.Graphics.DrawImage(includedIcon, e.Rects.IncludedRect.CenterR(includedIcon.Size));
			if (enabledIcon is not null)
			{
				e.Graphics.DrawImage(enabledIcon, e.Rects.EnabledRect.CenterR(includedIcon.Size));
			}
		}
#endif
}