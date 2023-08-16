using Extensions;

namespace Skyve.Systems;
public class LocaleCR : LocaleHelper
{
	private static readonly LocaleCR _instance = new();

	protected LocaleCR() : base($"Skyve.Systems.Properties.Compatibility.json") { }

	public static void Load() { _ = _instance; }

	public static Translation Get(string value)
	{
		return _instance.GetText(value);
	}

	public static Translation CR_NoAvailableReport => _instance.GetText(nameof(CR_NoAvailableReport));
	public static Translation CR_IncompatibleWithGameVersion => _instance.GetText(nameof(CR_IncompatibleWithGameVersion));
	public static Translation CR_RequiresIncompatibleMod => _instance.GetText(nameof(CR_RequiresIncompatibleMod));
	public static Translation CR_BreaksGame => _instance.GetText(nameof(CR_BreaksGame));
	public static Translation CR_BrokenMod => _instance.GetText(nameof(CR_BrokenMod));
	public static Translation CR_MajorIssuesNoNote => _instance.GetText(nameof(CR_MajorIssuesNoNote));
	public static Translation CR_MajorIssuesWithNote => _instance.GetText(nameof(CR_MajorIssuesWithNote));
	public static Translation CR_MinorIssuesNoNote => _instance.GetText(nameof(CR_MinorIssuesNoNote));
	public static Translation CR_MinorIssuesWithNote => _instance.GetText(nameof(CR_MinorIssuesWithNote));
	public static Translation CR_UserReportsNoNote => _instance.GetText(nameof(CR_UserReportsNoNote));
	public static Translation CR_UserReportsWithNote => _instance.GetText(nameof(CR_UserReportsWithNote));
	public static Translation CR_NotEnoughInformationUpdated => _instance.GetText(nameof(CR_NotEnoughInformationUpdated));
	public static Translation CR_NotEnoughInformationOutdated => _instance.GetText(nameof(CR_NotEnoughInformationOutdated));
	public static Translation CR_Stable => _instance.GetText(nameof(CR_Stable));
	public static Translation CR_NotReviewedUpdated => _instance.GetText(nameof(CR_NotReviewedUpdated));
	public static Translation CR_NotReviewedOutdated => _instance.GetText(nameof(CR_NotReviewedOutdated));
	public static Translation CR_NotInCatalogMod => _instance.GetText(nameof(CR_NotInCatalogMod));
	public static Translation CR_NotInCatalog => _instance.GetText(nameof(CR_NotInCatalog));
	public static Translation CR_MissingDLC => _instance.GetText(nameof(CR_MissingDLC));
	public static Translation CR_UnneededDependency => _instance.GetText(nameof(CR_UnneededDependency));
	public static Translation CR_WorksWhenDisabled => _instance.GetText(nameof(CR_WorksWhenDisabled));
	public static Translation CR_SuccessorsAvailable => _instance.GetText(nameof(CR_SuccessorsAvailable));
	public static Translation CR_SuccessorsAvailableMultiple => _instance.GetText(nameof(CR_SuccessorsAvailableMultiple));
	public static Translation CR_AlternativesAvailable => _instance.GetText(nameof(CR_AlternativesAvailable));
	public static Translation CR_AlternativesAvailableMultiple => _instance.GetText(nameof(CR_AlternativesAvailableMultiple));
	public static Translation CR_RequiredModsMissing => _instance.GetText(nameof(CR_RequiredModsMissing));
	public static Translation CR_Obsolete => _instance.GetText(nameof(CR_Obsolete));
	public static Translation CR_RemovedFromWorkshop => _instance.GetText(nameof(CR_RemovedFromWorkshop));
	public static Translation CR_Deprecated => _instance.GetText(nameof(CR_Deprecated));
	public static Translation CR_Abandoned => _instance.GetText(nameof(CR_Abandoned));
	public static Translation CR_AbandonedRetired => _instance.GetText(nameof(CR_AbandonedRetired));
	public static Translation CR_Retired => _instance.GetText(nameof(CR_Retired));
	public static Translation CR_Note => _instance.GetText(nameof(CR_Note));
	public static Translation CR_Recommendations => _instance.GetText(nameof(CR_Recommendations));
	public static Translation CR_SavesCantLoadWithout => _instance.GetText(nameof(CR_SavesCantLoadWithout));
	public static Translation CR_SourceNotPublic => _instance.GetText(nameof(CR_SourceNotPublic));
	public static Translation CR_NoDescription => _instance.GetText(nameof(CR_NoDescription));
	public static Translation CR_NoCommentSection => _instance.GetText(nameof(CR_NoCommentSection));
	public static Translation CR_SourceBundled => _instance.GetText(nameof(CR_SourceBundled));
	public static Translation CR_SourceNotPublicAbandoned => _instance.GetText(nameof(CR_SourceNotPublicAbandoned));
	public static Translation CR_SourceObfuscated => _instance.GetText(nameof(CR_SourceObfuscated));
	public static Translation CR_Reupload => _instance.GetText(nameof(CR_Reupload));
	public static Translation CR_BreaksEditors => _instance.GetText(nameof(CR_BreaksEditors));
	public static Translation CR_ModForModders => _instance.GetText(nameof(CR_ModForModders));
	public static Translation CR_TestVersion => _instance.GetText(nameof(CR_TestVersion));
	public static Translation CR_TestVersionStable => _instance.GetText(nameof(CR_TestVersionStable));
	public static Translation CR_MusicCopyright => _instance.GetText(nameof(CR_MusicCopyright));
	public static Translation CR_SameModDifferentReleaseType => _instance.GetText(nameof(CR_SameModDifferentReleaseType));
	public static Translation CR_SameFunctionality => _instance.GetText(nameof(CR_SameFunctionality));
	public static Translation CR_IncompatibleAccordingToAuthor => _instance.GetText(nameof(CR_IncompatibleAccordingToAuthor));
	public static Translation CR_IncompatibleAccordingToUsers => _instance.GetText(nameof(CR_IncompatibleAccordingToUsers));
	public static Translation CR_MajorIssuesWith => _instance.GetText(nameof(CR_MajorIssuesWith));
	public static Translation CR_MinorIssuesWith => _instance.GetText(nameof(CR_MinorIssuesWith));
	public static Translation CR_RequiresSpecificSettings => _instance.GetText(nameof(CR_RequiresSpecificSettings));
	public static Translation CR_SameFunctionalityCompatible => _instance.GetText(nameof(CR_SameFunctionalityCompatible));
	public static Translation CR_CompatibleAccordingToAuthor => _instance.GetText(nameof(CR_CompatibleAccordingToAuthor));
	public static Translation CR_IncompatibleAsset => _instance.GetText(nameof(CR_IncompatibleAsset));
	public static Translation LinkedPackages => _instance.GetText(nameof(LinkedPackages));
	public static Translation ConfirmEndSession => _instance.GetText(nameof(ConfirmEndSession));
	public static Translation StatusesCount => _instance.GetText(nameof(StatusesCount));
	public static Translation InteractionCount => _instance.GetText(nameof(InteractionCount));
	public static Translation CrDataLoadFailed => _instance.GetText(nameof(CrDataLoadFailed));
	public static Translation AddGlobalTag => _instance.GetText(nameof(AddGlobalTag));
	public static Translation PleaseReviewTheStability => _instance.GetText(nameof(PleaseReviewTheStability));
	public static Translation PleaseReviewPackageStatuses => _instance.GetText(nameof(PleaseReviewPackageStatuses));
	public static Translation PleaseReviewPackageInteractions => _instance.GetText(nameof(PleaseReviewPackageInteractions));
	public static Translation PleaseReviewPackageUsage => _instance.GetText(nameof(PleaseReviewPackageUsage));
	public static Translation OutputText => _instance.GetText(nameof(OutputText));
	public static Translation NoRequiredDlcs => _instance.GetText(nameof(NoRequiredDlcs));
	public static Translation DlcsSelected => _instance.GetText(nameof(DlcsSelected));
	public static Translation RequestReview => _instance.GetText(nameof(RequestReview));
	public static Translation RequestReviewInfo => _instance.GetText(nameof(RequestReviewInfo));
	public static Translation LastReviewDate => _instance.GetText(nameof(LastReviewDate));
	public static Translation ApplyChangedBeforeExit => _instance.GetText(nameof(ApplyChangedBeforeExit));
	public static Translation Broken => _instance.GetText(nameof(Broken));
	public static Translation Incompatible => _instance.GetText(nameof(Incompatible));
	public static Translation Banned => _instance.GetText(nameof(Banned));
	public static Translation Usage => _instance.GetText(nameof(Usage));
	public static Translation PackageType => _instance.GetText(nameof(PackageType));
	public static Translation Links => _instance.GetText(nameof(Links));
	public static Translation ReviewRequests => _instance.GetText(nameof(ReviewRequests));
	public static Translation RequestReviewDisclaimer => _instance.GetText(nameof(RequestReviewDisclaimer));
	public static Translation OtherCompatibilityWarnings => _instance.GetText(nameof(OtherCompatibilityWarnings));
}