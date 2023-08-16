using Skyve.App;
using Skyve.App.UserInterface.Generic;

using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_PlaysetSettings : PanelContent
{
	private bool loadingProfile;
	private readonly SlickCheckbox[] _launchOptions;

	private readonly IPlaysetManager _playsetManager;
	private readonly ILocationManager _locationManager;
	private readonly IPackageManager _packageManager;
	private readonly ISettings _settings;
	private readonly IBulkUtil _bulkUtil;
	private readonly IIOUtil _iOUtil;
	private readonly INotifier _notifier;
	private readonly ITagsService _tagsService;

	public PC_PlaysetSettings()
	{
		ServiceCenter.Get(out _bulkUtil, out _iOUtil, out _locationManager, out _playsetManager, out _packageManager, out _notifier, out _settings, out _tagsService);

		InitializeComponent();

		_launchOptions = new[] { CB_StartNewGame, CB_LoadSave, CB_NewAsset, CB_LoadAsset };

		SlickTip.SetTo(B_AddProfile.Controls[0], "NewPlayset_Tip");
		SlickTip.SetTo(B_TempProfile.Controls[0], "TempPlayset_Tip");
		SlickTip.SetTo(B_ViewProfiles, "ViewPlayset_Tip");
		SlickTip.SetTo(I_ProfileIcon, "ChangePlaysetColor");
		SlickTip.SetTo(B_EditName, "EditPlaysetName");
		SlickTip.SetTo(B_Save, "SavePlaysetChanges");

		foreach (var item in this.GetControls<SlickCheckbox>())
		{
			if (item != CB_LHT && item != CB_NoWorkshop && item.Parent != TLP_AdvancedDev)
			{
				SlickTip.SetTo(item, item.Text + "_Tip");
			}
		}

		LoadProfile(_playsetManager.CurrentPlayset as Playset);

		var saveGameTag = new ITag[] { new TagItem(TagSource.InGame, "SaveGame") };
		var mapTag = new ITag[] { new TagItem(TagSource.InGame, "Map") };

		DD_SaveFile.StartingFolder = CrossIO.Combine(_locationManager.AppDataPath, "Saves");
		DD_SaveFile.PinnedFolders = new()
		{
			["Your Save-games"] = CrossIO.Combine(_locationManager.AppDataPath, "Saves"),
			["Workshop Save-games"] = IOSelectionDialog.CustomDirectory,
		};
		DD_SaveFile.CustomFiles = _packageManager.Assets.Where(x => _tagsService.HasAllTags(x, saveGameTag)).Select(x => new IOSelectionDialog.CustomFile
		{
			Name = x.Name,
			Icon = x.GetThumbnail(),
			Path = x.FilePath
		}).ToList();

		DD_SkipFile.StartingFolder = _locationManager.AppDataPath;
		DD_SkipFile.PinnedFolders = new() { ["App Data"] = _locationManager.AppDataPath };

		DD_NewMap.StartingFolder = _locationManager.MapsPath;
		DD_NewMap.PinnedFolders = new()
		{
			["Custom Maps"] = _locationManager.MapsPath,
			["Vanilla Maps"] = CrossIO.Combine(_locationManager.GameContentPath, "Maps"),
			["Workshop Maps"] = IOSelectionDialog.CustomDirectory,
		};
		DD_NewMap.CustomFiles = _packageManager.Assets.Where(x => _tagsService.HasAllTags(x, mapTag)).Select(x => new IOSelectionDialog.CustomFile
		{
			Name = x.Name,
			Icon = x.GetThumbnail(),
			Path = x.FilePath
		}).ToList();

		TLP_AdvancedDev.Visible = _settings.UserSettings.AdvancedLaunchOptions;

		_notifier.PlaysetChanged += ProfileManager_ProfileChanged;
	}

	private void ProfileManager_ProfileChanged()
	{
		this.TryInvoke(() => LoadProfile(_playsetManager.CurrentPlayset as Playset));
	}

	protected override void LocaleChanged()
	{
		Text = Locale.PlaysetBubble;
		L_TempProfile.Text = Locale.TemporaryPlaysetCanNotBeEdited;
		DD_ProfileUsage.Text = Locale.PlaysetUsage;
		L_Info.Text = Locale.PlaysetSaveInfo;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		slickIcon1.Size = slickIcon2.Size = B_EditName.Size = B_Save.Size = I_ProfileIcon.Size = I_Info.Size = I_TempProfile.Size = I_Favorite.Size = UI.Scale(new Size(24, 24), UI.FontScale) + new Size(8, 8);
		slickSpacer1.Height = (int)(1.5 * UI.FontScale);
		P_Options.Padding = UI.Scale(new Padding(5, 0, 5, 0), UI.UIScale);
		slickSpacer1.Margin = B_TempProfile.Padding = B_AddProfile.Padding = TLP_ProfileName.Padding = P_Options.Margin = UI.Scale(new Padding(5), UI.UIScale);
		L_TempProfile.Font = UI.Font(10.5F);
		L_CurrentProfile.Font = UI.Font(12.75F, FontStyle.Bold);
		B_ViewProfiles.Font = UI.Font(9.75F);
		TLP_AdvancedDev.Margin = TLP_GeneralSettings.Margin = TLP_LaunchSettings.Margin = TLP_LSM.Margin = UI.Scale(new Padding(10), UI.UIScale);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		B_TempProfile.BackColor = B_AddProfile.BackColor = FormDesign.Design.ButtonColor;
		TLP_LaunchSettings.BackColor = TLP_AdvancedDev.BackColor = TLP_GeneralSettings.BackColor = TLP_LSM.BackColor = design.BackColor.Tint(Lum: design.Type.If(FormDesignType.Dark, 1, -1));
		L_TempProfile.ForeColor = design.YellowColor;
		P_Options.BackColor = design.AccentBackColor;
	}

	public override bool CanExit(bool toBeDisposed)
	{
		if (I_ProfileIcon.Loading)
		{
			if (toBeDisposed)
			{
				Notification.Create(Locale.PlaysetStillLoading, null, PromptIcons.Hand, null).Show(Form, 10);
			}

			return false;
		}

		if (TB_Name.Visible)
		{
			if (toBeDisposed)
			{
				Notification.Create(Locale.ApplyPlaysetNameBeforeExit, null, PromptIcons.Hand, null).Show(Form, 10);
			}

			return false;
		}

		return true;
	}

	internal void Ctrl_LoadProfile(ICustomPlayset obj)
	{
		I_ProfileIcon.Loading = true;
		L_CurrentProfile.Text = obj.Name;
		TLP_Options.Enabled = B_EditName.Visible = B_Save.Visible = false;
		_playsetManager.SetCurrentPlayset(obj);
	}

	private void LoadProfile(Playset? profile)
	{
		if (profile == null)
		{
			return;
		}

		loadingProfile = true;

		TLP_ProfileName.BackColor = profile.Color ?? FormDesign.Design.ButtonColor;
		TLP_ProfileName.ForeColor = TLP_ProfileName.BackColor.GetTextColor();
		I_ProfileIcon.ImageName = profile.GetIcon();
		I_Favorite.ImageName = profile.IsFavorite ? "I_StarFilled" : "I_Star";
		L_TempProfile.Visible = I_TempProfile.Visible = profile.Temporary;
		B_TempProfile.Visible = !profile.Temporary;
		I_Favorite.Visible = I_ProfileIcon.Enabled = L_Info.Visible = I_Info.Visible = !profile.Temporary;
		TLP_Options.Enabled = true;
		TLP_GeneralSettings.Visible = !profile.Temporary;

		SlickTip.SetTo(I_Favorite, profile.IsFavorite ? "UnFavoriteThisPlayset" : "FavoriteThisPlayset");

		TLP_Main.SetColumn(B_TempProfile, profile.Temporary ? 4 : 3);
		TLP_Main.SetColumn(B_AddProfile, profile.Temporary ? 3 : 4);

		TLP_Options.SetRow(TLP_GeneralSettings, profile.Temporary ? 2 : 0);
		TLP_Options.SetRow(TLP_LSM, profile.Temporary ? 0 : 1);

		B_EditName.Visible = B_Save.Visible = !profile.Temporary && !TB_Name.Visible;

		I_ProfileIcon.Loading = false;
		L_CurrentProfile.Text = profile.Name;
		CB_AutoSave.Checked = profile.AutoSave;
		DD_ProfileUsage.SelectedItem = profile.Usage > 0 ? profile.Usage : (PackageUsage)(-1);

		CB_NoWorkshop.Checked = profile.LaunchSettings.NoWorkshop;
		CB_NoAssets.Checked = profile.LaunchSettings.NoAssets;
		CB_NoMods.Checked = profile.LaunchSettings.NoMods;
		CB_LHT.Checked = profile.LaunchSettings.LHT;
		CB_UseCitiesExe.Checked = profile.LaunchSettings.UseCitiesExe;
		CB_UnityProfiler.Checked = profile.LaunchSettings.UnityProfiler;
		CB_DebugMono.Checked = profile.LaunchSettings.DebugMono;
		CB_LoadSave.Checked = profile.LaunchSettings.LoadSaveGame;
		CB_StartNewGame.Checked = profile.LaunchSettings.StartNewGame;
		CB_DevUI.Checked = profile.LaunchSettings.DevUi;
		CB_RefreshWorkshop.Checked = profile.LaunchSettings.RefreshWorkshop;
		DD_NewMap.SelectedFile = profile.LaunchSettings.MapToLoad;
		DD_SaveFile.SelectedFile = profile.LaunchSettings.SaveToLoad;
		TB_CustomArgs.Text = profile.LaunchSettings.CustomArgs;
		CB_NewAsset.Checked = profile.LaunchSettings.NewAsset;
		CB_LoadAsset.Checked = profile.LaunchSettings.LoadAsset;

		CB_LoadUsed.Checked = profile.LsmSettings.LoadUsed;
		CB_LoadEnabled.Checked = profile.LsmSettings.LoadEnabled;
		CB_SkipFile.Checked = profile.LsmSettings.UseSkipFile;
		DD_SkipFile.SelectedFile = profile.LsmSettings.SkipFile;

		DD_SaveFile.Enabled = CB_LoadSave.Checked;
		DD_SkipFile.Enabled = CB_SkipFile.Checked;
		DD_NewMap.Enabled = CB_StartNewGame.Checked;

		loadingProfile = false;
	}

	private void ValueChanged(object sender, EventArgs e)
	{
		DD_SaveFile.Enabled = CB_LoadSave.Checked;
		DD_SkipFile.Enabled = CB_SkipFile.Checked;
		DD_NewMap.Enabled = CB_StartNewGame.Checked;

		if (loadingProfile)
		{
			return;
		}

		if (_launchOptions.Contains(sender) && (sender as SlickCheckbox)!.Checked)
		{
			foreach (var item in _launchOptions)
			{
				if (item == sender)
				{
					continue;
				}

				item.Checked = false;
			}
		}

		var playset = (_playsetManager.CurrentPlayset as Playset)!;

		playset.AutoSave = CB_AutoSave.Checked;
		playset.Usage = DD_ProfileUsage.SelectedItem;

		playset.LaunchSettings.NoWorkshop = CB_NoWorkshop.Checked;
		playset.LaunchSettings.NoAssets = CB_NoAssets.Checked;
		playset.LaunchSettings.NoMods = CB_NoMods.Checked;
		playset.LaunchSettings.LHT = CB_LHT.Checked;
		playset.LaunchSettings.StartNewGame = CB_StartNewGame.Checked;
		playset.LaunchSettings.MapToLoad = _iOUtil.ToRealPath(DD_NewMap.SelectedFile);
		playset.LaunchSettings.SaveToLoad = _iOUtil.ToRealPath(DD_SaveFile.SelectedFile);
		playset.LaunchSettings.LoadSaveGame = CB_LoadSave.Checked;
		playset.LaunchSettings.UseCitiesExe = CB_UseCitiesExe.Checked;
		playset.LaunchSettings.UnityProfiler = CB_UnityProfiler.Checked;
		playset.LaunchSettings.DebugMono = CB_DebugMono.Checked;
		playset.LaunchSettings.RefreshWorkshop = CB_RefreshWorkshop.Checked;
		playset.LaunchSettings.DevUi = CB_DevUI.Checked;
		playset.LaunchSettings.CustomArgs = TB_CustomArgs.Text;
		playset.LaunchSettings.NewAsset = CB_NewAsset.Checked;
		playset.LaunchSettings.LoadAsset = CB_LoadAsset.Checked;

		playset.LsmSettings.SkipFile = _iOUtil.ToRealPath(DD_SkipFile.SelectedFile);
		playset.LsmSettings.LoadEnabled = CB_LoadEnabled.Checked;
		playset.LsmSettings.LoadUsed = CB_LoadUsed.Checked;
		playset.LsmSettings.UseSkipFile = CB_SkipFile.Checked;

		_playsetManager.Save(playset);
	}

	private void B_LoadProfiles_Click(object sender, EventArgs e)
	{
		if (!I_ProfileIcon.Loading)
		{
			Form.PushPanel(new PC_PlaysetList());
		}
	}

	private void B_NewProfile_Click(object sender, EventArgs e)
	{
		Form.PushPanel<PC_PlaysetAdd>();
	}

	internal void B_EditName_Click(object sender, EventArgs e)
	{
		TB_Name.Visible = true;
		B_EditName.Visible = B_Save.Visible = false;
		L_CurrentProfile.Visible = false;
		TB_Name.Text = L_CurrentProfile.Text;

		this.TryBeginInvoke(() =>
		{
			TB_Name.Focus();
			TB_Name.SelectAll();
		});
	}

	private void TB_Name_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode is Keys.Enter or Keys.Escape)
		{
			e.SuppressKeyPress = true;
			e.Handled = true;
		}
	}

	private void TB_Name_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		if (e.KeyCode == Keys.Enter)
		{
			e.IsInputKey = true;

			TB_Name.Visible = false;
		}

		if (e.KeyCode == Keys.Escape)
		{
			e.IsInputKey = true;

			TB_Name.Text = string.Empty;
			TB_Name.Visible = false;
		}
	}

	private void TB_Name_IconClicked(object sender, EventArgs e)
	{
		TB_Name.Visible = false;
	}

	private void TB_Name_Leave(object sender, EventArgs e)
	{
		if (!TB_Name.Visible)
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(TB_Name.Text))
		{
			TB_Name.Visible = false;
			B_EditName.Visible = B_Save.Visible = true;
			L_CurrentProfile.Visible = true;
			return;
		}

		if (!_playsetManager.RenamePlayset(_playsetManager.CurrentPlayset, TB_Name.Text))
		{
			TB_Name.SetError();
			return;
		}

		if (_playsetManager.CurrentPlayset.Name != TB_Name.Text)
		{
			Notification.Create(Locale.PlaysetNameChangedIllegalChars, null, PromptIcons.Info, null)
				.Show(Form, 15);
		}

		L_CurrentProfile.Text = _playsetManager.CurrentPlayset.Name;
		TB_Name.Visible = false;
		B_EditName.Visible = B_Save.Visible = true;
		L_CurrentProfile.Visible = true;
	}

	private void T_ProfileUsage_SelectedValueChanged(object sender, EventArgs e)
	{
		if (loadingProfile)
		{
			return;
		}

		var invalidPackages = _playsetManager.GetInvalidPackages(DD_ProfileUsage.SelectedItem);

		if (invalidPackages.Any())
		{
			if (ShowPrompt($"{Locale.SomePackagesWillBeDisabled}\r\n{Locale.AffectedPackagesAre}\r\n• {invalidPackages.ListStrings("\r\n• ")}", PromptButtons.OKCancel, PromptIcons.Warning) == DialogResult.Cancel)
			{
				DD_ProfileUsage.SelectedItem = (PackageUsage)(-1);

				return;
			}

			_bulkUtil.SetBulkIncluded(invalidPackages, false);
		}

		ValueChanged(sender, e);

		I_ProfileIcon.Image = _playsetManager.CurrentPlayset.GetIcon();
	}

	private void LsmSettingsChanged(object sender, EventArgs e)
	{
		if (loadingProfile)
		{
			return;
		}

		ValueChanged(sender, e);

		_playsetManager.SaveLsmSettings(_playsetManager.CurrentPlayset);
	}

	private void B_Save_Click(object sender, EventArgs e)
	{
		if (_playsetManager.CurrentPlayset.Save())
		{
			B_Save.ImageName = "I_Check";

			new BackgroundAction(() =>
			{
				B_Save.ImageName = "I_Save";
			}).RunIn(2000);
		}
		else
		{
			ShowPrompt(Locale.CouldNotCreatePlayset, icon: PromptIcons.Error);
		}
	}

	private void B_TempProfile_Click(object sender, EventArgs e)
	{
		_playsetManager.SetCurrentPlayset(Playset.TemporaryPlayset);
	}

	private void DD_SaveFile_FileSelected(string obj)
	{
		DD_SaveFile.SelectedFile = obj;
		ValueChanged(DD_SaveFile, EventArgs.Empty);
	}

	private void DD_SkipFile_FileSelected(string obj)
	{
		DD_SkipFile.SelectedFile = obj;
		LsmSettingsChanged(DD_SkipFile, EventArgs.Empty);
	}

	private void DD_NewMap_FileSelected(string obj)
	{
		DD_NewMap.SelectedFile = obj;
		ValueChanged(DD_NewMap, EventArgs.Empty);
	}

	private bool DD_ValidFile(object sender, string arg)
	{
		return (sender as DragAndDropControl)!.ValidExtensions.Any(x => x.Equals(Path.GetExtension(arg), StringComparison.CurrentCultureIgnoreCase));
	}

	private void I_ProfileIcon_Click(object sender, EventArgs e)
	{
		if (_playsetManager.CurrentPlayset.Temporary)
		{
			return;
		}

		var colorDialog = new SlickColorPicker(_playsetManager.CurrentPlayset.Color ?? Color.Red);

		if (colorDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}

		TLP_ProfileName.BackColor = colorDialog.Color;
		TLP_ProfileName.ForeColor = TLP_ProfileName.BackColor.GetTextColor();
		_playsetManager.CurrentPlayset.Color = colorDialog.Color;
		_playsetManager.Save(_playsetManager.CurrentPlayset);
	}

	private void I_Favorite_Click(object sender, EventArgs e)
	{
		if (_playsetManager.CurrentPlayset.Temporary)
		{
			return;
		}

		_playsetManager.CurrentPlayset.IsFavorite = !_playsetManager.CurrentPlayset.IsFavorite;
		_playsetManager.Save(_playsetManager.CurrentPlayset);

		I_Favorite.ImageName = _playsetManager.CurrentPlayset.IsFavorite ? "I_StarFilled" : "I_Star";
		SlickTip.SetTo(I_Favorite, _playsetManager.CurrentPlayset.IsFavorite ? "UnFavoriteThisPlayset" : "FavoriteThisPlayset");
	}
}
