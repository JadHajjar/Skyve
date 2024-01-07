using Extensions.Sql;

#if !API
using Newtonsoft.Json;
#endif

using Skyve.Domain;
using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Skyve.Systems.Compatibility.Domain.Api;

[DynamicSqlClass("UserProfiles")]
public class UserProfile : IDynamicSql
#if !API
	, IOnlinePlayset
#endif
{
	[DynamicSqlProperty(PrimaryKey = true, Identity = true)]
	public int ProfileId { get; set; }
	[DynamicSqlProperty(Indexer = true, ColumnName = "AuthorId")]
	public ulong Author { get; set; }
	[DynamicSqlProperty]
	public string? Name { get; set; }
	[DynamicSqlProperty]
	public int ModCount { get; set; }
	[DynamicSqlProperty]
	public int AssetCount { get; set; }
	[DynamicSqlProperty]
	public DateTime DateCreated { get; set; }
	[DynamicSqlProperty]
	public DateTime DateUpdated { get; set; }
	[DynamicSqlProperty]
	public bool Public { get; set; }
	[DynamicSqlProperty]
	public byte[]? Banner { get; set; }
	[DynamicSqlProperty]
	public int? Color { get; set; }
	[DynamicSqlProperty]
	public int Downloads { get; set; }
	[DynamicSqlProperty(ColumnName = "Usage")]
	public int? ProfileUsage { get; set; }

	public UserProfileContent[]? Contents { get; set; }
	public int Id { get; set; }

#if !API
	private Bitmap? _banner;
	[JsonIgnore] public bool IsFavorite { get; set; }
	[JsonIgnore] public bool IsMissingItems => false;
	[JsonIgnore] public DateTime LastEditDate => DateUpdated;
	[JsonIgnore] public DateTime LastUsed => DateUpdated;
	[JsonIgnore] public PackageUsage Usage => (PackageUsage)(ProfileUsage ?? -1);
	[JsonIgnore] public bool Temporary => false;
	Color? ICustomPlayset.Color { get => Color == null ? null : System.Drawing.Color.FromArgb(Color.Value); set { } }
	Bitmap? ICustomPlayset.Banner
	{
		get
		{
			if (_banner is not null)
			{
				return _banner;
			}

			if (Banner is null || Banner.Length == 0)
			{
				return null;
			}

			using var ms = new MemoryStream(Banner);

			return _banner = new Bitmap(ms);
		}
		set => Banner = value is null ? null : (byte[])new ImageConverter().ConvertTo(value, typeof(byte[]));
	}

	IUser? IPlayset.Author => ServiceCenter.Get<Skyve.Domain.Systems.IWorkshopService>().GetUser(Author);
	string? IPlayset.BannerUrl { get; }
	DateTime IPlayset.DateUsed { get; }
	[JsonIgnore] public IEnumerable<IPlaysetEntry> Entries => Contents ?? Enumerable.Empty<IPlaysetEntry>();
	//[JsonIgnore] public IEnumerable<IPackage> Packages => Contents?.Select(x => (IPackage)new PlaysetEntryPackage(x)) ?? Enumerable.Empty<IPackage>();
	bool ICustomPlayset.AutoSave { get; }
	bool ICustomPlayset.UnsavedChanges { get; }
	bool ICustomPlayset.DisableWorkshop { get; }

	bool ICustomPlayset.Save()
	{
		return false;
	}
#endif
}
