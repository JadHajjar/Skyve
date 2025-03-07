using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface ITroubleshootSystem
{
	int CurrentStage { get; }
	bool IsInProgress { get; }
	int TotalStages { get; }
	string CurrentAction { get; }
	bool WaitingForGameLaunch { get; }
	bool WaitingForGameClose { get; }
	bool WaitingForPrompt { get; }

	event Action? StageChanged;
	event Action? AskForConfirmation;
	event Action<IEnumerable<ILocalPackageIdentity>>? PromptResult;

	void GoToNextStage();
	Task<bool> Start(ITroubleshootSettings settings);
	Task<bool> ApplyConfirmation(bool issuePersists);
	Task<bool> Stop(bool keepSettings);
}
