using Skyve.App.Interfaces;
using Skyve.App.UserInterface.Lists;

using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_PlaysetList : PanelContent
{
	private readonly PlaysetListControl LC_Items;

	private readonly IPlaysetManager _profileManager = ServiceCenter.Get<IPlaysetManager>();
	private readonly INotifier _notifier = ServiceCenter.Get<INotifier>();

	public PC_PlaysetList() : this(null) { }

	public PC_PlaysetList(IEnumerable<ICustomPlayset>? profiles)
	{
		InitializeComponent();

		DD_Sorting.Height = TB_Search.Height = 0;

		LC_Items = new PlaysetListControl(false) { Dock = DockStyle.Fill, GridView = true };
		LC_Items.CanDrawItem += Ctrl_CanDrawItem;
		panel1.Controls.Add(LC_Items);

		if (profiles is null)
		{
			LC_Items.LoadProfile += Ctrl_LoadProfile;
			LC_Items.MergeProfile += Ctrl_MergeProfile;
			LC_Items.ExcludeProfile += Ctrl_ExcludeProfile;
			LC_Items.DisposeProfile += Ctrl_DisposeProfile;
			LC_Items.Loading = !_notifier.PlaysetsLoaded;

			if (!LC_Items.Loading)
			{
				LC_Items.SetItems(_profileManager.Playsets);
			}

			_notifier.PlaysetChanged += LoadProfile;
		}
		else
		{
			L_Counts.Visible = TLP_ProfileName.Visible = B_AddProfile.Visible = B_TempProfile.Visible = B_Discover.Visible = false;

			DD_Sorting.Parent = null;
			TLP_Main.SetColumn(DD_Usage, 2);
			TLP_Main.SetColumnSpan(TB_Search, 2);

			LC_Items.ReadOnly = true;
			LC_Items.SetItems(profiles);
			LC_Items.SetSorting(ProfileSorting.Downloads);
		}

		SlickTip.SetTo(B_GridView, "Switch to Grid-View");
		SlickTip.SetTo(B_ListView, "Switch to List-View");
		SlickTip.SetTo(B_AddProfile.Controls[0], "NewPlayset_Tip");
		SlickTip.SetTo(B_TempProfile.Controls[0], "TempPlayset_Tip");
		SlickTip.SetTo(I_ProfileIcon, "ChangePlaysetColor");
		SlickTip.SetTo(B_EditName, "ChangePlaysetSettings");
		SlickTip.SetTo(B_Save, "SavePlaysetChanges");

		RefreshCounts();
	}

	private void Ctrl_CanDrawItem(object sender, CanDrawItemEventArgs<ICustomPlayset> e)
	{
		var valid = true;

		if (e.Item.Usage > 0 && DD_Usage.SelectedItems.Count() > 0)
		{
			valid &= DD_Usage.SelectedItems.Contains(e.Item.Usage);
		}

		if (!string.IsNullOrWhiteSpace(TB_Search.Text))
		{
			var author = e.Item.Author;

			valid &= TB_Search.Text.SearchCheck(e.Item.Name) || (author is not null && TB_Search.Text.SearchCheck(author.Name));
		}

		e.DoNotDraw = !valid;
	}

	private void Ctrl_DisposeProfile(ICustomPlayset obj)
	{
		_profileManager.DeletePlayset(obj);
	}

	private void Ctrl_ExcludeProfile(ICustomPlayset obj)
	{
		FLP_Profiles.Enabled = false;
		_profileManager.ExcludeFromCurrentPlayset(obj);
	}

	private void Ctrl_MergeProfile(ICustomPlayset obj)
	{
		FLP_Profiles.Enabled = false;
		_profileManager.MergeIntoCurrentPlayset(obj);
	}

	private void Ctrl_LoadProfile(ICustomPlayset obj)
	{
		if (!I_ProfileIcon.Loading)
		{
			I_ProfileIcon.Loading = true;
			L_CurrentProfile.Text = obj.Name;
			I_Favorite.Visible = B_Save.Visible = false;
			_profileManager.SetCurrentPlayset(obj);
		}
	}

	protected override void OnCreateControl()
	{
		base.OnCreateControl();

		if (!LC_Items.ReadOnly)
		{
			LoadProfile();
		}
	}

	private void RefreshCounts()
	{
		if (L_Counts.Visible)
		{
			var favorites = _profileManager.Playsets.Count(x => x.IsFavorite);
			var total = _profileManager.Playsets.Count(x => !x.Temporary);
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

		slickIcon1.Size = slickIcon2.Size = B_EditName.Size = B_Save.Size = I_ProfileIcon.Size = I_Favorite.Size = UI.Scale(new Size(24, 24), UI.FontScale) + new Size(8, 8);
		L_CurrentProfile.Font = UI.Font(12.75F, FontStyle.Bold);
		roundedPanel.Margin = TB_Search.Margin = L_Counts.Margin = L_FilterCount.Margin = DD_Sorting.Margin = DD_Usage.Margin = UI.Scale(new Padding(7), UI.FontScale);
		B_TempProfile.Padding = B_AddProfile.Padding = TLP_ProfileName.Padding = B_ListView.Padding = B_GridView.Padding = UI.Scale(new Padding(5), UI.FontScale);
		B_ListView.Size = B_GridView.Size = UI.Scale(new Size(24, 24), UI.FontScale);
		L_Counts.Font = L_FilterCount.Font = UI.Font(7.5F, FontStyle.Bold);
		B_Discover.Font = UI.Font(9.75F, FontStyle.Bold);
		DD_Usage.Width = DD_Sorting.Width = (int)(180 * UI.FontScale);
		TB_Search.Width = (int)(250 * UI.FontScale);
		roundedPanel.Padding = new Padding((int)(2.5 * UI.FontScale) + 1, (int)(5 * UI.FontScale), (int)(2.5 * UI.FontScale), (int)(5 * UI.FontScale));

		var size = (int)(30 * UI.FontScale) - 6;
		TB_Search.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = new Size(0, size);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_ProfileName.Invalidate();
		B_TempProfile.BackColor = B_AddProfile.BackColor = FormDesign.Design.ButtonColor;
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

	private void LoadProfile()
	{
		this.TryInvoke(() =>
		{
			var profile = _profileManager.CurrentPlayset;

			if (profile is null)
				return;
			TLP_ProfileName.BackColor = profile.Color ?? FormDesign.Design.ButtonColor;
			TLP_ProfileName.ForeColor = TLP_ProfileName.BackColor.GetTextColor();
			I_ProfileIcon.ImageName = profile.GetIcon();
			I_Favorite.ImageName = profile.IsFavorite ? "I_StarFilled" : "I_Star";
			B_TempProfile.Visible = !profile.Temporary;
			I_ProfileIcon.Enabled = !profile.Temporary;

			tableLayoutPanel1.SetColumn(B_TempProfile, profile.Temporary ? 2 : 1);
			tableLayoutPanel1.SetColumn(B_AddProfile, profile.Temporary ? 1 : 2);

			SlickTip.SetTo(I_Favorite, profile.IsFavorite ? "UnFavoriteThisPlayset" : "FavoriteThisPlayset");

			I_Favorite.Visible = B_Save.Visible = !profile.Temporary;

			I_ProfileIcon.Loading = false;
			L_CurrentProfile.Text = profile.Name;

			LC_Items.Invalidate();
		});
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
		B_GridView.Selected = false;
		B_ListView.Selected = true;
		LC_Items.GridView = false;
	}

	private void B_GridView_Click(object sender, EventArgs e)
	{
		B_GridView.Selected = true;
		B_ListView.Selected = false;
		LC_Items.GridView = true;
	}

	private void B_AddProfile_Click(object sender, EventArgs e)
	{
		Form.PushPanel<PC_PlaysetAdd>();
	}

	private void B_TempProfile_Click(object sender, EventArgs e)
	{
		//_profileManager.SetCurrentPlayset(_profileManager.TemporaryPlayset);
	}

	private async void B_Save_Click(object sender, EventArgs e)
	{
		if (_profileManager.CurrentPlayset.Save())
		{
			B_Save.ImageName = "I_Check";

			await Task.Delay(1500);

			B_Save.ImageName = "I_Save";
		}
		else
		{
			ShowPrompt(Locale.CouldNotCreatePlayset, icon: PromptIcons.Error);
		}
	}

	private void B_EditName_Click(object sender, EventArgs e)
	{
		Form.PushPanel(ServiceCenter.Get<IAppInterfaceService>().PlaysetSettingsPanel());
	}

	private void I_ProfileIcon_Click(object sender, EventArgs e)
	{
		if (_profileManager.CurrentPlayset.Temporary)
		{
			return;
		}

		var colorDialog = new SlickColorPicker(_profileManager.CurrentPlayset.Color ?? Color.Red);

		if (colorDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		TLP_ProfileName.BackColor = colorDialog.Color;
		TLP_ProfileName.ForeColor = TLP_ProfileName.BackColor.GetTextColor();
		_profileManager.CurrentPlayset.Color = colorDialog.Color;
		//_profileManager.Save(_profileManager.CurrentPlayset);
	}

	private void I_Favorite_Click(object sender, EventArgs e)
	{
		if (_profileManager.CurrentPlayset.Temporary)
		{
			return;
		}

		_profileManager.CurrentPlayset.IsFavorite = !_profileManager.CurrentPlayset.IsFavorite;
		//_profileManager.Save(_profileManager.CurrentPlayset);

		I_Favorite.ImageName = _profileManager.CurrentPlayset.IsFavorite ? "I_StarFilled" : "I_Star";
		SlickTip.SetTo(I_Favorite, _profileManager.CurrentPlayset.IsFavorite ? "UnFavoriteThisPlayset" : "FavoriteThisPlayset");
	}

	private async void B_Discover_Click(object sender, EventArgs e)
	{
		try
		{
			B_Discover.Loading = true;

			var profiles = await ServiceCenter.Get<ISkyveApiUtil>().GetPublicPlaysets();

			Invoke(() => Form.PushPanel(new PC_PlaysetList(profiles)));
		}
		catch (Exception ex)
		{
			ShowPrompt(ex, Locale.FailedToRetrievePlaysets);
		}

		B_Discover.Loading = false;
	}

	public void Import(string file)
	{
		try
		{
			var profile = _profileManager.Playsets.FirstOrDefault(x => x.Name!.Equals(Path.GetFileNameWithoutExtension(file), StringComparison.InvariantCultureIgnoreCase));

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
				{ CrossIO.DeleteFile(file, true); }
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
				profile ??= _profileManager.ImportPlayset(file);
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
		catch (Exception ex) { ShowPrompt(ex, Locale.FailedToImportPlayset); }
	}
}
