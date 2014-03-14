/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Common;
using DemoApp.Configuration;
using Sp.Agent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DemoApp.Licenses
{
	public class LicenseListModel : ViewModelBase
	{
		public RelayCommand BackCommand { get; set; }
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

			Licenses = new ObservableCollection<LicenseItemModel>( LicenseRepository.RetrieveAllLicenses( productContext ) );
			foreach ( var license in Licenses )
				license.ItemRemoved += RemoveSelectedItem;
			BackCommand = new RelayCommand( () => DisplayState.Navigate( new MainPage() ) );
		}

		public void Reload()
		{
			Licenses.Clear();
			foreach ( var item in LicenseRepository.RetrieveAllLicenses( SpAgent.Product ) )
				Licenses.Add( item );
		}

		void RemoveSelectedItem( object sender, EventArgs e )
		{
			if ( DisplayState.Warn( "Are you sure you want to remove this license?" ) )
			{
				var license = (LicenseItemModel)sender;
				license.ItemRemoved -= RemoveSelectedItem;

				SpAgent.Product.Stores.Delete( license.ActivationKey );
				Licenses.Remove( license );
			}
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
			if ( ItemRemoved != null )
				ItemRemoved( this, EventArgs.Empty );
		}
	}

	public class LicenseRepository
	{
		public static int LicenseCount( IProductContext productContext )
		{
			return productContext.Licenses.All().Count();
		}

		public static IEnumerable<LicenseItemModel> RetrieveAllLicenses( IProductContext productContext )
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
	}
}