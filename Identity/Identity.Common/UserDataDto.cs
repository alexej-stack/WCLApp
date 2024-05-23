namespace Identity.Common;

public class UserDataDto(string userName, string lastName, string phoneNumber, string email)
{
	public string Id { get; init; }
	public string UserName { get; } = userName;
	public string LastName { get; } = lastName;
	public string PhoneNumber { get; } = phoneNumber;
	public string Email { get; } = email;
}