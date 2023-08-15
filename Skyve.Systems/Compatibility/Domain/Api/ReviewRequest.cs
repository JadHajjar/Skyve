using Extensions.Sql;

using System;

namespace SkyveApp.Systems.Compatibility.Domain.Api;
[DynamicSqlClass("ReviewRequests")]
public class ReviewRequest : ReviewRequestNoLog
{
	[DynamicSqlProperty]
	public byte[]? LogFile { get; set; }
}

[DynamicSqlClass("ReviewRequests")]
public class ReviewRequestNoLog : IDynamicSql
{
	[DynamicSqlProperty(PrimaryKey = true, Indexer = true)]
	public ulong PackageId { get; set; }
	[DynamicSqlProperty(PrimaryKey = true)]
	public ulong UserId { get; set; }
	[DynamicSqlProperty]
	public int PackageStability { get; set; }
	[DynamicSqlProperty]
	public int PackageUsage { get; set; }
	[DynamicSqlProperty]
	public int PackageType { get; set; }
	[DynamicSqlProperty]
	public int StatusType { get; set; }
	[DynamicSqlProperty]
	public int StatusAction { get; set; }
	[DynamicSqlProperty]
	public string? RequiredDLCs { get; set; }
	[DynamicSqlProperty]
	public string? StatusPackages { get; set; }
	[DynamicSqlProperty]
	public string? StatusNote { get; set; }
	[DynamicSqlProperty]
	public string? PackageNote { get; set; }
	[DynamicSqlProperty]
	public bool IsInteraction { get; set; }
	[DynamicSqlProperty]
	public bool IsStatus { get; set; }
	[DynamicSqlProperty]
	public DateTime Timestamp { get; set; }
}
