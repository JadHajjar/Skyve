using Extensions;
using Extensions.Sql;

using Skyve.Domain;

using System.Drawing;
using System.IO;

namespace Skyve.Systems.Compatibility.Domain.Api;

[DynamicSqlClass("UserProfileContents")]
public class UserProfileContent : IDynamicSql
#if !API
	, IPlaysetEntry
#endif
{
	[DynamicSqlProperty(Indexer = true)]
	public int ProfileId { get; set; }
	[DynamicSqlProperty]
	public string? RelativePath { get; set; }
	[DynamicSqlProperty]
	public ulong SteamId { get; set; }
	[DynamicSqlProperty]
	public bool IsMod { get; set; }
	[DynamicSqlProperty]
	public bool Enabled { get; set; }

#if !API
	string ILocalPackageIdentity.FilePath => RelativePath ?? string.Empty;
	ulong IPackageIdentity.Id => SteamId;
	string? IPackageIdentity.Url => SteamId == 0 ? null : $"https://steamcommunity.com/workshop/filedetails/?id={SteamId}";
	string IPackageIdentity.Name
	{
		get
		{
			var name = this.GetWorkshopInfo()?.Name;

			return name is not null
				? name
				: !string.IsNullOrEmpty(RelativePath)
				? Path.GetFileNameWithoutExtension(RelativePath)
				: (string)LocaleHelper.GetGlobalText("UnknownPackage");
		}
	}

	public bool GetThumbnail(out Bitmap? thumbnail, out string? thumbnailUrl)
	{
		var info = this.GetWorkshopInfo();

		if (info is not null)
		{
			return info.GetThumbnail(out thumbnail, out thumbnailUrl);
		}

		thumbnail = null;
		thumbnailUrl = null;
		return false;
	}
#endif
}