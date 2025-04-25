using Skyve.Domain.Enums;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface ITroubleshootSystem
{
	int CurrentStage { get; }
	bool IsInProgress { get; }
	bool IsBusy { get; }
	int TotalStages { get; }
	string CurrentAction { get; }
	bool WaitingForGameLaunch { get; }
	bool WaitingForGameClose { get; }
	bool WaitingForPrompt { get; }

	event Action? StageChanged;
	event Action? AskForConfirmation;
	event Action<IEnumerable<ILocalPackageIdentity>>? PromptResult;

	void GoToNextStage();
	Task<TroubleshootResult> Start(ITroubleshootSettings settings);
	Task<TroubleshootResult> ApplyConfirmation(bool issuePersists);
	Task<TroubleshootResult> Stop(bool keepSettings);
}
