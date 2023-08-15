using Extensions;

namespace SkyveApp.Systems.CS1.Utilities;
public class LocaleCRNotes : LocaleHelper
{
	private static readonly LocaleCRNotes _instance = new();

	protected LocaleCRNotes() : base($"SkyveApp.Systems.Properties.CompatibilityNotes.json") { }

	public static Translation Get(string value)
	{
		return _instance.GetText(value);
	}

	public static void Load() { }
}