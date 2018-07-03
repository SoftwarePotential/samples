using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DemoApp.Converter
{

	[ValueConversion( typeof( bool ), typeof( bool ) )]
	public class InverseBooleanVisibilityConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter,
			CultureInfo culture )
		{
			var invertedBool = _inverseBooleanConverter.Convert( value, typeof( bool ), parameter, culture );
			return _booleanToVisibilityConverter.Convert( invertedBool, targetType, parameter, culture );
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			CultureInfo culture )
		{
			throw new NotSupportedException();
		}

		readonly InverseBooleanConverter _inverseBooleanConverter = new InverseBooleanConverter();
		readonly BooleanToVisibilityConverter _booleanToVisibilityConverter = new BooleanToVisibilityConverter();
	}
}