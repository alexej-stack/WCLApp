using System.Windows;
using WCLApp.Common;

namespace Section1;

public class Section1
	: ISection
{
	public string Header => "View 1";
	public FrameworkElement AuthContent { get; }
	public FrameworkElement UnAuthContent { get; }

	public bool IsAuthenticated { get; private set; }

	public Section1(IAuthClient authClient)
	{
		var content = new SectionView1();
		content.DataContext = new Section1ViewModel(authClient);
		AuthContent = content;
		UnAuthContent = new UnAuth();
		authClient.AuthStatusChanged += AuthClientOnAuthStatusChanged;
	}

	private void AuthClientOnAuthStatusChanged(object? sender, AuthStateChangedEventArgs e)
	{
		IsAuthenticated = e.IsLogged;
	}
}