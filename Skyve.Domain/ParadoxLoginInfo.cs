using Extensions;

namespace Skyve.Domain;

public struct ParadoxLoginInfo
{
	public string Email { get; set; }
	public string Password { get; set; }

	public readonly bool IsValid(string salt)
	{
		try
		{
			var email = Encryption.Decrypt(Email, salt);
			var password = Encryption.Decrypt(Password, salt);

			return !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password);
		}
		catch
		{
			return false;
		}
	}
}