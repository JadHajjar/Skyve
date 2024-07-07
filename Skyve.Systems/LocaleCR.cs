using Extensions;

namespace Skyve.Systems;
public class LocaleCR : LocaleHelper
{
	private static readonly LocaleCR _instance = new();

	public static void Load() { _ = _instance; }

	public static Translation Get(string value)
	{
		return _instance.GetText(value);
	}

	protected LocaleCR() : base($"Skyve.Systems.Properties.Compatibility.json") { }

	/// <summary>
	/// <para>You should disable '{1}'.</para>
	/// <para>Plural: You should disable the following packages.</para>
	/// </summary>
	public static Translation ActionExcludeOther => _instance.GetText("Action_ExcludeOther");

	/// <summary>
	/// You should disable '{0}'.
	/// </summary>
	public static Translation ActionExcludeThis => _instance.GetText("Action_ExcludeThis");

	/// <summary>
	/// <para>You should add the following package to your playset.</para>
	/// <para>Plural: You should add the following packages to your playset.</para>
	/// </summary>
	public static Translation ActionIncludeOther => _instance.GetText("Action_IncludeOther");

	/// <summary>
	/// You should include '{0}'
	/// </summary>
	public static Translation ActionIncludeThis => _instance.GetText("Action_IncludeThis");

	/// <summary>
	/// 
	/// </summary>
	public static Translation ActionNoAction => _instance.GetText("Action_NoAction");

	/// <summary>
	/// Please follow the following instructions:
	/// </summary>
	public static Translation ActionRequiresConfiguration => _instance.GetText("Action_RequiresConfiguration");

	/// <summary>
	/// <para>Between '{0}' and '{1}', you should choose only one of them.</para>
	/// <para>Plural: Between '{0}' and the following packages, you should only choose one.</para>
	/// </summary>
	public static Translation ActionSelectOne => _instance.GetText("Action_SelectOne");

	/// <summary>
	/// <para>You should add the following package to your playset.</para>
	/// <para>Plural: You should add the following packages to your playset.</para>
	/// </summary>
	public static Translation ActionSubscribeToPackages => _instance.GetText("Action_SubscribeToPackages");

	/// <summary>
	/// <para>You should switch to '{1}'.</para>
	/// <para>Plural: You should switch to one of the following packages.</para>
	/// </summary>
	public static Translation ActionSwitch => _instance.GetText("Action_Switch");

	/// <summary>
	/// <para>You should remove '{1}' from your playset.</para>
	/// <para>Plural: You should remove the following packages from your playset.</para>
	/// </summary>
	public static Translation ActionUnsubscribeOther => _instance.GetText("Action_UnsubscribeOther");

	/// <summary>
	/// You should remove '{0}' from your playset.
	/// </summary>
	public static Translation ActionUnsubscribeThis => _instance.GetText("Action_UnsubscribeThis");

	/// <summary>
	/// Add a global tag
	/// </summary>
	public static Translation AddGlobalTag => _instance.GetText("AddGlobalTag");

	/// <summary>
	/// Add Interaction
	/// </summary>
	public static Translation AddInteraction => _instance.GetText("AddInteraction");

	/// <summary>
	/// Please add a note describing what is wrong with this package.
	/// </summary>
	public static Translation AddMeaningfulNote => _instance.GetText("AddMeaningfulNote");

	/// <summary>
	/// Add Packages
	/// </summary>
	public static Translation AddPackages => _instance.GetText("AddPackages");

	/// <summary>
	/// Add Status
	/// </summary>
	public static Translation AddStatus => _instance.GetText("AddStatus");

	/// <summary>
	/// Alternative To ..
	/// </summary>
	public static Translation Alternative => _instance.GetText("Alternative");

	/// <summary>
	/// Apply all recommended actions
	/// </summary>
	public static Translation ApplyAllActions => _instance.GetText("ApplyAllActions");

	/// <summary>
	/// You have un-saved changes.  Would you like to apply those changes before continuing?
	/// </summary>
	public static Translation ApplyChangedBeforeExit => _instance.GetText("ApplyChangedBeforeExit");

	/// <summary>
	/// Apply &amp; Continue
	/// </summary>
	public static Translation ApplyContinue => _instance.GetText("ApplyContinue");

