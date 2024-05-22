using System.Security.Cryptography;
using System.Text;

namespace Identity.API.Helpers;

public static class PasswordHasher
{
	public static string HashPassword(string password)
	{
		using (var sha512 = SHA512.Create())
		{
			byte[] hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(hashBytes);
		}
	}

	public static bool VerifyPassword(string password, string hashedPassword)
	{
		string hashedInputPassword = HashPassword(password);
		return hashedInputPassword == hashedPassword;
	}
}