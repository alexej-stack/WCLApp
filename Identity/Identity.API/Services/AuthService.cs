using Identity.API.Data;
using Identity.Common;
using Identity.Service.Common;
using Identity.API.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services;

/// <summary>
/// This service realize equivalence of Microsoft.AspNetCore.Identity.
/// Instead of it can use SignInManager and inherit ApplicationDbContext from IdentityDbContext.
/// But how I understand task, I don't need use external frameworks, and I realize my own, but know how use Microsoft tools.
/// </summary>
public class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> userManager;
	private readonly SignInManager<ApplicationUser> signInManager;

	public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
	{
		this.userManager = userManager;
		this.signInManager = signInManager;
	}

	public async Task<RegisterResult> RegisterUserAsync(RegisterUserData userData, string password)
	{
		var user = new ApplicationUser
		{
			UserName = userData.UserName,
			LastName = userData.LastName,
			PhoneNumber = userData.PhoneNumber,
			Email = userData.Email
		};

		var result = await userManager.CreateAsync(user, password);

		if (result.Succeeded)
		{
			await signInManager.SignInAsync(user, isPersistent: false);
		}

		return new RegisterResult(result.Succeeded, result.Errors.Select(er => $"{er.Code}|{er.Description}"));
	}

	public async Task<LoginResult> AuthenticateAsync(string username, string password)
	{
		var user = await userManager.FindByNameAsync(username);
		if (user != null)
		{
			var result =
				await signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
			return new LoginResult(result.Succeeded);
		}

		return new LoginResult(false);
	}

	public async Task<UserDataDto> GetCurrentUserAsync()
	{
		var user = await userManager.GetUserAsync(signInManager.Context.User);
		return user?.GetUserData();
	}

	public async Task<UserDataDto> GetUserDataAsync(string username)
	{
		var user = await userManager.FindByNameAsync(username);
		return user?.GetUserData();
	}

	public async Task<bool> LogoutAsync()
	{
		await signInManager.SignOutAsync();
		return true;
	}
}