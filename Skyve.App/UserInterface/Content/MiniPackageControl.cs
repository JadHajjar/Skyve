using Skyve.App.Interfaces;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class MiniPackageControl : SlickControl
{
	private readonly IPackageIdentity? _package;
	private readonly IWorkshopService _workshopService = ServiceCenter.Get<IWorkshopService>();
	private readonly ISubscriptionsManager _subscriptionsManager = ServiceCenter.Get<ISubscriptionsManager>();
	private readonly IModLogicManager _modLogicManager = ServiceCenter.Get<IModLogicManager>();
	private readonly IModUtil _modUtil = ServiceCenter.Get<IModUtil>();

	public IPackageIdentity? Package => _package ?? _workshopService.GetInfo(new GenericPackageIdentity(Id));
	public ulong Id { get; }

	public bool ReadOnly { get; set; }
	public bool Large { get; set; }
	public bool ShowIncluded { get; set; }
	public bool IsDlc { get; set; }

	public MiniPackageControl(ulong modId)
	{
		Id = modId;
	}

	public MiniPackageControl(IPackageIdentity package)
	{
		_package = package;
		Id = package.Id;
	}

	protected override void UIChanged()
	{
		Height = (int)((Large ? 32 : 24) * UI.FontScale);

		Padding = UI.Scale(new Padding(4), UI.FontScale);
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Alt)
		{
			Invalidate();
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var imageRect = ClientRectangle.Pad(Padding);
		imageRect = imageRect.Align(new Size(imageRect.Height, imageRect.Height), ContentAlignment.MiddleRight);

		Cursor = ClientRectangle.Pad(1, 1, ReadOnly && !ShowIncluded ? 1 : (imageRect.Width + Padding.Horizontal), 1).Contains(e.Location)
			|| ((!ReadOnly || ShowIncluded) && !IsDlc && imageRect.Contains(e.Location)) ? Cursors.Hand : Cursors.Default;
	}

	protected override async void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (Package is null)
		{
			return;
		}

		switch (e.Button)
		{
			case MouseButtons.Left:
			case MouseButtons.None:
				var imageRect = ClientRectangle.Pad(Padding);
				imageRect = imageRect.Align(new Size(imageRect.Height, imageRect.Height), ContentAlignment.MiddleRight);

				if (!ReadOnly && imageRect.Contains(e.Location))
				{
					Dispose();
				}
				else if (ShowIncluded &&!IsDlc && imageRect.Contains(e.Location))
				{
					var isIncluded = Package.IsIncluded(out var partialIncluded) && !partialIncluded;

					Loading = true;

					if (!isIncluded || ModifierKeys.HasFlag(Keys.Alt))
					{
						await _modUtil.SetIncluded(Package, !isIncluded);
					}
					else
					{
						var enable = !Package.IsEnabled();

						if (enable || !_modLogicManager.IsRequired(Package.GetLocalPackageIdentity(), _modUtil))
						{
							await _modUtil.SetEnabled(Package, enable);
						}
					}

					Loading = false;
				}
				else if (ClientRectangle.Pad(1, 1, imageRect.Width + Padding.Horizontal, 1).Contains(e.Location))
				{
					ServiceCenter.Get<IInterfaceService>().OpenPackagePage(Package, false);
				}

				break;
			case MouseButtons.Right:
				var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(Package);

				this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, items));

				break;
			case MouseButtons.Middle:
				if (!ReadOnly)
				{
					Dispose();
				}

				break;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var imageRect = ClientRectangle.Pad(Padding);
		imageRect.Width = imageRect.Height;
		var image = Package?.GetThumbnail();
		var textRect = ClientRectangle.Pad(1, 1, ReadOnly && !ShowIncluded ? 1 : (imageRect.Width + Padding.Horizontal), 1);

		if (HoverState.HasFlag(HoverState.Hovered) && textRect.Contains(PointToClient(Cursor.Position)))
		{
			using var backBrush = new SolidBrush(Color.FromArgb(25, FormDesign.Design.ForeColor));

			e.Graphics.FillRoundedRectangle(backBrush, textRect, Padding.Left);
		}

		if (image is not null)
		{
			if (Package!.IsLocal())
			{
				using var unsatImg = new Bitmap(image, imageRect.Size).Tint(Sat: 0);
				e.Graphics.DrawRoundedImage(unsatImg, imageRect, Padding.Left);
			}
			else
			{
				e.Graphics.DrawRoundedImage(image, imageRect, Padding.Left);
			}
		}
		else
		{
			using var generic = IconManager.GetIcon(IsDlc ? "Dlc" : "Package", imageRect.Height).Color(BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, imageRect, (int)(4 * UI.FontScale));
			e.Graphics.DrawImage(generic, imageRect.CenterR(generic.Size));
		}

		List<(Color Color, string Text)>? tags = null;

		textRect = textRect.Pad(imageRect.Right + Padding.Left, 0, 0, 0);
		var text = Package?.CleanName(out tags) ?? Locale.UnknownPackage;

		if (ShowIncluded && Package is not null)
		{
			if (!IsDlc)
			DrawIncludedButton(e, ClientRectangle.Pad(Padding).Align(imageRect.Size, ContentAlignment.MiddleRight));
		}

		using var textBrush = new SolidBrush(ForeColor);
		using var font = UI.Font(Large ? 9.75F : 8.25F).FitToWidth(text, textRect, e.Graphics);
		e.Graphics.DrawString(text, base.Font, textBrush, textRect, new StringFormat { LineAlignment = StringAlignment.Center });

		var tagRect = new Rectangle(textRect.X + (int)e.Graphics.Measure(text, Font).Width, textRect.Y, 0, textRect.Height);

		if (tags is not null)
		{
			foreach (var item in tags)
			{
				tagRect.X += Padding.Left + e.Graphics.DrawLabel(item.Text, null, item.Color, tagRect, ContentAlignment.MiddleLeft, smaller: true).Width;
			}
		}

		if (!ReadOnly && HoverState.HasFlag(HoverState.Hovered))
		{
			imageRect = ClientRectangle.Pad(Padding).Align(imageRect.Size, ContentAlignment.MiddleRight);

			using var img = IconManager.GetIcon("Trash", imageRect.Height * 3 / 4);

			e.Graphics.DrawImage(img.Color(imageRect.Contains(PointToClient(Cursor.Position)) ? FormDesign.Design.RedColor : FormDesign.Design.ForeColor, (byte)(HoverState.HasFlag(HoverState.Pressed) ? 255 : 175)), imageRect.CenterR(img.Size));
		}

		if (Dock == DockStyle.None)
		{
			Width = tagRect.X + Padding.Right;
		}
	}

	private void DrawIncludedButton(PaintEventArgs e, Rectangle buttonRect)
	{
		var localIdentity = Package!.GetLocalPackageIdentity();
		var isIncluded = Package!.IsIncluded(out var isPartialIncluded);
		var isEnabled = Package!.IsEnabled();
		Color activeColor = default;
		string text;

		if (localIdentity is null && Package!.IsLocal())
		{
			return; // missing local item
		}

		var required = _modLogicManager.IsRequired(localIdentity, _modUtil);
		var isHovered = Loading || (HoverState.HasFlag(HoverState.Hovered) && buttonRect.Contains(PointToClient(Cursor.Position)));

		if (isHovered)
		{
			if (!isIncluded)
			{
				text = Locale.IncludeItem;
			}
			else if (required)
			{
				text = Locale.ThisModIsRequiredYouCantDisableIt;
			}
			else if (isEnabled)
			{
				text = Locale.DisableItem;
			}
			else
			{
				text = Locale.EnableItem;
			}
		}
		else
		{
			if (isPartialIncluded)
			{
				text = Locale.PartiallyIncluded;
			}
			else if (!isIncluded)
			{
				text = Locale.IncludeItem;
			}
			else if (isEnabled)
			{
				text = Locale.Enabled;
			}
			else
			{
				text = Locale.Disabled;
			}
		}

		if (!required && isIncluded && isHovered)
		{
			isPartialIncluded = false;
			isEnabled = !isEnabled;
		}

		if (isEnabled)
		{
			activeColor = isPartialIncluded ? FormDesign.Design.YellowColor : FormDesign.Design.GreenColor;
		}

		if (required && activeColor != default)
		{
			activeColor = activeColor.MergeColor(BackColor, !FormDesign.Design.IsDarkTheme ? 35 : 20);
		}
		else if (activeColor == default && isHovered)
		{
			activeColor = isIncluded ? isEnabled ? FormDesign.Design.GreenColor : FormDesign.Design.RedColor : isHovered ? FormDesign.Design.ActiveColor : FormDesign.Design.ForeColor;
		}
		else
		{
			if (activeColor == default)
			{
				activeColor = Color.FromArgb(isIncluded ? 20 : 200, FormDesign.Design.ForeColor);
			}
			else if (isHovered)
			{
				activeColor = activeColor.MergeColor(ForeColor, 75);
			}
		}

		if (isHovered)
		{
			using var brush = new SolidBrush(Color.FromArgb(50, activeColor));

			e.Graphics.FillRoundedRectangle(brush, buttonRect, Padding.Left);
		}

		Rectangle iconRect;

		if (Loading)
		{
			iconRect = buttonRect.CenterR(buttonRect.Height * 3 / 5, buttonRect.Height * 3 / 5);

			if (_subscriptionsManager.Status.ModId != Package!.Id || _subscriptionsManager.Status.Progress == 0 || !_subscriptionsManager.Status.IsActive)
			{
				DrawLoader(e.Graphics, iconRect, activeColor);
			}
			else
			{
				var width = Math.Min(Math.Min(iconRect.Width, iconRect.Height), (int)(32 * UI.UIScale));
				var size = (float)Math.Max(2, width / (8D - (Math.Abs(100 - LoaderPercentage) / 50)));
				var drawSize = new SizeF(width - size, width - size);
				var rect = new RectangleF(new PointF(iconRect.X + ((iconRect.Width - drawSize.Width) / 2), iconRect.Y + ((iconRect.Height - drawSize.Height) / 2)), drawSize).Pad(size / 2);
				using var pen = new Pen(activeColor, size) { StartCap = LineCap.Round, EndCap = LineCap.Round };

				e.Graphics.DrawArc(pen, rect, -90, 360 * _subscriptionsManager.Status.Progress);
			}
		}
		else
		{
			var icon = new DynamicIcon(_subscriptionsManager.IsSubscribing(Package!) ? "Wait" : isPartialIncluded ? "Slash" : isEnabled ? "Ok" : !isIncluded ? "Add" : (isHovered && ModifierKeys.HasFlag(Keys.Alt)) ? "X" : "Enabled");
			using var includedIcon = icon.Get(buttonRect.Height * 3 / 4).Color(activeColor);

			iconRect = buttonRect.CenterR( includedIcon.Size);
			e.Graphics.DrawImage(includedIcon, iconRect);
		}
	}
}
