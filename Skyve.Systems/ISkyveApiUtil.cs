using Skyve.Domain;
using Skyve.Systems.Compatibility.Domain;

using SkyveApi.Domain.Generic;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Systems;

public interface ISkyveApiUtil
{
	Task<IOnlinePlayset[]?> GetPublicPlaysets();
	Task<ReviewRequest?> GetReviewRequest(ulong userId, ulong packageId);
	Task<ReviewRequest[]?> GetReviewRequests();
	Task<IOnlinePlayset[]?> GetUserPlaysets(IUser user);
	Task<ApiResponse> ProcessReviewRequest(ReviewRequest request);
	Task<ApiResponse> SendReviewRequest(ReviewRequest request);
	Task<Dictionary<string, string>?> Translations();
}