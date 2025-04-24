using Extensions;

using Skyve.Domain.Systems;

namespace Skyve.Systems;
public class Locale : LocaleHelper, ILocale
{
	private static readonly Locale _instance = new();

	public static void Load() { _ = _instance; }

	public Translation Get(string key)
	{
		return GetGlobalText(key);
	}

	public static Translation IncludeAll => GetGlobalText(nameof(IncludeAll));
	public static Translation ExcludeAll => GetGlobalText(nameof(ExcludeAll));
	public static Translation ExcludeAllDisabled => GetGlobalText(nameof(ExcludeAllDisabled));
	public static Translation IncludeAllFiltered => GetGlobalText(nameof(IncludeAllFiltered));
	public static Translation ExcludeAllFiltered => GetGlobalText(nameof(ExcludeAllFiltered));
	public static Translation ExcludeAllDisabledFiltered => GetGlobalText(nameof(ExcludeAllDisabledFiltered));
	public static Translation IncludeAllSelected => GetGlobalText(nameof(IncludeAllSelected));
	public static Translation ExcludeAllSelected => GetGlobalText(nameof(ExcludeAllSelected));
	public static Translation ExcludeAllDisabledSelected => GetGlobalText(nameof(ExcludeAllDisabledSelected));
	public static Translation SubscribeToItem => GetGlobalText(nameof(SubscribeToItem));
	public static Translation IncludeItem => GetGlobalText(nameof(IncludeItem));
	public static Translation ExcludeItem => GetGlobalText(nameof(ExcludeItem));
	public static Translation ViewOnWorkshop => GetGlobalText(nameof(ViewOnWorkshop));
	public static Translation CopyWorkshopLink => GetGlobalText(nameof(CopyWorkshopLink));
	public static Translation CopyWorkshopMarkdownLink => GetGlobalText(nameof(CopyWorkshopMarkdownLink));
	public static Translation CopyWorkshopId => GetGlobalText(nameof(CopyWorkshopId));
	public static Translation IncludeThisItemInAllPlaysets => GetGlobalText(nameof(IncludeThisItemInAllPlaysets));
	public static Translation ExcludeThisItemInAllPlaysets => GetGlobalText(nameof(ExcludeThisItemInAllPlaysets));

	public Locale() : base($"Skyve.Systems.Properties.Locale.json") { }

	/// <summary>
	/// ABANDONED
	/// </summary>
	public static Translation ABANDONED => _instance.GetText("ABANDONED");

	/// <summary>
	/// <para>This action can not be reversed.</para>
	/// <para>Plural: This action can not be reversed. And will affect {0} packages.</para>
	/// </summary>
	public static Translation ActionUnreversible => _instance.GetText("ActionUnreversible");

	/// <summary>
	/// Activate this playset
	/// </summary>
	public static Translation ActivatePlayset => _instance.GetText("ActivatePlayset");

	/// <summary>
	/// Active Playset
	/// </summary>
	public static Translation ActivePlayset => _instance.GetText("ActivePlayset");

	/// <summary>
	/// Add a custom tag
	/// </summary>
	public static Translation AddCustomTag => _instance.GetText("AddCustomTag");

	/// <summary>
	/// Please add a meaningful description to your request.
	/// </summary>
	public static Translation AddMeaningfulDescription => _instance.GetText("AddMeaningfulDescription");

	/// <summary>
	/// Add Playset
	/// </summary>
	public static Translation AddPlayset => _instance.GetText("AddPlayset");

	/// <summary>
	/// Add a new tag or search here
	/// </summary>
	public static Translation AddTagBox => _instance.GetText("AddTagBox");

	/// <summary>
	/// Add '{0}' to your search
	/// </summary>
	public static Translation AddToSearch => _instance.GetText("AddToSearch");

	/// <summary>
	/// Show extra launch options
	/// </summary>
	public static Translation AdvancedLaunchOptions => _instance.GetText("AdvancedLaunchOptions");

	/// <summary>
	/// Displays extra launch options used by developers in the playset's settings.
	/// </summary>
	public static Translation AdvancedLaunchOptionsTip => _instance.GetText("AdvancedLaunchOptions_Tip");

	/// <summary>
	/// Advanced Settings
	/// </summary>
	public static Translation AdvancedSettings => _instance.GetText("AdvancedSettings");

	/// <summary>
	/// Affected packages are:
	/// </summary>
	public static Translation AffectedPackagesAre => _instance.GetText("AffectedPackagesAre");

	/// <summary>
	/// All File Types
	/// </summary>
	public static Translation AllContentTypes => _instance.GetText("AllContentTypes");

	/// <summary>
	/// All DLCs
	/// </summary>
	public static Translation AllDlcs => _instance.GetText("AllDlcs");

	/// <summary>
	/// All Usages
	/// </summary>
	public static Translation AllUsages => _instance.GetText("AllUsages");

	/// <summary>
	/// ALPHA
	/// </summary>
	public static Translation ALPHA => _instance.GetText("ALPHA");

	/// <summary>
	/// Alt+Click to {0}
	/// </summary>
	public static Translation AltClickTo => _instance.GetText("AltClickTo");

	/// <summary>
	/// Show the Filters section by default
	/// </summary>
	public static Translation AlwaysOpenFiltersAndActions => _instance.GetText("AlwaysOpenFiltersAndActions");

	/// <summary>
	/// Always have the filters section open when entering a content tab
	/// </summary>
	public static Translation AlwaysOpenFiltersAndActionsTip => _instance.GetText("AlwaysOpenFiltersAndActions_Tip");

	/// <summary>
	/// Any Author
	/// </summary>
	public static Translation AnyAuthor => _instance.GetText("AnyAuthor");

	/// <summary>
	/// Any Issue
	/// </summary>
	public static Translation AnyIssue => _instance.GetText("AnyIssue");

	/// <summary>
	/// Any Status
	/// </summary>
	public static Translation AnyStatus => _instance.GetText("AnyStatus");

	/// <summary>
	/// Any Tags
	/// </summary>
	public static Translation AnyTags => _instance.GetText("AnyTags");

	/// <summary>
	/// Any Usage
	/// </summary>
	public static Translation AnyUsage => _instance.GetText("AnyUsage");

	/// <summary>
	/// App Data
	/// </summary>
	public static Translation AppData => _instance.GetText("App Data");

	/// <summary>
	/// Applying settings...
	/// </summary>
	public static Translation ApplyingSettings => _instance.GetText("ApplyingSettings");

	/// <summary>
	/// Please finish editing the playset's name before exiting this page.
	/// </summary>
	public static Translation ApplyPlaysetNameBeforeExit => _instance.GetText("ApplyPlaysetNameBeforeExit");

	/// <summary>
	/// Archived
	/// </summary>
	public static Translation Archived => _instance.GetText("Archived");

	/// <summary>
	/// Are you sure you want to proceed?
	/// </summary>
	public static Translation AreYouSure => _instance.GetText("AreYouSure");

	/// <summary>
	/// Would you like for the shortcut to also launch the game with your playset?
	/// </summary>
	public static Translation AskToLaunchGameForShortcut => _instance.GetText("AskToLaunchGameForShortcut");

	/// <summary>
	/// <para>Asset</para>
	/// <para>Plural: Assets</para>
	/// </summary>
	public static Translation Asset => _instance.GetText("Asset");

	/// <summary>
	/// Assets Enabled
	/// </summary>
	public static Translation AssetsEnabled => _instance.GetText("AssetsEnabled");

	/// <summary>
	/// Ignore Internet connection checks
	/// </summary>
	public static Translation AssumeInternetConnectivity => _instance.GetText("AssumeInternetConnectivity");

	/// <summary>
	/// Skips checking for internet and automatically assumes you are connected.
	/// </summary>
	public static Translation AssumeInternetConnectivityTip => _instance.GetText("AssumeInternetConnectivity_Tip");

	/// <summary>
	/// Author
	/// </summary>
	public static Translation Author => _instance.GetText("Author");

	/// <summary>
	/// <para>{0} author selected</para>
	/// <para>Plural: {0} authors selected</para>
	/// </summary>
	public static Translation AuthorsSelected => _instance.GetText("AuthorsSelected");

	/// <summary>
	/// Available Widgets
	/// </summary>
	public static Translation AvailableWidgets => _instance.GetText("AvailableWidgets");

	/// <summary>
	/// <para>Backup</para>
	/// <para>Plural: Backups</para>
	/// </summary>
	public static Translation Backup => _instance.GetText("Backup");

	/// <summary>
	/// Backup Center
	/// </summary>
	public static Translation BackupCenter => _instance.GetText("BackupCenter");

	/// <summary>
	/// Cleanup Settings
	/// </summary>
	public static Translation BackupCleanup => _instance.GetText("BackupCleanup");

	/// <summary>
	/// Select which types of files to include in your backups.
	/// </summary>
	public static Translation BackupContentInfo => _instance.GetText("BackupContentInfo");

	/// <summary>
	/// <para>{1} backup</para>
	/// <para>Plural: {0} {1} backups</para>
	/// </summary>
	public static Translation BackupCount => _instance.GetText("BackupCount");

	/// <summary>
	/// Manually back up savegame files, settings, and other relevant information to safeguard your cities and mods.  This ensures your current cities and preferences are safely stored in case of system issues or re-installation.
	/// </summary>
	public static Translation BackupDescriptionInfo => _instance.GetText("BackupDescriptionInfo");

	/// <summary>
	/// Backup Failed
	/// </summary>
	public static Translation BackupFailed => _instance.GetText("BackupFailed");

	/// <summary>
	/// Backup History
	/// </summary>
	public static Translation BackupHistory => _instance.GetText("BackupHistory");

	/// <summary>
	/// Include local mods in scheduled backups
	/// </summary>
	public static Translation BackupIncludeLocalMods => _instance.GetText("BackupIncludeLocalMods");

	/// <summary>
	/// Include maps in scheduled backups
	/// </summary>
	public static Translation BackupIncludeMaps => _instance.GetText("BackupIncludeMaps");

	/// <summary>
	/// Include savegame files in scheduled backups
	/// </summary>
	public static Translation BackupIncludeSaveGames => _instance.GetText("BackupIncludeSaveGames");

	/// <summary>
	/// Backup is in progress
	/// </summary>
	public static Translation BackupInProgress => _instance.GetText("BackupInProgress");

	/// <summary>
	/// Backup Files Location
	/// </summary>
	public static Translation BackupPath => _instance.GetText("BackupPath");

	/// <summary>
	/// Select a folder where you want to save your backups
	/// </summary>
	public static Translation BackupPathPlaceholder => _instance.GetText("BackupPathPlaceholder");

	/// <summary>
	/// Backup &amp; Restore
	/// </summary>
	public static Translation BackupRestore => _instance.GetText("BackupRestore");

	/// <summary>
	/// Backup Schedule
	/// </summary>
	public static Translation BackupSchedule => _instance.GetText("BackupSchedule");

	/// <summary>
	/// Schedule Settings
	/// </summary>
	public static Translation BackupScheduleSettings => _instance.GetText("BackupScheduleSettings");

	/// <summary>
	/// Basic Settings
	/// </summary>
	public static Translation BasicSettings => _instance.GetText("BasicSettings");

	/// <summary>
	/// BETA
	/// </summary>
	public static Translation BETA => _instance.GetText("BETA");

	/// <summary>
	/// Bio
	/// </summary>
	public static Translation Bio => _instance.GetText("Bio");

	/// <summary>
	/// Broken Update
	/// </summary>
	public static Translation BrokenUpdate => _instance.GetText("BrokenUpdate");

	/// <summary>
	/// Bulk Actions
	/// </summary>
	public static Translation BulkActions => _instance.GetText("BulkActions");

