﻿using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;
public class IncludeAllButton : SlickControl
{
	public delegate Task TaskAction();

	private readonly bool _doubleButtons;
	private readonly Func<List<IPackageIdentity>> GetPackages;
	private Rectangle IncludedRect;
	private Rectangle ActionRect;
#if CS1
	private Rectangle EnabledRect;
#endif

	public event EventHandler? ActionClicked;
	public TaskAction? IncludeAllClicked;
	public TaskAction? ExcludeAllClicked;
	public TaskAction? EnableAllClicked;
	public TaskAction? DisableAllClicked;
	public TaskAction? SubscribeAllClicked;
	private Size size;
	private bool _isSelected;
	private readonly ISettings _settings;
	private readonly IPackageManager _packageManager;
	private readonly IPackageUtil _packageUtil;

	public bool IsSelected
	{
		get => _isSelected; set
		{
			_isSelected = value;

			//if (!value)
			//{
			//	Size = size;
			//	ActionRect = ClientRectangle.Align(IncludedRect.Size, ContentAlignment.MiddleRight);
			//}
			//else
			{
				SetExpandedSize();
			}

			Invalidate();
		}
	}

	public IncludeAllButton(Func<List<IPackageIdentity>> getMethod)
	{
		ServiceCenter.Get(out _settings, out _packageManager, out _packageUtil);

		Margin = default;
		Cursor = Cursors.Hand;
		GetPackages = getMethod;
		_doubleButtons = _settings.UserSettings.AdvancedIncludeEnable;
	}

	protected override void UIChanged()
	{
		Margin = UI.Scale(new Padding(4, 4, 4, 5));
		Padding = UI.Scale(new Padding(3));

		var itemHeight = UI.Scale(28);

#if CS2
		size = Size = new Size((itemHeight * 2) + Margin.Horizontal, itemHeight - UI.Scale(4));

		var rect = ClientRectangle.Align(new Size(itemHeight, Height), ContentAlignment.MiddleLeft);
		IncludedRect = rect;
		rect.X += rect.Width + Margin.Horizontal;
		ActionRect = rect;
#else
		Size = new Size(ItemHeight * (_doubleButtons ? 3 : 2), ItemHeight - UI.Scale(4));
#endif

		//if (IsSelected)
		{
			SetExpandedSize();
		}
	}

	private void SetExpandedSize()
	{
		var action = new DynamicIcon("Actions");
		using var graphics = CreateGraphics();
		using var font = UI.Font(8.25F);
		using var actionIcon = action.Get(IncludedRect.Width * 3 / 4);
		var size = SlickButton.GetSize(graphics, actionIcon, "BulkActions", font, Padding);

		Size = new Size(IncludedRect.Width + Margin.Horizontal + size.Width, Height);
		ActionRect = new Rectangle(Width - size.Width, IncludedRect.Y, size.Width, IncludedRect.Height);
	}

#if CS2
	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var packages = GetPackages();
		var subscribe = packages.Any(x => !_packageUtil.IsIncluded(x));
		var enable = subscribe || packages.Any(x => !_packageUtil.IsEnabled(x));