	/// <summary>
	/// Apply recommended action
	/// </summary>
	public static Translation ApplyRecommendedAction => _instance.GetText("ApplyRecommendedAction");

	/// <summary>
	/// Apply Requested Changes
	/// </summary>
	public static Translation ApplyRequestedChanges => _instance.GetText("ApplyRequestedChanges");

	/// <summary>
	/// Asset Creation
	/// </summary>
	public static Translation AssetCreation => _instance.GetText("AssetCreation");

	/// <summary>
	/// Attention Required
	/// </summary>
	public static Translation AttentionRequired => _instance.GetText("AttentionRequired");

	/// <summary>
	/// <para>{0} {1} requires your attention</para>
	/// <para>Plural: {0} {1} require your attention</para>
	/// </summary>
	public static Translation AttentionRequiredCount => _instance.GetText("AttentionRequiredCount");

	/// <summary>
	/// The author of {0}, {1}, is known to be malicious and/or for including harmful content like viruses in their packages.
	/// </summary>
	public static Translation AuthorMalicious => _instance.GetText("AuthorMalicious");

	/// <summary>
	/// The author of {0}, {1}, has retired or has been inactive for a long time.  Future updates &amp; stability for their packages is not guaranteed.
	/// </summary>
	public static Translation AuthorRetired => _instance.GetText("AuthorRetired");

	/// <summary>
	/// Banned
	/// </summary>
	public static Translation Banned => _instance.GetText("Banned");

	/// <summary>
	/// Black-list by ID
	/// </summary>
	public static Translation BlackListId => _instance.GetText("BlackListId");

	/// <summary>
	/// Black-list by name
	/// </summary>
	public static Translation BlackListName => _instance.GetText("BlackListName");

	/// <summary>
	/// Stable, potentially breaks on patch
	/// </summary>
	public static Translation BreaksOnPatch => _instance.GetText("BreaksOnPatch");

	/// <summary>
	/// Broken
	/// </summary>
	public static Translation Broken => _instance.GetText("Broken");

	/// <summary>
	/// Causes Issues
	/// </summary>
	public static Translation CausesIssues => _instance.GetText("CausesIssues");

	/// <summary>
	/// Causes Issues With ..
	/// </summary>
	public static Translation CausesIssuesWith => _instance.GetText("CausesIssuesWith");

	/// <summary>
	/// Caution
	/// </summary>
	public static Translation Caution => _instance.GetText("Caution");

	/// <summary>
	/// {0} {1} where caution is advised
	/// </summary>
	public static Translation CautionCount => _instance.GetText("CautionCount");

	/// <summary>
	/// City-Building
	/// </summary>
	public static Translation CityBuilding => _instance.GetText("CityBuilding");

	/// <summary>
	/// Are you sure you want to conclude your session?
	/// </summary>
	public static Translation ConfirmEndSession => _instance.GetText("ConfirmEndSession");

	/// <summary>
	/// Failed to load data, try again later
	/// </summary>
	public static Translation CrDataLoadFailed => _instance.GetText("CrDataLoadFailed");

	/// <summary>
	/// Alternatives
	/// </summary>
	public static Translation CRTAlternatives => _instance.GetText("CRT_Alternatives");

	/// <summary>
	/// Ambiguity
	/// </summary>
	public static Translation CRTAmbiguous => _instance.GetText("CRT_Ambiguous");

	/// <summary>
	/// Compatibility Issues
	/// </summary>
	public static Translation CRTCompatibility => _instance.GetText("CRT_Compatibility");

	/// <summary>
	/// Missing DLCs/CCPs
	/// </summary>
	public static Translation CRTDlcMissing => _instance.GetText("CRT_DlcMissing");

	/// <summary>
	/// Optional Packages
	/// </summary>
	public static Translation CRTOptionalPackages => _instance.GetText("CRT_OptionalPackages");

	/// <summary>
	/// Review Request Status
	/// </summary>
	public static Translation CRTRequestReview => _instance.GetText("CRT_RequestReview");

	/// <summary>
	/// Required Item
	/// </summary>
	public static Translation CRTRequiredItem => _instance.GetText("CRT_RequiredItem");

	/// <summary>
	/// Required Packages
	/// </summary>
	public static Translation CRTRequiredPackages => _instance.GetText("CRT_RequiredPackages");