	/// <summary>
	/// Would you like to reset to the configuration you were using before starting the troubleshoot?  Select 'No' to keep the current configuration of packages.
	/// </summary>
	public static Translation CancelTroubleshootMessage => _instance.GetText("CancelTroubleshootMessage");

	/// <summary>
	/// Cancel &amp; Reset Troubleshooting?
	/// </summary>
	public static Translation CancelTroubleshootTitle => _instance.GetText("CancelTroubleshootTitle");

	/// <summary>
	/// Change-log
	/// </summary>
	public static Translation Changelog => _instance.GetText("Changelog");

	/// <summary>
	/// Change this playset's color
	/// </summary>
	public static Translation ChangePlaysetColor => _instance.GetText("ChangePlaysetColor");

	/// <summary>
	/// Change this playset's settings
	/// </summary>
	public static Translation ChangePlaysetSettings => _instance.GetText("ChangePlaysetSettings");

	/// <summary>
	/// Make sure Skyve has access to your documents folder, it uses it to store your UI &amp; Language settings.
	/// </summary>
	public static Translation CheckDocumentsFolder => _instance.GetText("CheckDocumentsFolder");

	/// <summary>
	/// Check the logs for any errors and try again.
	/// </summary>
	public static Translation CheckLogsAndTryAgain => _instance.GetText("CheckLogsAndTryAgain");

	/// <summary>
	/// You can not backup your files inside the this folder, Skyve does not have access to save files here.  Choose a different location that is not in the same folder, preferably on another drive.
	/// </summary>
	public static Translation ChooseDifferentBackupLocation => _instance.GetText("ChooseDifferentBackupLocation");

	/// <summary>
	/// Delete old backups once the total amount of backups exceeds:
	/// </summary>
	public static Translation CleanupCountBased => _instance.GetText("CleanupCountBased");

	/// <summary>
	/// What is a cleanup?
	/// </summary>
	public static Translation CleanupInfoTitle => _instance.GetText("CleanupInfoTitle");

	/// <summary>
	/// Delete old backups once the total storage used exceeds:
	/// </summary>
	public static Translation CleanupStorageBased => _instance.GetText("CleanupStorageBased");

	/// <summary>
	/// Delete all backups olders than:
	/// </summary>
	public static Translation CleanupTimeBased => _instance.GetText("CleanupTimeBased");

	/// <summary>
	/// <para>Code Mod</para>
	/// <para>Plural: Code Mods</para>
	/// </summary>
	public static Translation CodeMod => _instance.GetText("CodeMod");

	/// <summary>
	/// Color authors &amp; users' name and avatar
	/// </summary>
	public static Translation ColoredAuthorNames => _instance.GetText("ColoredAuthorNames");

	/// <summary>
	/// Adds a unique color for each author/user, the user's name and avatar will use that color so it's easier to keep track of specific users.
	/// </summary>
	public static Translation ColoredAuthorNamesTip => _instance.GetText("ColoredAuthorNames_Tip");

	/// <summary>
	/// Compatibility Center
	/// </summary>
	public static Translation CompatibilityCenter => _instance.GetText("CompatibilityCenter");

	/// <summary>
	/// Compatibility Info
	/// </summary>
	public static Translation CompatibilityInfo => _instance.GetText("CompatibilityInfo");

	/// <summary>
	/// Compatibility Report
	/// </summary>
	public static Translation CompatibilityReport => _instance.GetText("CompatibilityReport");

	/// <summary>
	/// Compatibility Status
	/// </summary>
	public static Translation CompatibilityStatus => _instance.GetText("CompatibilityStatus");

	/// <summary>
	/// Advanced-user UI mode
	/// </summary>
	public static Translation ComplexListUI => _instance.GetText("ComplexListUI");

	/// <summary>
	/// Adds more advanced information and adds extra actions to the user interface.
	/// </summary>
	public static Translation ComplexListUITip => _instance.GetText("ComplexListUI_Tip");

	/// <summary>
	/// Are you sure you want to delete the playset '{0}'?
	/// </summary>
	public static Translation ConfirmDeletePlayset => _instance.GetText("ConfirmDeletePlayset");

	/// <summary>
	/// Confirm Selection
	/// </summary>
	public static Translation ConfirmSelection => _instance.GetText("ConfirmSelection");

	/// <summary>
	/// <para>Contains {0} {1}</para>
	/// <para>Plural: Contains {0} {1}</para>
	/// </summary>
	public static Translation ContainCount => _instance.GetText("ContainCount");

	/// <summary>
	/// Content Summary
	/// </summary>
	public static Translation ContentSummary => _instance.GetText("ContentSummary");

	/// <summary>
	/// File Types
	/// </summary>
	public static Translation ContentToBackup => _instance.GetText("ContentToBackup");

	/// <summary>
	/// Clone your active playset's subscribed mods into a new playset.  This might take a moment.
	/// </summary>
	public static Translation ContinueFromCurrent => _instance.GetText("ContinueFromCurrent");

	/// <summary>
	/// Ctrl + Click to select multiple packages at once.
	/// </summary>
	public static Translation ControlToSelectMultiplePackages => _instance.GetText("ControlToSelectMultiplePackages");

	/// <summary>
	/// Copy all items' names &amp; IDs
	/// </summary>
	public static Translation CopyAllIds => _instance.GetText("CopyAllIds");

	/// <summary>
	/// Copy all filtered items' names &amp; IDs
	/// </summary>
	public static Translation CopyAllIdsFiltered => _instance.GetText("CopyAllIdsFiltered");

	/// <summary>
	/// Copy all selected items' names &amp; IDs
	/// </summary>
	public static Translation CopyAllIdsSelected => _instance.GetText("CopyAllIdsSelected");

	/// <summary>
	/// <para>Copy the author's ID</para>
	/// <para>Plural: Copy the selected packages' author IDs</para>
	/// </summary>
	public static Translation CopyAuthorId => _instance.GetText("CopyAuthorId");

	/// <summary>
	/// Copy the author's profile link
	/// </summary>
	public static Translation CopyAuthorLink => _instance.GetText("CopyAuthorLink");

	/// <summary>
	/// <para>Copy the author's name</para>
	/// <para>Plural: Copy the selected packages' author names</para>
	/// </summary>
	public static Translation CopyAuthorName => _instance.GetText("CopyAuthorName");

	/// <summary>
	/// Copy the game's log file to clipboard
	/// </summary>
	public static Translation CopyLogFile => _instance.GetText("CopyLogFile");

	/// <summary>
	/// Copy Skyve's log file to clipboard
	/// </summary>
	public static Translation CopyLOTLogFile => _instance.GetText("CopyLOTLogFile");

	/// <summary>
	/// <para>Copy the package's name</para>
	/// <para>Plural: Copy the selected packages' names</para>
	/// </summary>
	public static Translation CopyPackageName => _instance.GetText("CopyPackageName");

	/// <summary>
	/// Copy this playset's ID
	/// </summary>
	public static Translation CopyPlaysetLink => _instance.GetText("CopyPlaysetLink");

	/// <summary>
	/// Copy '{0}' to your clipboard
	/// </summary>
	public static Translation CopyToClipboard => _instance.GetText("CopyToClipboard");

	/// <summary>
	/// Copy version number
	/// </summary>
	public static Translation CopyVersionNumber => _instance.GetText("CopyVersionNumber");

	/// <summary>
	/// Could not create a new playset.
	/// </summary>
	public static Translation CouldNotCreatePlayset => _instance.GetText("CouldNotCreatePlayset");

	/// <summary>
	/// Could not upload your file at this time
	/// </summary>
	public static Translation CouldNotUploadFile => _instance.GetText("CouldNotUploadFile");

	/// <summary>
	/// Create a new playset or activate a previous one by clicking here
	/// </summary>
	public static Translation CreatePlaysetHere => _instance.GetText("CreatePlaysetHere");

	/// <summary>
	/// Create shortcut
	/// </summary>
	public static Translation CreateShortcut => _instance.GetText("CreateShortcut");

	/// <summary>
	/// Creates a shortcut to the app on your desktop.
	/// </summary>
	public static Translation CreateShortcutTip => _instance.GetText("CreateShortcut_Tip");

	/// <summary>
	/// Create a shortcut for it on your desktop
	/// </summary>
	public static Translation CreateShortcutPlayset => _instance.GetText("CreateShortcutPlayset");

	/// <summary>
	/// Creator pack by {0}
	/// </summary>
	public static Translation CreatorPackBy => _instance.GetText("CreatorPackBy");

	/// <summary>
	/// Custom Maps
	/// </summary>
	public static Translation CustomMaps => _instance.GetText("Custom Maps");

	/// <summary>
	/// Custom Color
	/// </summary>
	public static Translation CustomColor => _instance.GetText("CustomColor");

	/// <summary>
	/// Custom Launch Arguments
	/// </summary>
	public static Translation CustomLaunchArguments => _instance.GetText("CustomLaunchArguments");

	/// <summary>
	/// Custom Tags
	/// </summary>
	public static Translation CustomTags => _instance.GetText("CustomTags");

	/// <summary>
	/// Custom Thumbnail
	/// </summary>
	public static Translation CustomThumbnail => _instance.GetText("CustomThumbnail");

	/// <summary>
	/// Dashboard
	/// </summary>
	public static Translation Dashboard => _instance.GetText("Dashboard");

	/// <summary>
	/// Customize your dashboard by dragging each section from its title. You can also resize a section using the dotted triangle on its bottom-right corner.
	/// </summary>
	public static Translation DashboardCustomizationInfo => _instance.GetText("DashboardCustomizationInfo");

	/// <summary>
	/// Date Subscribed
	/// </summary>
	public static Translation DateSubscribed => _instance.GetText("DateSubscribed");

	/// <summary>
	/// Date Updated
	/// </summary>
	public static Translation DateUpdated => _instance.GetText("DateUpdated");

	/// <summary>
	/// Deactivate your current playset
	/// </summary>
	public static Translation DeactivatePlaysetTip => _instance.GetText("DeactivatePlayset_Tip");

	/// <summary>
	/// Your last session's errors will be automatically shown here, results are updated live.
	/// </summary>
	public static Translation DefaultLogViewInfo => _instance.GetText("DefaultLogViewInfo");

	/// <summary>
	/// <para>Delete this asset</para>
	/// <para>Plural: Delete the selected assets</para>
	/// </summary>
	public static Translation DeleteAsset => _instance.GetText("DeleteAsset");

	/// <summary>
	/// Delete this backup
	/// </summary>
	public static Translation DeleteBackup => _instance.GetText("DeleteBackup");

	/// <summary>
	/// <para>Delete this package</para>
	/// <para>Plural: Delete the selected packages</para>
	/// </summary>
	public static Translation DeletePackage => _instance.GetText("DeletePackage");

	/// <summary>
	/// <para>Dependency</para>
	/// <para>Plural: Dependencies</para>
	/// </summary>
	public static Translation Dependency => _instance.GetText("Dependency");

	/// <summary>
	/// DEPRECATED
	/// </summary>
	public static Translation DEPRECATED => _instance.GetText("DEPRECATED");

	/// <summary>
	/// Un-select all items
	/// </summary>
	public static Translation DeselectAll => _instance.GetText("DeselectAll");

	/// <summary>
	/// Detected Issues
	/// </summary>
	public static Translation DetectedIssues => _instance.GetText("DetectedIssues");

	/// <summary>
	/// Developer Launch Options
	/// </summary>
	public static Translation DevOptions => _instance.GetText("DevOptions");

	/// <summary>
	/// Disable all items
	/// </summary>
	public static Translation DisableAll => _instance.GetText("DisableAll");

	/// <summary>
	/// Disable filtered items
	/// </summary>
	public static Translation DisableAllFiltered => _instance.GetText("DisableAllFiltered");

