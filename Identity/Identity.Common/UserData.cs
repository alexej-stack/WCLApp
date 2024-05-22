namespace Identity.Common;

public class UserData(string userName, string lastName, string phoneNumber, string email)
{
	public Guid Id { get; init; }
	public string UserName { get; } = userName;
	public string LastName { get; } = lastName;
	public string PhoneNumber { get; } = phoneNumber;
	public string Email { get; } = email;
}