	/// <summary>
	/// Stability
	/// </summary>
	public static Translation CRTStability => _instance.GetText("CRT_Stability");

	/// <summary>
	/// Statuses
	/// </summary>
	public static Translation CRTStatus => _instance.GetText("CRT_Status");

	/// <summary>
	/// Successors
	/// </summary>
	public static Translation CRTSuccessors => _instance.GetText("CRT_Successors");

	/// <summary>
	/// <para>Delete this request</para>
	/// <para>Plural: Delete these requests</para>
	/// </summary>
	public static Translation DeleteRequests => _instance.GetText("DeleteRequests");

	/// <summary>
	/// Dependency Mod
	/// </summary>
	public static Translation DependencyMod => _instance.GetText("DependencyMod");

	/// <summary>
	/// Deprecated
	/// </summary>
	public static Translation Deprecated => _instance.GetText("Deprecated");

	/// <summary>
	/// <para>{0} DLC selected</para>
	/// <para>Plural: {0} DLCs selected</para>
	/// </summary>
	public static Translation DlcsSelected => _instance.GetText("DlcsSelected");

	/// <summary>
	/// Donate
	/// </summary>
	public static Translation Donation => _instance.GetText("Donation");

	/// <summary>
	/// Edit History
	/// </summary>
	public static Translation EditHistory => _instance.GetText("EditHistory");

	/// <summary>
	/// Edit Note
	/// </summary>
	public static Translation EditNote => _instance.GetText("EditNote");

	/// <summary>
	/// Leave a note for other managers to see in the future
	/// </summary>
	public static Translation EditNoteInfo => _instance.GetText("EditNoteInfo");

	/// <summary>
	/// Disable
	/// </summary>
	public static Translation Exclude => _instance.GetText("Exclude");

	/// <summary>
	/// <para>{0} {1} should be excluded</para>
	/// <para>Plural: {0} {1} should be excluded</para>
	/// </summary>
	public static Translation ExcludeCount => _instance.GetText("ExcludeCount");

	/// <summary>
	/// Disable this package
	/// </summary>
	public static Translation ExcludeThis => _instance.GetText("ExcludeThis");

	/// <summary>
	/// Switch to this package
	/// </summary>
	public static Translation FlippedSwitch => _instance.GetText("FlippedSwitch");

	/// <summary>
	/// Generic Package
	/// </summary>
	public static Translation GenericPackage => _instance.GetText("GenericPackage");

	/// <summary>
	/// Tags all players will see in Skyve
	/// </summary>
	public static Translation GlobalTagsInfo => _instance.GetText("GlobalTagsInfo");

	/// <summary>
	/// Has issues
	/// </summary>
	public static Translation HasIssues => _instance.GetText("HasIssues");

	/// <summary>
	/// Has issues, no more updates
	/// </summary>
	public static Translation HasIssuesNoFutureUpdates => _instance.GetText("HasIssuesNoFutureUpdates");

	/// <summary>
	/// Hide reviewed mods
	/// </summary>
	public static Translation HideReviewedPackages => _instance.GetText("HideReviewedPackages");

	/// <summary>
	/// IMT Markings
	/// </summary>
	public static Translation IMTMarkings => _instance.GetText("IMTMarkings");

	/// <summary>
	/// Incompatible
	/// </summary>
	public static Translation Incompatible => _instance.GetText("Incompatible");

	/// <summary>
	/// Incompatible With ..
	/// </summary>
	public static Translation IncompatibleWith => _instance.GetText("IncompatibleWith");

	/// <summary>
	/// Incomplete Description
	/// </summary>
	public static Translation IncompleteDescription => _instance.GetText("IncompleteDescription");

	/// <summary>
	/// <para>'{1}' can be used as an alternative to '{0}'.</para>
	/// <para>Plural: Any one of the following packages can be used as an alternative to '{0}'.</para>
	/// </summary>
	public static Translation InteractionAlternative => _instance.GetText("Interaction_Alternative");

	/// <summary>
	/// <para>'{0}' can cause issues when used with '{1}'.</para>
	/// <para>Plural: '{0}' can cause issues when used with the following mods.</para>
	/// </summary>
	public static Translation InteractionCausesIssuesWith => _instance.GetText("Interaction_CausesIssuesWith");

