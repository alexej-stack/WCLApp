using Identity.Common;
using WCLApp.Common;

namespace Section1;

public class Section1ViewModel : ViewModelBase
{
	private readonly IAuthClient authClient;
	private UserDataDto currentUser;

	public UserDataDto CurrentUser
	{
		get => currentUser;
		private set => SetProperty(ref currentUser, value);
	}

	public Section1ViewModel(IAuthClient authClient)
	{
		this.authClient = authClient;
		authClient.AuthStatusChanged += async (sender, e) => await AuthClient_AuthStatusChangedAsync(sender, e);
	}

	private async Task AuthClient_AuthStatusChangedAsync(object? sender, AuthStateChangedEventArgs e)
	{
		CurrentUser = await authClient.GetUserAsync(e.UserName);
	}
}