	/// <summary>
	/// Disable selected items
	/// </summary>
	public static Translation DisableAllSelected => _instance.GetText("DisableAllSelected");

	/// <summary>
	/// Disable Content-Creator related warnings
	/// </summary>
	public static Translation DisableContentCreatorWarnings => _instance.GetText("DisableContentCreatorWarnings");

	/// <summary>
	/// Disables warnings related to creating content and copyright claims.
	/// </summary>
	public static Translation DisableContentCreatorWarningsTip => _instance.GetText("DisableContentCreatorWarnings_Tip");

	/// <summary>
	/// Disabled
	/// </summary>
	public static Translation Disabled => _instance.GetText("Disabled");

	/// <summary>
	/// Disable this mod
	/// </summary>
	public static Translation DisableItem => _instance.GetText("DisableItem");

	/// <summary>
	/// Disable newly added assets by default
	/// </summary>
	public static Translation DisableNewAssetsByDefault => _instance.GetText("DisableNewAssetsByDefault");

	/// <summary>
	/// When you subscribe or add an asset, the tool will automatically exclude or include it based on this setting when the tool is launched.
	/// </summary>
	public static Translation DisableNewAssetsByDefaultTip => _instance.GetText("DisableNewAssetsByDefault_Tip");

	/// <summary>
	/// Disable newly added mods by default
	/// </summary>
	public static Translation DisableNewModsByDefault => _instance.GetText("DisableNewModsByDefault");

	/// <summary>
	/// When you subscribe or add a mod, the tool will automatically exclude or include it based on this setting when the tool is launched.
	/// </summary>
	public static Translation DisableNewModsByDefaultTip => _instance.GetText("DisableNewModsByDefault_Tip");

	/// <summary>
	/// <para>Disable in all your playsets</para>
	/// <para>Plural: Disable the selected items in all your playsets</para>
	/// </summary>
	public static Translation DisableThisItemInAllPlaysets => _instance.GetText("DisableThisItemInAllPlaysets");

	/// <summary>
	/// Discover Playsets
	/// </summary>
	public static Translation DiscoverPlaysets => _instance.GetText("DiscoverPlaysets");

	/// <summary>
	/// Look for mods on the workshop...
	/// </summary>
	public static Translation DiscoverWorkshop => _instance.GetText("DiscoverWorkshop");

	/// <summary>
	/// Dismiss
	/// </summary>
	public static Translation Dismiss => _instance.GetText("Dismiss");

	/// <summary>
	/// {0} total DLCs owned
	/// </summary>
	public static Translation DlcCount => _instance.GetText("DlcCount");

	/// <summary>
	/// DLCs &amp; CCPs
	/// </summary>
	public static Translation DLCs => _instance.GetText("DLCs");

	/// <summary>
	/// If your owned DLCs are not showing correctly, make sure to open the game at least once
	/// </summary>
	public static Translation DlcUpdateNotice => _instance.GetText("DlcUpdateNotice");

	/// <summary>
	/// Backup Files Now
	/// </summary>
	public static Translation DoBackupNow => _instance.GetText("DoBackupNow");

	/// <summary>
	/// Donate
	/// </summary>
	public static Translation Donate => _instance.GetText("Donate");

	/// <summary>
	/// Download Playset
	/// </summary>
	public static Translation DownloadPlayset => _instance.GetText("DownloadPlayset");

	/// <summary>
	/// Download this playset to your local computer
	/// </summary>
	public static Translation DownloadPlaysetTip => _instance.GetText("DownloadPlaysetTip");

	/// <summary>
	/// Release to import the {0} from this report
	/// </summary>
	public static Translation DropImportFile => _instance.GetText("DropImportFile");

	/// <summary>
	/// <para>Edit this package's compatibility</para>
	/// <para>Plural: Edit selected packages' compatibility</para>
	/// </summary>
	public static Translation EditCompatibility => _instance.GetText("EditCompatibility");

	/// <summary>
	/// Edit Dashboard
	/// </summary>
	public static Translation EditDashboard => _instance.GetText("EditDashboard");

	/// <summary>
	/// Editing multiple packages' tags will overwrite each of their custom tags.
	/// </summary>
	public static Translation EditingMultipleTags => _instance.GetText("EditingMultipleTags");

	/// <summary>
	/// <para>Edit this package's Links</para>
	/// <para>Plural: Edit selected items' Links</para>
	/// </summary>
	public static Translation EditLinks => _instance.GetText("EditLinks");

	/// <summary>
	/// Make Assets
	/// </summary>
	public static Translation EditorPlay => _instance.GetText("EditorPlay");

	/// <summary>
	/// Edit this playset's name
	/// </summary>
	public static Translation EditPlaysetName => _instance.GetText("EditPlaysetName");

	/// <summary>
	/// Edit this playset's thumbnail
	/// </summary>
	public static Translation EditPlaysetThumbnail => _instance.GetText("EditPlaysetThumbnail");

	/// <summary>
	/// <para>Edit this package's Tags</para>
	/// <para>Plural: Edit selected items' Tags</para>
	/// </summary>
	public static Translation EditTags => _instance.GetText("EditTags");

	/// <summary>
	/// <para>Edit all of this package's assets' Tags</para>
	/// <para>Plural: Edit all of the selected packages' assets' Tags</para>
	/// </summary>
	public static Translation EditTagsOfPackage => _instance.GetText("EditTagsOfPackage");

	/// <summary>
	/// Enable all items
	/// </summary>
	public static Translation EnableAll => _instance.GetText("EnableAll");

	/// <summary>
	/// Enable filtered items
	/// </summary>
	public static Translation EnableAllFiltered => _instance.GetText("EnableAllFiltered");

	/// <summary>
	/// Enable selected items
	/// </summary>
	public static Translation EnableAllSelected => _instance.GetText("EnableAllSelected");

	/// <summary>
	/// Enable scheduled backups
	/// </summary>
	public static Translation EnableAutoBackup => _instance.GetText("EnableAutoBackup");

	/// <summary>
	/// Enabled
	/// </summary>
	public static Translation Enabled => _instance.GetText("Enabled");

	/// <summary>
	/// <para>{0} {1} enabled</para>
	/// <para>Plural: {0} {1} enabled</para>
	/// </summary>
	public static Translation EnabledCount => _instance.GetText("EnabledCount");

	/// <summary>
	/// Enable this mod
	/// </summary>
	public static Translation EnableItem => _instance.GetText("EnableItem");

	/// <summary>
	/// <para>Enable in all your playsets</para>
	/// <para>Plural: Enable the selected items in all your playsets</para>
	/// </summary>
	public static Translation EnableThisItemInAllPlaysets => _instance.GetText("EnableThisItemInAllPlaysets");

	/// <summary>
	/// Errors in the Log
	/// </summary>
	public static Translation ErrorsInLog => _instance.GetText("ErrorsInLog");

	/// <summary>
	/// Exclude
	/// </summary>
	public static Translation Exclude => _instance.GetText("Exclude");

	/// <summary>
	/// Excluded
	/// </summary>
	public static Translation Excluded => _instance.GetText("Excluded");

	/// <summary>
	/// EXPERIMENTAL
	/// </summary>
	public static Translation EXPERIMENTAL => _instance.GetText("EXPERIMENTAL");

	/// <summary>
	/// Describe your issue here
	/// </summary>
	public static Translation ExplainIssue => _instance.GetText("ExplainIssue");

	/// <summary>
	/// Include as much information as you can about what is happening and how to replicate it here
	/// </summary>
	public static Translation ExplainIssueInfo => _instance.GetText("ExplainIssueInfo");

	/// <summary>
	/// Fade out disabled mods &amp; assets
	/// </summary>
	public static Translation FadeDisabledItems => _instance.GetText("FadeDisabledItems");

	/// <summary>
	/// Disabled mods &amp; assets in your local content lists will be faded out.
	/// </summary>
	public static Translation FadeDisabledItemsTip => _instance.GetText("FadeDisabledItems_Tip");

	/// <summary>
	/// Failed to apply the requested changes.
	/// </summary>
	public static Translation FailedToApplyChanges => _instance.GetText("FailedToApplyChanges");

	/// <summary>
	/// Failed to delete item
	/// </summary>
	public static Translation FailedToDeleteItem => _instance.GetText("FailedToDeleteItem");

	/// <summary>
	/// Failed to delete your playset
	/// </summary>
	public static Translation FailedToDeletePlayset => _instance.GetText("FailedToDeletePlayset");

	/// <summary>
	/// Failed to download this playset.  Please try again later.
	/// </summary>
	public static Translation FailedToDownloadPlayset => _instance.GetText("FailedToDownloadPlayset");

	/// <summary>
	/// Failed to fetch your logs
	/// </summary>
	public static Translation FailedToFetchLogs => _instance.GetText("FailedToFetchLogs");

	/// <summary>
	/// Failed to import this playset.
	/// </summary>
	public static Translation FailedToImportPlayset => _instance.GetText("FailedToImportPlayset");

	/// <summary>
	/// Failed to open Theme Changer
	/// </summary>
	public static Translation FailedToOpenTC => _instance.GetText("FailedToOpenTC");

	/// <summary>
	/// Failed to retrieve the Playset Discovery
	/// </summary>
	public static Translation FailedToRetrievePlaysets => _instance.GetText("FailedToRetrievePlaysets");

	/// <summary>
	/// Failed to save the selected language
	/// </summary>
	public static Translation FailedToSaveLanguage => _instance.GetText("FailedToSaveLanguage");

	/// <summary>
	/// Failed to update your playset
	/// </summary>
	public static Translation FailedToUpdatePlayset => _instance.GetText("FailedToUpdatePlayset");

	/// <summary>
	/// Failed to upload your playset
	/// </summary>
	public static Translation FailedToUploadPlayset => _instance.GetText("FailedToUploadPlayset");

	/// <summary>
	/// Faulty Packages Detected
	/// </summary>
	public static Translation FaultyPackagesTitle => _instance.GetText("FaultyPackagesTitle");

	/// <summary>
	/// Favorite Playsets
	/// </summary>
	public static Translation FavoritePlaysets => _instance.GetText("FavoritePlaysets");

	/// <summary>
	/// Playsets favorited: {0}, Total: {1}
	/// </summary>
	public static Translation FavoritePlaysetTotal => _instance.GetText("FavoritePlaysetTotal");

	/// <summary>
	/// Favorite this playset
	/// </summary>
	public static Translation FavoriteThisPlayset => _instance.GetText("FavoriteThisPlayset");

	/// <summary>
	/// Total playsets: {0}
	/// </summary>
	public static Translation FavoriteTotal => _instance.GetText("FavoriteTotal");

	/// <summary>
	/// Only show disabled items.
	/// </summary>
	public static Translation FilterByDisabled => _instance.GetText("FilterByDisabled");

	/// <summary>
	/// Only show enabled items.
	/// </summary>
	public static Translation FilterByEnabled => _instance.GetText("FilterByEnabled");

	/// <summary>
	/// Only show excluded items.
	/// </summary>
	public static Translation FilterByExcluded => _instance.GetText("FilterByExcluded");

	/// <summary>
	/// Only show included items.
	/// </summary>
	public static Translation FilterByIncluded => _instance.GetText("FilterByIncluded");

	/// <summary>
	/// Add '{0}' to your author filters
	/// </summary>
	public static Translation FilterByThisAuthor => _instance.GetText("FilterByThisAuthor");

	/// <summary>
	/// Filter by this package status
	/// </summary>
	public static Translation FilterByThisPackageStatus => _instance.GetText("FilterByThisPackageStatus");

	/// <summary>
	/// Filter by this report status
	/// </summary>
	public static Translation FilterByThisReportStatus => _instance.GetText("FilterByThisReportStatus");