	/// <summary>
	/// The following are multiple versions of the same mod. Using them at the same time will cause conflicts.
	/// </summary>
	public static Translation InteractionIdentical => _instance.GetText("Interaction_Identical");

	/// <summary>
	/// <para>'{0}' and '{1}' are not compatible with each other.</para>
	/// <para>Plural: '{0}' is not compatible with any of the following packages.</para>
	/// </summary>
	public static Translation InteractionIncompatibleWith => _instance.GetText("Interaction_IncompatibleWith");

	/// <summary>
	/// <para>'{0}' will load directly after '{1}'.</para>
	/// <para>Plural: '{0}' will load directly after the following packages.</para>
	/// </summary>
	public static Translation InteractionLoadAfter => _instance.GetText("Interaction_LoadAfter");

	/// <summary>
	/// <para>You're missing an optional package for '{0}'.</para>
	/// <para>Plural: You're missing the following optional packages for '{0}'.</para>
	/// </summary>
	public static Translation InteractionOptionalPackages => _instance.GetText("Interaction_OptionalPackages");

	/// <summary>
	/// <para>'{0}' is required for '{1}'.</para>
	/// <para>Plural: '{0}' is required for the following packages.</para>
	/// </summary>
	public static Translation InteractionRequiredItem => _instance.GetText("Interaction_RequiredItem");

	/// <summary>
	/// <para>You're missing a required package for '{0}'.</para>
	/// <para>Plural: You're missing the following required packages for '{0}'.</para>
	/// </summary>
	public static Translation InteractionRequiredPackages => _instance.GetText("Interaction_RequiredPackages");

	/// <summary>
	/// <para>'{0}' and '{1}' have the same functionalities.</para>
	/// <para>Plural: '{0}' and the following packages have the same functionalities.</para>
	/// </summary>
	public static Translation InteractionSameFunctionality => _instance.GetText("Interaction_SameFunctionality");

	/// <summary>
	/// <para>'{0}' is succeeded by '{1}'.</para>
	/// <para>Plural: '{0}' is succeeded by either one of the following packages.</para>
	/// </summary>
	public static Translation InteractionSucceededBy => _instance.GetText("Interaction_SucceededBy");

	/// <summary>
	/// Interactions ({0})
	/// </summary>
	public static Translation InteractionCount => _instance.GetText("InteractionCount");

	/// <summary>
	/// Interaction Type
	/// </summary>
	public static Translation InteractionType => _instance.GetText("InteractionType");

	/// <summary>
	/// It was last reviewed on {0}
	/// </summary>
	public static Translation LastReviewDate => _instance.GetText("LastReviewDate");

	/// <summary>
	/// Linked Packages
	/// </summary>
	public static Translation LinkedPackages => _instance.GetText("LinkedPackages");

	/// <summary>
	/// Links
	/// </summary>
	public static Translation Links => _instance.GetText("Links");

	/// <summary>
	/// Load After ..
	/// </summary>
	public static Translation LoadAfter => _instance.GetText("LoadAfter");

	/// <summary>
	/// Compatibility Management
	/// </summary>
	public static Translation ManageCompatibilityData => _instance.GetText("ManageCompatibilityData");

	/// <summary>
	/// Manage Package
	/// </summary>
	public static Translation ManagePackage => _instance.GetText("ManagePackage");

	/// <summary>
	/// Map Creation
	/// </summary>
	public static Translation MapCreation => _instance.GetText("MapCreation");

	/// <summary>
	/// Missing Dependency
	/// </summary>
	public static Translation MissingDependency => _instance.GetText("MissingDependency");

	/// <summary>
	/// <para>{0} {1} is missing dependencies</para>
	/// <para>Plural: {0} {1} are missing dependencies</para>
	/// </summary>
	public static Translation MissingDependencyCount => _instance.GetText("MissingDependencyCount");

	/// <summary>
	/// Included music may be copyrighted
	/// </summary>
	public static Translation MusicCanBeCopyrighted => _instance.GetText("MusicCanBeCopyrighted");

	/// <summary>
	/// Music Pack
	/// </summary>
	public static Translation MusicPack => _instance.GetText("MusicPack");

	/// <summary>
	/// Name-List/Game Translations
	/// </summary>
	public static Translation NameList => _instance.GetText("NameList");

	/// <summary>
	/// Do Nothing
	/// </summary>
	public static Translation NoAction => _instance.GetText("NoAction");

