using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Skyve.Domain.Systems;
public interface IUpdateManager
{
	bool IsFirstTime();
	bool IsPackageKnown(ILocalPackageData package);
	DateTime GetLastUpdateTime(ILocalPackageData package);
	void SendUpdateNotifications();
	IEnumerable<ILocalPackageData>? GetNewOrUpdatedPackages();
	Task SendUnreadCommentsNotifications();
	void MarkCommentAsRead(IPackageIdentity package);
	DateTime GetLastReadComment(IPackageIdentity package);
	Task SendReviewRequestNotifications();
	Task MarkReviewReplyAsRead(IPackageIdentity package);
}
