using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dropdowns;
internal class SortingDropDown : SlickSelectionDropDown<PackageSorting>
{
	public SkyvePage SkyvePage { get; set; }

	protected override void OnHandleCreated(EventArgs e)
	{
		base.OnHandleCreated(e);

		if (Live)
		{
			Items = Enum.GetValues(typeof(PackageSorting)).Cast<PackageSorting>().Where(x => x < PackageSorting.Mod).ToArray();

			selectedItem = (PackageSorting)ServiceCenter.Get<ISettings>().UserSettings.PageSettings.GetOrAdd(SkyvePage).Sorting;
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Middle)
		{
			SelectedItem = PackageSorting.Default;
		}

		base.OnMouseClick(e);
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		Height = 0;
	}

	protected override bool SearchMatch(string searchText, PackageSorting item)
	{
		return searchText.SearchCheck(LocaleHelper.GetGlobalText($"Sorting_{item}"));
	}

	public override void ResetValue()
	{

	}

	protected override void PaintItem(PaintEventArgs e, Rectangle rectangle, Color foreColor, HoverState hoverState, PackageSorting item)
	{
		var text = LocaleHelper.GetGlobalText($"Sorting_{item}");

		using var icon = IconManager.GetIcon(GetIcon(item), rectangle.Height - 2).Color(foreColor);

		e.Graphics.DrawImage(icon, rectangle.Align(icon.Size, ContentAlignment.MiddleLeft));

		e.Graphics.DrawString(text, Font, new SolidBrush(foreColor), rectangle.Pad(icon.Width + Padding.Left, 0, 0, 0).AlignToFontSize(Font, ContentAlignment.MiddleLeft, e.Graphics), new StringFormat { Trimming = StringTrimming.EllipsisCharacter });
	}

	private static string GetIcon(PackageSorting item)
	{
		return item switch
		{
			PackageSorting.Name => "I_FileName",
			PackageSorting.Author => "I_Developer",
			PackageSorting.FileSize => "I_MicroSd",
			PackageSorting.CompatibilityReport => "I_CompatibilityReport",
			PackageSorting.UpdateTime => "I_UpdateTime",
			PackageSorting.SubscribeTime => "I_Add",
			PackageSorting.Status => "I_Broken",
			PackageSorting.Subscribers => "I_People",
			PackageSorting.Votes => "I_Vote",
			PackageSorting.LoadOrder => "I_Wrench",
			_ => "I_Check",
		};
	}
}
