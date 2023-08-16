using Extensions.Sql;

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
	, ICustomPlayset
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

#if !API
	private Bitmap? _banner;
	public bool IsFavorite { get; set; }
	public bool IsMissingItems => false;
	public DateTime LastEditDate => DateUpdated;
	public DateTime LastUsed => DateUpdated;
	public PackageUsage Usage => (PackageUsage)(ProfileUsage ?? -1);
	public bool Temporary => false;
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
		set
		{
			if (value is null)
			{
				Banner = null;
			}
			else
			{
				Banner = (byte[])new ImageConverter().ConvertTo(value, typeof(byte[]));
			}
		}
	}
	IUser? IPlayset.Author { get; }
	string? IPlayset.BannerUrl { get; }
	DateTime IPlayset.DateUsed { get; }
	public IEnumerable<IPlaysetEntry> Entries => Contents ?? Enumerable.Empty<IPlaysetEntry>();
	public IEnumerable<IPackage> Packages => Contents?.Select(x => (IPackage)new PlaysetEntryPackage(x)) ?? Enumerable.Empty<IPackage>();
	bool ICustomPlayset.AutoSave { get; }
	bool ICustomPlayset.UnsavedChanges { get; }
	bool ICustomPlayset.DisableWorkshop { get; }

	bool ICustomPlayset.Save()
	{
		return false;
	}
#endif
}
