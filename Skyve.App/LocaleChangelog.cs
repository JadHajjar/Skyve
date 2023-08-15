namespace SkyveApp;
internal class LocaleChangelog : LocaleHelper
{
	private static readonly LocaleChangelog _instance = new LocaleChangelog();

	protected LocaleChangelog() : base($"SkyveApp.Properties.Changelog.json") { }

	public static void Load() { _ = _instance; }
}
