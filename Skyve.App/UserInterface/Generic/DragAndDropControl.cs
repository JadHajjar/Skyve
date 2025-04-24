using Skyve.App.Utilities;

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Generic;

[DefaultEvent("FileSelected")]
public class DragAndDropControl : SlickControl
{
	private bool isDragActive;
	private bool isDragAvailable;
	private string? _selectedFile;
	private readonly IOSelectionDialog _selectionDialog;

	public event Action<string>? FileSelected;
	public event Func<object, string, bool>? ValidFile;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[Bindable(true)]
	public override string Text { get => base.Text; set => base.Text = value; }

	[Category("Appearance"), DefaultValue(null)]
	public string? ImageName { get; set; }

	[Category("Behavior"), DefaultValue(null)]
	public string[]? ValidExtensions { get => _selectionDialog.ValidExtensions; set => _selectionDialog.ValidExtensions = value; }

	[Category("Behavior"), DefaultValue(null)]
	public string? StartingFolder { get => _selectionDialog.StartingFolder; set => _selectionDialog.StartingFolder = value; }

	[Category("Behavior"), DefaultValue(null)]
	public string? SelectedFile
	{
		get => _selectedFile; set
		{
			_selectedFile = value;
			Invalidate();
		}
	}

	[Category("Behavior"), DefaultValue(null)]
	public Dictionary<string, string>? PinnedFolders { get => _selectionDialog.PinnedFolders; set => _selectionDialog.PinnedFolders = value; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
	public List<IOSelectionDialog.CustomFile>? CustomFiles { get => _selectionDialog.CustomFiles; set => _selectionDialog.CustomFiles = value; }

	public DragAndDropControl()
	{
		_selectionDialog = new()
		{
			Filter = DialogFilter
		};
		AllowDrop = true;
		Cursor = Cursors.Hand;
	}

	private bool DialogFilter(FileInfo arg)
	{
		return ValidFile?.Invoke(this, arg.FullName) ?? true;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		if (Live)
		{
			Size = UI.Scale(new Size(300, 80));
			Padding = UI.Scale(new Padding(10), UI.UIScale);

			if (Margin == new Padding(3))
			{
				Margin = Padding;
			}
		}
	}

	protected override void OnDragEnter(DragEventArgs drgevent)
	{
		base.OnDragEnter(drgevent);

		isDragActive = true;

		if (drgevent.Data.GetDataPresent(DataFormats.FileDrop) && (ValidFile?.Invoke(this, ((string[])drgevent.Data.GetData(DataFormats.FileDrop)).FirstOrDefault()) ?? true))
		{
			drgevent.Effect = DragDropEffects.Copy;
			isDragAvailable = true;
			Invalidate();
		}
		else
		{
			drgevent.Effect = DragDropEffects.None;
			isDragAvailable = false;
			Invalidate();
		}
	}

	protected override void OnDragLeave(EventArgs e)
	{
		base.OnDragLeave(e);

		isDragActive = false;
		Invalidate();
	}

	protected override void OnDragDrop(DragEventArgs drgevent)
	{
		base.OnDragDrop(drgevent);

		var file = ((string[])drgevent.Data.GetData(DataFormats.FileDrop)).FirstOrDefault();

		if (file != null)
		{
			if (CrossIO.CurrentPlatform is not Platform.Windows)
			{
				var realPath = ServiceCenter.Get<IIOUtil>().ToRealPath(file);

				if (CrossIO.FileExists(realPath))
				{
					file = realPath!;
				}
			}

			FileSelected?.Invoke(file);
		}

		isDragActive = false;
		Invalidate();
	}

	protected override void OnMouseClick(MouseEventArgs e)
	{
		base.OnMouseClick(e);

		if (e.Button == MouseButtons.Middle && !string.IsNullOrWhiteSpace(SelectedFile))
		{
			FileSelected?.Invoke(string.Empty);
		}

		if (e.Button != MouseButtons.Left)
		{
			return;
		}

		var availableWidth = string.IsNullOrWhiteSpace(SelectedFile) ? Width : Width - UI.Scale(125);

		if (!string.IsNullOrWhiteSpace(SelectedFile))
		{
			using var removeIcon = IconManager.GetIcon("X");

			var fileRect = new Rectangle(0, 0, Width - availableWidth, Height).Pad(Padding.Left);
			var removeRect = fileRect.Align(new Size(removeIcon.Width + Padding.Left, removeIcon.Height + Padding.Top), ContentAlignment.TopRight);

			if (removeRect.Contains(e.Location))
			{
				FileSelected?.Invoke(string.Empty);
				return;
			}
			else if (fileRect.Contains(e.Location))
			{
				PlatformUtil.OpenFolder(SelectedFile);

				return;
			}
		}

		if (_selectionDialog.PromptFile(Program.MainForm) == DialogResult.OK)
		{
			if (ValidFile?.Invoke(this, _selectionDialog.SelectedPath) ?? true)
			{
				FileSelected?.Invoke(_selectionDialog.SelectedPath);
			}
			else
			{
				MessagePrompt.Show(Locale.SelectedFileInvalid, PromptButtons.OK, PromptIcons.Warning, Program.MainForm);
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		e.Graphics.SetUp(BackColor);

		var fileHovered = false;
		var border = UI.Scale(4);
		var cursor = PointToClient(MousePosition);
		var availableWidth = string.IsNullOrWhiteSpace(SelectedFile) ? Width : Width - UI.Scale(125);

		if (!string.IsNullOrWhiteSpace(SelectedFile))
		{
			fileHovered = DrawFile(e, border, cursor, availableWidth);
		}

		var color = fileHovered ? FormDesign.Design.ForeColor : isDragActive ? FormDesign.Design.ActiveForeColor : HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.ForeColor) : FormDesign.Design.ForeColor;

		if (isDragActive)
		{
			using var brush = new SolidBrush(isDragAvailable ? FormDesign.Design.ActiveColor : FormDesign.Design.RedColor);
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad((int)UI.Scale(1.5)), border);
		}
		else if (HoverState.HasFlag(HoverState.Hovered) && !fileHovered)
		{
			using var brush = new SolidBrush(Color.FromArgb(25, color));
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad((int)UI.Scale(1.5)), border);
		}

		using var pen = new Pen(Color.FromArgb(100, color), UI.Scale(1.5F)) { DashStyle = DashStyle.Dash };
		e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad((int)UI.Scale(1.5)), border);

