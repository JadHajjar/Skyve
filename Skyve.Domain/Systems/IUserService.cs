using System;

namespace Skyve.Domain.Systems;
public interface IUserService
{
	IKnownUser User { get; }

	event Action UserInfoUpdated;

	IKnownUser TryGetAuthor(string? id);
	bool IsUserVerified(IUser author);
}
