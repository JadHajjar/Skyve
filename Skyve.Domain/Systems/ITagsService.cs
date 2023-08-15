using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
public interface ITagsService
{
	IEnumerable<ITag> GetDistinctTags();
	IEnumerable<ITag> GetTags(IPackage package, bool ignoreParent = false);
	int GetTagUsage(ITag tag);
	bool HasAllTags(IPackage package, IEnumerable<ITag> tags);
	void SetTags(IPackage package, IEnumerable<string> value);
}
