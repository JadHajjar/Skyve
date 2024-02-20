using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IModLogicManager
{
	void Analyze(IPackage mod, IModUtil modUtil);
	void ApplyRequiredStates(IModUtil modUtil);
	bool AreMultipleSkyvesPresent(out List<IPackageIdentity> skyveInstances);
	IEnumerable<IPackage> GetCollection(string key);
	bool IsForbidden(ILocalPackageIdentity? mod);
	bool IsPseudoMod(IPackage package);
	bool IsRequired(ILocalPackageIdentity? mod, IModUtil modUtil);
	void ModRemoved(IPackage mod);
}
