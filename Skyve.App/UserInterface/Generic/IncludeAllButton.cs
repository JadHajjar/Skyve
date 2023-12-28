using Skyve.App.UserInterface.Lists;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class IncludeAllButton<T> : SlickControl where T : IPackageIdentity
{
	private readonly bool _doubleButtons;
	private readonly Func<List<T>> GetPackages;
	private Rectangle IncludedRect;
	private Rectangle EnabledRect;
	private Rectangle ActionRect;

	public event EventHandler? ActionClicked;
	public event EventHandler? IncludeAllClicked;
	public event EventHandler? ExcludeAllClicked;
	public event EventHandler? EnableAllClicked;
	public event EventHandler? DisableAllClicked;
	public event EventHandler? SubscribeAllClicked;

	private readonly ISettings _settings;
	private readonly IPackageManager _packageManager;
	private readonly IPackageUtil _packageUtil;

	public IncludeAllButton(Func<List<T>> getMethod)
	{
		ServiceCenter.Get(out _settings, out _packageManager, out _packageUtil);

		Margin = default;
		Cursor = Cursors.Hand;
		GetPackages = getMethod;
		_doubleButtons = _settings.UserSettings.AdvancedIncludeEnable;
	}

	protected override void UIChanged()
	{
		Margin = Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);

		var ItemHeight = (int)(28 * UI.FontScale);
		var includeItemHeight = ItemHeight;

		Size = new Size(includeItemHeight * (_doubleButtons ? 3 : 2), includeItemHeight - (int)(4 * UI.FontScale));
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var packages = GetPackages();
		var localPackages = packages.ToList(x => _packageManager.GetPackageById(x));
		var subscribe = localPackages.Any(x => x is null);

		if (IncludedRect.Contains(e.Location))
		{
			if (subscribe)
			{
				SlickTip.SetTo(this, "SubscribeAll");
			}
			else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsIncluded(x!.LocalData!)))
			{
				SlickTip.SetTo(this, "ExcludeAll");
			}
			else
			{
				SlickTip.SetTo(this, "IncludeAll");
			}
		}
		else if (EnabledRect.Contains(e.Location))
		{
			if (subscribe)
			{
				SlickTip.SetTo(this, "SubscribeAll");
			}
			else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsEnabled(x!.LocalData!)))
			{
				SlickTip.SetTo(this, "DisableAll");
			}
			else
			{
				SlickTip.SetTo(this, "EnableAll");
			}
		}
		else if (ActionRect.Contains(e.Location))
		{
			SlickTip.SetTo(this, "OtherActions");
		}
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			var packages = GetPackages();
			var localPackages = packages.ToList(x => _packageManager.GetPackageById(x));
			var subscribe = localPackages.Any(x => x is null);

			if (IncludedRect.Contains(e.Location))
			{
				if (subscribe)
				{
					SubscribeAllClicked?.Invoke(this, e);
				}
				else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsIncluded(x!.LocalData!)))
				{
					ExcludeAllClicked?.Invoke(this, e);
				}
				else
				{
					IncludeAllClicked?.Invoke(this, e);
				}
			}
			else if (EnabledRect.Contains(e.Location))
			{
				if (subscribe)
				{
					SubscribeAllClicked?.Invoke(this, e);
				}
				else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsEnabled(x!.LocalData!)))
				{
					DisableAllClicked?.Invoke(this, e);
				}
				else
				{
					EnableAllClicked?.Invoke(this, e);
				}
			}
			else if (ActionRect.Contains(e.Location))
			{
				ActionClicked?.Invoke(this, e);
			}
		}
		else if (e.Button == MouseButtons.Right)
		{
			ActionClicked?.Invoke(this, e);
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var width = (int)(28 * UI.FontScale);
		var rectangle = ClientRectangle;
		var CursorLocation = PointToClient(Cursor.Position);
		var color = FormDesign.Design.ActiveColor;
		var packages = GetPackages();
		var localPackages = packages.ToList(x => _packageManager.GetPackageById(x));
		var subscribe = localPackages.Any(x => x is null);
		var include = !subscribe && localPackages.All(x => _packageUtil.IsEnabled(x!.LocalData!));
		var enable = !subscribe && localPackages.All(x => _packageUtil.IsEnabled(x!.LocalData!));

		if (_doubleButtons && !subscribe)
		{
			IncludedRect = rectangle.Align(new Size(width, Height), ContentAlignment.MiddleLeft);
			EnabledRect = IncludedRect.Pad(IncludedRect.Width, 0, -IncludedRect.Width, 0);
			ActionRect = EnabledRect.Pad(EnabledRect.Width, 0, -EnabledRect.Width, 0);
		}
		else
		{
			EnabledRect = default;
			IncludedRect = rectangle.Align(new Size(width, Height), ContentAlignment.MiddleLeft);
			ActionRect = IncludedRect.Pad(IncludedRect.Width, 0, -IncludedRect.Width, 0);
		}

		if (HoverState.HasFlag(HoverState.Hovered))
		{
			if (IncludedRect.Contains(CursorLocation))
			{
				color = include ? FormDesign.Design.RedColor : FormDesign.Design.GreenColor;
			}
			else if (EnabledRect.Contains(CursorLocation))
			{
				color = enable ? FormDesign.Design.RedColor : FormDesign.Design.GreenColor;
			}
		}

		var incl = new DynamicIcon(subscribe ? "I_Add" : include ? "I_Ok" : "I_Enabled");
		var inclIcon = incl.Get(width * 3 / 4);

		if (HoverState.HasFlag(HoverState.Hovered) && IncludedRect.Contains(CursorLocation))
		{
			using var brush1 = IncludedRect.Gradient(HoverState.HasFlag(HoverState.Pressed) ? color : Color.FromArgb(30, ForeColor), 1.5F);
			e.Graphics.FillRoundedRectangle(brush1, IncludedRect, 4);
		}

		e.Graphics.DrawImage(inclIcon.Color(!IncludedRect.Contains(CursorLocation) ? ForeColor : HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : color), IncludedRect.CenterR(inclIcon.Size));

		if (_doubleButtons && EnabledRect != default)
		{
			var enl = new DynamicIcon(enable ? "I_Checked" : "I_Checked_OFF");
			var enlIcon = enl.Get(width * 3 / 4);

			if (HoverState.HasFlag(HoverState.Hovered) && EnabledRect.Contains(CursorLocation))
			{
				using var brush2 = EnabledRect.Gradient(HoverState.HasFlag(HoverState.Pressed) ? color : Color.FromArgb(30, ForeColor), 1.5F);
				e.Graphics.FillRoundedRectangle(brush2, EnabledRect, 4);
			}
			e.Graphics.DrawImage(enlIcon.Color(!EnabledRect.Contains(CursorLocation) ? ForeColor : HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : color), EnabledRect.CenterR(enlIcon.Size));
		}

		var action = new DynamicIcon("I_Actions");
		var actionIcon = action.Get(width * 3 / 4);

		if (HoverState.HasFlag(HoverState.Hovered) && ActionRect.Contains(CursorLocation))
		{
			using var brush3 = ActionRect.Gradient(HoverState.HasFlag(HoverState.Pressed) ? color : Color.FromArgb(30, ForeColor), 1.5F);
			e.Graphics.FillRoundedRectangle(brush3, ActionRect, 4);
		}

		e.Graphics.DrawImage(actionIcon.Color(!ActionRect.Contains(CursorLocation) ? ForeColor : HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveForeColor : color), ActionRect.CenterR(actionIcon.Size));
	}
}
