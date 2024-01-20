using Skyve.App.Interfaces;

using System.IO;
using System.Windows.Forms;

namespace Skyve.App.UserInterface.Panels;
public partial class PC_PlaysetAdd : PanelContent
{
	private readonly IPlaysetManager _profileManager = ServiceCenter.Get<IPlaysetManager>();

	public PC_PlaysetAdd()
	{
		InitializeComponent();

		DAD_NewProfile.StartingFolder = ServiceCenter.Get<ISettings>().FolderSettings.AppDataPath;
	}

	protected override void UIChanged()
	{
		base.UIChanged();

		B_Cancel.Font = UI.Font(9.75F);
		DAD_NewProfile.Margin = UI.Scale(new Padding(10), UI.UIScale);
	}

	private void NewProfile_Click(object sender, EventArgs e)
	{
		//var newProfile = _profileManager.CreateNewPlayset("New Playset");

		//if (!_profileManager.Save(newProfile))
		//{
		//	ShowPrompt(Locale.CouldNotCreatePlayset, icon: PromptIcons.Error);
		//	return;
		//}

		//_profileManager.AddPlayset(newProfile);

		//_profileManager.SetCurrentPlayset(newProfile);

		//var panel = ServiceCenter.Get<IAppInterfaceService>().PlaysetSettingsPanel();

		//if (Form.SetPanel(null, panel))
		//{
		//	panel.EditName();
		//}
	}

	private void CopyProfile_Click(object sender, EventArgs e)
	{
		//var newProfile = _profileManager.CurrentPlayset.Clone();
		//newProfile.Name = _profileManager.GetNewPlaysetName();

		//if (!newProfile.Save())
		//{
		//	ShowPrompt(Locale.CouldNotCreatePlayset, icon: PromptIcons.Error);
		//	return;
		//}

		//_profileManager.AddPlayset(newProfile);

		//_profileManager.SetCurrentPlayset(newProfile);

		//var panel = ServiceCenter.Get<IAppInterfaceService>().PlaysetSettingsPanel();	

		//if (Form.SetPanel(null, panel))
		//{
		//	panel.EditName();
		//}
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

#if CS1
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
		else
#endif
		if (profile is null)
		{
			profile = _profileManager.ImportPlayset(obj);
		}

		try
		{
			var panel = ServiceCenter.Get<IAppInterfaceService>().PlaysetSettingsPanel();

			if (Form.SetPanel(null, panel))
			{
				panel.LoadProfile(profile!);
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
