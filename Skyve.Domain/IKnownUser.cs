namespace SkyveApp.Domain;
public interface IKnownUser : IUser
{
	public bool Retired { get; }
	public bool Verified { get; }
	public bool Malicious { get; }
	public bool Manager { get; }
}
