﻿using Extensions.Sql;

namespace SkyveApp.Systems.Compatibility.Domain.Api;

[DynamicSqlClass("Authors")]
public class Author : IDynamicSql
{
	[DynamicSqlProperty(PrimaryKey = true)]
	public ulong SteamId { get; set; }
	[DynamicSqlProperty]
	public string? Name { get; set; }
	[DynamicSqlProperty]
	public bool Retired { get; set; }
	[DynamicSqlProperty]
	public bool Verified { get; set; }
	[DynamicSqlProperty]
	public bool Malicious { get; set; }
#if !API
	public bool Manager { get; set; }
#endif
}
