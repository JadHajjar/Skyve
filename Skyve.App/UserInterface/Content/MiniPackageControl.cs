using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Panels;

using System.Drawing;
using System.Drawing.Drawing2D;
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

	public MiniPackageControl(ulong steamId)
	{
		Cursor = Cursors.Hand;
		Id = steamId;
	}

	public MiniPackageControl(IPackage package)
	{
		Cursor = Cursors.Hand;
		_package = package;
		Id = package.Id;
	}

	protected override void UIChanged()
	{
		Height = (int)((Large ? 42 : 32) * UI.FontScale);

		Padding = UI.Scale(new Padding(5), UI.FontScale);
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
				else
				{
					Program.MainForm.PushPanel(null, /*Package.GetWorkshopInfo()?.IsCollection == true ? new PC_ViewCollection(Package) :*/ new PC_PackagePage(Package));
				}

				break;
			case MouseButtons.Right:
				var items = ServiceCenter.Get<IRightClickService>().GetRightClickMenuItems(Package);

				this.TryBeginInvoke(() => SlickToolStrip.Show(Program.MainForm, items));
				break;
			case MouseButtons.Middle:
				Dispose();
				break;
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		using var backBrush = new SolidBrush(Color.FromArgb(10, !FormDesign.Design.IsDarkTheme ? Color.Black : Color.White));
		e.Graphics.FillRoundedRectangle(backBrush, ClientRectangle.Pad(Padding), Padding.Left);

		if (HoverState.HasFlag(HoverState.Hovered))
		{
			using var brush = new LinearGradientBrush(ClientRectangle.Pad(Height / 2, 0, 0, 0), Color.FromArgb(150, FormDesign.Design.ActiveColor), Color.Empty, LinearGradientMode.Horizontal);
			e.Graphics.FillRectangle(brush, ClientRectangle.Pad(Height / 2, 0, 0, 0).Pad(Padding));
		}

		var imageRect = ClientRectangle.Pad(Padding);
		imageRect.Width = imageRect.Height;
		var image = Package?.GetThumbnail();

		if (image is not null)
		{
			if (Package!.IsLocal)
			{
				using var unsatImg = new Bitmap(image, imageRect.Size).Tint(Sat: 0);
				e.Graphics.DrawRoundedImage(unsatImg, imageRect, (int)(4 * UI.FontScale), FormDesign.Design.AccentBackColor);
			}
			else
			{
				e.Graphics.DrawRoundedImage(image, imageRect, (int)(4 * UI.FontScale), FormDesign.Design.AccentBackColor);
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

		var textRect = ClientRectangle.Pad(imageRect.Right + Padding.Left, Padding.Top, !ReadOnly && HoverState.HasFlag(HoverState.Hovered) ? imageRect.Right + Padding.Left : 0, Padding.Bottom).AlignToFontSize(Font, ContentAlignment.MiddleLeft);
		var text = Package?.CleanName(out tags) ?? Locale.UnknownPackage;

		e.Graphics.DrawString(text, Font, new SolidBrush(ForeColor), textRect, new StringFormat { Trimming = StringTrimming.EllipsisCharacter, LineAlignment = StringAlignment.Center });

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

			if (imageRect.Contains(PointToClient(Cursor.Position)))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(HoverState.HasFlag(HoverState.Pressed) ? 50 : 20, FormDesign.Design.RedColor.MergeColor(ForeColor, 65))), imageRect.Pad(1), (int)(4 * UI.FontScale));
			}

			using var img = IconManager.GetIcon("I_Disposable");

			e.Graphics.DrawImage(img.Color(FormDesign.Design.RedColor, (byte)(HoverState.HasFlag(HoverState.Pressed) ? 255 : 175)), imageRect.CenterR(img.Size));
		}

		if (Dock == DockStyle.None)
		{
			Width = tagRect.X + Padding.Right;
		}
	}
}
