namespace Identity.Common;

public class RegisterUserData(string userName, string lastName, string phoneNumber, string email)
{
	public string UserName { get; } = userName;
	public string LastName { get; } = lastName;
	public string PhoneNumber { get; } = phoneNumber;
	public string Email { get; } = email;
}