		if (IncludedRect.Contains(e.Location))
		{
			if (subscribe)
			{
				SlickTip.SetTo(this, "IncludeAll");
			}
			else if (enable)
			{
				SlickTip.SetTo(this, "EnableAll");
			}
			else
			{
				SlickTip.SetTo(this, "DisableAll");
			}

			Cursor.Current = Cursors.Hand;
		}
		else if (ActionRect.Contains(e.Location))
		{
			SlickTip.SetTo(this, "BulkActions");

			Cursor.Current = Cursors.Hand;
		}
		else
		{
			SlickTip.SetTo(this, null);

			Cursor.Current = Cursors.Default;
		}
	}

	protected override async void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Left)
		{
			var packages = GetPackages();
			var subscribe = packages.Any(x => !_packageUtil.IsIncluded(x));
			var enable = subscribe || packages.Any(x => !_packageUtil.IsEnabled(x));

			if (IncludedRect.Contains(e.Location) && !Loading)
			{
				Loading = true;

				if (subscribe)
				{
					if (IncludeAllClicked is not null)
					{
						await IncludeAllClicked();
					}
				}
				else if (enable)
				{
					if (EnableAllClicked is not null)
					{
						await EnableAllClicked();
					}
				}
				else
				{
					if (DisableAllClicked is not null)
					{
						await DisableAllClicked();
					}
				}

				Loading = false;
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

		var cursorLocation = PointToClient(Cursor.Position);
		var packages = GetPackages();
		var subscribe = packages.Any(x => !_packageUtil.IsIncluded(x));
		var enable = subscribe || packages.Any(x => !_packageUtil.IsEnabled(x));

		if (IncludedRect.Contains(cursorLocation))
		{
			enable = !enable;
		}

		{
			SlickButton.GetColors(out var fore, out var back, IncludedRect.Contains(cursorLocation) ? HoverState : default, subscribe ? ColorStyle.Active : enable ? ColorStyle.Red : ColorStyle.Green);

			var incl = new DynamicIcon(subscribe ? "Add" : enable ? "Enabled" : "Ok");
			using var inclIcon = incl.Get(IncludedRect.Width * 3 / 4);
			using var brush1 = IncludedRect.Gradient(back, 1.5F);

			e.Graphics.FillRoundedRectangle(brush1, IncludedRect, Margin.Left);

			if (Loading)
			{
				DrawLoader(e.Graphics, IncludedRect.CenterR(inclIcon.Size), fore);
			}
			else
			{
				e.Graphics.DrawImage(inclIcon.Color(fore), IncludedRect.CenterR(inclIcon.Size));
			}
		}

		{
			SlickButton.GetColors(out var fore, out var back, ActionRect.Contains(cursorLocation) ? HoverState : default, buttonType: IsSelected ? ButtonType.Active : ButtonType.Normal);

			var action = new DynamicIcon("Actions");
			using var actionIcon = action.Get(IncludedRect.Width * 3 / 4);
			using var brush2 = ActionRect.Gradient(back, 1.5F);

			e.Graphics.FillRoundedRectangle(brush2, ActionRect, Margin.Left);

			//if (IsSelected)
			//{
				using var brush3 = ActionRect.Gradient(fore, 1.5F);
				using var font = UI.Font(8.25F);
				using var stringFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

				e.Graphics.DrawString(LocaleHelper.GetGlobalText("BulkActions"), font, brush3, ActionRect.Pad(Padding), stringFormat);
				e.Graphics.DrawImage(actionIcon.Color(fore), ActionRect.Pad(Padding).Align(actionIcon.Size, ContentAlignment.MiddleLeft));
			//}
			//else
			//{
			//	e.Graphics.DrawImage(actionIcon.Color(fore), ActionRect.CenterR(actionIcon.Size));
			//}
		}
	}
#else

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
			else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsIncluded(x!)))
			{
				SlickTip.SetTo(this, "ExcludeAll");
			}
			else
			{
				SlickTip.SetTo(this, "IncludeAll");
			}

			Cursor.Current = Cursors.Hand;
		}
		else if (EnabledRect.Contains(e.Location))
		{
			if (subscribe)
			{
				SlickTip.SetTo(this, "SubscribeAll");
			}
			else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsEnabled(x!)))
			{
				SlickTip.SetTo(this, "DisableAll");
			}
			else
			{
				SlickTip.SetTo(this, "EnableAll");
			}

			Cursor.Current = Cursors.Hand;
		}
		else if (ActionRect.Contains(e.Location))
		{
			SlickTip.SetTo(this, "BulkActions");

			Cursor.Current = Cursors.Hand;
		}
		else
		{
			Cursor.Current = Cursors.Default;
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
				else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsIncluded(x!)))
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
				else if (localPackages.SelectWhereNotNull().All(x => _packageUtil.IsEnabled(x!)))
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

		var width = UI.Scale(28);
		var rectangle = ClientRectangle;
		var CursorLocation = PointToClient(Cursor.Position);
		var color = FormDesign.Design.ActiveColor;
		var packages = GetPackages();
		var localPackages = packages.ToList(x => _packageManager.GetPackageById(x));
		var subscribe = localPackages.Any(x => x is null);
		var include = !subscribe && localPackages.Any(x => _packageUtil.IsIncluded(x!));
		var enable = !subscribe && localPackages.Any(x => _packageUtil.IsEnabled(x!));

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

		{
			var incl = new DynamicIcon(subscribe ? "Add" : include ? "Ok" : "Enabled");
			using var inclIcon = incl.Get(width * 3 / 4);

			SlickButton.GetColors(out var fore, out var back, IncludedRect.Contains(CursorLocation) ? HoverState : default, subscribe ? ColorStyle.Active : include ? ColorStyle.Red : ColorStyle.Green);

			using var brush1 = IncludedRect.Gradient(back, 1.5F);

			e.Graphics.FillRoundedRectangle(brush1, IncludedRect, 4);
			e.Graphics.DrawImage(inclIcon.Color(fore), IncludedRect.CenterR(inclIcon.Size));
		}

		if (_doubleButtons && EnabledRect != default)
		{
			var enl = new DynamicIcon(enable ? "Checked" : "Checked_OFF");
			using var enlIcon = enl.Get(width * 3 / 4);

			{
				SlickButton.GetColors(out var fore, out var back, EnabledRect.Contains(CursorLocation) ? HoverState : default, include ? ColorStyle.Red : ColorStyle.Green);

				using var brush2 = EnabledRect.Gradient(back, 1.5F);

				e.Graphics.FillRoundedRectangle(brush2, EnabledRect, 4);
				e.Graphics.DrawImage(enlIcon.Color(fore), EnabledRect.CenterR(enlIcon.Size));
			}
		}

		var action = new DynamicIcon("Actions");
		using var actionIcon = action.Get(width * 3 / 4);

		{
			SlickButton.GetColors(out var fore, out var back, ActionRect.Contains(CursorLocation) ? HoverState : default);
			using var brush3 = ActionRect.Gradient(back, 1.5F);
			e.Graphics.FillRoundedRectangle(brush3, ActionRect, 4);
			e.Graphics.DrawImage(actionIcon.Color(fore), EnabledRect.CenterR(actionIcon.Size));
		}
	}
#endif
}
