﻿using SkyveApp.Domain.CS1;
using SkyveApp.Systems.CS1.Utilities;

using System.IO;
using System.Windows.Forms;

namespace SkyveApp.UserInterface.Panels;
public partial class PC_PlaysetAdd : PanelContent
{
	private readonly IPlaysetManager _profileManager = ServiceCenter.Get<IPlaysetManager>();

	public PC_PlaysetAdd()
	{
		InitializeComponent();

		DAD_NewProfile.StartingFolder = ServiceCenter.Get<ILocationManager>().AppDataPath;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		B_Cancel.Font = UI.Font(9.75F);
		DAD_NewProfile.Margin = UI.Scale(new Padding(10), UI.UIScale);
	}

	private void NewProfile_Click(object sender, EventArgs e)
	{
		var newProfile = new Playset() { Name = _profileManager.GetNewPlaysetName(), LastEditDate = DateTime.Now };

		if (!_profileManager.Save(newProfile))
		{
			ShowPrompt(Locale.PlaysetCreationFailed, icon: PromptIcons.Error);
			return;
		}

		_profileManager.AddPlayset(newProfile);

		_profileManager.SetCurrentPlayset(newProfile);

		var panel = new PC_PlaysetSettings();

		if (Form.SetPanel(null, panel))
		{
			panel.B_EditName_Click(sender, e);
		}
	}

	private void CopyProfile_Click(object sender, EventArgs e)
	{
		var newProfile = _profileManager.CurrentPlayset.Clone();
		newProfile.Name = _profileManager.GetNewPlaysetName();

		if (!newProfile.Save())
		{
			ShowPrompt(Locale.CouldNotCreatePlayset, icon: PromptIcons.Error);
			return;
		}

		_profileManager.AddPlayset(newProfile);

		_profileManager.SetCurrentPlayset(newProfile);

		var panel = new PC_PlaysetSettings();

		if (Form.SetPanel(null, panel))
		{
			panel.B_EditName_Click(sender, e);
		}
	}

	private void B_Cancel_Click(object sender, EventArgs e)
	{
		PushBack();
	}

	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
	{
		if (keyData == Keys.Escape)
		{
			PushBack();
			return true;
		}

		return base.ProcessCmdKey(ref msg, keyData);
	}

	private void DAD_NewProfile_FileSelected(string obj)
	{
		var profile = _profileManager.Playsets.FirstOrDefault(x => x.Name!.Equals(Path.GetFileNameWithoutExtension(obj), StringComparison.InvariantCultureIgnoreCase));

		if (obj.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
		{
			if (profile is not null)
			{
				ShowPrompt(Locale.PlaysetNameUsed, icon: PromptIcons.Hand);
				return;
			}

			profile = _profileManager.ConvertLegacyPlayset(obj, false);

			if (profile is null)
			{
				ShowPrompt(Locale.FailedToImportLegacyPlayset, icon: PromptIcons.Error);
				return;
			}
		}
		else if (profile is null)
		{
			profile = _profileManager.ImportPlayset(obj);
		}

		try
		{
			var panel = new PC_PlaysetSettings();

			if (Form.SetPanel(null, panel))
			{
				panel.Ctrl_LoadProfile(profile!);
			}
		}
		catch (Exception ex) { ShowPrompt(ex, "Failed to import your playset"); }
	}

	private async void B_ImportLink_Click(object sender, EventArgs e)
	{
		var result = ShowInputPrompt(Locale.PastePlaysetId);

		if (result.DialogResult != DialogResult.OK)
		{
			return;
		}

		try
		{
			await ServiceCenter.Get<IOnlinePlaysetUtil>().DownloadPlayset(result.Input);
		}
		catch (Exception ex) { Program.MainForm.TryInvoke(() => MessagePrompt.Show(ex, Locale.FailedToDownloadPlayset, form: Program.MainForm)); }
	}
}