		var text = LocaleHelper.GetGlobalText(Text);
		using var font = UI.Font(9.75F).FitTo(text, new Rectangle(0, 0, availableWidth - (2 * Padding.Horizontal) - IconManager.GetLargeScale(), Height-Padding.Vertical), e.Graphics);
		var size = e.Graphics.Measure(text, font, availableWidth - (2 * Padding.Horizontal) - IconManager.GetLargeScale());
		var width = (int)size.Width + UI.Scale( 3) + Padding.Left + (UI.FontScale >= 2 ? 48 : 24);
		var rect = new Rectangle(Width - availableWidth, 0, availableWidth, Height).CenterR(width, Math.Max(IconManager.GetLargeScale(), (int)size.Height + UI.Scale(3)));

		if (Loading)
		{
			DrawLoader(e.Graphics, rect.Align(UI.Scale(new Size(32,32)), ContentAlignment.MiddleLeft));
		}
		else
		{
			using var icon = IconManager.GetLargeIcon("DragDrop").Color(color);

			if (icon is not null)
			{
				e.Graphics.DrawImage(icon, rect.Align(icon.Size, ContentAlignment.MiddleLeft));
			}
		}

		using var brush1 = new SolidBrush(color);
		e.Graphics.DrawString(text, font, brush1, rect.Align(size.ToSize(), ContentAlignment.MiddleRight).Pad(-2));

