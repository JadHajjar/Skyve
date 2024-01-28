using System;

namespace Skyve.Domain.Systems;
public interface IUserService
{
	IKnownUser User { get; }

	event Action UserInfoUpdated;

	bool IsUserVerified(IUser author);
}
