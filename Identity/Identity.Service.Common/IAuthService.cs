using Identity.Common;

namespace Identity.Service.Common;

public interface IAuthService
{
	Task<RegisterResult> RegisterUserAsync(RegisterUserData userData, string password);
	Task<LoginResult> AuthenticateAsync(string username, string password);
	Task<UserDataDto> GetCurrentUserAsync();
	Task<bool> LogoutAsync();
	Task<UserDataDto> GetUserDataAsync(string username);
}