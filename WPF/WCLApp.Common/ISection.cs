using System.Windows;

namespace WCLApp.Common;

public interface ISection
{
	string Header { get; }
	FrameworkElement AuthContent { get; }

	FrameworkElement UnAuthContent { get; }

	bool IsAuthenticated { get; }
	//Task<bool> AuthenticateAsync();
}