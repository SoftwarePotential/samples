using System;
using System.Globalization;
using System.Windows.Data;

namespace DemoApp.Converter
{
	[ValueConversion( typeof( string ), typeof( string ) )]
	public class ExpirationDateConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( (DateTime)value == DateTime.MaxValue )
				return "Unlimited";
			return value;
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
