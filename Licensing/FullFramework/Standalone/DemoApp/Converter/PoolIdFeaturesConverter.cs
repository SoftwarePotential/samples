using System;
using System.Globalization;
using System.Windows.Data;

namespace DemoApp.Converter
{
	[ValueConversion( typeof( string ), typeof( string ) )]
	public class PoolIdFeaturesConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
		{
			string separator = (string)parameter ?? ",";
			return ((string)(value)).Replace( ":", separator );
		}

		public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}
