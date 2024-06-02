using System.Collections.Generic;

namespace Skyve.Domain;

public interface IAuthor : IUser
{
	public List<ILink> Links { get; }
	public string Bio { get; }
	public int FollowerCount { get; }
}
