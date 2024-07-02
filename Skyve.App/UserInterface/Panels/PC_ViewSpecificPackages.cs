using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_ViewSpecificPackages : PC_ContentList
{
	private readonly ISettings _settings = ServiceCenter.Get<ISettings>();
	private readonly IPackageManager _contentManager = ServiceCenter.Get<IPackageManager>();
	private readonly List<IPackageIdentity> _packages;
	private readonly string _title;

	public override SkyvePage Page => SkyvePage.Packages;

	public PC_ViewSpecificPackages(List<IPackageIdentity> packages, string title)
	{
		_packages = packages;
		_title = title;
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		return await Task.FromResult(_packages ?? []);
	}

	protected override void LocaleChanged()
	{
		base.LocaleChanged();

		Text = _title;
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
