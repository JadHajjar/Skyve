using Extensions.Sql;

using Skyve.Domain.Enums;

using System;

namespace Skyve.Domain;
public class AuthEntry : IDynamicSql
{
	[DynamicSqlProperty(PrimaryKey = true)]
	public Guid Guid { get; set; }
	[DynamicSqlProperty]
	public AuthType Type { get; set; }
	[DynamicSqlProperty]
	public string? Value { get; set; }
}
