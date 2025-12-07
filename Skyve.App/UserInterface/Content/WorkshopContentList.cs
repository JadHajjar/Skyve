using Skyve.App.UserInterface.Generic;
using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class WorkshopContentList : ContentList
{
	private ulong _id;

	public WorkshopContentList(SkyvePage page, bool loaded, GetAllItems getItems, Func<LocaleHelper.Translation> getItemText) : base(page, loaded, getItems, getItemText)
	{
		DD_Sorting.WorkshopSort = true;
		I_SortOrder.Parent = null;

		TLP_Main.SetColumn(DD_SearchTime, 3);
		DD_SearchTime.Visible = true;

		PaginationControl = new()
		{
			Dock = DockStyle.Bottom,
			Margin = default
		};

		TagsControl = new();

		TagsControl.SelectedTagChanged += FilterChanged;

		TLP_Main.Controls.Add(TagsControl, TLP_Main.ColumnCount - 1, TLP_Main.RowCount - 1);

		TLP_Main.RowStyles.Add(new());
		TLP_Main.RowCount++;

		TLP_Main.Controls.Add(PaginationControl, 0, TLP_Main.RowCount - 1);
		TLP_Main.SetColumnSpan(PaginationControl, TLP_Main.ColumnCount - 1);

		TLP_Main.SetRowSpan(TagsControl, 2);

//#if CS2
//		var tagService = ServiceCenter.Get<ITagsService>();
//		_workshopTags = [
//			tagService.CreateWorkshopTag("All", ""),
//			tagService.CreateWorkshopTag("Code Mods", "Code Mod"),
//			tagService.CreateWorkshopTag("Assets", "Prefab"),
//			tagService.CreateWorkshopTag("Maps", "Map"),
//			tagService.CreateWorkshopTag("Save-games", "Savegame"),
//		];
//#endif
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		DD_SearchTime.Enabled = DD_Sorting.SelectedItem is PackageSorting.Best;
	}

	//protected override void UIChanged()
	//{
	//	base.UIChanged();

	//	C_TagButtons.Height = C_TagButtons.Height == 0 ? 0 : UI.Scale(28);
	//}

	//protected override void RefreshAuthorAndTags()
	//{
	//	base.RefreshAuthorAndTags();

	//	if (TagsControl is not null)
	//	{
	//		C_TagButtons.Height = UI.Scale(28);
	//	}
	//}

	protected override async void OnSearch()
	{
		var id = ++_id;

		I_Refresh.Loading = true;

		if (PaginationControl is not null)
		{
			PaginationControl.Page = 0;
		}

		if (!string.IsNullOrWhiteSpace(TB_Search.Text))
		{
			await Task.Delay(250);
		}

		if (id == _id)
		{
			await RefreshItems();
		}
	}

	private void C_TagButtons_MouseMove(object sender, MouseEventArgs e)
	{
	}

	private void C_TagButtons_MouseClick(object sender, MouseEventArgs e)
	{
	}

	private void C_TagButtons_Paint(object sender, PaintEventArgs e)
	{
		if (TagsControl is null || WorkshopTagsControl.Tags.Count == 0)
		{
			return;
		}

		//e.Graphics.SetUp(FormDesign.Design.BackColor);

		//using var font = UI.Font(9F, FontStyle.Bold);

		//var cursor = C_TagButtons.PointToClient(Cursor.Position);
		//var xPos = 0;
		//var maxWidth = UI.Scale(6) + (int)(_workshopTags ?? WorkshopTagsControl.Tags).Where(x => x.IsSelectable).Max(x => e.Graphics.Measure(x.Value.ToUpper(), font).Width);

		//foreach (var tag in _workshopTags ?? WorkshopTagsControl.Tags.Where(x => x.IsSelectable))
		//{
		//	var isSelected = WorkshopTagsControl.SelectedTags.Contains(tag.Key);
		//	var rect = new Rectangle(xPos, 0, maxWidth, C_TagButtons.Height);

		//	if (rect.Contains(cursor) && C_TagButtons.HoverState.HasFlag(HoverState.Hovered))
		//	{
		//		e.Graphics.FillRectangle(new SolidBrush(FormDesign.Design.ButtonColor), rect);
		//	}

		//	e.Graphics.DrawString(tag.Value.ToUpper(), font, new SolidBrush(FormDesign.Design.ButtonForeColor), rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

		//	xPos += maxWidth;
		//}
	}
}