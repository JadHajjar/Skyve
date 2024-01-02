using Extensions.Sql;

namespace Skyve.Systems.Compatibility.Domain.Api;

[DynamicSqlClass("Authors")]
public class Author : IDynamicSql
{
	[DynamicSqlProperty(PrimaryKey = true)]
	public ulong Id { get; set; }
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