	/// <summary>
	/// Add '{0}' to the filtered tags
	/// </summary>
	public static Translation FilterByThisTag => _instance.GetText("FilterByThisTag");

	/// <summary>
	/// Hide code mods in the packages page
	/// </summary>
	public static Translation FilterOutPackagesWithMods => _instance.GetText("FilterOutPackagesWithMods");

	/// <summary>
	/// Packages that include a mod will not show on the Packages page. They will still show on the Mods page.
	/// </summary>
	public static Translation FilterOutPackagesWithModsTip => _instance.GetText("FilterOutPackagesWithMods_Tip");

	/// <summary>
	/// Hide single-asset packages from the packages page
	/// </summary>
	public static Translation FilterOutPackagesWithOneAsset => _instance.GetText("FilterOutPackagesWithOneAsset");

	/// <summary>
	/// Packages that only have a single asset in them will not show on the Packages page. They will still show on the Assets page.
	/// </summary>
	public static Translation FilterOutPackagesWithOneAssetTip => _instance.GetText("FilterOutPackagesWithOneAsset_Tip");

	/// <summary>
	/// Filter items using this date
	/// </summary>
	public static Translation FilterSinceThisDate => _instance.GetText("FilterSinceThisDate");

	/// <summary>
	/// First Backup
	/// </summary>
	public static Translation FirstBackup => _instance.GetText("FirstBackup");

	/// <summary>
	/// You are doing your first ever backup. This might take a while depending on how many savegames and files you have.  Future backups will take less time as they will only backup new or updated files.
	/// </summary>
	public static Translation FirstBackupDescription => _instance.GetText("FirstBackupDescription");

	/// <summary>
	/// Fix All Issues
	/// </summary>
	public static Translation FixAllIssues => _instance.GetText("FixAllIssues");

	/// <summary>
	/// Attempt to re-download all packages that have issues, this will open steam for a brief moment
	/// </summary>
	public static Translation FixAllTip => _instance.GetText("FixAllTip");

	/// <summary>
	/// Swap Alt+Click behavior for filtering in the content lists
	/// </summary>
	public static Translation FlipItemCopyFilterAction => _instance.GetText("FlipItemCopyFilterAction");

	/// <summary>
	/// Swaps the default behavior of clicking on labels so that normal clicking filters and Alt+Clicking performs the default action.
	/// </summary>
	public static Translation FlipItemCopyFilterActionTip => _instance.GetText("FlipItemCopyFilterAction_Tip");

	/// <summary>
	/// Folder Settings
	/// </summary>
	public static Translation FolderSettings => _instance.GetText("FolderSettings");

	/// <summary>
	/// <para>{1} follower</para>
	/// <para>Plural: {1} followers</para>
	/// </summary>
	public static Translation FollowersCount => _instance.GetText("FollowersCount");

	/// <summary>
	/// Free
	/// </summary>
	public static Translation Free => _instance.GetText("Free");

	/// <summary>
	/// Build Cities
	/// </summary>
	public static Translation GamePlay => _instance.GetText("GamePlay");

	/// <summary>
	/// {0} included &amp; enabled: {1}, out of {2}
	/// </summary>
	public static Translation GenericIncludedAndEnabledTotal => _instance.GetText("GenericIncludedAndEnabledTotal");

	/// <summary>
	/// {0} included: {1}, of which are enabled: {2}, out of {3}
	/// </summary>
	public static Translation GenericIncludedEnabledTotal => _instance.GetText("GenericIncludedEnabledTotal");

	/// <summary>
	/// {0} included: {1}, out of {2}
	/// </summary>
	public static Translation GenericIncludedTotal => _instance.GetText("GenericIncludedTotal");

	/// <summary>
	/// Generic Issue
	/// </summary>
	public static Translation GenericIssue => _instance.GetText("GenericIssue");

	/// <summary>
	/// Generic Playset
	/// </summary>
	public static Translation GenericPlayset => _instance.GetText("GenericPlayset");

	/// <summary>
	/// Help &amp; Logs
	/// </summary>
	public static Translation HelpLogs => _instance.GetText("HelpLogs");

	/// <summary>
	/// Help &amp; Reset
	/// </summary>
	public static Translation HelpReset => _instance.GetText("HelpReset");

	/// <summary>
	/// Help &amp; Support
	/// </summary>
	public static Translation HelpSupport => _instance.GetText("HelpSupport");

	/// <summary>
	/// Improve Translations
	/// </summary>
	public static Translation HelpTranslate => _instance.GetText("HelpTranslate");

	/// <summary>
	/// Help translate this tool into your native language on Crowdin.
	/// </summary>
	public static Translation HelpTranslateTip => _instance.GetText("HelpTranslate_Tip");

	/// <summary>
	/// Hide Extra Widgets
	/// </summary>
	public static Translation HideExtraWidgets => _instance.GetText("HideExtraWidgets");

	/// <summary>
	/// Hide Filters
	/// </summary>
	public static Translation HideFilters => _instance.GetText("HideFilters");

	/// <summary>
	/// ID &amp; Tags
	/// </summary>
	public static Translation IDAndTags => _instance.GetText("IDAndTags");

	/// <summary>
	/// Load text from your clipboard
	/// </summary>
	public static Translation ImportFromClipboard => _instance.GetText("ImportFromClipboard");

	/// <summary>
	/// Import a playset using a playset ID
	/// </summary>
	public static Translation ImportFromLink => _instance.GetText("ImportFromLink");

	/// <summary>
	/// Import From Text
	/// </summary>
	public static Translation ImportFromText => _instance.GetText("ImportFromText");

	/// <summary>
	/// Any text format is supported
	/// </summary>
	public static Translation ImportFromTextInfo => _instance.GetText("ImportFromTextInfo");

	/// <summary>
	/// Include
	/// </summary>
	public static Translation Include => _instance.GetText("Include");

	/// <summary>
	/// Include Auto-Save savegames while backing up savegame files
	/// </summary>
	public static Translation IncludeAutoSavesBackup => _instance.GetText("IncludeAutoSavesBackup");

	/// <summary>
	/// Included
	/// </summary>
	public static Translation Included => _instance.GetText("Included");

	/// <summary>
	/// <para>{0} {1} included</para>
	/// <para>Plural: {0} {1} included</para>
	/// </summary>
	public static Translation IncludedCount => _instance.GetText("IncludedCount");

	/// <summary>
	/// <para>{0} {1} included &amp; enabled</para>
	/// <para>Plural: {0} {1} included &amp; enabled</para>
	/// </summary>
	public static Translation IncludedEnabledCount => _instance.GetText("IncludedEnabledCount");

	/// <summary>
	/// Include / Exclude {0} from the '{1}' playset
	/// </summary>
	public static Translation IncludeExcludeOtherPlayset => _instance.GetText("IncludeExcludeOtherPlayset");

	/// <summary>
	/// Includes items you do not have
	/// </summary>
	public static Translation IncludesItemsYouDoNotHave => _instance.GetText("IncludesItemsYouDoNotHave");

	/// <summary>
	/// Info
	/// </summary>
	public static Translation Info => _instance.GetText("Info");

	/// <summary>
	/// Invalid
	/// </summary>
	public static Translation Invalid => _instance.GetText("Invalid");

	/// <summary>
	/// Invalid Folder
	/// </summary>
	public static Translation InvalidFolder => _instance.GetText("InvalidFolder");

	/// <summary>
	/// Investigate only assets and content packages during the troubleshooting process.
	/// </summary>
	public static Translation InvestigateAssets => _instance.GetText("InvestigateAssets");

	/// <summary>
	/// Investigate only code mods during the troubleshooting process.
	/// </summary>
	public static Translation InvestigateMods => _instance.GetText("InvestigateMods");

	/// <summary>
	/// <para>{0} item</para>
	/// <para>Plural: {0} items</para>
	/// </summary>
	public static Translation ItemsCount => _instance.GetText("ItemsCount");

	/// <summary>
	/// <para>{0} {1} is hidden because of your playset's usage</para>
	/// <para>Plural: {0} {1} are hidden because of your playset's usage</para>
	/// </summary>
	public static Translation ItemsHidden => _instance.GetText("ItemsHidden");

	/// <summary>
	/// You're about to subscribe to packages that might be broken or have other serious issues.
	/// </summary>
	public static Translation ItemsShouldNotBeSubscribedInfo => _instance.GetText("ItemsShouldNotBeSubscribedInfo");

	/// <summary>
	/// Get help on Discord
	/// </summary>
	public static Translation JoinDiscord => _instance.GetText("JoinDiscord");

	/// <summary>
	/// Ask for help &amp; receive updates about the tool on the Discord server.
	/// </summary>
	public static Translation JoinDiscordTip => _instance.GetText("JoinDiscord_Tip");

	/// <summary>
	/// Add extra arguments when launching the game
	/// </summary>
	public static Translation LaunchArgsInfo => _instance.GetText("LaunchArgsInfo");

	/// <summary>
	/// Launch Settings
	/// </summary>
	public static Translation LaunchSettings => _instance.GetText("LaunchSettings");

	/// <summary>
	/// Click to launch/stop Cities: Skylines or use {0} anywhere in the app
	/// </summary>
	public static Translation LaunchTooltip => _instance.GetText("LaunchTooltip");

	/// <summary>
	/// Left-Hand Traffic
	/// </summary>
	public static Translation LHT => _instance.GetText("LHT");

	/// <summary>
	/// Link assets from mods to the mod's 'Included' status
	/// </summary>
	public static Translation LinkModAssets => _instance.GetText("LinkModAssets");

	/// <summary>
	/// Some mods include assets that are required for them to function properly. Enabling this option links those assets to the mod itself and hides those assets from the Assets page so you don't exclude them by mistake.
	/// </summary>
	public static Translation LinkModAssetsTip => _instance.GetText("LinkModAssets_Tip");

	/// <summary>
	/// Links
	/// </summary>
	public static Translation Links => _instance.GetText("Links");

	/// <summary>
	/// <para>{0} {1} loaded</para>
	/// <para>Plural: {0} {1} loaded</para>
	/// </summary>
	public static Translation LoadedCount => _instance.GetText("LoadedCount");

	/// <summary>
	/// Starts with the editor's 'Load' screen on launch
	/// </summary>
	public static Translation LoadsAssetOnLaunch => _instance.GetText("LoadsAssetOnLaunch");

	/// <summary>
	/// Continue a previous savegame
	/// </summary>
	public static Translation LoadSaveGame => _instance.GetText("LoadSaveGame");

	/// <summary>
	/// Load a savegame when launching Cities: Skylines. Not selecting a save-file will continue your latest save
	/// </summary>
	public static Translation LoadSaveGameTip => _instance.GetText("LoadSaveGame_Tip");

	/// <summary>
	/// Continues your latest savegame on launch
	/// </summary>
	public static Translation LoadsSaveGameOnLaunch => _instance.GetText("LoadsSaveGameOnLaunch");

	/// <summary>
	/// Continues the '{0}' savegame on launch
	/// </summary>
	public static Translation LoadsSaveGameWithMap => _instance.GetText("LoadsSaveGameWithMap");

	/// <summary>
	/// Local
	/// </summary>
	public static Translation Local => _instance.GetText("Local");

	/// <summary>
	/// Drop or select a log file to be simplified and display the list of errors inside the log
	/// </summary>
	public static Translation LogFileDrop => _instance.GetText("LogFileDrop");

	/// <summary>
	/// Log Folders
	/// </summary>
	public static Translation LogFolders => _instance.GetText("LogFolders");

	/// <summary>
	/// Logged in as
	/// </summary>
	public static Translation LoggedInAs => _instance.GetText("LoggedInAs");

	/// <summary>
	/// Logged in as {0}
	/// </summary>
	public static Translation LoggedInUser => _instance.GetText("LoggedInUser");

