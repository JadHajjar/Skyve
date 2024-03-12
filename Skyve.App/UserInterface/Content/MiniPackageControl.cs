using Skyve.App.Interfaces;

using System.Drawing;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Content;
public class MiniPackageControl : SlickControl
{
	private readonly IPackage? _package;
	private readonly IWorkshopService _workshopService = ServiceCenter.Get<IWorkshopService>();

	public IPackage? Package => _package ?? _workshopService.GetPackage(new GenericPackageIdentity(Id));
	public ulong Id { get; }

	public bool ReadOnly { get; set; }
	public bool Large { get; set; }
	public bool ShowIncluded { get; set; }

	public MiniPackageControl(ulong steamId)
	{
		Id = steamId;
	}

	public MiniPackageControl(IPackage package)
	{
		_package = package;
		Id = package.Id;
	}

	protected override void UIChanged()
	{
		Height = (int)((Large ? 32 : 24) * UI.FontScale);

		Padding = UI.Scale(new Padding(4), UI.FontScale);
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		base.OnMouseMove(e);

		var imageRect = ClientRectangle.Pad(Padding);

		Cursor = ClientRectangle.Pad(1, 1, ReadOnly && !ShowIncluded ? 1 :( imageRect.Width + Padding.Horizontal), 1).Contains(e.Location) ? Cursors.Hand : Cursors.Default;
	}

	protected override void OnMouseClick(MouseEventArgs e)
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

		if (HoverState.HasFlag(HoverState.Hovered))
		{
			using var backBrush = new SolidBrush(Color.FromArgb(25, FormDesign.Design.ForeColor));

			e.Graphics.FillRoundedRectangle(backBrush, textRect, Padding.Left);
		}

		if (image is not null)
		{
			if (Package!.IsLocal)
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
			using var generic = IconManager.GetIcon("I_Package", imageRect.Height).Color(BackColor);
			using var brush = new SolidBrush(FormDesign.Design.IconColor);

			e.Graphics.FillRoundedRectangle(brush, imageRect, (int)(4 * UI.FontScale));
			e.Graphics.DrawImage(generic, imageRect.CenterR(generic.Size));
		}

		List<(Color Color, string Text)>? tags = null;

		textRect = textRect.Pad(imageRect.Right + Padding.Left, 0, 0, 0);
		var text = Package?.CleanName(out tags) ?? Locale.UnknownPackage;

		if (ShowIncluded && Package?.IsIncluded() == true && Package?.IsEnabled() == true)
		{
			using var checkIcon = IconManager.GetIcon("I_Ok", imageRect.Height * 3 / 4).Color(FormDesign.Design.GreenColor);

			e.Graphics.DrawImage(checkIcon, textRect.Pad(Padding).Align(checkIcon.Size, ContentAlignment.MiddleRight));

			textRect = textRect.Pad(0, 0, checkIcon.Width + Padding.Horizontal, 0);
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

			using var img = IconManager.GetIcon("I_Trash", imageRect.Height * 3 / 4);

			e.Graphics.DrawImage(img.Color(imageRect.Contains(PointToClient(Cursor.Position)) ? FormDesign.Design.RedColor : FormDesign.Design.ForeColor, (byte)(HoverState.HasFlag(HoverState.Pressed) ? 255 : 175)), imageRect.CenterR(img.Size));
		}

		if (Dock == DockStyle.None)
		{
			Width = tagRect.X + Padding.Right;
		}
	}
}
