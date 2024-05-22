namespace WCLApp;

public interface IDialogService
{
	bool ShowRegisterDialog(out string userName, out string password);
}