		if (!Enabled)
		{
			using var brush = new SolidBrush(Color.FromArgb(100, BackColor));
			e.Graphics.FillRectangle(brush, ClientRectangle);
		}
	}

	private bool DrawFile(PaintEventArgs e, int border, Point cursor, int availableWidth)
	{
		var fileRect = new Rectangle(0, 0, Width - availableWidth, Height).Pad(Padding.Left*3/2);
		var backRect = new Rectangle(0, 0, Width - availableWidth, Height).Pad(Padding.Left);

		SlickButton.GetColors(out var textColor, out var backColor, backRect.Contains(cursor)? HoverState:default, ColorStyle.Text, buttonType: ButtonType.Active);

		e.Graphics.FillRoundedRectangleWithShadow(backRect, Padding.Left / 2, Padding.Left / 2, backColor, Color.FromArgb(10, backColor));

		using var removeIcon = IconManager.GetIcon("X", backRect.Height/4).Color(textColor);
		using var fileIcon = IconManager.GetLargeIcon(ImageName ?? "File").Color(textColor);
		using var font = UI.Font(9.75F).FitTo(Path.GetFileNameWithoutExtension(SelectedFile), fileRect.Pad(0, fileIcon.Height + (Padding.Top / 2), 0, 0), e.Graphics);

		var textSize = e.Graphics.Measure(Path.GetFileNameWithoutExtension(SelectedFile), font, fileRect.Width);
		var fileHeight = (int)textSize.Height + fileIcon.Height + (Padding.Top / 2);
		var removeRect = backRect.Align(new Size(removeIcon.Width + Padding.Left, removeIcon.Height + Padding.Top), ContentAlignment.TopRight);

		fileRect = fileRect.CenterR(fileRect.Width, fileHeight);

		var iconRect = fileRect.Align(fileIcon.Size, ContentAlignment.TopCenter);

		if (Loading)
		{
			DrawLoader(e.Graphics, iconRect);
		}
		else
		{
			e.Graphics.DrawImage(fileIcon, iconRect);
		}

		using var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };
		using var brush = new SolidBrush(textColor);

		e.Graphics.DrawString(Path.GetFileNameWithoutExtension(SelectedFile), font, brush, fileRect, format);


		//using var fileIcon = IconManager.GetLargeIcon(ImageName ?? "File").Color(FormDesign.Design.MenuForeColor);
		//using var removeIcon = IconManager.GetIcon("X").Color(FormDesign.Design.MenuForeColor);
		//var textSize = e.Graphics.Measure(Path.GetFileNameWithoutExtension(SelectedFile), new Font(Font, FontStyle.Bold), Width - availableWidth - Padding.Horizontal);
		//var fileHeight = (int)textSize.Height + 3 + fileIcon.Height + Padding.Top;
		//var fileRect = new Rectangle(0, 0, Width - availableWidth, Height).Pad(Padding.Left);
		//var fileContentRect = fileRect.CenterR(Math.Max((int)textSize.Width + 3, fileIcon.Width), fileHeight);

		//e.Graphics.FillRoundedRectangle(new SolidBrush((fileHovered = HoverState.HasFlag(HoverState.Hovered) && fileRect.Contains(cursor)) && !removeRect.Contains(cursor) ? FormDesign.Design.MenuColor.MergeColor(FormDesign.Design.ActiveColor) : FormDesign.Design.MenuColor), fileRect, border);
		//e.Graphics.DrawImage(fileIcon, fileContentRect.Align(fileIcon.Size, ContentAlignment.TopCenter));
		//e.Graphics.DrawString(Path.GetFileNameWithoutExtension(SelectedFile), new Font(Font, FontStyle.Bold), new SolidBrush(FormDesign.Design.MenuForeColor), fileContentRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });

		if (HoverState.HasFlag(HoverState.Hovered) && removeRect.Contains(cursor))
		{
			e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(HoverState.HasFlag(HoverState.Pressed) ? 255 : 100, FormDesign.Design.RedColor)), removeRect, border);
		}

		e.Graphics.DrawImage(removeIcon, removeRect.CenterR(removeIcon.Size));

		return backRect.Contains(cursor);
	}
}