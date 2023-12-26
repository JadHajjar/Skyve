using System.Collections.Generic;

namespace Skyve.Domain.Systems;
public interface IModLogicManager
{
	void Analyze(ILocalPackageData mod, IModUtil modUtil);
	void ApplyRequiredStates(IModUtil modUtil);
	bool AreMultipleSkyvesPresent(out List<ILocalPackageData> skyveInstances);
	bool IsForbidden(ILocalPackageData mod);
	bool IsPseudoMod(IPackage package);
	bool IsRequired(ILocalPackageData mod, IModUtil modUtil);
	void ModRemoved(ILocalPackageData mod);
}