	/// <summary>
	/// Copy logs to clipboard
	/// </summary>
	public static Translation LogZipCopy => _instance.GetText("LogZipCopy");

	/// <summary>
	/// Create a zip file containing all logging information needed for someone to assist you with your issue, you can paste it directly into a discord channel.
	/// </summary>
	public static Translation LogZipCopyTip => _instance.GetText("LogZipCopy_Tip");

	/// <summary>
	/// Save logs to file
	/// </summary>
	public static Translation LogZipFile => _instance.GetText("LogZipFile");

	/// <summary>
	/// Create a zip file containing all logging information needed for someone to assist you with your issue, the folder containing the file will open automatically.
	/// </summary>
	public static Translation LogZipFileTip => _instance.GetText("LogZipFile_Tip");

	/// <summary>
	/// Maintenance
	/// </summary>
	public static Translation Maintenance => _instance.GetText("Maintenance");

	/// <summary>
	/// Make this playset Private
	/// </summary>
	public static Translation MakePrivate => _instance.GetText("MakePrivate");

	/// <summary>
	/// Make this playset Public
	/// </summary>
	public static Translation MakePublic => _instance.GetText("MakePublic");

	/// <summary>
	/// Manage
	/// </summary>
	public static Translation Manage => _instance.GetText("Manage");

	/// <summary>
	/// Select a map file to start a new game with
	/// </summary>
	public static Translation MapFileInfo => _instance.GetText("MapFileInfo");

	/// <summary>
	/// Mark all as read
	/// </summary>
	public static Translation MarkAllAsRead => _instance.GetText("MarkAllAsRead");

	/// <summary>
	/// Mark as read
	/// </summary>
	public static Translation MarkAsRead => _instance.GetText("MarkAsRead");

	/// <summary>
	/// You're about to subscribe to {0} mods. Are you sure you want to continue?
	/// </summary>
	public static Translation MassSubscribeWarningInfo => _instance.GetText("MassSubscribeWarningInfo");

	/// <summary>
	/// Mass subscription warning
	/// </summary>
	public static Translation MassSubscribeWarningTitle => _instance.GetText("MassSubscribeWarningTitle");

	/// <summary>
	/// Missing
	/// </summary>
	public static Translation Missing => _instance.GetText("Missing");

	/// <summary>
	/// Missing Package
	/// </summary>
	public static Translation MissingPackage => _instance.GetText("MissingPackage");

	/// <summary>
	/// Missing Packages
	/// </summary>
	public static Translation MissingPackages => _instance.GetText("MissingPackages");

	/// <summary>
	/// <para>Mod</para>
	/// <para>Plural: Mods</para>
	/// </summary>
	public static Translation Mod => _instance.GetText("Mod");

	/// <summary>
	/// Mod ID
	/// </summary>
	public static Translation ModID => _instance.GetText("ModID");

	/// <summary>
	/// Item is private
	/// </summary>
	public static Translation ModIsPrivate => _instance.GetText("ModIsPrivate");

	/// <summary>
	/// Mods Enabled
	/// </summary>
	public static Translation ModsEnabled => _instance.GetText("ModsEnabled");

	/// <summary>
	/// <para>Copy package to a local folder</para>
	/// <para>Plural: Copy all selected packages to a local folder</para>
	/// </summary>
	public static Translation MovePackageToLocalFolder => _instance.GetText("MovePackageToLocalFolder");

	/// <summary>
	/// Multiple Skyves Detected
	/// </summary>
	public static Translation MultipleSkyvesDetected => _instance.GetText("MultipleSkyvesDetected");

	/// <summary>
	/// New
	/// </summary>
	public static Translation New => _instance.GetText("New");

	/// <summary>
	/// Start a new game
	/// </summary>
	public static Translation NewGame => _instance.GetText("NewGame");

	/// <summary>
	/// Starts a new game when launching Cities: Skylines. You can specify the map you'd like to start in
	/// </summary>
	public static Translation NewGameTip => _instance.GetText("NewGame_Tip");

	/// <summary>
	/// New Packages
	/// </summary>
	public static Translation NewPackages => _instance.GetText("NewPackages");

	/// <summary>
	/// <para>'{1}' got installed since your last session</para>
	/// <para>Plural: {0} new packages got installed since your last session</para>
	/// </summary>
	public static Translation NewPackagesSinceSession => _instance.GetText("NewPackagesSinceSession");

	/// <summary>
	/// Create or import a new playset
	/// </summary>
	public static Translation NewPlaysetTip => _instance.GetText("NewPlayset_Tip");

	/// <summary>
	/// Next Stage
	/// </summary>
	public static Translation NextStage => _instance.GetText("NextStage");

	/// <summary>
	/// No Active Playset
	/// </summary>
	public static Translation NoActivePlayset => _instance.GetText("NoActivePlayset");

	/// <summary>
	/// Disable All Assets
	/// </summary>
	public static Translation NoAssets => _instance.GetText("NoAssets");

	/// <summary>
	/// <para>No Compatibility issues detected.</para>
	/// <para>Plural: No compatibility issues with your {0} were detected.</para>
	/// </summary>
	public static Translation NoCompatibilityIssues => _instance.GetText("NoCompatibilityIssues");

	/// <summary>
	/// Could not load DLCs, please connect to the internet
	/// </summary>
	public static Translation NoDlcsNoInternet => _instance.GetText("NoDlcsNoInternet");

	/// <summary>
	/// No DLCs match your search
	/// </summary>
	public static Translation NoDlcsOpenGame => _instance.GetText("NoDlcsOpenGame");

	/// <summary>
	/// No packages found on your computer.
	/// </summary>
	public static Translation NoLocalPackagesFound => _instance.GetText("NoLocalPackagesFound");

	/// <summary>
	/// No logs match what you're looking for.
	/// </summary>
	public static Translation NoLogsMatchFilters => _instance.GetText("NoLogsMatchFilters");

	/// <summary>
	/// Disable All Code-Mods
	/// </summary>
	public static Translation NoMods => _instance.GetText("NoMods");

	/// <summary>
	/// No notifications
	/// </summary>
	public static Translation NoNotifications => _instance.GetText("NoNotifications");

	/// <summary>
	/// No packages match what you're looking for.
	/// </summary>
	public static Translation NoPackagesMatchFilters => _instance.GetText("NoPackagesMatchFilters");

	/// <summary>
	/// No playsets to display
	/// </summary>
	public static Translation NoPlaysetsFound => _instance.GetText("NoPlaysetsFound");

	/// <summary>
	/// No playsets match your search
	/// </summary>
	public static Translation NoPlaysetsMatchFilters => _instance.GetText("NoPlaysetsMatchFilters");

	/// <summary>
	/// Disable Thumbnail
	/// </summary>
	public static Translation NoThumbnail => _instance.GetText("NoThumbnail");

	/// <summary>
	/// Notifications
	/// </summary>
	public static Translation Notifications => _instance.GetText("Notifications");

	/// <summary>
	/// OBSOLETE
	/// </summary>
	public static Translation OBSOLETE => _instance.GetText("OBSOLETE");

	/// <summary>
	/// Only show warnings and errors
	/// </summary>
	public static Translation OnlyShowErrors => _instance.GetText("OnlyShowErrors");

	/// <summary>
	/// Open the author's page
	/// </summary>
	public static Translation OpenAuthorPage => _instance.GetText("OpenAuthorPage");

	/// <summary>
	/// Open Change-log
	/// </summary>
	public static Translation OpenChangelog => _instance.GetText("OpenChangelog");

	/// <summary>
	/// Open the Wiki
	/// </summary>
	public static Translation OpenGuide => _instance.GetText("OpenGuide");

	/// <summary>
	/// View the online guide to get started with Skyve.
	/// </summary>
	public static Translation OpenGuideTip => _instance.GetText("OpenGuide_Tip");

	/// <summary>
	/// Open the folder containing this package
	/// </summary>
	public static Translation OpenLocalFolder => _instance.GetText("OpenLocalFolder");

	/// <summary>
	/// Open Game Log
	/// </summary>
	public static Translation OpenLog => _instance.GetText("OpenLog");

	/// <summary>
	/// Open the game's log folder
	/// </summary>
	public static Translation OpenLogFolder => _instance.GetText("OpenLogFolder");

	/// <summary>
	/// Open Skyve's log folder
	/// </summary>
	public static Translation OpenLOTLogFolder => _instance.GetText("OpenLOTLogFolder");

	/// <summary>
	/// <para>Open file location</para>
	/// <para>Plural: Open all selected packages' file locations</para>
	/// </summary>
	public static Translation OpenPackageFolder => _instance.GetText("OpenPackageFolder");

	/// <summary>
	/// Open this Package's page
	/// </summary>
	public static Translation OpenPackagePage => _instance.GetText("OpenPackagePage");

	/// <summary>
	/// View this playset's file on your computer
	/// </summary>
	public static Translation OpenPlaysetFolder => _instance.GetText("OpenPlaysetFolder");

	/// <summary>
	/// Open this playset's page
	/// </summary>
	public static Translation OpenPlaysetPage => _instance.GetText("OpenPlaysetPage");

	/// <summary>
	/// Open Skyve Log
	/// </summary>
	public static Translation OpenSkyveLog => _instance.GetText("OpenSkyveLog");

	/// <summary>
	/// Please open Steam in order to continue your previous task.
	/// </summary>
	public static Translation OpenSteamToContinue => _instance.GetText("OpenSteamToContinue");

	/// <summary>
	/// Other Playsets
	/// </summary>
	public static Translation OtherPlaysets => _instance.GetText("OtherPlaysets");

	/// <summary>
	/// {0} out of {1}
	/// </summary>
	public static Translation OutOf => _instance.GetText("OutOf");

	/// <summary>
	/// Out of date
	/// </summary>
	public static Translation OutOfDate => _instance.GetText("OutOfDate");

	/// <summary>
	/// Out of date {0}
	/// </summary>
	public static Translation OutOfDateItem => _instance.GetText("OutOfDateItem");

	/// <summary>
	/// Owned
	/// </summary>
	public static Translation Owned => _instance.GetText("Owned");

	/// <summary>
	/// <para>Package</para>
	/// <para>Plural: Packages</para>
	/// </summary>
	public static Translation Package => _instance.GetText("Package");

	/// <summary>
	/// '{0}' may be out of date, it was updated {1}
	/// </summary>
	public static Translation PackageIsMaybeOutOfDate => _instance.GetText("PackageIsMaybeOutOfDate");

	/// <summary>
	/// '{0}' was removed from your computer
	/// </summary>
	public static Translation PackageIsNotDownloaded => _instance.GetText("PackageIsNotDownloaded");

	/// <summary>
	/// '{0}' is out of date You are {1} behind
	/// </summary>
	public static Translation PackageIsOutOfDate => _instance.GetText("PackageIsOutOfDate");

	/// <summary>
	/// <para>'{0}' is out of date, you are on an older version of the mod.</para>
	/// <para>Plural: {0} {1} are out of date, you are on older versions of these mods.</para>
	/// </summary>
	public static Translation PackageIsOutOfDateVersion => _instance.GetText("PackageIsOutOfDateVersion");

	/// <summary>
	/// Packages Enabled
	/// </summary>
	public static Translation PackagesEnabled => _instance.GetText("PackagesEnabled");

	/// <summary>
	/// Packages Included &amp; Disabled
	/// </summary>
	public static Translation PackagesIncludedDisabled => _instance.GetText("PackagesIncludedDisabled");

	/// <summary>
	/// Packages Selected ({0})
	/// </summary>
	public static Translation PackagesSelected => _instance.GetText("PackagesSelected");

	/// <summary>
	/// Click on a package to select it, click on 'Confirm Selection' once you're finished.
	/// </summary>
	public static Translation PackagesSelectionInfo => _instance.GetText("PackagesSelectionInfo");

