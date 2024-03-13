using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Lists;

using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_PlaysetList : PanelContent
{
	private readonly PlaysetListControl LC_Items;

	private readonly IPlaysetManager _playsetManager = ServiceCenter.Get<IPlaysetManager>();
	private readonly INotifier _notifier = ServiceCenter.Get<INotifier>();

	public PC_PlaysetList() : this(null) { }

	public PC_PlaysetList(IEnumerable<IPlayset>? profiles)
	{
		InitializeComponent();

		DD_Sorting.Height = TB_Search.Height = 0;

		LC_Items = new PlaysetListControl(false) { Dock = DockStyle.Fill, GridView = true };
		LC_Items.CanDrawItem += Ctrl_CanDrawItem;
		panel1.Controls.Add(LC_Items);

		if (profiles is null)
		{
			LC_Items.LoadPlaysetStarted += LC_Items_LoadPlaysetStarted;

			if (_notifier.IsPlaysetsLoaded)
			{
				LC_Items.SetItems(_playsetManager.Playsets);
			}
			else
			{
				LC_Items.Loading = true;
			}

			_notifier.PlaysetChanged += LoadPlayset;
		}
		else
		{
			L_Counts.Visible = TLP_PlaysetName.Visible = B_AddPlayset.Visible = B_DeactivatePlayset.Visible = B_Discover.Visible = false;

			DD_Sorting.Parent = null;
			TLP_Main.SetColumn(DD_Usage, 2);
			TLP_Main.SetColumnSpan(TB_Search, 2);

			LC_Items.ReadOnly = true;
			LC_Items.SetItems(profiles);
			LC_Items.SetSorting(ProfileSorting.Downloads);
		}

		C_ViewTypeControl.GridView = true;

		SlickTip.SetTo(B_Add, "NewPlayset_Tip");
		SlickTip.SetTo(B_Deactivate, "DeactivatePlayset_Tip");
		SlickTip.SetTo(B_Edit, "ChangePlaysetSettings");

		RefreshCounts();
	}

	private void LC_Items_LoadPlaysetStarted()
	{
		B_Edit.Loading = B_Edit.Visible = true;
	}

	private void Ctrl_CanDrawItem(object sender, CanDrawItemEventArgs<IPlayset> e)
	{
		var valid = true;

		if (e.Item.GetCustomPlayset().Usage > 0 && DD_Usage.SelectedItems.Count() > 0)
		{
			valid &= DD_Usage.SelectedItems.Contains(e.Item.GetCustomPlayset().Usage);
		}

		if (!string.IsNullOrWhiteSpace(TB_Search.Text))
		{
			var author = e.Item.GetCustomPlayset().OnlineInfo?.Author;

			valid &= TB_Search.Text.SearchCheck(e.Item.Name) || (author is not null && TB_Search.Text.SearchCheck(author.Name));
		}

		e.DoNotDraw = !valid;
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (!LC_Items.ReadOnly)
		{
			LoadPlayset();
		}
	}

	private void RefreshCounts()
	{
		if (L_Counts.Visible)
		{
			var favorites = _playsetManager.Playsets.Count(x => x.GetCustomPlayset().IsFavorite);
			var total = _playsetManager.Playsets.Count(x => !x.Temporary);
			var text = string.Empty;

			if (favorites == 0)
			{
				text = string.Format(Locale.FavoriteTotal, total);
			}
			else
			{
				text = string.Format(Locale.FavoritePlaysetTotal, favorites, total);
			}

			if (L_Counts.Text != text)
			{
				L_Counts.Text = text;
			}
		}

		var filteredCount = LC_Items.FilteredItems.Count();

		L_FilterCount.Text = Locale.ShowingCount.FormatPlural(filteredCount, Locale.Playset.FormatPlural(filteredCount).ToLower());
	}

	protected override void LocaleChanged()
	{
		Text = LC_Items.ReadOnly ? Locale.DiscoverPlaysets : Locale.YourPlaysets;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		B_Deactivate.Size = B_Add.Size = B_Edit.Size = UI.Scale(new Size(24, 24), UI.FontScale);
		L_CurrentPlayset.Font = UI.Font(11F, FontStyle.Bold);
		roundedPanel.Margin = TB_Search.Margin = L_Counts.Margin = L_FilterCount.Margin = DD_Sorting.Margin = DD_Usage.Margin = UI.Scale(new Padding(6), UI.FontScale);
		B_DeactivatePlayset.Padding = B_AddPlayset.Padding = TLP_PlaysetName.Padding = UI.Scale(new Padding(3), UI.FontScale);
		L_Counts.Font = L_FilterCount.Font = UI.Font(7.5F, FontStyle.Bold);
		B_Discover.Font = UI.Font(9F, FontStyle.Bold);
		B_Discover.Margin = TLP_PlaysetName.Margin = B_DeactivatePlayset.Margin = B_AddPlayset.Margin = UI.Scale(new Padding(5), UI.FontScale);
		DD_Usage.Width = DD_Sorting.Width = (int)(180 * UI.FontScale);
		TB_Search.Width = (int)(250 * UI.FontScale);
		roundedPanel.Padding = new Padding((int)(2.5 * UI.FontScale) + 1, (int)(5 * UI.FontScale), (int)(2.5 * UI.FontScale), (int)(5 * UI.FontScale));
		TLP_PlaysetName.MinimumSize = new Size(0, B_AddPlayset.Height);
		TLP_Top.Padding = new Padding(roundedPanel.Margin.Left, 0, roundedPanel.Margin.Right, 0);

		var size = (int)(30 * UI.FontScale) - 6;
		TB_Search.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = new Size(0, size);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_PlaysetName.Invalidate();
		B_DeactivatePlayset.BackColor = B_AddPlayset.BackColor = FormDesign.Design.ButtonColor;
		tableLayoutPanel3.BackColor = design.AccentBackColor;
		L_Counts.ForeColor = L_FilterCount.ForeColor = design.InfoColor;
	}

	public override bool KeyPressed(ref Message msg, Keys keyData)
	{
		if (keyData is (Keys.Control | Keys.F))
		{
			TB_Search.Focus();
			TB_Search.SelectAll();

			return true;
		}

		return false;
	}

	private void LoadPlayset()
	{
		this.TryInvoke(() =>
		{
			var playset = _playsetManager.CurrentPlayset;
			var customPlayset = _playsetManager.CurrentCustomPlayset;

			TLP_PlaysetName.BackColor = customPlayset?.Color ?? FormDesign.Design.ButtonColor;
			TLP_PlaysetName.ForeColor = TLP_PlaysetName.BackColor.GetTextColor();
			L_CurrentPlayset.Text = playset?.Name ?? Locale.NoActivePlayset;

			B_Edit.Visible = B_DeactivatePlayset.Visible = playset is not null;
			TLP_Top.ColumnStyles[1] = playset is not null ? new() : new ColumnStyle(SizeType.Absolute, 0);

			LC_Items.Invalidate();
		});

		B_Edit.Loading = false;
	}

	private void FilterChanged(object sender, EventArgs e)
	{
		TB_Search.ImageName = string.IsNullOrWhiteSpace(TB_Search.Text) ? "I_Search" : "I_ClearSearch";

		LC_Items.FilterChanged();
		RefreshCounts();
	}

	private void DD_Sorting_SelectedItemChanged(object sender, EventArgs e)
	{
		if (IsHandleCreated)
		{
			LC_Items.SetSorting(DD_Sorting.SelectedItem);
		}
	}

	private void TB_Search_IconClicked(object sender, EventArgs e)
	{
		TB_Search.Text = string.Empty;
	}

	private void B_ListView_Click(object sender, EventArgs e)
	{
		C_ViewTypeControl.GridView = false;
		LC_Items.GridView = false;
	}

	private void B_GridView_Click(object sender, EventArgs e)
	{
		C_ViewTypeControl.GridView = true;
		LC_Items.GridView = true;
	}

	private void B_AddProfile_Click(object sender, EventArgs e)
	{
		Form.PushPanel(ServiceCenter.Get<IAppInterfaceService>().NewPlaysetPanel());
	}

	private async void B_Deactivate_Click(object sender, EventArgs e)
	{
		if (!B_Edit.Loading)
		{
			B_Edit.Loading = B_Edit.Visible = true;
			await _playsetManager.DeactivateActivePlayset();
			B_Edit.Loading = false;
		}
	}

	private void B_Edit_Click(object sender, EventArgs e)
	{
		if (!B_Edit.Loading && _playsetManager.CurrentPlayset is not null)
		{
			ServiceCenter.Get<IAppInterfaceService>().OpenPlaysetPage(_playsetManager.CurrentPlayset);
		}
	}

	private async void B_Discover_Click(object sender, EventArgs e)
	{
		//try
		//{
		//	B_Discover.Loading = true;

		//	var profiles = await ServiceCenter.Get<ISkyveApiUtil>().GetPublicPlaysets();

		//	Invoke(() => Form.PushPanel(new PC_PlaysetList(profiles)));
		//}
		//catch (Exception ex)
		//{
		//	ShowPrompt(ex, Locale.FailedToRetrievePlaysets);
		//}

		//B_Discover.Loading = false;
	}

	public async void Import(string file)
	{
		try
		{
			var profile = _playsetManager.Playsets.FirstOrDefault(x => x.Name!.Equals(Path.GetFileNameWithoutExtension(file), StringComparison.InvariantCultureIgnoreCase));

			if (Path.GetExtension(file).ToLower() is ".zip")
			{
				using var stream = File.OpenRead(file);
				using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read, false);

				var entry = zipArchive.GetEntry("Skyve\\LogProfile.json") ?? zipArchive.GetEntry("Skyve/LogProfile.json");

				if (entry is null)
				{
					return;
				}

				file = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(file)}.json");

				try
				{
					CrossIO.DeleteFile(file, true);
				}
				catch { }

				entry.ExtractToFile(file);
			}

#if CS1
			if (file.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
			{
				if (profile is not null)
				{
					ShowPrompt(Locale.PlaysetNameUsed, icon: PromptIcons.Hand);
					return;
				}

				//profile = _profileManager.ConvertLegacyPlayset(file, false);

				if (profile is null)
				{
					ShowPrompt(Locale.FailedToImportLegacyPlayset, icon: PromptIcons.Error);
					return;
				}
			}
			else
#endif
			{
				profile ??= await _playsetManager.ImportPlayset(file);
			}

			if (profile is not null)
			{
				throw new NotImplementedException();
				//var panel = new PC_GenericPackageList(profile.Packages, true)
				//{
				//	Text = profile.Name
				//};

				//Form.PushPanel(panel);
			}
		}
		catch (Exception ex)
		{
			ShowPrompt(ex, Locale.FailedToImportPlayset);
		}
	}
}
