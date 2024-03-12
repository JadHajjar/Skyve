using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class PackageCrList : SlickStackedListControl<IPackageIdentity, PackageCrList.Rectangles>
{
	private readonly IWorkshopService _workshopService;
	private readonly ICompatibilityManager _compatibilityManager;

	public IPackageIdentity? CurrentPackage { get; set; }

	public bool ShowCompleted { get; set; } = true;

    public PackageCrList()
	{
		ServiceCenter.Get(out _workshopService, out _compatibilityManager);
		HighlightOnHover = true;
		SeparateWithLines = true;
		ItemHeight = 35;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);
		Font = UI.Font(7F, FontStyle.Bold);
	}

	protected override IEnumerable<DrawableItem<IPackageIdentity, Rectangles>> OrderItems(IEnumerable<DrawableItem<IPackageIdentity, Rectangles>> items)
	{
		return items.OrderByDescending(x => _workshopService.GetInfo(x.Item)?.ServerTime);
	}

	protected override bool IsItemActionHovered(DrawableItem<IPackageIdentity, Rectangles> item, Point location)
	{
		return true;
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<IPackageIdentity, Rectangles> e)
	{
		base.OnPaintItemList(e);

		var cr = e.Item.GetPackageInfo();
		var stability = cr?.Stability ?? PackageStability.NotReviewed;
		var clipRectangle = e.ClipRectangle;
		var imageRect = clipRectangle.Pad(Padding);
		var thumbnail = e.Item.GetThumbnail();
		var isUpToDate = ShowCompleted && cr?.ReviewDate > e.Item.GetWorkshopInfo()?.ServerTime;

		imageRect.Width = imageRect.Height;

		if (CurrentPackage == e.Item)
		{
			var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor);
			e.Graphics.FillRoundedRectangle(activeBrush, clipRectangle.Align(new Size(2 * Padding.Left, imageRect.Height), ContentAlignment.MiddleRight), Padding.Left);

			clipRectangle.Width -= 3 * Padding.Left;
		}

		if (thumbnail is null)
		{
			using var generic = IconManager.GetIcon(isUpToDate ? "I_OK" : "I_Paradox", isUpToDate ? (imageRect.Height * 3 / 4) : imageRect.Height).Color(e.BackColor);
			using var backBrush = new SolidBrush(isUpToDate ? FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.IconColor, 35) : FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(backBrush, imageRect, (int)(5 * UI.FontScale));
			e.Graphics.DrawImage(generic, imageRect.CenterR(generic.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(thumbnail, imageRect, (int)(5 * UI.FontScale), FormDesign.Design.BackColor);

			using var pen = new Pen(e.BackColor, 2.5F);
			e.Graphics.DrawRoundedRectangle(pen, imageRect, (int)(5 * UI.FontScale));

			if (isUpToDate && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var greenBrush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.GreenColor));
				using var icon = IconManager.GetIcon("I_Ok", imageRect.Height * 3 / 4).Color(FormDesign.Design.GreenColor.GetTextColor());

				e.Graphics.FillRoundedRectangle(greenBrush, imageRect, (int)(5 * UI.FontScale));
				e.Graphics.DrawImage(icon, imageRect.CenterR(icon.Size));
			}
		}

		var text = e.Item.CleanName(out var tags);
		var tagSizes = 0;

		for (var i = 0; i < tags.Count; i++)
		{
			var size = e.Graphics.MeasureLabel(tags[i].Text, null, smaller: true);

			tagSizes += Padding.Left + size.Width;
		}

		var textRect = clipRectangle.Pad(imageRect.Right + Padding.Left, Padding.Top / 2, tagSizes, clipRectangle.Height / 2 - Padding.Top);
		using var brushTitle = new SolidBrush(e.BackColor.GetTextColor());
		using var font = UI.Font(8F, FontStyle.Bold).FitTo(text, textRect.Pad(Padding.Left), e.Graphics);

		e.Graphics.DrawString(text, font, brushTitle, textRect.Location);

		textRect = clipRectangle.Pad(imageRect.Right + Padding.Left, Padding.Top / 2, 0, 0);
		var textSize = e.Graphics.Measure(text, font, textRect.Width - tagSizes - Padding.Right);
		var tagRect = new Rectangle(textRect.X + (int)textSize.Width, textRect.Y, 0, (int)textSize.Height);

		for (var i = 0; i < tags.Count; i++)
		{
			var rect = e.Graphics.DrawLabel(tags[i].Text, null, tags[i].Color, tagRect, ContentAlignment.MiddleLeft, smaller: true);

			tagRect.X += Padding.Left + rect.Width;
		}

		text = LocaleCR.Get(stability.ToString());
		textRect = new Rectangle(textRect.X, textRect.Bottom - (textRect.Height / 2), textRect.Width, textRect.Height / 2 + Padding.Bottom);
		using var font2 = UI.Font(7F, FontStyle.Bold).FitToWidth(text, textRect.Pad(Padding.Left), e.Graphics);
		using var brush = new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? brushTitle.Color : Color.FromArgb(200, CRNAttribute.GetNotification(stability).GetColor()));
		using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

		e.Graphics.DrawString(text, font2, brush, textRect, format);
	}

	public class Rectangles : IDrawableItemRectangles<IPackageIdentity>
	{
		public IPackageIdentity Item { get; set; }

		public bool GetToolTip(Control instance, Point location, out string text, out Point point)
		{
			text = string.Empty;
			point = default;
			return false;
		}

		public bool IsHovered(Control instance, Point location)
		{
			return true;
		}
	}
}
