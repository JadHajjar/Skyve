using Extensions.Sql;

using Skyve.Domain;
using Skyve.Domain.Enums;

namespace Skyve.Systems.Compatibility.Domain.Api;

[DynamicSqlClass("PackageLinks")]
#if API
public class PackageLink : IDynamicSql
{
	[DynamicSqlProperty(Indexer = true), System.Text.Json.Serialization.JsonIgnore]
	public ulong PackageId { get; set; }
#else
public struct PackageLink : IDynamicSql, ILink
{
#endif

	[DynamicSqlProperty]
	public LinkType Type { get; set; }

	[DynamicSqlProperty]
	public string? Url { get; set; }

	[DynamicSqlProperty]
	public string? Title { get; set; }
}
