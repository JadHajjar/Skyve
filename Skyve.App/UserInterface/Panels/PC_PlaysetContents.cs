using System.Threading;
using System.Threading.Tasks;

namespace Skyve.App.UserInterface.Panels;
public class PC_PlaysetContents : PC_ContentList
{
	private readonly IPlaysetManager _playsetManager = ServiceCenter.Get<IPlaysetManager>();

	public override SkyvePage Page => SkyvePage.Generic;

	public IPlayset Playset { get; }

	public PC_PlaysetContents(IPlayset playset) : base(true, true, new TemporaryPlaysetPackageUtil(ServiceCenter.Get<IPackageUtil>()))
	{
		Playset = playset;

		Text = Playset.Name;
		LC_Items.IsGenericPage = true;
		LC_Items.TB_Search.Placeholder = "SearchGenericPackages";
	}

	protected override async Task<IEnumerable<IPackageIdentity>> GetItems(CancellationToken cancellationToken)
	{
		if (Playset is ITemporaryPlayset temporaryPlayset)
		{
			return await temporaryPlayset.GetPackages();
		}

		return await _playsetManager.GetPlaysetContents(Playset);
	}

	protected override LocaleHelper.Translation GetItemText()
	{
		return Locale.Package;
	}

	private class TemporaryPlaysetPackageUtil(IPackageUtil packageUtil) : IPackageUtil
	{
		public string? GetSelectedVersion(IPackageIdentity package, string? playsetId = null)
		{
			return packageUtil.GetSelectedVersion(package, playsetId);
		}

		public DownloadStatus GetStatus(IPackageIdentity? mod, out string reason)
		{
			return packageUtil.GetStatus(mod, out reason);
		}

		public bool IsEnabled(IPackageIdentity package, string? playsetId = null, bool withVersion = true)
		{
			return (package as IPlaysetPackage)?.IsEnabled ?? false;
		}

		public bool IsIncluded(IPackageIdentity package, string? playsetId = null, bool withVersion = true)
		{
			return true;
		}

		public bool IsIncluded(IPackageIdentity package, out bool partiallyIncluded, string? playsetId = null, bool withVersion = true)
		{
			partiallyIncluded = false;
			return true;
		}

		public bool IsIncludedAndEnabled(IPackageIdentity package, string? playsetId = null, bool withVersion = true)
		{
			return (package as IPlaysetPackage)?.IsEnabled ?? false;
		}

		public Task SetEnabled(IPackageIdentity package, bool value, string? playsetId = null)
		{
			return Task.CompletedTask;
		}

		public Task SetEnabled(IEnumerable<IPackageIdentity> packages, bool value, string? playsetId = null)
		{
			return Task.CompletedTask;
		}

		public Task SetIncluded(IPackageIdentity package, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true)
		{
			return Task.CompletedTask;
		}

		public Task SetIncluded(IEnumerable<IPackageIdentity> packages, bool value, string? playsetId = null, bool withVersion = true, bool promptForDependencies = true)
		{
			return Task.CompletedTask;
		}
	}
}
