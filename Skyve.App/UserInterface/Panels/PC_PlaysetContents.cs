using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_PlaysetContents : PC_ContentList
{
	private readonly IPlaysetManager _playsetManager = ServiceCenter.Get<IPlaysetManager>();

	public override SkyvePage Page => SkyvePage.Generic;

	public IPlayset Playset { get; }

	public PC_PlaysetContents(IPlayset playset) : base(true, true)
	{
		Playset = playset;

		Text = Playset.Name;
		LC_Items.IsGenericPage = true;
		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems()
	{
		return await _playsetManager.GetPlaysetContents(Playset);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}
}
