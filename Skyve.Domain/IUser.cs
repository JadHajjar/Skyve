namespace Skyve.Domain;
public interface IUser : IThumbnailObject
{
	string Name { get; }
	string? ProfileUrl { get; }
	string? AvatarUrl { get; }
	object? Id { get; }
}
