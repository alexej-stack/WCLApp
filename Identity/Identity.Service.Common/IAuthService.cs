using Identity.Common;

namespace Identity.Service.Common;

public interface IAuthService
{
	Task<bool> RegisterUserAsync(RegisterUserData userData, string password);
	Task<bool> AuthenticateAsync(string username, string password);
	UserData GetCurrentUser();
	Task<bool> LogoutAsync();
	Task<UserData> GetUserDataAsync(string username);
}