	/// <summary>
	/// No Issues
	/// </summary>
	public static Translation NoIssues => _instance.GetText("NoIssues");

	/// <summary>
	/// No Links
	/// </summary>
	public static Translation NoLinks => _instance.GetText("NoLinks");

	/// <summary>
	/// No Required DLCs
	/// </summary>
	public static Translation NoRequiredDlcs => _instance.GetText("NoRequiredDlcs");

	/// <summary>
	/// No Tags
	/// </summary>
	public static Translation NoTags => _instance.GetText("NoTags");

	/// <summary>
	/// Note
	/// </summary>
	public static Translation Note => _instance.GetText("Note");

	/// <summary>
	/// Additional information to be displayed for the mod
	/// </summary>
	public static Translation NoteInfo => _instance.GetText("NoteInfo");

	/// <summary>
	/// Not enough information
	/// </summary>
	public static Translation NotEnoughInformation => _instance.GetText("NotEnoughInformation");

	/// <summary>
	/// Not reviewed yet
	/// </summary>
	public static Translation NotReviewed => _instance.GetText("NotReviewed");

	/// <summary>
	/// <para>This package has one other compatibility warning</para>
	/// <para>Plural: This package has {0} other compatibility warnings</para>
	/// </summary>
	public static Translation OtherCompatibilityWarnings => _instance.GetText("OtherCompatibilityWarnings");

	/// <summary>
	/// Expected Output
	/// </summary>
	public static Translation OutputText => _instance.GetText("OutputText");

	/// <summary>
	/// Package Type
	/// </summary>
	public static Translation PackageType => _instance.GetText("PackageType");

	/// <summary>
	/// Please review the interactions of this package before saving. Some have an invalid Interaction Type or don't have a linked package.
	/// </summary>
	public static Translation PleaseReviewPackageInteractions => _instance.GetText("PleaseReviewPackageInteractions");

	/// <summary>
	/// Please review the statuses of this package before saving. Some have an invalid Status Type.
	/// </summary>
	public static Translation PleaseReviewPackageStatuses => _instance.GetText("PleaseReviewPackageStatuses");

	/// <summary>
	/// Please select a valid usage for this package.
	/// </summary>
	public static Translation PleaseReviewPackageUsage => _instance.GetText("PleaseReviewPackageUsage");

	/// <summary>
	/// Please review the stability of this package before saving.
	/// </summary>
	public static Translation PleaseReviewTheStability => _instance.GetText("PleaseReviewTheStability");

	/// <summary>
	/// Procedural Object Font/Module
	/// </summary>
	public static Translation POFont => _instance.GetText("POFont");

	/// <summary>
	/// Previous
	/// </summary>
	public static Translation Previous => _instance.GetText("Previous");

	/// <summary>
	/// Render it! Preset
	/// </summary>
	public static Translation RenderItPreset => _instance.GetText("RenderItPreset");

	/// <summary>
	/// Request a review
	/// </summary>
	public static Translation RequestReview => _instance.GetText("RequestReview");

	/// <summary>
	/// *Your account's ID and logs are sent with your request
	/// </summary>
	public static Translation RequestReviewDisclaimer => _instance.GetText("RequestReviewDisclaimer");

	/// <summary>
	/// Are you experiencing issues with this package?
	/// </summary>
	public static Translation RequestReviewInfo => _instance.GetText("RequestReviewInfo");

	/// <summary>
	/// Send a new request to be reviewed
	/// </summary>
	public static Translation RequestReviewUpdate => _instance.GetText("RequestReviewUpdate");

	/// <summary>
	/// Required DLCs / CCPs
	/// </summary>
	public static Translation RequiredDLCs => _instance.GetText("RequiredDLCs");

	/// <summary>
	/// Required Item
	/// </summary>
	public static Translation RequiredItem => _instance.GetText("RequiredItem");

	/// <summary>
	/// <para>{0} {1} that is required but is not enabled</para>
	/// <para>Plural: {0} {1} that are required but are not enabled</para>
	/// </summary>
	public static Translation RequiredItemCount => _instance.GetText("RequiredItemCount");

	/// <summary>
	/// Requires ..
	/// </summary>
	public static Translation RequiredPackages => _instance.GetText("RequiredPackages");

