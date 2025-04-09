using Skyve.App.UserInterface.Content;
using Skyve.App.UserInterface.Dropdowns;

using Skyve.App.UserInterface.Panels;
using Skyve.Compatibility.Domain;
using Skyve.Compatibility.Domain.Enums;
using Skyve.Compatibility.Domain.Interfaces;

using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.CompatibilityReport;
public partial class IPackageStatusControl<T, TBase> : SlickControl where T : struct, Enum where TBase : IPackageStatus<T>, new()
{
	public readonly PackageStatusTypeDropDown<T> typeDropDown;

	public event EventHandler? ValuesChanged;
	public IPackageIdentity? CurrentPackage { get; }

	public IPackageStatusControl(IPackageIdentity? currentPackage, TBase? item = default, bool restricted = false)
	{
		InitializeComponent();

		CurrentPackage = currentPackage;
		L_LinkedPackages.Text = LocaleCR.LinkedPackages;
		L_OutputTitle.Text = LocaleCR.OutputText;

		typeDropDown = new(restricted)
		{
			Dock = DockStyle.Top,
			Text = typeof(T).Name,
		};

		P_Main.Controls.Add(typeDropDown, 0, 0);
		P_Main.SetColumnSpan(typeDropDown, 3);

		AutoInvalidate = false;
		AutoSize = true;

		typeDropDown.SelectedItemChanged += TypeDropDown_SelectedItemChanged;

		if (!IsHandleCreated)
		{
			CreateHandle();
		}

		if (item is not null)
		{
			typeDropDown.SelectedItem = item.Type;
			DD_Action.SelectedItem = item.Action;
			TB_Note.Text = item.Note;
			TB_Note.Visible = I_Note.Selected = !string.IsNullOrWhiteSpace(item.Note);

			foreach (var package in item.Packages ?? [])
			{
				P_Packages.Controls.Add(new MiniPackageControl(package) { Dock = DockStyle.Top });
			}
		}
		else
		{
			if (typeDropDown.Items.Contains(typeDropDown.SelectedItem))
			{
				TypeDropDown_SelectedItemChanged(this, EventArgs.Empty);
			}
			else
			{
				typeDropDown.SelectedItem = typeDropDown.Items[0];
			}
		}

		SlickTip.SetTo(I_Copy, "Copy");
		SlickTip.SetTo(I_Paste, "Paste");
		SlickTip.SetTo(I_AddPackage, "Add Package");
		SlickTip.SetTo(I_Note, "Show/Hide Note");

		typeDropDown.SelectedItemChanged += ValuesChanged;
		DD_Action.SelectedItemChanged += ValuesChanged;
		TB_Note.TextChanged += ValuesChanged;
		TB_Note.TextChanged += ValuesChanged;
		P_Packages.ControlAdded += (s, e) => ValuesChanged?.Invoke(s, e);
		P_Packages.ControlRemoved += (s, e) => ValuesChanged?.Invoke(s, e);
	}

	private void TypeDropDown_SelectedItemChanged(object sender, EventArgs e)
	{
		var actions = CRNAttribute.GetAttribute(typeDropDown.SelectedItem).ValidActions;

		if (actions.Length == 0)
		{
			DD_Action.Items = Enum.GetValues(typeof(StatusAction)).Cast<StatusAction>().Where(x => CRNAttribute.GetAttribute(x).Browsable).ToArray();
			DD_Action.SelectedItem = DD_Action.Items[0];
		}
		else
		{
			DD_Action.Items = actions;
			DD_Action.SelectedItem = DD_Action.Items[0];
		}

		DD_Action.IsFlipped = InteractionType.Successor.Equals(typeDropDown.SelectedItem);

		L_Output.Invalidate();
	}

	public TBase PackageStatus => new()
	{
		Type = typeDropDown.SelectedItem,
		Action = DD_Action.SelectedItem,
		Note = TB_Note.Text,
		Packages = P_Packages.Controls.OfType<MiniPackageControl>().Select(x => new CompatibilityPackageReference(x.Package)).ToArray(),
	};

	protected override void DesignChanged(FormDesign design)
	{
		P_Main.BackColor = design.AccentBackColor;
		L_OutputTitle.ForeColor = design.LabelColor;
		L_Output.ForeColor = design.InfoColor;
	}

