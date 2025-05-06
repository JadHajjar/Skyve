using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class PackageCrList : SlickStackedListControl<IPackageIdentity>
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

		Padding = UI.Scale(new Padding(3, 2, 3, 2));
		Font = UI.Font(7F, FontStyle.Bold);
	}

	protected override IEnumerable<IDrawableItem<IPackageIdentity>> OrderItems(IEnumerable<IDrawableItem<IPackageIdentity>> items)
	{
		return items.OrderByDescending(x => _workshopService.GetInfo(x.Item)?.ServerTime);
	}

	protected override bool IsItemActionHovered(DrawableItem<IPackageIdentity, GenericDrawableItemRectangles<IPackageIdentity>> item, Point location)
	{
		return true;
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<IPackageIdentity, GenericDrawableItemRectangles<IPackageIdentity>> e)
	{

		var cr = e.Item.GetPackageInfo();
		var stability = cr?.Stability ?? PackageStability.NotReviewed;
		var clipRectangle = e.ClipRectangle;
		var imageRect = clipRectangle.Pad(Padding.Left);
		var thumbnail = e.Item.GetThumbnail() ?? ItemListControl.WorkshopThumb;
		var isUpToDate = ShowCompleted && cr?.ReviewDate.ToLocalTime() > e.Item.GetWorkshopInfo()?.ServerTime.ToLocalTime();

		imageRect.Width = imageRect.Height;

		if (CurrentPackage == e.Item)
		{
			var filledRect = e.ClipRectangle.Pad(0, -Padding.Top / 2, Padding.Right / 2, -Padding.Bottom / 2);

			e.BackColor = BackColor.MergeColor(FormDesign.Design.ActiveColor, 75);

			base.OnPaintItemList(e);

			var activeBrush = new SolidBrush(FormDesign.Design.ActiveColor);
			e.Graphics.FillRoundedRectangle(activeBrush, clipRectangle.Align(new Size(2 * Padding.Left, imageRect.Height), ContentAlignment.MiddleRight), Padding.Left);

			clipRectangle.Width -= 3 * Padding.Left;
		}
		else
		{
			base.OnPaintItemList(e);
		}

		if (thumbnail is null)
		{
			using var generic = IconManager.GetIcon(isUpToDate ? "Ok" : "Paradox", isUpToDate ? (imageRect.Height * 3 / 4) : imageRect.Height).Color(ForeColor);
			using var backBrush = new SolidBrush(isUpToDate ? FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.IconColor, 35) : FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(backBrush, imageRect, UI.Scale(3));
			e.Graphics.DrawImage(generic, imageRect.CenterR(generic.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(thumbnail, imageRect, UI.Scale(3), e.BackColor);

			if (isUpToDate && !e.HoverState.HasFlag(HoverState.Hovered))
			{
				using var greenBrush = new SolidBrush(Color.FromArgb(150, FormDesign.Design.GreenColor));
				using var icon = IconManager.GetIcon("Ok", imageRect.Height * 3 / 4).Color(FormDesign.Design.GreenColor.GetTextColor());

				e.Graphics.FillRoundedRectangle(greenBrush, imageRect, UI.Scale(3));
				e.Graphics.DrawImage(icon, imageRect.CenterR(icon.Size));
			}
		}

		var textRect = clipRectangle.Pad(imageRect.Right + Padding.Left, Padding.Top / 2, Padding.Right, (clipRectangle.Height / 2) - Padding.Top);
		using var brushTitle = new SolidBrush(e.BackColor.GetTextColor());

		DrawTextAndTags(e, e.Item, brushTitle, e.BackColor, textRect);

		textRect = clipRectangle.Pad(imageRect.Right + Padding.Left, (clipRectangle.Height / 2) - Padding.Top, Padding.Right, Padding.Top / 2);

		var text = ShowCompleted ? LocaleCR.Get(stability.ToString()).One : LocaleCR.ActiveReportsCount.FormatPlural((e.Item as ReviewRequest)?.Count ?? 0);
		using var font2 = UI.Font(7F).FitToWidth(text, textRect.Pad(Padding.Left), e.Graphics);
		using var format = new StringFormat { LineAlignment = StringAlignment.Far };
		using var brush = ShowCompleted
			? new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? brushTitle.Color : Color.FromArgb(200, CRNAttribute.GetNotification(stability).GetColor()))
			: new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? brushTitle.Color : Color.FromArgb(200, FormDesign.Design.RedColor.MergeColor(FormDesign.Design.GreenColor, Math.Min(5, (e.Item as ReviewRequest)?.Count ?? 0) * 20)));

		e.Graphics.DrawString(text, font2, brush, textRect, format);
	}

	private int DrawTextAndTags(PaintEventArgs e, IPackageIdentity package, SolidBrush textBrush, Color backColor, Rectangle rect)
	{
		var text = package.CleanName(out var tags) ?? Locale.UnknownPackage;
		using var font = UI.Font(8F, FontStyle.Bold);
		using var format = new StringFormat { LineAlignment = StringAlignment.Center };

		using var highResBmp = new Bitmap(UI.Scale(500), rect.Height);
		using var highResG = Graphics.FromImage(highResBmp);

		highResG.SetUp(backColor);

		var textSize = highResG.Measure(text, font);

		highResG.DrawString(text, font, textBrush, new Rectangle(default, highResBmp.Size), format);

		var tagRect = new Rectangle((int)textSize.Width + (Margin.Left / 4), 0, 0, rect.Height);

		if (tags is not null)
		{
			foreach (var item in tags)
			{
				tagRect.X += (Margin.Left / 4) + highResG.DrawLabel(item.Text, null, item.Color, tagRect, ContentAlignment.MiddleLeft, smaller: true).Width;
			}
		}

		var factor = Math.Min(1, (double)rect.Width / tagRect.X);

		e.Graphics.SetClip(rect);
		e.Graphics.DrawImage(highResBmp, new Rectangle(rect.X, rect.Y, (int)(highResBmp.Width * factor), (int)(rect.Height * factor)));
		e.Graphics.ResetClip();

		return (int)(rect.Height * factor);
	}
}
