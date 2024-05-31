using System.Collections.Generic;

namespace Skyve.Domain;

public interface IAuthor : IUser
{
	public List<ILink> ExternalLinks { get; set; }
	public string Bio { get; set; }
	public int FollowerCount { get; set; }
}
