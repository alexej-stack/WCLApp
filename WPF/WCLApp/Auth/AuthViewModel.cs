using System.Windows;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using WCLApp.Common;

namespace WCLApp.Auth;

public class AuthViewModel : ViewModelBase
{
	private readonly IAuthClient authClient;
	private readonly IDialogService dialogService;
	private string login;
	private string password;
	private bool isLoggedIn;

	public string LoginString
	{
		get => login;
		set => SetProperty(ref login, value);
	}

	public string Password
	{
		get => password;
		set => SetProperty(ref password, value);
	}

	public bool IsLoggedIn
	{
		get => isLoggedIn;
		set => SetProperty(ref isLoggedIn, value);
	}

	public ICommand LoginCommand { get; }
	public ICommand OpenRegisterCommand { get; }
	public ICommand LogoutCommand { get; }

	public AuthViewModel(IAuthClient authClient, IDialogService dialogService)
	{
		this.authClient = authClient;
		this.dialogService = dialogService;
		IsLoggedIn = false;
		LoginCommand = new RelayCommand(async () => await Login());
		OpenRegisterCommand = new RelayCommand(OpenRegisterDialog);
		LogoutCommand = new RelayCommand(async () => await Logout());
		authClient.AuthStatusChanged += AuthClient_AuthStatusChanged;
	}

	private void AuthClient_AuthStatusChanged(object? sender, AuthStateChangedEventArgs e)
	{
		IsLoggedIn = e.IsLogged;
	}


	private async Task Login()
	{
		var loginResult = await authClient.LoginAsync(LoginString, Password);
	}

	private void OpenRegisterDialog()
	{
		dialogService.ShowRegisterDialog(out var userName, out var password);
		LoginString = userName;
		Password = password;
		LoginCommand.Execute(null);
	}

	private async Task Logout()
	{
		var success = await authClient.LogoutAsync();
		if (success)
		{
			IsLoggedIn = false;
			LoginString = string.Empty;
			Password = string.Empty;
		}
	}
}