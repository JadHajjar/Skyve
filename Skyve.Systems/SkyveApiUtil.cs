using Extensions;

using SkyveApp.Domain.Systems;
using SkyveApp.Systems.Compatibility.Domain;
using SkyveApp.Systems.Compatibility.Domain.Api;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkyveApp.Systems;
public class SkyveApiUtil
{
	private readonly IUserService _userService;

	public SkyveApiUtil(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<T?> Get<T>(string url, params (string, object)[] queryParams)
	{
		return await ApiUtil.Get<T>(KEYS.API_URL + url
			, new[] { ("API_KEY", KEYS.API_KEY), ("USER_ID", Encryption.Encrypt(_userService.User.Id?.ToString() ?? string.Empty, KEYS.SALT)) }
			, queryParams);
	}

	public async Task<T?> Delete<T>(string url, params (string, object)[] queryParams)
	{
		return await ApiUtil.Delete<T>(KEYS.API_URL + url
			, new[] { ("API_KEY", KEYS.API_KEY), ("USER_ID", Encryption.Encrypt(_userService.User.Id?.ToString() ?? string.Empty, KEYS.SALT)) }
			, queryParams);
	}

	public async Task<T?> Post<TBody, T>(string url, TBody body, params (string, object)[] queryParams)
	{
		return await ApiUtil.Post<TBody, T>(KEYS.API_URL + url
			, body
			, new[] { ("API_KEY", KEYS.API_KEY), ("USER_ID", Encryption.Encrypt(_userService.User.Id?.ToString() ?? string.Empty, KEYS.SALT)) }
			, queryParams);
	}

	public async Task<bool> IsCommunityManager()
	{
		return await Get<bool>("/IsCommunityManager");
	}

	public async Task<CompatibilityData?> Catalogue()
	{
		return await Get<CompatibilityData>("/Catalogue");
	}

	public async Task<CompatibilityData?> Catalogue(object steamId)
	{
		return await Get<CompatibilityData>("/Package", ("steamId", steamId));
	}

	public async Task<ApiResponse> SaveEntry(PostPackage package)
	{
		return await Post<PostPackage, ApiResponse>("/SaveEntry", package);
	}

	public async Task<Dictionary<string, string>?> Translations()
	{
		return await Get<Dictionary<string, string>>("/Translations");
	}

	public async Task<ApiResponse> SendReviewRequest(ReviewRequest request)
	{
		return await Post<ReviewRequest, ApiResponse>("/RequestReview", request);
	}

	public async Task<ApiResponse> ProcessReviewRequest(ReviewRequest request)
	{
		return await Post<ReviewRequest, ApiResponse>("/ProcessReviewRequest", request);
	}

	public async Task<ReviewRequest[]?> GetReviewRequests()
	{
		return await Get<ReviewRequest[]>("/GetReviewRequests");
	}

	public async Task<ReviewRequest?> GetReviewRequest(ulong userId, ulong packageId)
	{
		return await Get<ReviewRequest>("/GetReviewRequest", (nameof(userId), userId), (nameof(packageId), packageId));
	}

	public async Task<UserProfile[]?> GetUserProfiles(object userId)
	{
		return await Get<UserProfile[]>("/GetUserProfiles", (nameof(userId), userId));
	}

	public async Task<UserProfile?> GetUserProfileContents(int profileId)
	{
		return await Get<UserProfile>("/GetUserProfileContents", (nameof(profileId), profileId));
	}

	public async Task<UserProfile?> GetUserProfileByLink(string link)
	{
		return await Get<UserProfile>("/GetUserProfileByLink", (nameof(link), link));
	}

	public async Task<ApiResponse> DeleteUserProfile(int profileId)
	{
		return await Delete<ApiResponse>("/DeleteUserProfile", (nameof(profileId), profileId));
	}

	public async Task<ApiResponse> SaveUserProfile(UserProfile profile)
	{
		return await Post<UserProfile, ApiResponse>("/SaveUserProfile", profile);
	}

	public async Task<UserProfile[]?> GetPublicProfiles()
	{
		return await Get<UserProfile[]>("/GetPublicProfiles");
	}

	public async Task<ApiResponse> SetProfileVisibility(int profileId, bool @public)
	{
		return await Post<bool, ApiResponse>("/SetProfileVisibility", @public, (nameof(profileId), profileId));
	}

	public async Task<ApiResponse> GetUserGuid()
	{
		return await Get<ApiResponse>("/GetUserGuid");
	}
}
