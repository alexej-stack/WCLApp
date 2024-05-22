using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.Windows;
using WCLApp.Common;
using Identity.Common;

namespace WCLApp.Auth
{
	public class RegisterViewModel : ViewModelBase
	{
		private readonly IAuthClient authClient;
		private readonly IMessageService messageService;
		private string userName;
		private string password;
		private string lastName;
		private string phoneNumber;
		private string email;

		public RegisterViewModel(IAuthClient authClient, IMessageService messageService)
		{
			this.authClient = authClient;
			this.messageService = messageService;
			RegisterCommand = new RelayCommand(async () => await Register());
			CancelCommand = new RelayCommand(Cancel);
		}

		public string UserName
		{
			get => userName;
			set => SetProperty(ref userName, value);
		}

		public string Password
		{
			get => password;
			set => SetProperty(ref password, value);
		}


		public string LastName
		{
			get => lastName;
			set => SetProperty(ref lastName, value);
		}

		public string PhoneNumber
		{
			get => phoneNumber;
			set => SetProperty(ref phoneNumber, value);
		}

		public string Email
		{
			get => email;
			set => SetProperty(ref email, value);
		}

		public bool IsRegistered { get; private set; }

		public ICommand RegisterCommand { get; }
		public ICommand CancelCommand { get; }

		private async Task Register()
		{
			var user = new RegisterUserData(UserName, LastName, PhoneNumber, Email);
			IsRegistered = await authClient.RegisterUserAsync(user, Password);

			if (IsRegistered)
			{
				CloseDialog(true);
			}
			else
			{
				MessageBox.Show("Registration failed");
			}
		}

		private void Cancel()
		{
			CloseDialog(false);
		}

		private void CloseDialog(bool dialogResult)
		{
			messageService.Send(new CloseDialogMessage(dialogResult));
		}
	}
}