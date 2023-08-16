using static Extensions.LocaleHelper;

namespace Skyve.Domain.Systems;
public interface ILocale
{
	public Translation Get(string key);
}