	/// <summary>
	/// Requirement Alternative To ..
	/// </summary>
	public static Translation RequirementAlternative => _instance.GetText("RequirementAlternative");

	/// <summary>
	/// Requires a specific configuration
	/// </summary>
	public static Translation RequiresConfiguration => _instance.GetText("RequiresConfiguration");

	/// <summary>
	/// Re-Upload
	/// </summary>
	public static Translation Reupload => _instance.GetText("Reupload");

	/// <summary>
	/// Re-use Previous Data
	/// </summary>
	public static Translation ReuseData => _instance.GetText("ReuseData");

	/// <summary>
	/// Re-uses the compatibility data from your last saved package for this one. Hold Shift to also apply &amp; continue
	/// </summary>
	public static Translation ReuseDataTip => _instance.GetText("ReuseData_Tip");

	/// <summary>
	/// Review Requests
	/// </summary>
	public static Translation ReviewRequests => _instance.GetText("ReviewRequests");

	/// <summary>
	/// Same Functionality As ..
	/// </summary>
	public static Translation SameFunctionality => _instance.GetText("SameFunctionality");

	/// <summary>
	/// Saves can't load without it
	/// </summary>
	public static Translation SavesCantLoadWithoutIt => _instance.GetText("SavesCantLoadWithoutIt");

	/// <summary>
	/// Scenario-Making
	/// </summary>
	public static Translation ScenarioMaking => _instance.GetText("ScenarioMaking");

	/// <summary>
	/// Select either one
	/// </summary>
	public static Translation SelectOne => _instance.GetText("SelectOne");

	/// <summary>
	/// Skip
	/// </summary>
	public static Translation Skip => _instance.GetText("Skip");

	/// <summary>
	/// Snooze all issues
	/// </summary>
	public static Translation SnoozeAll => _instance.GetText("SnoozeAll");

	/// <summary>
	/// Snooze all filtered issues
	/// </summary>
	public static Translation SnoozeAllFiltered => _instance.GetText("SnoozeAllFiltered");

	/// <summary>
	/// Source code is available
	/// </summary>
	public static Translation SourceAvailable => _instance.GetText("SourceAvailable");

	/// <summary>
	/// Source-code is not available
	/// </summary>
	public static Translation SourceCodeNotAvailable => _instance.GetText("SourceCodeNotAvailable");

	/// <summary>
	/// Stability
	/// </summary>
	public static Translation Stability => _instance.GetText("Stability");

	/// <summary>
	/// '{0}' is marked as not compatible with your current game version. Assets rarely break with new updates, so you should be okay using it.
	/// </summary>
	public static Translation StabilityAssetIncompatible => _instance.GetText("Stability_AssetIncompatible");

	/// <summary>
	/// '{0}' has not been reviewed yet. The compatibility info may not be accurate; though assets are generally stable.
	/// </summary>
	public static Translation StabilityAssetNotReviewed => _instance.GetText("Stability_AssetNotReviewed");

	/// <summary>
	/// '{0}' is very likely to break when the new patch is released.
	/// </summary>
	public static Translation StabilityBreaksOnPatch => _instance.GetText("Stability_BreaksOnPatch");

	/// <summary>
	/// '{0}' is broken. It is known to either break the game or just does not work.
	/// </summary>
	public static Translation StabilityBroken => _instance.GetText("Stability_Broken");

	/// <summary>
	/// '{1}' broke after patch {0} of the game. Please wait until it is updated before using it. In the meantime, you should disable it.
	/// </summary>
	public static Translation StabilityBrokenFromPatch => _instance.GetText("Stability_BrokenFromPatch");

	/// <summary>
	/// '{1}' broke after patch {0} of the game. Please wait until it is updated before using it.
	/// </summary>
	public static Translation StabilityBrokenFromPatchSafe => _instance.GetText("Stability_BrokenFromPatchSafe");

	/// <summary>
	/// '{1}' broke after patch {0} of the game. But was recently updated, it might be safe to use.
	/// </summary>
	public static Translation StabilityBrokenFromPatchUpdated => _instance.GetText("Stability_BrokenFromPatchUpdated");

	/// <summary>
	/// '{0}' can cause issues. Be careful while using it.
	/// </summary>
	public static Translation StabilityHasIssues => _instance.GetText("Stability_HasIssues");

