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
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();

	public PC_PlaysetList() : this(null) { }

	public PC_PlaysetList(IEnumerable<IPlayset>? playsets)
	{
		if (!_settings.UserSettings.PageSettings.ContainsKey(SkyvePage.Playsets))
		{
			_settings.UserSettings.PageSettings[SkyvePage.Playsets] = new() { GridView = true };
		}

		InitializeComponent();

		DD_Sorting.Height = TB_Search.Height = 0;

		LC_Items = new PlaysetListControl(false) { Dock = DockStyle.Fill, GridView = true };
		LC_Items.CanDrawItem += Ctrl_CanDrawItem;
		panel1.Controls.Add(LC_Items);

		if (playsets is null)
		{
			LC_Items.LoadPlaysetStarted += LC_Items_LoadPlaysetStarted;
			LC_Items.Loading = !_notifier.IsPlaysetsLoaded;

			_notifier.PlaysetUpdated += LoadPlayset;
			_notifier.PlaysetChanged += LoadPlayset;
		}
		else
		{
			L_Counts.Visible = TLP_PlaysetName.Visible = B_AddPlayset.Visible = B_Edit.Visible = B_DeactivatePlayset.Visible = B_Discover.Visible = false;

			DD_Sorting.Parent = null;
			TLP_Main.SetColumn(DD_Usage, 2);
			TLP_Main.SetColumnSpan(TB_Search, 2);

			LC_Items.ReadOnly = true;
			LC_Items.SetItems(playsets);
			LC_Items.SetSorting(PlaysetSorting.Downloads);
		}

		C_ViewTypeControl.GridView = LC_Items.GridView = _settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Playsets).GridView;

		SlickTip.SetTo(B_AddPlayset, "NewPlayset_Tip");
		SlickTip.SetTo(B_DeactivatePlayset, "DeactivatePlayset_Tip");
		SlickTip.SetTo(B_Edit, "ChangePlaysetSettings");

		RefreshCounts();
	}

	private void LC_Items_LoadPlaysetStarted()
	{
		PB_ActivePlayset.Loading = true;

		B_Edit.Visible = B_DeactivatePlayset.Visible = false;
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
#if CS1
			var total = _playsetManager.Playsets.Count(x => !x.Temporary);
#else
			var total = _playsetManager.Playsets.Count();
#endif
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
		L_CurrentPlaysetTitle.Text = Locale.ActivePlayset;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		PB_ActivePlayset.Size = B_Edit.Size = B_DeactivatePlayset.Size = UI.Scale(new Size(28, 28));
		PB_ActivePlayset.Margin = B_Edit.Margin = B_DeactivatePlayset.Margin = L_CurrentPlaysetTitle.Margin = UI.Scale(new Padding(3, 0, 2, 0));
		L_CurrentPlayset.Margin = UI.Scale(new Padding(0, 0, 2, 2));
		L_CurrentPlayset.Font = UI.Font(11F, FontStyle.Bold);
		B_AddPlayset.Font = UI.Font(9.75f);
		L_CurrentPlaysetTitle.Font = UI.Font(7F);
		TLP_PlaysetName.Padding = UI.Scale(new Padding(3));
		roundedPanel.Margin = TB_Search.Margin = L_Counts.Margin = L_FilterCount.Margin = DD_Sorting.Margin = DD_Usage.Margin = UI.Scale(new Padding(6));
		L_Counts.Font = L_FilterCount.Font = UI.Font(7.5F, FontStyle.Bold);
		B_Discover.Font = UI.Font(9F, FontStyle.Bold);
		B_Discover.Margin = TLP_PlaysetName.Margin = B_AddPlayset.Margin = UI.Scale(new Padding(5));
		DD_Usage.Width = DD_Sorting.Width = UI.Scale(180);
		TB_Search.Width = UI.Scale(250);
		roundedPanel.Padding = new Padding((int)(2.5 * UI.FontScale) + 1, UI.Scale(5), (int)(2.5 * UI.FontScale), UI.Scale(5));
		TLP_PlaysetName.MinimumSize = new Size(0, B_AddPlayset.Height);
		TLP_Top.Padding = new Padding(roundedPanel.Margin.Left, 0, roundedPanel.Margin.Right, 0);

		var size = UI.Scale(30) - 6;
		TB_Search.MaximumSize = new Size(9999, size);
		TB_Search.MinimumSize = new Size(0, size);
	}

	protected override void DesignChanged(FormDesign design)
	{
		base.DesignChanged(design);

		TLP_PlaysetName.Invalidate();

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
		PB_ActivePlayset.Loading = false;

		if (LC_Items.Loading)
		{
			LC_Items.Loading = false;
		}

		if (!LC_Items.ReadOnly)
		{
			LC_Items.SetItems(_playsetManager.Playsets);
		}

		this.TryInvoke(() =>
		{
			var playset = _playsetManager.CurrentPlayset;
			var customPlayset = _playsetManager.CurrentCustomPlayset;

			TLP_PlaysetName.BackColor = customPlayset?.Color?.MergeColor(FormDesign.Design.ButtonColor, 75) ?? FormDesign.Design.ButtonColor;
			TLP_PlaysetName.ForeColor = TLP_PlaysetName.BackColor.GetTextColor();
			L_CurrentPlayset.Text = playset?.Name ?? Locale.NoActivePlayset;
			L_CurrentPlaysetTitle.ForeColor = TLP_PlaysetName.ForeColor.MergeColor(TLP_PlaysetName.BackColor, 70);

			PB_ActivePlayset.Visible = B_Edit.Visible = B_DeactivatePlayset.Visible = playset is not null;

			LC_Items.Invalidate();
			PB_ActivePlayset.Invalidate();

			RefreshCounts();
		});
	}

	private void FilterChanged(object sender, EventArgs e)
	{
		TB_Search.ImageName = string.IsNullOrWhiteSpace(TB_Search.Text) ? "Search" : "ClearSearch";

		LC_Items.FilterChanged();
		RefreshCounts();
	}

	private void DD_Sorting_SelectedItemChanged(object sender, EventArgs e)
	{
		if (IsHandleCreated)
		{
			LC_Items.SetSorting(DD_Sorting.SelectedItem);

			_settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Playsets).Sorting = (int)DD_Sorting.SelectedItem;
			_settings.UserSettings.Save();
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
		_settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Playsets).GridView = false;
		_settings.UserSettings.Save();
	}

	private void B_GridView_Click(object sender, EventArgs e)
	{
		C_ViewTypeControl.GridView = true;
		LC_Items.GridView = true;
		_settings.UserSettings.PageSettings.GetOrAdd(SkyvePage.Playsets).GridView = true;
		_settings.UserSettings.Save();
	}

	private void B_AddPlayset_Click(object sender, EventArgs e)
	{
		Form.PushPanel(ServiceCenter.Get<IAppInterfaceService>().NewPlaysetPanel());
	}

	private async void B_Deactivate_Click(object sender, EventArgs e)
	{
		if (!PB_ActivePlayset.Loading)
		{
			PB_ActivePlayset.Loading = true;
			await _playsetManager.DeactivateActivePlayset();
			PB_ActivePlayset.Loading = false;
		}
	}

	private void B_Edit_Click(object sender, EventArgs e)
	{
		if (!PB_ActivePlayset.Loading && _playsetManager.CurrentPlayset is not null)
		{
			ServiceCenter.Get<IAppInterfaceService>().OpenPlaysetPage(_playsetManager.CurrentPlayset);
		}
	}

	private void B_Discover_Click(object sender, EventArgs e)
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
			var playset = _playsetManager.Playsets.FirstOrDefault(x => x.Name!.Equals(Path.GetFileNameWithoutExtension(file), StringComparison.InvariantCultureIgnoreCase));

			if (Path.GetExtension(file).ToLower() is ".zip")
			{
				using var stream = File.OpenRead(file);
				using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Read, false);

