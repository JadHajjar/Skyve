using System;
using System.Collections.Generic;

namespace SkyveApp.Domain.Systems;
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
	event Action<List<ILocalPackage>>? PromptResult;

	void ApplyConfirmation(bool issuePersists);
	void NextStage();
	void Start(ITroubleshootSettings settings);
	void Stop(bool keepSettings);
}