	protected override void UIChanged()
	{
		MinimumSize = UI.Scale(new Size(260, 0), UI.UIScale);
		P_Main.Padding = UI.Scale(new Padding(12));
		Margin = slickSpacer1.Margin = L_OutputTitle.Margin = UI.Scale(new Padding(3));
		TB_Note.MinimumSize = new Size(0, UI.Scale(64));
		I_Paste.Size = I_Copy.Size = I_AddPackage.Size = I_Note.Size = I_Close.Size = UI.Scale(new Size(28, 28));
		I_Paste.Padding = I_Copy.Padding = I_AddPackage.Padding = I_Note.Padding = I_Close.Padding = UI.Scale(new Padding(3));
		L_OutputTitle.Font = UI.Font(7.5F, FontStyle.Bold);
	}

	private void I_Note_Click(object sender, EventArgs e)
	{
		TB_Note.Visible = I_Note.Selected = !I_Note.Selected;
	}

	private void I_AddPackage_Click(object sender, EventArgs e)
	{
		var form = new PC_WorkshopPackageSelection(P_Packages.Controls.OfType<MiniPackageControl>().Select(x => x.Id));

		form.PackageSelected += Form_PackageSelected;

		Program.MainForm.PushPanel(form);
	}

	private void Form_PackageSelected(IEnumerable<ulong> packages)
	{
		foreach (var item in packages)
		{
			if (!P_Packages.Controls.OfType<MiniPackageControl>().Any(x => x.Id == item))
			{
				P_Packages.Controls.Add(new MiniPackageControl(item) { Dock = DockStyle.Top });
			}
		}
	}

	private void CloseIcon_Click(object sender, EventArgs e)
	{
		Dispose();
	}

	private void I_Copy_Click(object sender, EventArgs e)
	{
		if (P_Packages.Controls.Count > 0)
		{
			Clipboard.SetText(P_Packages.Controls.OfType<MiniPackageControl>().Select(x => x.Id).ListStrings(","));
		}
	}

	private void I_Paste_Click(object sender, EventArgs e)
	{
		if (!Clipboard.ContainsText())
		{
			return;
		}

#if CS2
		var matches = Regex.Matches(Clipboard.GetText(), "(\\d{5,6})");
#else
		var matches = Regex.Matches(Clipboard.GetText(), "(\\d{8,20})");
#endif

		foreach (Match item in matches)
		{
			if (ulong.TryParse(item.Value, out var id))
			{
				if (!P_Packages.Controls.OfType<MiniPackageControl>().Any(x => x.Id == id))
				{
					P_Packages.Controls.Add(new MiniPackageControl(id) { Dock = DockStyle.Top });
				}
			}
		}
	}

	private void RefreshOutput(object sender, ControlEventArgs e)
	{
		L_Output.Invalidate();
	}

	private void RefreshOutput(object sender, EventArgs e)
	{
		L_Output.Invalidate();
	}

	private void L_Output_Paint(object sender, PaintEventArgs e)
	{
		e.Graphics.SetUp(L_Output.BackColor);

		string message;

		var action = LocaleCR.Get($"Action_{DD_Action.SelectedItem}");
		if (typeDropDown.SelectedItem is InteractionType.Successor)
		{
			var translation = LocaleCR.Get($"{typeof(T).Name.Remove("Type")}_{InteractionType.SucceededBy}");
			message = string.Format($"{translation.One}\r\n\r\n{action.Zero}", (P_Packages.Controls.FirstOrDefault(x => true) as MiniPackageControl)?.Package?.CleanName(), CurrentPackage?.CleanName()).Trim();
		}
		else
		{
			var actionText = P_Packages.Controls.Count switch { 0 => action.Zero, 1 => action.One, _ => action.Plural } ?? action.One;
			var translation = LocaleCR.Get($"{typeof(T).Name.Remove("Type")}_{typeDropDown.SelectedItem}");
			var text = P_Packages.Controls.Count switch { 0 => translation.Zero, 1 => translation.One, _ => translation.Plural } ?? translation.One;
			message = string.Format($"{text}\r\n\r\n{actionText}", CurrentPackage?.CleanName(), (P_Packages.Controls.FirstOrDefault(x => true) as MiniPackageControl)?.Package?.CleanName()).Trim();
		}

		e.Graphics.DrawString(message, L_Output.Font, new SolidBrush(L_Output.ForeColor), L_Output.ClientRectangle);

		L_Output.Height = (int)e.Graphics.Measure(message, L_Output.Font, L_Output.Width).Height + 3;
	}
}
