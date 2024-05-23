using Identity.Common;
using Identity.Service.Common;

namespace WCLApp.Common;

public interface IAuthClient
{
	event EventHandler<AuthStateChangedEventArgs> AuthStatusChanged;

	Task<UserDataDto> GetUserAsync(string userName);
	Task<LoginResult> LoginAsync(string login, string password);
	Task<bool> LogoutAsync();
	Task<RegisterResult> RegisterUserAsync(RegisterUserData user, string password);
}

public class AuthStateChangedEventArgs : EventArgs
{
	public string UserName { get; }
	public bool IsLogged { get; }

	public AuthStateChangedEventArgs(string userName, bool isLogged)
	{
		UserName = userName;
		IsLogged = isLogged;
	}
}