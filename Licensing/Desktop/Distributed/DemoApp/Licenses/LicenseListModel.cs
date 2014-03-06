/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Sp.Agent;
using System.Windows;
using DemoApp.Common;

namespace DemoApp.Licenses
{
	public class LicenseListModel
	{
		public string ProductName { get; private set; }
		public string ProductVersion { get; private set; }
		public ObservableCollection<LicenseItemModel> Licenses { get; set; }

		public LicenseListModel()
		{
			IProductContext productContext = SpAgent.Product;

			ProductName = productContext.ProductName;
			ProductVersion = productContext.ProductVersion;

			// If there's no product context, then probably SpAgent hasn't been initialized 
			// (this can happen inside Visual Studio Designer)
			if ( productContext == null )
				return;

			Licenses = new ObservableCollection<LicenseItemModel>( RetrieveAllLicenses( productContext ) );
			foreach ( var license in Licenses )
				license.ItemRemoved += RemoveSelectedItem;
		}
				
		public void Reload( )
		{
			Licenses.Clear();
			foreach ( var item in RetrieveAllLicenses( SpAgent.Product ) )
				Licenses.Add( item );
		}

		public IEnumerable<LicenseItemModel> CreateLicenseList()
		{
			IProductContext productContext = SpAgent.Product;
			return RetrieveAllLicenses( productContext );
		}

		IEnumerable<LicenseItemModel> RetrieveAllLicenses( IProductContext productContext )
		{
			return
				productContext.Licenses.All()
				.Select( l => new LicenseItemModel()
				{
					ActivationKey = l.ActivationKey,
					ValidUntil = l.ValidUntil,
					Features = l.Advanced.AllFeatures().Select( f => f.Key )
				} );
		}

		void RemoveSelectedItem( object sender, EventArgs e )
		{
			var license = (LicenseItemModel)sender;
			license.ItemRemoved -= RemoveSelectedItem;
			Licenses.Remove( license );
		}
	}

	public class LicenseItemModel
	{
		public string ActivationKey { get; set; }
		public DateTime ValidUntil { get; set; }
		public IEnumerable<string> Features { get; set; }
		public RelayCommand RemoveLicenseCommand { get; set; }
		public event EventHandler ItemRemoved;

		public LicenseItemModel()
		{
			RemoveLicenseCommand = new RelayCommand( RemoveLicense );
		}

		void RemoveLicense()
		{
			if ( MessageBox.Show( "Are you sure you want to remove this license?", "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes )
			{
				SpAgent.Product.Stores.Delete( ActivationKey );
				if ( ItemRemoved != null )
					ItemRemoved( this, EventArgs.Empty );
			}
		}
	}	

	#region Converters
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
	#endregion
}
