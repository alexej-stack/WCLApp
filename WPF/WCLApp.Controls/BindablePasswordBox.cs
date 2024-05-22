using System.Windows.Controls;
using System.Windows;

namespace WCLApp.Controls;

public class BindablePasswordBox : Control
{
	static BindablePasswordBox()
	{
		DefaultStyleKeyProperty.OverrideMetadata(typeof(BindablePasswordBox),
			new FrameworkPropertyMetadata(typeof(BindablePasswordBox)));
	}

	public static readonly DependencyProperty PasswordProperty =
		DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
			new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				OnPasswordPropertyChanged));

	private bool skipUpdate;

	public string Password
	{
		get => (string)GetValue(PasswordProperty);
		set => SetValue(PasswordProperty, value);
	}

	private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is BindablePasswordBox passwordBox && passwordBox.passwordBox != null)
		{
			if (!passwordBox.skipUpdate)
			{
				passwordBox.skipUpdate = true;
				passwordBox.passwordBox.Password = (string)e.NewValue;
				passwordBox.skipUpdate = false;
			}
		}
	}

	private PasswordBox passwordBox;

	public override void OnApplyTemplate()
	{
		base.OnApplyTemplate();

		passwordBox = GetTemplateChild("PART_PasswordBox") as PasswordBox;

		if (passwordBox != null)
		{
			passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
			passwordBox.Password = Password;
		}
	}

	private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		if (!skipUpdate)
		{
			skipUpdate = true;
			Password = passwordBox.Password;
			skipUpdate = false;
		}
	}
}