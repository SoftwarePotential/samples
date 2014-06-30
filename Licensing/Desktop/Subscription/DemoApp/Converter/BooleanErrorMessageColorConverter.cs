using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DemoApp.Converter
{
	[ValueConversion( typeof( bool ), typeof( bool ) )]
	public class BooleanErrorMessageColorConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter,
			CultureInfo culture )
		{
			if ( value == null )
				return "Red";

			bool valueAsBool = (bool)value;
			return valueAsBool ? "Green" : "Red";
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			CultureInfo culture )
		{
			throw new NotSupportedException();
		}
	}
}
