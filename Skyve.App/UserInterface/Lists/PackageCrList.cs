using Skyve.App.UserInterface.Panels;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Lists;
public class PackageCrList : SlickStackedListControl<ulong, PackageCrList.Rectangles>
{
	private readonly IWorkshopService _workshopService;
	private readonly ICompatibilityManager _compatibilityManager;
	public PackageCrList()
	{
		ServiceCenter.Get(out _workshopService, out _compatibilityManager);
		HighlightOnHover = true;
		SeparateWithLines = true;
		ItemHeight = 32;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);
		Font = UI.Font(7F, FontStyle.Bold);
	}

	protected override IEnumerable<DrawableItem<ulong, Rectangles>> OrderItems(IEnumerable<DrawableItem<ulong, Rectangles>> items)
	{
		return items.OrderByDescending(x => _workshopService.GetInfo(new GenericPackageIdentity(x.Item))?.ServerTime);
	}

	protected override bool IsItemActionHovered(DrawableItem<ulong, Rectangles> item, Point location)
	{
		return true;
	}

	protected override void OnPaintItemList(ItemPaintEventArgs<ulong, Rectangles> e)
	{
		base.OnPaintItemList(e);

		var Package = _workshopService.GetInfo(new GenericPackageIdentity(e.Item));
		var imageRect = e.ClipRectangle.Pad(Padding);
		imageRect.Width = imageRect.Height;
		var image = Package?.GetThumbnail();
		var panel = PanelContent.GetParentPanel(this);

		if (panel is PC_CompatibilityManagement cm && cm.CurrentPackage?.Id == e.Item || panel is PC_ReviewRequests rr && rr.CurrentPackage == e.Item)
		{
			e.Graphics.FillRoundedRectangle(new SolidBrush(FormDesign.Design.ActiveColor), imageRect.Align(new Size(2 * Padding.Left, imageRect.Height), ContentAlignment.MiddleLeft), Padding.Left);

			imageRect.X += 3 * Padding.Left;
		}

		if (image is not null)
		{
			e.Graphics.DrawRoundedImage(image, imageRect, (int)(3 * UI.FontScale), FormDesign.Design.AccentBackColor);
		}
		else
		{
			using var generic = IconManager.GetIcon("I_Package", imageRect.Height).Color(BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, imageRect, (int)(3 * UI.FontScale));
			e.Graphics.DrawImage(generic, imageRect.CenterR(generic.Size));
		}

		List<(Color Color, string Text)>? tags = null;

		var textRect = e.ClipRectangle.Pad(imageRect.Right + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.TopLeft);

		e.Graphics.DrawString(Package?.CleanName(out tags) ?? Locale.UnknownPackage, Font, new SolidBrush(e.HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : ForeColor), textRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter });

		var tagRect = new Rectangle(textRect.Right, textRect.Y, 0, textRect.Height);

		if (tags is not null)
		{
			foreach (var item in tags)
			{
				tagRect.X -= Padding.Left + e.Graphics.DrawLabel(item.Text, null, item.Color, tagRect, ContentAlignment.TopRight, smaller: true).Width;
			}
		}

		if (Package is null)
		{
			return;
		}

		var cr = _compatibilityManager.GetPackageInfo(Package);

		if (cr is null)
		{
			return;
		}

		e.Graphics.DrawLabel(LocaleCR.Get(cr.Stability.ToString()), null, CRNAttribute.GetNotification(cr.Stability).GetColor().MergeColor(FormDesign.Design.BackColor), e.ClipRectangle.Pad(imageRect.Right + Padding.Left, 0, 0, 0), ContentAlignment.BottomLeft, smaller: true);

		if (cr.ReviewDate > Package.ServerTime)
		{
			e.Graphics.DrawLabel(Locale.UpToDate, null, FormDesign.Design.GreenColor.MergeColor(FormDesign.Design.BackColor), e.ClipRectangle.Pad(imageRect.Right + Padding.Left, 0, 0, 0), ContentAlignment.BottomRight, smaller: true);
		}
	}

	public class Rectangles : IDrawableItemRectangles<ulong>
	{
		public ulong Item { get; set; }

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