	/// <summary>
	/// '{0}' can cause issues, and the author is no longer supporting it, future updates or bug fixes are unlikely.
	/// </summary>
	public static Translation StabilityHasIssuesNoFutureUpdates => _instance.GetText("Stability_HasIssuesNoFutureUpdates");

	/// <summary>
	/// '{0}' is marked as not compatible with your current game version. Use it cautiously, or disable it while waiting for it to be updated.
	/// </summary>
	public static Translation StabilityIncompatible => _instance.GetText("Stability_Incompatible");

	/// <summary>
	/// This is a local version of its original mod '{0}'.  Local versions are usually used for testing purposes.
	/// </summary>
	public static Translation StabilityLocal => _instance.GetText("Stability_Local");

	/// <summary>
	/// There is not enough information to properly review '{0}'.
	/// </summary>
	public static Translation StabilityNotEnoughInformation => _instance.GetText("Stability_NotEnoughInformation");

	/// <summary>
	/// '{0}' has not been reviewed yet.  The compatibility info may not be accurate.
	/// </summary>
	public static Translation StabilityNotReviewed => _instance.GetText("Stability_NotReviewed");

	/// <summary>
	/// '{0}' is stable. It's author is still actively working on it and updating it.
	/// </summary>
	public static Translation StabilityStable => _instance.GetText("Stability_Stable");

	/// <summary>
	/// '{0}' is stable, though the author is no longer supporting it, future updates or bug fixes are unlikely.
	/// </summary>
	public static Translation StabilityStableNoFutureUpdates => _instance.GetText("Stability_StableNoFutureUpdates");

	/// <summary>
	/// '{0}' is stable, though the author will no longer release feature updates. They will however still update it in case of new bugs.
	/// </summary>
	public static Translation StabilityStableNoNewFeatures => _instance.GetText("Stability_StableNoNewFeatures");

	/// <summary>
	/// Stable
	/// </summary>
	public static Translation Stable => _instance.GetText("Stable");

	/// <summary>
	/// Stable, no more updates
	/// </summary>
	public static Translation StableNoFutureUpdates => _instance.GetText("StableNoFutureUpdates");

	/// <summary>
	/// Stable, only bug-fix updates
	/// </summary>
	public static Translation StableNoNewFeatures => _instance.GetText("StableNoNewFeatures");

	/// <summary>
	/// Standard Mod
	/// </summary>
	public static Translation StandardMod => _instance.GetText("StandardMod");

	/// <summary>
	/// '{0}' has not been updated in a long time.
	/// </summary>
	public static Translation StatusAutoDeprecated => _instance.GetText("Status_AutoDeprecated");

	/// <summary>
	/// '{0}' is known to cause issues with the game.
	/// </summary>
	public static Translation StatusCausesIssues => _instance.GetText("Status_CausesIssues");

	/// <summary>
	/// This is a dependency mod, but it does not look like you are using any package that requires it.
	/// </summary>
	public static Translation StatusDependencyMod => _instance.GetText("Status_DependencyMod");

	/// <summary>
	/// '{0}' is deprecated, it is no longer supported by its author.
	/// </summary>
	public static Translation StatusDeprecated => _instance.GetText("Status_Deprecated");

	/// <summary>
	/// The Workshop description of this mod is too short. You should not use it unless you trust its author.
	/// </summary>
	public static Translation StatusIncompleteDescription => _instance.GetText("Status_IncompleteDescription");

	/// <summary>
	/// <para>You're missing a DLC to use this package.</para>
	/// <para>Plural: You're missing the following DLCs to use this package.</para>
	/// </summary>
	public static Translation StatusMissingDlc => _instance.GetText("Status_MissingDlc");

	/// <summary>
	/// The music inside this pack might be copyrighted. It's best not to use it while streaming.
	/// </summary>
	public static Translation StatusMusicCanBeCopyrighted => _instance.GetText("Status_MusicCanBeCopyrighted");

	/// <summary>
	/// <para>'{0}' is a re-upload of '{1}'.</para>
	/// <para>Zero: '{0}' is a re-upload of an original package.</para>
	/// </summary>
	public static Translation StatusReupload => _instance.GetText("Status_Reupload");

