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
			Font = UI.Font(9.75F);

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
			using var fileIcon = IconManager.GetLargeIcon("File").Color(FormDesign.Design.MenuForeColor);
			using var removeIcon = IconManager.GetIcon("X").Color(FormDesign.Design.MenuForeColor);

			var textSize = e.Graphics.Measure(Path.GetFileNameWithoutExtension(SelectedFile), new Font(Font, FontStyle.Bold), Width - availableWidth - Padding.Horizontal);
			var fileHeight = (int)textSize.Height + 3 + fileIcon.Height + Padding.Top;
			var fileRect = new Rectangle(0, 0, Width - availableWidth, Height).Pad(Padding.Left);
			var fileContentRect = fileRect.CenterR(Math.Max((int)textSize.Width + 3, fileIcon.Width), fileHeight);
			var removeRect = fileRect.Align(new Size(removeIcon.Width + Padding.Left, removeIcon.Height + Padding.Top), ContentAlignment.TopRight);

			e.Graphics.FillRoundedRectangle(new SolidBrush((fileHovered = HoverState.HasFlag(HoverState.Hovered) && fileRect.Contains(cursor)) && !removeRect.Contains(cursor) ? FormDesign.Design.MenuColor.MergeColor(FormDesign.Design.ActiveColor) : FormDesign.Design.MenuColor), fileRect, border);
			e.Graphics.DrawImage(fileIcon, fileContentRect.Align(fileIcon.Size, ContentAlignment.TopCenter));
			e.Graphics.DrawString(Path.GetFileNameWithoutExtension(SelectedFile), new Font(Font, FontStyle.Bold), new SolidBrush(FormDesign.Design.MenuForeColor), fileContentRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far });

			if (HoverState.HasFlag(HoverState.Hovered) && removeRect.Contains(cursor))
			{
				e.Graphics.FillRoundedRectangle(new SolidBrush(Color.FromArgb(HoverState.HasFlag(HoverState.Pressed) ? 255 : 100, FormDesign.Design.RedColor)), removeRect, border);
			}

			e.Graphics.DrawImage(removeIcon, removeRect.CenterR(removeIcon.Size));
		}

		var color = fileHovered ? FormDesign.Design.ForeColor : isDragActive ? FormDesign.Design.ActiveForeColor : HoverState.HasFlag(HoverState.Pressed) ? FormDesign.Design.ActiveColor : HoverState.HasFlag(HoverState.Hovered) ? FormDesign.Design.ActiveColor.MergeColor(FormDesign.Design.ForeColor) : FormDesign.Design.ForeColor;

		if (isDragActive)
		{
			using var brush = new SolidBrush(isDragAvailable ? FormDesign.Design.ActiveColor : FormDesign.Design.RedColor);
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad((int)(1.5 * UI.FontScale)), border);
		}
		else if (HoverState.HasFlag(HoverState.Hovered) && !fileHovered)
		{
			using var brush = new SolidBrush(Color.FromArgb(25, color));
			e.Graphics.FillRoundedRectangle(brush, ClientRectangle.Pad((int)(1.5 * UI.FontScale)), border);
		}

		using var pen = new Pen(Color.FromArgb(100, color), (float)(1.5 * UI.FontScale)) { DashStyle = DashStyle.Dash };
		e.Graphics.DrawRoundedRectangle(pen, ClientRectangle.Pad((int)(1.5 * UI.FontScale)), border);

		var text = LocaleHelper.GetGlobalText(Text);
		var size = e.Graphics.Measure(text, Font, availableWidth - (2 * Padding.Horizontal) - (UI.FontScale >= 2 ? 48 : 24));
		var width = (int)size.Width + 3 + Padding.Left + (UI.FontScale >= 2 ? 48 : 24);
		var rect = new Rectangle(Width - availableWidth, 0, availableWidth, Height).CenterR(width, Math.Max(UI.FontScale >= 2 ? 48 : 24, (int)size.Height + 3));

		if (Loading)
		{
			DrawLoader(e.Graphics, rect.Align(new Size(UI.FontScale >= 2 ? 48 : 24, UI.FontScale >= 2 ? 48 : 24), ContentAlignment.MiddleLeft));
		}
		else
		{
			using var icon = IconManager.GetLargeIcon("DragDrop").Color(color);

			e.Graphics.DrawImage(icon, rect.Align(icon.Size, ContentAlignment.MiddleLeft));
		}

		e.Graphics.DrawString(text, Font, new SolidBrush(color), rect.Align(size.ToSize(), ContentAlignment.MiddleRight).Pad(-2));

		if (!Enabled)
		{
			e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(100, BackColor)), ClientRectangle);
		}
	}
}