using System.Net.Http;
using System.Net.Http.Json;
using Identity.Common;
using WCLApp.Common;

namespace WCLApp;

public class AuthClient : IAuthClient
{
	private readonly Uri baseUri;
	private UserData currentUser;
	public event EventHandler<AuthStateChangedEventArgs> AuthStatusChanged;


	public AuthClient(AppSettings settings)
	{
		baseUri = settings.AuthUri;
	}

	public async Task<bool> RegisterUserAsync(RegisterUserData user, string password)
	{
		using var httpClient = new HttpClient() { BaseAddress = baseUri };
		var request = new RegisterUserRequestDto(user, password);
		var response = await httpClient.PostAsJsonAsync("v2/auth/user", request);
		currentUser = await GetUserAsync(user.UserName);
		if (response.IsSuccessStatusCode)
		{
			OnAuthStatusChanged(user.UserName, true);
			return true;
		}

		return false;
	}

	public async Task<bool> LoginAsync(string login, string password)
	{
		using var httpClient = new HttpClient() { BaseAddress = baseUri };
		var request = new LoginUserRequestDto(login, password);
		var response = await httpClient.PostAsJsonAsync("v2/auth/login", request);
		//ToDo: need check placement
		currentUser = await GetUserAsync(login);
		if (response.IsSuccessStatusCode)
		{
			OnAuthStatusChanged(login, true);
			return true;
		}

		return false;
	}

	public async Task<bool> LogoutAsync()
	{
		using var httpClient = new HttpClient() { BaseAddress = baseUri };
		var response = await httpClient.PostAsync("v2/auth/logout", null);

		if (response.IsSuccessStatusCode)
		{
			var name = currentUser.UserName;
			currentUser = null;
			OnAuthStatusChanged(name, false);
			return true;
		}

		return false;
	}

	public async Task<UserData> GetUserAsync(string userName)
	{
		using var httpClient = new HttpClient() { BaseAddress = baseUri };
		var response = await httpClient.GetAsync($"v2/auth/user/{userName}");

		if (response.IsSuccessStatusCode)
		{
			var user = await response.Content.ReadFromJsonAsync<UserData>();
			return user;
		}

		return null;
	}

	protected virtual void OnAuthStatusChanged(string username, bool isLogged)
	{
		AuthStatusChanged?.Invoke(this, new AuthStateChangedEventArgs(username, isLogged));
	}

	public record RegisterUserRequestDto(RegisterUserData User, string Password);

	public record LoginUserRequestDto(string Login, string Password);
}