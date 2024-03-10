namespace Skyve.App.Interfaces;

public class PlaysetSettingsPanel : PanelContent
{
	public PlaysetSettingsPanel(bool load) : base(load)
	{
    }

    public PlaysetSettingsPanel()
    {
        
    }

    public virtual void EditName() { }
	public virtual void LoadPlayset(IPlayset customPlayset) { }
}