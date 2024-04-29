using System;

namespace Skyve.Domain;

public interface IModComment
{
	string Username { get; set; }
	DateTime Created { get; set; }
	string Message { get; set; }
	int PostId { get; set; }
	string Url { get; set; }
	int UserId { get; set; }
	string UserTitle { get; set; }
}