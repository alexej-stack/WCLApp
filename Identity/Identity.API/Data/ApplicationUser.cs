using System.ComponentModel.DataAnnotations;
using Identity.API.Helpers;

namespace Identity.API.Data;

public class ApplicationUser
{
	public Guid Id { get; set; } = Guid.NewGuid();

	[Required] public string UserName { get; set; }

	[Required] public string Password { get; set; }

	public string LastName { get; set; }

	[RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Invalid phone number")]
	public string PhoneNumber { get; set; }

	[Required]
	[RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
	public string Email { get; set; }

	public ApplicationUser()
	{
	}

	public ApplicationUser(string password, string name, string lastName, string phoneNumber)
	{
		UserName = name;
		Password = PasswordHasher.HashPassword(password);
		LastName = lastName;
		PhoneNumber = phoneNumber;
	}
}