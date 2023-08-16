namespace Skyve.Domain.Systems;
public interface IModLogicManager
{
	void Analyze(IMod mod, IModUtil modUtil);
	void ApplyRequiredStates(IModUtil modUtil);
	bool AreMultipleSkyvesPresent();
	bool IsForbidden(IMod mod);
	bool IsPseudoMod(IPackage package);
	bool IsRequired(IMod mod, IModUtil modUtil);
	void ModRemoved(IMod mod);
}