	/// <summary>
	/// Package Status
	/// </summary>
	public static Translation PackageStatus => _instance.GetText("PackageStatus");

	/// <summary>
	/// <para>'{1}' got updated since your last session</para>
	/// <para>Plural: {0} packages got updated since your last session</para>
	/// </summary>
	public static Translation PackagesUpdatedSinceSession => _instance.GetText("PackagesUpdatedSinceSession");

	/// <summary>
	/// Package Updates
	/// </summary>
	public static Translation PackageUpdates => _instance.GetText("PackageUpdates");

	/// <summary>
	/// Package Usage
	/// </summary>
	public static Translation PackageUsage => _instance.GetText("PackageUsage");

	/// <summary>
	/// Partially downloaded
	/// </summary>
	public static Translation PartiallyDownloaded => _instance.GetText("PartiallyDownloaded");

	/// <summary>
	/// Partially Included
	/// </summary>
	public static Translation PartiallyIncluded => _instance.GetText("PartiallyIncluded");

	/// <summary>
	/// Paste the ID of the playset you'd like to import
	/// </summary>
	public static Translation PastePlaysetId => _instance.GetText("PastePlaysetId");

	/// <summary>
	/// <para>Playset</para>
	/// <para>Plural: Playsets</para>
	/// </summary>
	public static Translation Playset => _instance.GetText("Playset");

	/// <summary>
	/// No custom color selected
	/// </summary>
	public static Translation PlaysetColorNotSet => _instance.GetText("PlaysetColorNotSet");

	/// <summary>
	/// Custom color enabled
	/// </summary>
	public static Translation PlaysetColorSet => _instance.GetText("PlaysetColorSet");

	/// <summary>
	/// Delete this playset
	/// </summary>
	public static Translation PlaysetDelete => _instance.GetText("PlaysetDelete");

	/// <summary>
	/// Remove all of this playset's mods from your active playset
	/// </summary>
	public static Translation PlaysetExclude => _instance.GetText("PlaysetExclude");

	/// <summary>
	/// Playset Filter
	/// </summary>
	public static Translation PlaysetFilter => _instance.GetText("PlaysetFilter");

	/// <summary>
	/// Add all of this playset's mods to your active playset
	/// </summary>
	public static Translation PlaysetMerge => _instance.GetText("PlaysetMerge");

	/// <summary>
	/// Playset Name
	/// </summary>
	public static Translation PlaysetName => _instance.GetText("PlaysetName");

	/// <summary>
	/// Your playset is still loading, please wait before exiting this page.
	/// </summary>
	public static Translation PlaysetStillLoading => _instance.GetText("PlaysetStillLoading");

	/// <summary>
	/// No custom thumbnail selected
	/// </summary>
	public static Translation PlaysetThumbnailNotSet => _instance.GetText("PlaysetThumbnailNotSet");

	/// <summary>
	/// Custom thumbnail enabled
	/// </summary>
	public static Translation PlaysetThumbnailSet => _instance.GetText("PlaysetThumbnailSet");

	/// <summary>
	/// Playset Usage
	/// </summary>
	public static Translation PlaysetUsage => _instance.GetText("PlaysetUsage");

	/// <summary>
	/// Selecting a dedicated usage for your playset allows Skyve to automatically hide mods that either do not match, or are not compatible with, your selected usage.
	/// </summary>
	public static Translation PlaysetUsageInfo => _instance.GetText("PlaysetUsageInfo");

	/// <summary>
	/// Preferences
	/// </summary>
	public static Translation Preferences => _instance.GetText("Preferences");

	/// <summary>
	/// Ready
	/// </summary>
	public static Translation Ready => _instance.GetText("Ready");

	/// <summary>
	/// Recently Updated
	/// </summary>
	public static Translation RecentlyUpdated => _instance.GetText("RecentlyUpdated");

	/// <summary>
	/// Recently Updated {0}
	/// </summary>
	public static Translation RecentlyUpdatedItem => _instance.GetText("RecentlyUpdatedItem");

	/// <summary>
	/// <para>Re-download this package</para>
	/// <para>Plural: Re-download all selected Packages</para>
	/// </summary>
	public static Translation ReDownloadPackage => _instance.GetText("ReDownloadPackage");

	/// <summary>
	/// References
	/// </summary>
	public static Translation References => _instance.GetText("References");

	/// <summary>
	/// Reload all data
	/// </summary>
	public static Translation ReloadAllData => _instance.GetText("ReloadAllData");

	/// <summary>
	/// Removed by author
	/// </summary>
	public static Translation RemovedByAuthor => _instance.GetText("RemovedByAuthor");

	/// <summary>
	/// Rename this playset
	/// </summary>
	public static Translation RenamePlayset => _instance.GetText("RenamePlayset");

	/// <summary>
	/// Reply to requester
	/// </summary>
	public static Translation ReplyToRequester => _instance.GetText("ReplyToRequester");

	/// <summary>
	/// Reset Options
	/// </summary>
	public static Translation ResetButton => _instance.GetText("ResetButton");

	/// <summary>
	/// Reset your settings to the default ones. This does not affect folder settings.
	/// </summary>
	public static Translation ResetButtonTip => _instance.GetText("ResetButton_Tip");

	/// <summary>
	/// Reset compatibility's cache
	/// </summary>
	public static Translation ResetCompatibilityCache => _instance.GetText("ResetCompatibilityCache");

	/// <summary>
	/// Reset all folder settings
	/// </summary>
	public static Translation ResetFolderButton => _instance.GetText("ResetFolderButton");

	/// <summary>
	/// This action will require you to launch the game before you can use the tool again.
	/// </summary>
	public static Translation ResetFolderButtonTip => _instance.GetText("ResetFolderButton_Tip");

	/// <summary>
	/// Clear cached images
	/// </summary>
	public static Translation ResetImageCache => _instance.GetText("ResetImageCache");

	/// <summary>
	/// Caution! Resetting anything can not be undone.
	/// </summary>
	public static Translation ResetInfo => _instance.GetText("ResetInfo");

	/// <summary>
	/// Reset Layout
	/// </summary>
	public static Translation ResetLayout => _instance.GetText("ResetLayout");

	/// <summary>
	/// Reset mods' cache
	/// </summary>
	public static Translation ResetModsCache => _instance.GetText("ResetModsCache");

	/// <summary>
	/// Reset this playset's color
	/// </summary>
	public static Translation ResetPlaysetColor => _instance.GetText("ResetPlaysetColor");

	/// <summary>
	/// Reset this playset's thumbnail
	/// </summary>
	public static Translation ResetPlaysetImage => _instance.GetText("ResetPlaysetImage");

	/// <summary>
	/// Reset scrolling when opening a package's page
	/// </summary>
	public static Translation ResetScrollOnPackageClick => _instance.GetText("ResetScrollOnPackageClick");

	/// <summary>
	/// When opening a package's page, resets the scrolling so that the selected package is at the top of the list.
	/// </summary>
	public static Translation ResetScrollOnPackageClickTip => _instance.GetText("ResetScrollOnPackageClick_Tip");

	/// <summary>
	/// Reset all snoozes
	/// </summary>
	public static Translation ResetSnoozes => _instance.GetText("ResetSnoozes");

	/// <summary>
	/// Restart Required
	/// </summary>
	public static Translation RestartRequired => _instance.GetText("RestartRequired");

	/// <summary>
	/// Restore this backup
	/// </summary>
	public static Translation RestoreBackup => _instance.GetText("RestoreBackup");

	/// <summary>
	/// Select which backups you'd like to restore. A backup will be taken before the restore process begins to safeguard your current files.
	/// </summary>
	public static Translation RestoreDescriptionInfo => _instance.GetText("RestoreDescriptionInfo");

	/// <summary>
	/// Failed to restore this backup. Check the logs for more information
	/// </summary>
	public static Translation RestoreFailed => _instance.GetText("RestoreFailed");

	/// <summary>
	/// <para>Are you sure you want to restore this backup of {1}?</para>
	/// <para>Plural: Are you sure you want to restore these backups?</para>
	/// </summary>
	public static Translation RestoreFileConfirm => _instance.GetText("RestoreFileConfirm");

	/// <summary>
	/// <para>One restore point from {1}</para>
	/// <para>Plural: {0} restore points, most recently from {1}</para>
	/// </summary>
	public static Translation RestorePoint => _instance.GetText("RestorePoint");

	/// <summary>
	/// Restore Successful
	/// </summary>
	public static Translation RestoreSuccessful => _instance.GetText("RestoreSuccessful");

	/// <summary>
	/// The issue you've reported has now been fixed. You can use the {0} again.
	/// </summary>
	public static Translation ReviewIsFixed => _instance.GetText("ReviewIsFixed");

	/// <summary>
	/// The issue you've reported needs more information to be identified. Please send a new Request with a savegame attached to it to be reviewed.
	/// </summary>
	public static Translation ReviewIsSaveFileNeeded => _instance.GetText("ReviewIsSaveFileNeeded");

	/// <summary>
	/// The compatibility information of '{0}' was updated after your review request.
	/// </summary>
	public static Translation ReviewIsUpdated => _instance.GetText("ReviewIsUpdated");

	/// <summary>
	/// Your review request for '{0}' has been received but is still pending review.
	/// </summary>
	public static Translation ReviewPending => _instance.GetText("ReviewPending");

	/// <summary>
	/// Failed to send your review request for '{0}'.  {1}
	/// </summary>
	public static Translation ReviewRequestFailed => _instance.GetText("ReviewRequestFailed");

	/// <summary>
	/// Review Request Update
	/// </summary>
	public static Translation ReviewRequestNotification => _instance.GetText("ReviewRequestNotification");

	/// <summary>
	/// Your review request for '{0}' has a new update
	/// </summary>
	public static Translation ReviewRequestNotificationInfo => _instance.GetText("ReviewRequestNotificationInfo");

	/// <summary>
	/// Your review request for '{0}' has been sent!  Thank you for your feedback.
	/// </summary>
	public static Translation ReviewRequestSent => _instance.GetText("ReviewRequestSent");

	/// <summary>
	/// Select a savegame to send with your request to help us investigate the issue
	/// </summary>
	public static Translation ReviewSaveFileInfo => _instance.GetText("ReviewSaveFileInfo");

	/// <summary>
	/// Do a backup each time the game is closed
	/// </summary>
	public static Translation ScheduleOnGameClose => _instance.GetText("ScheduleOnGameClose");

	/// <summary>
	/// Do a backup each time a city is saved
	/// </summary>
	public static Translation ScheduleOnNewSave => _instance.GetText("ScheduleOnNewSave");

	/// <summary>
	/// Do daily backups on specific times
	/// </summary>
	public static Translation ScheduleOnSpecificTimes => _instance.GetText("ScheduleOnSpecificTimes");

	/// <summary>
	/// Search through your DLCs..
	/// </summary>
	public static Translation SearchDlcs => _instance.GetText("SearchDlcs");

	/// <summary>
	/// Search through these packages..
	/// </summary>
	public static Translation SearchGenericPackages => _instance.GetText("SearchGenericPackages");

	/// <summary>
	/// Search through your packages..
	/// </summary>
	public static Translation SearchPackages => _instance.GetText("SearchPackages");

	/// <summary>
	/// Search through these Playsets..
	/// </summary>
	public static Translation SearchPlaysets => _instance.GetText("SearchPlaysets");

	/// <summary>
	/// Search the workshop instead...
	/// </summary>
	public static Translation SearchWorkshop => _instance.GetText("SearchWorkshop");

	/// <summary>
	/// Search the workshop in your browser...
	/// </summary>
	public static Translation SearchWorkshopBrowser => _instance.GetText("SearchWorkshopBrowser");

