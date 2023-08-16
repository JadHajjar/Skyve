namespace Skyve.App.Interfaces;

public abstract class PlaysetSettingsPanel : PanelContent
{
	public abstract void EditName();
	public abstract void LoadProfile(ICustomPlayset customPlayset);
}