	/// <summary>
	/// <para>After saving your city with '{0}' enabled, you will no longer be able to load that save without '{0}' enabled.  Use '{1}' to recover your save if needed. Read its Workshop description for more info.</para>
	/// <para>Zero: After saving your city with '{0}' enabled, you will no longer be able to load that save without '{0}' enabled.</para>
	/// </summary>
	public static Translation StatusSavesCantLoadWithoutIt => _instance.GetText("Status_SavesCantLoadWithoutIt");

	/// <summary>
	/// This package will not warn about missing source code.
	/// </summary>
	public static Translation StatusSourceAvailable => _instance.GetText("Status_SourceAvailable");

	/// <summary>
	/// The source code of this mod is not available. You should not use it unless you trust its author.
	/// </summary>
	public static Translation StatusSourceCodeNotAvailable => _instance.GetText("Status_SourceCodeNotAvailable");

	/// <summary>
	/// This package will be ignored while processing the mods to be troubleshooted.
	/// </summary>
	public static Translation StatusStandardMod => _instance.GetText("Status_StandardMod");

	/// <summary>
	/// '{0}' is succeeded by '{1}', and you are already using '{1}'. There is no need for '{0}' anymore.
	/// </summary>
	public static Translation StatusSucceeded => _instance.GetText("Status_Succeeded");

	/// <summary>
	/// <para>This is a 'Test' version of '{1}'. Be aware that updates to it might cause unexpected issues.</para>
	/// <para>Zero: This is a 'Test' version, be aware that test versions of mods can cause issues and that updates to it might cause unexpected errors.</para>
	/// </summary>
	public static Translation StatusTestVersion => _instance.GetText("Status_TestVersion");

	/// <summary>
	/// Statuses ({0})
	/// </summary>
	public static Translation StatusesCount => _instance.GetText("StatusesCount");

	/// <summary>
	/// Status Type
	/// </summary>
	public static Translation StatusType => _instance.GetText("StatusType");

	/// <summary>
	/// Subscribe to linked packages
	/// </summary>
	public static Translation SubscribeToPackages => _instance.GetText("SubscribeToPackages");

	/// <summary>
	/// Successor of ..
	/// </summary>
	public static Translation Successor => _instance.GetText("Successor");

	/// <summary>
	/// Switch to linked package
	/// </summary>
	public static Translation Switch => _instance.GetText("Switch");

	/// <summary>
	/// <para>{0} {1} requires switching to another</para>
	/// <para>Plural: {0} {1} require switching to another</para>
	/// </summary>
	public static Translation SwitchCount => _instance.GetText("SwitchCount");

	/// <summary>
	/// Test Version
	/// </summary>
	public static Translation TestVersion => _instance.GetText("TestVersion");

	/// <summary>
	/// Theme-Making
	/// </summary>
	public static Translation ThemeMaking => _instance.GetText("ThemeMaking");

	/// <summary>
	/// Theme Mix
	/// </summary>
	public static Translation ThemeMix => _instance.GetText("ThemeMix");

	/// <summary>
	/// Unsubscribe
	/// </summary>
	public static Translation Unsubscribe => _instance.GetText("Unsubscribe");

	/// <summary>
	/// {0} {1} that you should unsubscribe from
	/// </summary>
	public static Translation UnsubscribeCount => _instance.GetText("UnsubscribeCount");

	/// <summary>
	/// Unsubscribe from linked packages
	/// </summary>
	public static Translation UnsubscribeOther => _instance.GetText("UnsubscribeOther");

	/// <summary>
	/// Unsubscribe from this package
	/// </summary>
	public static Translation UnsubscribeThis => _instance.GetText("UnsubscribeThis");

	/// <summary>
	/// Usage
	/// </summary>
	public static Translation Usage => _instance.GetText("Usage");

	/// <summary>
	/// View Request
	/// </summary>
	public static Translation ViewRequest => _instance.GetText("ViewRequest");

	/// <summary>
	/// Warning
	/// </summary>
	public static Translation Warning => _instance.GetText("Warning");

	/// <summary>
	/// {0} {1} with a warning
	/// </summary>
	public static Translation WarningCount => _instance.GetText("WarningCount");

	/// <summary>
	/// Website
	/// </summary>
	public static Translation Website => _instance.GetText("Website");

	/// <summary>
	/// Manage your mods
	/// </summary>
	public static Translation YourPackages => _instance.GetText("YourPackages");
}
