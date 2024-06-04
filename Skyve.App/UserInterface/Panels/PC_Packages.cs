using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_Packages : PC_ContentList
{
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();
	private readonly IPackageManager _contentManager = ServiceCenter.Get<IPackageManager>();

	public PC_Packages()
	{
		if (_settings.UserSettings.FilterIncludedByDefault)
		{
			LC_Items.OT_Included.SelectedValue = Generic.ThreeOptionToggle.Value.Option1;
		}
	}

	public override SkyvePage Page => SkyvePage.Packages;

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = $"{Locale.Package.Plural} - {ServiceCenter.Get<IPlaysetManager>().CurrentPlayset?.Name ?? Locale.NoActivePlayset}";
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		if (_settings.UserSettings.FilterOutPackagesWithOneAsset || _settings.UserSettings.FilterOutPackagesWithMods)
		{
			return _contentManager.Packages.Where(x =>
			{
				if (_settings.UserSettings.FilterOutPackagesWithOneAsset && (x.LocalData?.Assets.Length == 1))
				{
					return false;
				}

				if (_settings.UserSettings.FilterOutPackagesWithMods && x.IsCodeMod)
				{
					return false;
				}

				return true;
			});
		}

		return await Task.FromResult(_contentManager.Packages);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
