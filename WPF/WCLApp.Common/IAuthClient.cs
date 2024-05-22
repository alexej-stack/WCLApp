using Identity.Common;

namespace WCLApp.Common;

public interface IAuthClient
{
	event EventHandler<AuthStateChangedEventArgs> AuthStatusChanged;

	Task<UserData> GetUserAsync(string userName);
	Task<bool> LoginAsync(string login, string password);
	Task<bool> LogoutAsync();
	Task<bool> RegisterUserAsync(RegisterUserData user, string password);
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