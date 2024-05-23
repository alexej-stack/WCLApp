using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Identity.Common;
using Identity.Service.Common;
using WCLApp.Common;

namespace WCLApp;

public class AuthClient : IAuthClient
{
	private readonly Uri baseUri;
	private UserDataDto currentUser;
	public event EventHandler<AuthStateChangedEventArgs> AuthStatusChanged;


	public AuthClient(AppSettings settings)
	{
		baseUri = settings.AuthUri;
	}

	public async Task<RegisterResult> RegisterUserAsync(RegisterUserData user, string password)
	{
		using var httpClient = new HttpClient() { BaseAddress = baseUri };
		var request = new RegisterUserRequestDto(user, password);
		var response = await httpClient.PostAsJsonAsync("v2/auth/user", request);

		if (response.IsSuccessStatusCode)
		{
			currentUser = await GetUserAsync(user.UserName);
			OnAuthStatusChanged(user.UserName, true);
			return new RegisterResult(true, Enumerable.Empty<string>());
		}
		else
		{
			var errorContent = await response.Content.ReadAsStringAsync();
			var errors = ParseErrors(errorContent);
			return new RegisterResult(false, errors);
		}
	}

	public async Task<LoginResult> LoginAsync(string login, string password)
	{
		using var httpClient = new HttpClient() { BaseAddress = baseUri };
		var request = new LoginUserRequestDto(login, password);
		var response = await httpClient.PostAsJsonAsync("v2/auth/login", request);

		if (response.IsSuccessStatusCode)
		{
			currentUser = await GetUserAsync(login);
			OnAuthStatusChanged(login, true);
			return new LoginResult(true);
		}

		return new LoginResult(false);
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

	public async Task<UserDataDto> GetUserAsync(string userName)
	{
		using var httpClient = new HttpClient() { BaseAddress = baseUri };
		var response = await httpClient.GetAsync($"v2/auth/user/{userName}");

		if (response.IsSuccessStatusCode)
		{
			var user = await response.Content.ReadFromJsonAsync<UserDataDto>();
			return user;
		}

		return null;
	}

	protected virtual void OnAuthStatusChanged(string username, bool isLogged)
	{
		AuthStatusChanged?.Invoke(this, new AuthStateChangedEventArgs(username, isLogged));
	}

	private IEnumerable<string> ParseErrors(string errorContent)
	{
		var errorList = new List<string>();

		using JsonDocument doc = JsonDocument.Parse(errorContent);
		if (doc.RootElement.TryGetProperty("errors", out JsonElement errorsElement) &&
		    errorsElement.ValueKind == JsonValueKind.Array)
		{
			foreach (JsonElement error in errorsElement.EnumerateArray())
			{
				errorList.Add(error.GetString());
			}
		}

		return errorList;
	}

	public record RegisterUserRequestDto(RegisterUserData User, string Password);

	public record LoginUserRequestDto(string Login, string Password);
}