	/// <summary>
	/// Select all items
	/// </summary>
	public static Translation SelectAll => _instance.GetText("SelectAll");

	/// <summary>
	/// The file you selected is not valid
	/// </summary>
	public static Translation SelectedFileInvalid => _instance.GetText("SelectedFileInvalid");

	/// <summary>
	/// Select a package
	/// </summary>
	public static Translation SelectPackage => _instance.GetText("SelectPackage");

	/// <summary>
	/// Select a restore point from the list of backups to start the restore process.
	/// </summary>
	public static Translation SelectRestoreInfo => _instance.GetText("SelectRestoreInfo");

	/// <summary>
	/// Select this package
	/// </summary>
	public static Translation SelectThisPackage => _instance.GetText("SelectThisPackage");

	/// <summary>
	/// Send the reply
	/// </summary>
	public static Translation SendReply => _instance.GetText("SendReply");

	/// <summary>
	/// Send the request for review
	/// </summary>
	public static Translation SendReview => _instance.GetText("SendReview");

	/// <summary>
	/// Set up the settings first.
	/// </summary>
	public static Translation SetupSettingsFirst => _instance.GetText("SetupSettingsFirst");

	/// <summary>
	/// Upload this playset
	/// </summary>
	public static Translation SharePlayset => _instance.GetText("SharePlayset");

	/// <summary>
	/// Show excluded packages in the 'References' tab
	/// </summary>
	public static Translation ShowAllReferencedPackages => _instance.GetText("ShowAllReferencedPackages");

	/// <summary>
	/// Displays both excluded and included packages that reference the one you're viewing.
	/// </summary>
	public static Translation ShowAllReferencedPackagesTip => _instance.GetText("ShowAllReferencedPackages_Tip");

	/// <summary>
	/// Show dates relatively to the current time
	/// </summary>
	public static Translation ShowDatesRelatively => _instance.GetText("ShowDatesRelatively");

	/// <summary>
	/// Changes the format of dates that are displayed in content lists to relative time. For example; Yesterday at 2:10 pm
	/// </summary>
	public static Translation ShowDatesRelativelyTip => _instance.GetText("ShowDatesRelatively_Tip");

	/// <summary>
	/// Show Filters
	/// </summary>
	public static Translation ShowFilters => _instance.GetText("ShowFilters");

	/// <summary>
	/// Shows the folder settings used by the tool in the options page.
	/// </summary>
	public static Translation ShowFolderSettingsTip => _instance.GetText("ShowFolderSettings_Tip");

	/// <summary>
	/// <para>Showing {0} {1}</para>
	/// <para>Plural: Showing {0} {1}</para>
	/// </summary>
	public static Translation ShowingCount => _instance.GetText("ShowingCount");

	/// <summary>
	/// Skyve detected that some of your packages are not perfectly downloaded, would you like to try fixing them first?
	/// </summary>
	public static Translation SkyveDetectedFaultyPackages => _instance.GetText("SkyveDetectedFaultyPackages");

	/// <summary>
	/// Show, and snap dashboard sections to, a grid guide
	/// </summary>
	public static Translation SnapDashToGrid => _instance.GetText("SnapDashToGrid");

	/// <summary>
	/// While editing dashboard sections, displays a 12-column grid which sections snap to.
	/// </summary>
	public static Translation SnapDashToGridTip => _instance.GetText("SnapDashToGrid_Tip");

	/// <summary>
	/// Snooze
	/// </summary>
	public static Translation Snooze => _instance.GetText("Snooze");

	/// <summary>
	/// Some packages will be removed by changing the usage of this playset.
	/// </summary>
	public static Translation SomePackagesWillBeDisabled => _instance.GetText("SomePackagesWillBeDisabled");

	/// <summary>
	/// Sort By
	/// </summary>
	public static Translation SortBy => _instance.GetText("Sort By");

	/// <summary>
	/// Author Name
	/// </summary>
	public static Translation SortingAuthor => _instance.GetText("Sorting_Author");

	/// <summary>
	/// Playset Color
	/// </summary>
	public static Translation SortingColor => _instance.GetText("Sorting_Color");

	/// <summary>
	/// Compatibility Report
	/// </summary>
	public static Translation SortingCompatibilityReport => _instance.GetText("Sorting_CompatibilityReport");

	/// <summary>
	/// Date Created
	/// </summary>
	public static Translation SortingDateCreated => _instance.GetText("Sorting_DateCreated");

	/// <summary>
	/// Default Sorting
	/// </summary>
	public static Translation SortingDefault => _instance.GetText("Sorting_Default");

	/// <summary>
	/// File Size
	/// </summary>
	public static Translation SortingFileSize => _instance.GetText("Sorting_FileSize");

	/// <summary>
	/// Last Edit Time
	/// </summary>
	public static Translation SortingLastEdit => _instance.GetText("Sorting_LastEdit");

	/// <summary>
	/// Last Time Used
	/// </summary>
	public static Translation SortingLastUsed => _instance.GetText("Sorting_LastUsed");

	/// <summary>
	/// Load Order
	/// </summary>
	public static Translation SortingLoadOrder => _instance.GetText("Sorting_LoadOrder");

	/// <summary>
	/// Name
	/// </summary>
	public static Translation SortingName => _instance.GetText("Sorting_Name");

	/// <summary>
	/// Package Status
	/// </summary>
	public static Translation SortingStatus => _instance.GetText("Sorting_Status");

	/// <summary>
	/// Total Subscribers
	/// </summary>
	public static Translation SortingSubscribers => _instance.GetText("Sorting_Subscribers");

	/// <summary>
	/// Subscribe Time
	/// </summary>
	public static Translation SortingSubscribeTime => _instance.GetText("Sorting_SubscribeTime");

	/// <summary>
	/// Update Time
	/// </summary>
	public static Translation SortingUpdateTime => _instance.GetText("Sorting_UpdateTime");

	/// <summary>
	/// Playset Usage
	/// </summary>
	public static Translation SortingUsage => _instance.GetText("Sorting_Usage");

	/// <summary>
	/// Total Likes
	/// </summary>
	public static Translation SortingVotes => _instance.GetText("Sorting_Votes");

	/// <summary>
	/// STABLE
	/// </summary>
	public static Translation STABLE => _instance.GetText("STABLE");

	/// <summary>
	/// Troubleshooting step {0} / {1}
	/// </summary>
	public static Translation StageCounter => _instance.GetText("StageCounter");

	/// <summary>
	/// Start Restore Process
	/// </summary>
	public static Translation StartRestore => _instance.GetText("StartRestore");

	/// <summary>
	/// Start a new playset from scratch without any included package.
	/// </summary>
	public static Translation StartScratch => _instance.GetText("StartScratch");

	/// <summary>
	/// Starts the editor with a new asset on launch
	/// </summary>
	public static Translation StartsNewAssetOnLaunch => _instance.GetText("StartsNewAssetOnLaunch");

	/// <summary>
	/// Starts a new game on launch
	/// </summary>
	public static Translation StartsNewGameOnLaunch => _instance.GetText("StartsNewGameOnLaunch");

	/// <summary>
	/// Starts a new game with the '{0}' map on launch
	/// </summary>
	public static Translation StartsNewGameWithMap => _instance.GetText("StartsNewGameWithMap");

	/// <summary>
	/// Status
	/// </summary>
	public static Translation Status => _instance.GetText("Status");

	/// <summary>
	/// Unknown Status
	/// </summary>
	public static Translation StatusUnknown => _instance.GetText("StatusUnknown");

	/// <summary>
	/// Steam is not currently running
	/// </summary>
	public static Translation SteamNotOpenTo => _instance.GetText("SteamNotOpenTo");

	/// <summary>
	/// Stop Troubleshooting?
	/// </summary>
	public static Translation StopTroubleshootTitle => _instance.GetText("StopTroubleshootTitle");

	/// <summary>
	/// Subscribers
	/// </summary>
	public static Translation Subscribers => _instance.GetText("Subscribers");

	/// <summary>
	/// <para>{1} subscriber</para>
	/// <para>Plural: {1} subscribers</para>
	/// </summary>
	public static Translation SubscribersCount => _instance.GetText("SubscribersCount");

	/// <summary>
	/// Switch to this mod
	/// </summary>
	public static Translation SwitchToItem => _instance.GetText("SwitchToItem");

	/// <summary>
	/// <para>Custom Tags: {0}</para>
	/// <para>Plural: Custom Tags: {0} packages</para>
	/// </summary>
	public static Translation TagsTitle => _instance.GetText("TagsTitle");

	/// <summary>
	/// TEST
	/// </summary>
	public static Translation TEST => _instance.GetText("TEST");

	/// <summary>
	/// TESTING
	/// </summary>
	public static Translation TESTING => _instance.GetText("TESTING");

	/// <summary>
	/// Load a text file containing Workshop IDs to easily subscribe to or manage them
	/// </summary>
	public static Translation TextImportMissingInfo => _instance.GetText("TextImportMissingInfo");

	/// <summary>
	/// Theme &amp; UI Scale
	/// </summary>
	public static Translation ThemeUIScale => _instance.GetText("ThemeUIScale");

	/// <summary>
	/// Change the app's color &amp; theme or increase the scale of the UI.
	/// </summary>
	public static Translation ThemeUIScaleTip => _instance.GetText("ThemeUIScale_Tip");

	/// <summary>
	/// This mod is required, you can not disable it.
	/// </summary>
	public static Translation ThisModIsRequiredYouCantDisableIt => _instance.GetText("ThisModIsRequiredYouCantDisableIt");

	/// <summary>
	/// <para>You will be subscribing to {0} package in this process.</para>
	/// <para>Plural: You will be subscribing to {0} packages in this process.</para>
	/// </summary>
	public static Translation ThisSubscribesTo => _instance.GetText("ThisSubscribesTo");

	/// <summary>
	/// <para>You will be unsubscribing from {0} package in this process.</para>
	/// <para>Plural: You will be unsubscribing from {0} packages in this process.</para>
	/// </summary>
	public static Translation ThisUnsubscribesFrom => _instance.GetText("ThisUnsubscribesFrom");

	/// <summary>
	/// {0} of included assets
	/// </summary>
	public static Translation TotalAssetSize => _instance.GetText("TotalAssetSize");

	/// <summary>
	/// <para>{0} total {1}</para>
	/// <para>Plural: {0} total {1}</para>
	/// </summary>
	public static Translation TotalCount => _instance.GetText("TotalCount");

	/// <summary>
	/// <para>{0} total {1}, {3}</para>
	/// <para>Plural: {0} total {1}, {3}</para>
	/// </summary>
	public static Translation TotalCountWarning => _instance.GetText("TotalCountWarning");

	/// <summary>
	/// <para>{0} total {1}, {2} selected</para>
	/// <para>Plural: {0} total {1}, {2} selected</para>
	/// </summary>
	public static Translation TotalSelectedCount => _instance.GetText("TotalSelectedCount");

	/// <summary>
	/// <para>{0} total {1}, {2} selected, {3}</para>
	/// <para>Plural: {0} total {1}, {2} selected, {3}</para>
	/// </summary>
	public static Translation TotalSelectedCountWarning => _instance.GetText("TotalSelectedCountWarning");

	/// <summary>
	/// Treat optional packages as required
	/// </summary>
	public static Translation TreatOptionalAsRequired => _instance.GetText("TreatOptionalAsRequired");

	/// <summary>
	/// Essentially, this displays the missing dependency notification for optional packages.
	/// </summary>
	public static Translation TreatOptionalAsRequiredTip => _instance.GetText("TreatOptionalAsRequired_Tip");

	/// <summary>
	/// Applying your action failed.
	/// </summary>
	public static Translation TroubleshootActionFailed => _instance.GetText("TroubleshootActionFailed");

