using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace DemoApp.Common
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

	public class MultiValueConverter : IMultiValueConverter
	{
		public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
		{
			return values.Clone();
		}

		public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException();
		}
	}
}