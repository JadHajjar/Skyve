namespace Skyve.Domain;

public struct ParadoxLoginInfo
{
	public string Email { get; set; }
	public string Password { get; set; }

	public readonly bool IsValid() => !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password);
}