#if CS2
				var entry = zipArchive.GetEntry("Skyve\\CurrentPlayset.json") ?? zipArchive.GetEntry("Skyve/CurrentPlayset.json");
#else
				var entry = zipArchive.GetEntry("Skyve\\LogProfile.json") ?? zipArchive.GetEntry("Skyve/LogProfile.json");
#endif

				if (entry is null)
				{
					return;
				}

				using var entryStream = entry.Open();
				using var reader = new StreamReader(entryStream);

				file = await reader.ReadToEndAsync();
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
				playset ??= await _playsetManager.CreateLogPlayset(file);
			}

			if (playset is not null)
			{
				Form.PushPanel(new PC_PlaysetContents(playset));
			}
		}
		catch (Exception ex)
		{
			ShowPrompt(ex, Locale.FailedToImportPlayset);
		}
	}

	private void PB_ActivePlayset_Paint(object sender, PaintEventArgs e)
	{
		var customPlayset = _playsetManager.CurrentCustomPlayset;

		if (customPlayset is null)
		{
			return;
		}

		var banner = customPlayset.GetThumbnail();
		var backColor = customPlayset.Color ?? banner?.GetThemedAverageColor() ?? FormDesign.Design.AccentBackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 7 : -6, Sat: 3);
		var onBannerColor = (banner?.GetThemedAverageColor() ?? FormDesign.Design.BackColor.Tint(Lum: FormDesign.Design.IsDarkTheme ? 7 : -6, Sat: 3)).GetTextColor();
		var borderRadius = UI.Scale(3);

		if (banner is null)
		{
			using var brush = new SolidBrush(Color.FromArgb(40, onBannerColor));

			e.Graphics.FillRoundedRectangle(brush, PB_ActivePlayset.ClientRectangle, borderRadius);

			using var icon = customPlayset.Usage.GetIcon().Get(PB_ActivePlayset.ClientRectangle.Width * 3 / 4).Color(onBannerColor);

			e.Graphics.DrawImage(icon, PB_ActivePlayset.ClientRectangle.CenterR(icon.Size));
		}
		else
		{
			e.Graphics.DrawRoundedImage(banner, PB_ActivePlayset.ClientRectangle, borderRadius, PB_ActivePlayset.BackColor);
		}
	}
}
