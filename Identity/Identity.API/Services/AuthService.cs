using Microsoft.EntityFrameworkCore;
using Identity.API.Data;
using Identity.Common;
using Identity.Service.Common;
using Identity.API.Helpers;

namespace Identity.API.Services;

/// <summary>
/// This service realize equivalence of Microsoft.AspNetCore.Identity.
/// Instead of it can use SignInManager and inherit ApplicationDbContext from IdentityDbContext.
/// But how I understand task, I don't need use external frameworks, and I realize my own, but know how use Microsoft tools.
/// </summary>
public class AuthService : IAuthService
{
	private readonly ApplicationDbContext dbContext;
	private readonly IHttpContextAccessor httpContextAccessor;
	private const string SessionKeyUser = "CurrentUser";

	public AuthService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<bool> RegisterUserAsync(RegisterUserData userData, string password)
	{
		var user = new ApplicationUser
		{
			UserName = userData.UserName,
			LastName = userData.LastName,
			PhoneNumber = userData.PhoneNumber,
			Email = userData.Email,
			Password = PasswordHasher.HashPassword(password)
		};

		if (await dbContext.Users.AnyAsync(u => u.UserName == user.UserName))
		{
			return false;
		}

		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();

		SetCurrentUserSession(user);
		return true;
	}

	public async Task<bool> AuthenticateAsync(string username, string password)
	{
		var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
		if (user != null && PasswordHasher.VerifyPassword(password, user.Password))
		{
			SetCurrentUserSession(user);
			return true;
		}

		return false;
	}


	public UserData GetCurrentUser()
	{
		var user = GetCurrentUserFromSession();
		return user.GetUserData();
	}

	public async Task<UserData> GetUserDataAsync(string username)
	{
		var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
		return user.GetUserData();
	}

	public Task<bool> LogoutAsync()
	{
		ClearCurrentUserSession();
		return Task.FromResult(true);
	}

	private void SetCurrentUserSession(ApplicationUser user)
	{
		httpContextAccessor.HttpContext?.Session.SetString(SessionKeyUser, user.UserName);
	}

	private ApplicationUser GetCurrentUserFromSession()
	{
		var userName = httpContextAccessor.HttpContext?.Session.GetString(SessionKeyUser);
		return dbContext.Users.FirstOrDefault(u => u.UserName == userName) ?? throw new ArgumentNullException();
	}

	private void ClearCurrentUserSession()
	{
		httpContextAccessor.HttpContext?.Session.Remove(SessionKeyUser);
	}
}