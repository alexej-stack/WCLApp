using System.ComponentModel.DataAnnotations;
using Identity.API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Data;

public sealed class ApplicationUser : IdentityUser
{
	public string LastName { get; set; }

	[RegularExpression(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$", ErrorMessage = "Invalid phone number")]
	public override string PhoneNumber { get; set; }

	[Required]
	[RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address")]
	public override string Email { get; set; }

	public ApplicationUser()
	{
	}

	public ApplicationUser(string password, string name, string lastName, string phoneNumber, string email) : base(name)
	{
		Email = email;
		PasswordHash = PasswordHasher.HashPassword(password);
		LastName = lastName;
		PhoneNumber = phoneNumber;
	}
}