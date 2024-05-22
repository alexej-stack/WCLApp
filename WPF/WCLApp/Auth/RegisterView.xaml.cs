using System.Windows;
using WCLApp.Common;

namespace WCLApp.Auth
{
	/// <summary>
	/// Interaction logic for RegisterView.xaml
	/// </summary>
	public partial class RegisterView : Window
	{
		public RegisterView(IMessageService messageService)
		{
			InitializeComponent();
			messageService.Register<CloseDialogMessage>(this, CloseDialog);
		}

		private void CloseDialog(CloseDialogMessage message)
		{
			DialogResult = message.DialogResult;
			Close();
		}
	}

	public class CloseDialogMessage
	{
		public bool DialogResult { get; }

		public CloseDialogMessage(bool dialogResult)
		{
			DialogResult = dialogResult;
		}
	}
}