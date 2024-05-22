using System.Windows;
using WCLApp.Auth;
using WCLApp.Common;

namespace WCLApp;

public class DialogService(IAuthClient authClient, IMessageService messageService) : IDialogService
{
	public bool ShowRegisterDialog(out string userName, out string password)
	{
		Window registerView = new RegisterView(messageService)
		{
			DataContext = new RegisterViewModel(authClient, messageService)
		};

		var result = registerView.ShowDialog();

		if (result == true && registerView.DataContext is RegisterViewModel registerViewModel)
		{
			userName = registerViewModel.UserName;
			password = registerViewModel.Password;
			return registerViewModel.IsRegistered;
		}

		userName = string.Empty;
		password = string.Empty;
		return false;
	}
}