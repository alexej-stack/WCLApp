using System.Windows;
using WCLApp.Common;

namespace Section2;

public class Section2
	: ISection
{
	public string Header => "View 2";
	public FrameworkElement AuthContent { get; }
	public FrameworkElement UnAuthContent { get; }

	public bool IsAuthenticated { get; private set; }

	public Section2(IAuthClient authClient)
	{
		AuthContent = new SectionView2();
		UnAuthContent = new SectionView2();
		authClient.AuthStatusChanged += AuthClientOnAuthStatusChanged;
	}

	private void AuthClientOnAuthStatusChanged(object? sender, AuthStateChangedEventArgs e)
	{
		IsAuthenticated = e.IsLogged;
	}
}