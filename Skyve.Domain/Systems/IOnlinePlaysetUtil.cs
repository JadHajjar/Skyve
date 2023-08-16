using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IOnlinePlaysetUtil
{
	Task Share(IPlayset playset);
	Task<bool> DownloadPlayset(ICustomPlayset playset);
	Task<bool> DownloadPlayset(string link);
	Task<bool> SetVisibility(IOnlinePlayset playset, bool @public);
	Task<bool> DeleteOnlinePlayset(IOnlinePlayset playset);
}
