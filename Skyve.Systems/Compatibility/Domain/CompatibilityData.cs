using Skyve.Systems.Compatibility.Domain.Api;

using System.Collections.Generic;

namespace Skyve.Systems.Compatibility.Domain;
public class CompatibilityData
{
	public List<CompatibilityPackageData>? Packages { get; set; }
	public List<Author>? Authors { get; set; }
	public List<ulong>? BlackListedIds { get; set; }
	public List<string>? BlackListedNames { get; set; }
}
