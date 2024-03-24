using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
internal class ViewTypeControl : SlickControl
{
	public delegate Task TaskAction();

	private Rectangle CompactRect;
	private Rectangle ListRect;
	private Rectangle GridRect;

	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool GridView { get; internal set; }
	[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CompactList { get; internal set; }

	[DefaultValue(true)]
	public bool WithCompactList { get; set; } = true;
	[DefaultValue(true)]
	public bool WithList { get; set; } = true;
	[DefaultValue(true)]
	public bool WithGrid { get; set; } = true;

	public event EventHandler? CompactClicked;
	public event EventHandler? ListClicked;
	public event EventHandler? GridClicked;

	public ViewTypeControl()
	{
		Cursor = Cursors.Hand;
	}

	protected override void UIChanged()
	{
		Margin = UI.Scale(new Padding(4, 4, 4, 5), UI.FontScale);
		Padding = UI.Scale(new Padding(3), UI.FontScale);

		var itemHeight = (int)(28 * UI.FontScale);

		Size = new Size((WithCompactList ? itemHeight : 0) + (WithList ? itemHeight : 0) + (WithGrid ? itemHeight : 0), itemHeight - (int)(4 * UI.FontScale));

		var rect = ClientRectangle.Align(new Size(itemHeight, Height), ContentAlignment.MiddleLeft);

		if (WithCompactList)
		{
			CompactRect = rect;
			rect.X += rect.Width;
		}

		if (WithList)
		{
			ListRect = rect;
			rect.X += rect.Width;
		}

		if (WithGrid)
		{
			GridRect = rect;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		if (CompactRect.Contains(e.Location))
		{
			SlickTip.SetTo(this, "Switch to Compact-View");
		}
		else if (ListRect.Contains(e.Location))
		{
			SlickTip.SetTo(this, "Switch to List-View");
		}
		else if (GridRect.Contains(e.Location))
		{
			SlickTip.SetTo(this, "Switch to Grid-View");
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		if (CompactRect.Contains(e.Location))
		{
			CompactClicked?.Invoke(this, e);
		}
		else if (ListRect.Contains(e.Location))
		{
			ListClicked?.Invoke(this, e);
		}
		else if (GridRect.Contains(e.Location))
		{
			GridClicked?.Invoke(this, e);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var CursorLocation = PointToClient(Cursor.Position);

		using var brush = new SolidBrush(FormDesign.Design.ButtonColor);

		e.Graphics.FillRoundedRectangle(brush, ClientRectangle, Padding.Left);

		if (WithCompactList)
		{
			var rect = CompactRect;
			using var icon = IconManager.GetIcon("CompactList", rect.Width * 3 / 4);

			if (rect.Contains(CursorLocation))
			{
				SlickButton.GetColors(out var fore, out var back, HoverState);
				using var brush1 = rect.Gradient(back, 1.5F);
				e.Graphics.FillRoundedRectangle(brush1, rect, Padding.Left);
				e.Graphics.DrawImage(icon.Color(CompactList && !HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : fore), rect.CenterR(icon.Size));
			}
			else
			{
				e.Graphics.DrawImage(icon.Color(CompactList ? FormDesign.Design.ActiveColor : FormDesign.Design.ButtonForeColor), rect.CenterR(icon.Size));
			}
		}

		if (WithList)
		{
			var rect = ListRect;
			using var icon = IconManager.GetIcon("List", rect.Width * 3 / 4);

			if (rect.Contains(CursorLocation))
			{
				SlickButton.GetColors(out var fore, out var back, HoverState);
				using var brush1 = rect.Gradient(back, 1.5F);
				e.Graphics.FillRoundedRectangle(brush1, rect, Padding.Left);
				e.Graphics.DrawImage(icon.Color(!CompactList && !GridView && !HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : fore), rect.CenterR(icon.Size));
			}
			else
			{
				e.Graphics.DrawImage(icon.Color(!CompactList && !GridView ? FormDesign.Design.ActiveColor : FormDesign.Design.ButtonForeColor), rect.CenterR(icon.Size));
			}
		}

		if (WithGrid)
		{
			var rect = GridRect;
			using var icon = IconManager.GetIcon("Grid", rect.Width * 3 / 4);

			if (rect.Contains(CursorLocation))
			{
				SlickButton.GetColors(out var fore, out var back, HoverState);
				using var brush1 = rect.Gradient(back, 1.5F);
				e.Graphics.FillRoundedRectangle(brush1, rect, Padding.Left);
				e.Graphics.DrawImage(icon.Color(GridView && !HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : fore), rect.CenterR(icon.Size));
			}
			else
			{
				e.Graphics.DrawImage(icon.Color(GridView ? FormDesign.Design.ActiveColor : FormDesign.Design.ButtonForeColor), rect.CenterR(icon.Size));
			}
		}
	}
}
