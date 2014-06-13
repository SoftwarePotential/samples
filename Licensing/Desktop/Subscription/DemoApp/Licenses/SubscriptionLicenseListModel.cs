/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Common;
using DemoApp.Licensing;
using Sp.Agent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApp.Licenses
{
	public class SubscriptionLicenseListModel : ViewModelBase
	{
		public RelayCommand BackCommand { get; set; }
		public string ProductName { get; private set; }
		public string ProductVersion { get; private set; }
		public ObservableCollection<LicenseItemModel> Licenses { get; set; }

		bool _renewalInProgress;
		public bool IsRenewalInProgress
		{
			get { return _renewalInProgress; }
			set
			{
				_renewalInProgress = value;
				//	ActivationCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "IsRenewalInProgress" );
			}
		}

		string _lastRenewalResultMessage;
		public string LastRenewalResultMessage
		{
			get { return _lastRenewalResultMessage; }
			set
			{
				_lastRenewalResultMessage = value;
				OnPropertyChanged( "LastRenewalResultMessage" );
			}
		}

		bool _lastRenewalSucceeded;
		public bool LastRenewalSucceeded
		{
			get { return _lastRenewalSucceeded; }
			set
			{
				_lastRenewalSucceeded = value;
				OnPropertyChanged( "LastRenewalSucceeded" );
			}
		}
		
		public SubscriptionLicenseListModel()
		{
			IProductContext productContext = SpAgent.Product;

			ProductName = productContext.ProductName;
			ProductVersion = productContext.ProductVersion;

			// If there's no product context, then probably SpAgent hasn't been initialized 
			// (this can happen inside Visual Studio Designer)
			if ( productContext == null )
				return;

			Licenses = new ObservableCollection<LicenseItemModel>( LicenseRepository.RetrieveLicensesDueForRenewal( productContext ) );
			foreach ( var license in Licenses )
				license.ItemRenewalRequested += RenewSelectedItem;
		}

		public void RenewSelectedItem( object license, EventArgs e )
		{
			SetRenewalInProgress( true );
			LastRenewalResultMessage = string.Empty;
			var key = ((LicenseItemModel)license).ActivationKey;
			SpAgentActivation.ActivateOnline( key, OnActivationComplete );
		}

		void OnActivationComplete( Task task, string activationKey )
		{
			SetRenewalInProgress( false );
			if ( task.IsFaulted )
			{
				string errorMessage = task.Exception.Flatten().InnerException.Message;
				LastRenewalResultMessage = "Error: " + errorMessage;							
			}
			else
			{
				LastRenewalResultMessage = "Successfully renewed license with activation key " + activationKey;
				LastRenewalSucceeded = true;
				Reload();
			}
		}

		void SetRenewalInProgress( bool isInProgress )
		{
			IsRenewalInProgress = isInProgress;
		}

		public void Reload()
		{
			Licenses.Clear();
			foreach ( var item in LicenseRepository.RetrieveLicensesDueForRenewal( SpAgent.Product ) )
				Licenses.Add( item );
		}
	}

	public class LicenseItemModel
	{
		public string ActivationKey { get; set; }
		public DateTime ValidUntil { get; set; }
		public IEnumerable<string> Features { get; set; }
		public RelayCommand RenewLicenseCommand { get; set; }
		public event EventHandler ItemRenewalRequested;

		public LicenseItemModel()
		{
			RenewLicenseCommand = new RelayCommand( RenewLicense );
		}

		void RenewLicense()
		{
			if ( ItemRenewalRequested != null )
				ItemRenewalRequested( this, EventArgs.Empty );
		}
	}

	public class LicenseRepository
	{
		public static IEnumerable<LicenseItemModel> RetrieveLicensesDueForRenewal( IProductContext productContext )
		{
			return
				productContext.Licenses.DueForRenewalNow()
				.Select( l => new LicenseItemModel()
				{
					ActivationKey = l.ActivationKey,
					ValidUntil = l.ValidUntil,
					Features = l.Advanced.AllFeatures().Select( f => f.Key )
				} );
		}
	}
}