	/// <summary>
	/// Did you still experience any issues with the current mod configuration?
	/// </summary>
	public static Translation TroubleshootAskIfFixed => _instance.GetText("TroubleshootAskIfFixed");

	/// <summary>
	/// Would you like to stop the troubleshooting here and keep the current configuration of packages?  Select 'No' to continue troubleshooting.
	/// </summary>
	public static Translation TroubleshootAskToStop => _instance.GetText("TroubleshootAskToStop");

	/// <summary>
	/// This is the troubleshooting menu, it will guide you throughout the process. You can close Skyve and continue later, it will remember your progress.
	/// </summary>
	public static Translation TroubleshootBubbleTip => _instance.GetText("TroubleshootBubbleTip");

	/// <summary>
	/// Another action is already ongoing. Wait for it, or restart Skyve if it seems stuck.
	/// </summary>
	public static Translation TroubleshootBusy => _instance.GetText("TroubleshootBusy");

	/// <summary>
	/// Stop the troubleshooting process.
	/// </summary>
	public static Translation TroubleshootCancelTip => _instance.GetText("TroubleshootCancelTip");

	/// <summary>
	/// I'm having issues with a mod or asset, but I'm not so sure which one is causing it.
	/// </summary>
	public static Translation TroubleshootCaused => _instance.GetText("TroubleshootCaused");

	/// <summary>
	/// The following packages might be the cause of your issues, the current troubleshoot playset will be kept for you to use.
	/// </summary>
	public static Translation TroubleshootCauseResult => _instance.GetText("TroubleshootCauseResult");

	/// <summary>
	/// <para>You have a package with a serious compatibility warning. You might want to try dealing with it first before troubleshooting.</para>
	/// <para>Plural: You have {0} packages with serious compatibility warnings. You might want to try dealing with them first before troubleshooting.</para>
	/// </summary>
	public static Translation TroubleshootCompAsk => _instance.GetText("TroubleshootCompAsk");

	/// <summary>
	/// Could not create a new playset for troubleshooting. Try again later or after restarting Skyve.
	/// </summary>
	public static Translation TroubleshootCouldNotCreatePlayset => _instance.GetText("TroubleshootCouldNotCreatePlayset");

	/// <summary>
	/// An error occurred, try again later, or check the logs for more information.
	/// </summary>
	public static Translation TroubleshootError => _instance.GetText("TroubleshootError");

	/// <summary>
	/// The troubleshooting was inconclusive. The issue might not be related to a specific mod, or it might be caused by multiple mods.
	/// </summary>
	public static Translation TroubleshootInconclusive => _instance.GetText("TroubleshootInconclusive");

	/// <summary>
	/// If you have a mod or asset that is causing an issue, or that is missing, and you don't known which one; Try troubleshooting it here:
	/// </summary>
	public static Translation TroubleshootInfo => _instance.GetText("TroubleshootInfo");

	/// <summary>
	/// The troubleshooting state is no longer valid. Please start a new troubleshooting session.
	/// </summary>
	public static Translation TroubleshootInvalidState => _instance.GetText("TroubleshootInvalidState");

	/// <summary>
	/// Troubleshoot Issues
	/// </summary>
	public static Translation TroubleshootIssues => _instance.GetText("TroubleshootIssues");

	/// <summary>
	/// I think I'm missing some mods or assets from my current playset that I've previously used in other playsets.
	/// </summary>
	public static Translation TroubleshootMissing => _instance.GetText("TroubleshootMissing");

	/// <summary>
	/// What kind of content would you like to troubleshoot?
	/// </summary>
	public static Translation TroubleshootModOrAsset => _instance.GetText("TroubleshootModOrAsset");

	/// <summary>
	/// I recently subscribed to new mods, or some mods recently updated, and my game suddenly started having issues.
	/// </summary>
	public static Translation TroubleshootNew => _instance.GetText("TroubleshootNew");

	/// <summary>
	/// Skip the current stage. Use it if you've already completed it and want to proceed.
	/// </summary>
	public static Translation TroubleshootNextStageTip => _instance.GetText("TroubleshootNextStageTip");

	/// <summary>
	/// You need to have an active playset to troubleshoot.
	/// </summary>
	public static Translation TroubleshootNoActivePlayset => _instance.GetText("TroubleshootNoActivePlayset");

	/// <summary>
	/// No mods match what you're trying to troubleshoot.
	/// </summary>
	public static Translation TroubleshootNoPacakgesToProcess => _instance.GetText("TroubleshootNoPacakgesToProcess");

	/// <summary>
	/// Which option best describes your situation?
	/// </summary>
	public static Translation TroubleshootSelection => _instance.GetText("TroubleshootSelection");

	/// <summary>
	/// Skip this step and continue with the troubleshooting steps.
	/// </summary>
	public static Translation TroubleshootSkipComp => _instance.GetText("TroubleshootSkipComp");

	/// <summary>
	/// Go back and view your compatibility report's page to make sure you're not using broken mods.
	/// </summary>
	public static Translation TroubleshootViewComp => _instance.GetText("TroubleshootViewComp");

	/// <summary>
	/// Un-Favorite this playset
	/// </summary>
	public static Translation UnFavoriteThisPlayset => _instance.GetText("UnFavoriteThisPlayset");

	/// <summary>
	/// Unfiltered
	/// </summary>
	public static Translation Unfiltered => _instance.GetText("Unfiltered");

	/// <summary>
	/// Unknown Package
	/// </summary>
	public static Translation UnknownPackage => _instance.GetText("UnknownPackage");

	/// <summary>
	/// Unknown User
	/// </summary>
	public static Translation UnknownUser => _instance.GetText("UnknownUser");

	/// <summary>
	/// There are un-saved changes to the packages of this playset
	/// </summary>
	public static Translation UnsavedChangesPlayset => _instance.GetText("UnsavedChangesPlayset");

	/// <summary>
	/// Un-Snooze
	/// </summary>
	public static Translation UnSnooze => _instance.GetText("UnSnooze");

	/// <summary>
	/// Unsubscribe
	/// </summary>
	public static Translation Unsubscribe => _instance.GetText("Unsubscribe");

	/// <summary>
	/// Update Playset
	/// </summary>
	public static Translation UpdatePlayset => _instance.GetText("UpdatePlayset");

	/// <summary>
	/// Update your local version of this playset
	/// </summary>
	public static Translation UpdatePlaysetTip => _instance.GetText("UpdatePlaysetTip");

	/// <summary>
	/// Update Time
	/// </summary>
	public static Translation UpdateTime => _instance.GetText("UpdateTime");

	/// <summary>
	/// Up to date
	/// </summary>
	public static Translation UpToDate => _instance.GetText("UpToDate");

	/// <summary>
	/// {0} Playset
	/// </summary>
	public static Translation UsagePlayset => _instance.GetText("UsagePlayset");

	/// <summary>
	/// * Please try to use English in your description.
	/// </summary>
	public static Translation UseEnglishPlease => _instance.GetText("UseEnglishPlease");

	/// <summary>
	/// Vanilla
	/// </summary>
	public static Translation Vanilla => _instance.GetText("Vanilla");

	/// <summary>
	/// Vanilla Maps
	/// </summary>
	public static Translation VanillaMaps => _instance.GetText("Vanilla Maps");

	/// <summary>
	/// Verified Author
	/// </summary>
	public static Translation VerifiedAuthor => _instance.GetText("VerifiedAuthor");

	/// <summary>
	/// Version
	/// </summary>
	public static Translation Version => _instance.GetText("Version");

	/// <summary>
	/// View all your {0}
	/// </summary>
	public static Translation ViewAllYourItems => _instance.GetText("ViewAllYourItems");

	/// <summary>
	/// View Assets with issues
	/// </summary>
	public static Translation ViewAssetsWithIssues => _instance.GetText("ViewAssetsWithIssues");

	/// <summary>
	/// View the full report
	/// </summary>
	public static Translation ViewCompatibilityReport => _instance.GetText("ViewCompatibilityReport");

	/// <summary>
	/// View Mods with issues
	/// </summary>
	public static Translation ViewModsWithIssues => _instance.GetText("ViewModsWithIssues");

	/// <summary>
	/// View more on the workshop
	/// </summary>
	public static Translation ViewMoreWorkshop => _instance.GetText("ViewMoreWorkshop");

	/// <summary>
	/// View this package's Github repository
	/// </summary>
	public static Translation ViewOnGithub => _instance.GetText("ViewOnGithub");

	/// <summary>
	/// View this package's compatibility info
	/// </summary>
	public static Translation ViewPackageCR => _instance.GetText("ViewPackageCR");

	/// <summary>
	/// View Your Playsets
	/// </summary>
	public static Translation ViewPlaysets => _instance.GetText("ViewPlaysets");

	/// <summary>
	/// View &amp; manage your playsets
	/// </summary>
	public static Translation ViewPlaysetsTip => _instance.GetText("ViewPlaysets_Tip");

	/// <summary>
	/// View recently updated {0}
	/// </summary>
	public static Translation ViewRecentlyUpdatedItems => _instance.GetText("ViewRecentlyUpdatedItems");

	/// <summary>
	/// View the mods in this playset
	/// </summary>
	public static Translation ViewThisPlaysetsPackages => _instance.GetText("ViewThisPlaysetsPackages");

	/// <summary>
	/// View troubleshooting options
	/// </summary>
	public static Translation ViewTroubleshootOptions => _instance.GetText("ViewTroubleshootOptions");

	/// <summary>
	/// View '{0}' on Steam
	/// </summary>
	public static Translation ViewXOnSteam => _instance.GetText("ViewXOnSteam");

	/// <summary>
	/// Likes
	/// </summary>
	public static Translation Votes => _instance.GetText("Votes");

	/// <summary>
	/// <para>{1} like</para>
	/// <para>Plural: {1} likes</para>
	/// </summary>
	public static Translation VotesCount => _instance.GetText("VotesCount");

	/// <summary>
	/// Waiting for your response...
	/// </summary>
	public static Translation WaitingForConfirmation => _instance.GetText("WaitingForConfirmation");

	/// <summary>
	/// Waiting for the game to be closed...
	/// </summary>
	public static Translation WaitingForGameClose => _instance.GetText("WaitingForGameClose");

	/// <summary>
	/// Waiting for the game to be launched...
	/// </summary>
	public static Translation WaitingForGameLaunch => _instance.GetText("WaitingForGameLaunch");

	/// <summary>
	/// Workshop
	/// </summary>
	public static Translation Workshop => _instance.GetText("Workshop");

	/// <summary>
	/// Workshop Maps
	/// </summary>
	public static Translation WorkshopMaps => _instance.GetText("Workshop Maps");

	/// <summary>
	/// Workshop Savegames
	/// </summary>
	public static Translation WorkshopSavegames => _instance.GetText("Workshop Savegames");

	/// <summary>
	/// Workshop &amp; In-Game Tags
	/// </summary>
	public static Translation WorkshopAndGameTags => _instance.GetText("WorkshopAndGameTags");

	/// <summary>
	/// Do you want to skip those packages?
	/// </summary>
	public static Translation WouldYouLikeToSkipThose => _instance.GetText("WouldYouLikeToSkipThose");

	/// <summary>
	/// <para>You have {0} package made by {1}</para>
	/// <para>Plural: You have {0} packages made by {1}</para>
	/// </summary>
	public static Translation YouHavePackagesUser => _instance.GetText("YouHavePackagesUser");

	/// <summary>
	/// Your Savegames
	/// </summary>
	public static Translation YourSavegames => _instance.GetText("Your Savegames");

	/// <summary>
	/// Your DLCs
	/// </summary>
	public static Translation YourDlcs => _instance.GetText("YourDlcs");

	/// <summary>
	/// Your Playsets
	/// </summary>
	public static Translation YourPlaysets => _instance.GetText("YourPlaysets");
}
