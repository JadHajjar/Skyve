using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Dashboard;
public abstract class IDashboardItem : SlickImageControl
{
	public event EventHandler? ResizeRequested;

	public string Key { get; }
	public bool MoveInProgress { get; internal set; }
	public bool ResizeInProgress { get; internal set; }

	public IDashboardItem()
	{
		Key = GetType().Name;
	}

	public abstract int CalculateHeight(int width, PaintEventArgs e);
	public abstract void DrawItem(PaintEventArgs e);
	public virtual Rectangle GetMoveArea()
	{
		return new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, (int)(25 * UI.FontScale));
	}

	protected override void UIChanged()
	{
		Padding = UI.Scale(new Padding(12, 12, 0, 0),UI.FontScale);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		var clip = MoveInProgress || ResizeInProgress ? ClientRectangle : ClientRectangle.Pad(Padding);

		e.Graphics.SetUp(BackColor);

		e.Graphics.SetClip(clip);

		try
		{ 
			using var pe = new PaintEventArgs(e.Graphics, clip);

			DrawItem(pe); 
		}
		catch { }

		if (MoveInProgress || ResizeInProgress)
		{
			using var brush = new SolidBrush(Color.FromArgb(100, FormDesign.Design.AccentBackColor));

			e.Graphics.FillRectangle(brush, ClientRectangle);
		}

		base.OnPaint(e);
	}

	protected void OnResizeRequested()
	{
		ResizeRequested?.Invoke(this, EventArgs.Empty);
	}
}
