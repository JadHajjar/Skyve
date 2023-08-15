using static Extensions.LocaleHelper;

namespace SkyveApp.Domain.Systems;
public interface ILocale
{
	public Translation Get(string key);
}
