﻿using SkyveApp.UserInterface.Lists;

using System.Drawing;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Generic;
internal class IncludeAllButton<T> : SlickControl where T : IPackage
{
	private readonly bool _doubleButtons;
	private readonly ItemListControl<T>? LC_Items;
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

	public IncludeAllButton(ItemListControl<T>? lC_Items)
	{
		Margin = default;
		Cursor = Cursors.Hand;
		LC_Items = lC_Items;
		_settings = ServiceCenter.Get<ISettings>();
		_doubleButtons = _settings.UserSettings.AdvancedIncludeEnable;
	}

	protected override void UIChanged()
	{
		Margin = Padding = UI.Scale(new Padding(3, 2, 3, 2), UI.FontScale);

		var ItemHeight = (int)(28 * UI.FontScale);
		var includeItemHeight = ItemHeight;

		Size = new Size(includeItemHeight * (_doubleButtons ? 3 : 2), includeItemHeight - (int)(4*UI.FontScale));
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var packages = LC_Items?.SafeGetItems() ?? new();
		var subscribe = packages.Any(x => x.Item.LocalPackage is null);

		if (IncludedRect.Contains(e.Location))
		{
			if (subscribe)
			{
				SlickTip.SetTo(this, "SubscribeAll");
			}
			else if (packages.SelectWhereNotNull(x => x.Item.LocalPackage).All(x => x!.IsIncluded()))
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
			else if (packages.SelectWhereNotNull(x => x.Item.LocalParentPackage?.Mod).All(x => x!.IsEnabled()))
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
			var packages = LC_Items?.SafeGetItems() ?? new();
			var subscribe = packages.Any(x => x.Item.LocalPackage is null);

			if (IncludedRect.Contains(e.Location))
			{
				if (subscribe)
				{
					SubscribeAllClicked?.Invoke(this, e);
				}
				else if (packages.SelectWhereNotNull(x => x.Item.LocalPackage).All(x => x!.IsIncluded()))
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
				else if (packages.SelectWhereNotNull(x => x.Item.LocalParentPackage?.Mod).All(x => x!.IsEnabled()))
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
		var packages = LC_Items?.SafeGetItems() ?? new();
		var subscribe = packages.Any(x => x.Item.LocalPackage is null);
		var include = !subscribe && packages.All(x => x.Item.LocalPackage!.IsIncluded());
		var enable = !subscribe && packages.SelectWhereNotNull(x => x.Item.LocalParentPackage?.Mod).All(x => x!.IsEnabled());

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
