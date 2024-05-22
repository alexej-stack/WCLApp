using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WCLApp.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class InverseBooleanToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return !boolValue ? Visibility.Visible : Visibility.Collapsed;
		}

		return Visibility.Collapsed; // Default value if conversion fails
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Visibility visibility)
		{
			return visibility == Visibility.Collapsed;
		}

		return true; // Default value if conversion fails
	}
}