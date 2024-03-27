using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_Mods : PC_ContentList
{
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();

	public PC_Mods()
	{
		if (_settings.UserSettings.FilterIncludedByDefault)
		{
			LC_Items.OT_Included.SelectedValue = Generic.ThreeOptionToggle.Value.Option1;
		}
	}

	public override SkyvePage Page => SkyvePage.Mods;

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = $"{Locale.Mod.Plural} - {ServiceCenter.Get<IPlaysetManager>().CurrentPlayset?.Name ?? Locale.NoActivePlayset}";
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		return await Task.FromResult(ServiceCenter.Get<IPackageManager>().Packages.Where(x => x.IsCodeMod));
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Mod;
	}
}
