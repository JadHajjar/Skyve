using Extensions.Sql;

namespace SkyveApp.Systems.Compatibility.Domain.Api;

[DynamicSqlClass("Packages")]
public class PostPackage : CompatibilityPackageData
{
	public Author? Author { get; set; }
	public bool BlackListId { get; set; }
	public bool BlackListName { get; set; }
}
