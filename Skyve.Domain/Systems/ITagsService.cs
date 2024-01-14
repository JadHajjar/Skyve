﻿using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface ITagsService
{
	IEnumerable<ITag> GetDistinctTags();
	IEnumerable<ITag> GetTags(IPackageIdentity package, bool ignoreParent = false);
	int GetTagUsage(ITag tag);
	bool HasAllTags(IPackageIdentity package, IEnumerable<ITag> tags);
	void SetTags(IPackageIdentity package, IEnumerable<string> value);
	ITag CreateWorkshopTag(string text);
	ITag CreateGlobalTag(string text);
	ITag CreateCustomTag(string text);
	ITag CreateIdTag(string text);
}
