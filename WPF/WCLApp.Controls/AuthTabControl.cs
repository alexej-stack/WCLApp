using System.Windows;
using System.Windows.Controls;

namespace WCLApp.Controls;

public class AuthPageControl : ContentControl
{
	public static readonly DependencyProperty AuthenticatedViewProperty =
		DependencyProperty.Register("AuthenticatedView", typeof(FrameworkElement), typeof(AuthPageControl),
			new PropertyMetadata(null, OnAuthenticatedViewChanged));

	public static readonly DependencyProperty UnauthenticatedViewProperty =
		DependencyProperty.Register("UnauthenticatedView", typeof(FrameworkElement), typeof(AuthPageControl),
			new PropertyMetadata(null, OnUnauthenticatedViewChanged));

	public static readonly DependencyProperty IsAuthenticatedProperty =
		DependencyProperty.Register("IsAuthenticated", typeof(bool), typeof(AuthPageControl),
			new PropertyMetadata(false, OnIsAuthenticatedChanged));

	private static void OnAuthenticatedViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is AuthPageControl authPageControl)
		{
			authPageControl.UpdateView();
		}
	}

	private static void OnUnauthenticatedViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is AuthPageControl authPageControl)
		{
			authPageControl.UpdateView();
		}
	}

	public FrameworkElement AuthenticatedView
	{
		get { return (FrameworkElement)GetValue(AuthenticatedViewProperty); }
		set { SetValue(AuthenticatedViewProperty, value); }
	}

	public FrameworkElement UnauthenticatedView
	{
		get { return (FrameworkElement)GetValue(UnauthenticatedViewProperty); }
		set { SetValue(UnauthenticatedViewProperty, value); }
	}

	public bool IsAuthenticated
	{
		get { return (bool)GetValue(IsAuthenticatedProperty); }
		set { SetValue(IsAuthenticatedProperty, value); }
	}

	private static void OnIsAuthenticatedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is AuthPageControl authPageControl)
		{
			authPageControl.UpdateView();
		}
	}

	private void UpdateView()
	{
		if (IsAuthenticated)
		{
			Content = AuthenticatedView;
		}
		else
		{
			Content = UnauthenticatedView;
		}
	}
}