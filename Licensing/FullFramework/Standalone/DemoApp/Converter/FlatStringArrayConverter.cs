using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace DemoApp.Converter
{
	[ValueConversion( typeof( IEnumerable<string> ), typeof( string ) )]
	public class FlatStringArrayConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string separator = (string)parameter ?? ",";
			return string.Join( separator, (IEnumerable<string>)value );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
