﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IModLogicManager
{
	void Analyze(IPackage mod, IModUtil modUtil);
	void ApplyRequiredStates(IModUtil modUtil);
	bool AreMultipleSkyvesPresent(out List<IPackageIdentity> skyveInstances);
	void Clear();
	IEnumerable<IPackage> GetCollection(string key);
	bool IsForbidden(ILocalPackageIdentity? mod);
	bool IsPseudoMod(IPackage package);
	bool IsRequired(ILocalPackageIdentity? mod, IModUtil modUtil, int? playsetId = null);
	void ModRemoved(IPackage mod);
}
