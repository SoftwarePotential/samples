/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Common;
using Sp.Agent;
using Sp.Agent.Distributor;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DemoApp.Checkout
{
	public class AvailableCheckoutsModel : ViewModelBase, IDataErrorInfo
	{
		ObservableCollection<IAvailableCheckout> _availableChecktous;
		IAvailableCheckout _selectedAvailableCheckout;
		DateTime _acquireCheckoutUntil;

		public RelayCommand AcquireCheckoutCommand { get; private set; }
		public RelayCommand RefreshCommmand { get; private set; }

		public ObservableCollection<IAvailableCheckout> AvailableCheckouts
		{
			get { return _availableChecktous; }
			set
			{
				_availableChecktous = value;
				OnPropertyChanged( "AvailableCheckouts" );
			}
		}

		public IAvailableCheckout SelectedAvailableCheckout
		{
			get { return _selectedAvailableCheckout; }
			set
			{
				_selectedAvailableCheckout = value;
				AcquireCheckoutCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "SelectedAvailableCheckout" );
			}
		}

		public DateTime AcquireCheckoutUntil
		{
			get
			{
				if ( _acquireCheckoutUntil == DateTime.MinValue )
					_acquireCheckoutUntil = DateTime.UtcNow;
				return _acquireCheckoutUntil;
			}
			set
			{
				_acquireCheckoutUntil = value.AddDays( 1 ).Subtract( TimeSpan.FromSeconds( 1 ) ); // Bring it up to just before midnight on the selected date
				AcquireCheckoutCommand.RaiseCanExecuteChanged();
				OnPropertyChanged( "AcquireCheckoutUntil" );
			}
		}

		public AvailableCheckoutsModel(IDisplayState displayState)
		{
			RefreshCommmand = new RelayCommand( Load );
			AcquireCheckoutCommand = new RelayCommand( AcquireCheckout, CanAcquireCheckout );
			DisplayState = displayState;
			Load();
		}

		public void Load()
		{
			try
			{
				IAvailableCheckout[] checkoutContextAvailableNow = CheckoutContext.AvailableNow();
				AvailableCheckouts = new ObservableCollection<IAvailableCheckout>( checkoutContextAvailableNow );
			}
			catch ( DistributorRequestException )
			{
				DisplayState.NotifyUser( "There has been an issue contacting your distributor server. Please try again. If the problem persists, please contact your system administrator." );
			}
			catch ( DistributorIntegrityException )
			{
				DisplayState.NotifyUser( "We have detected an integrity issue with your distributor server. Please contact your system administrator." );
			}
			catch ( NoDistributorException )
			{
				DisplayState.NotifyUser( "There is no distributor server configured. Please configure a server in the configuration dialog." );
			}
		}

		public void AcquireCheckout()
		{
			try
			{
				SelectedAvailableCheckout.Acquire( AcquireCheckoutUntil );
				DisplayState.Navigate( new CurrentCheckoutPage() );
			}
			catch ( NoLongerAvailableException )
			{
				DisplayState.NotifyUser( "The requested checkout is no longer available. Please refresh the list above and select another available chekcout." );
			}
			catch ( DistributorRequestException )
			{
				DisplayState.NotifyUser( "There has been an issue contacting your distributor server. Please try again. If the problem persists, please contact your system administrator." );
			}
			catch ( DistributorIntegrityException )
			{
				DisplayState.NotifyUser( "We have detected an integrity issue with your distributor server. Please contact your system administrator." );
			}
			catch ( Exception exc )
			{
				DisplayState.NotifyUser( "Error: " + exc.Message );
			}
		}

		public bool CanAcquireCheckout()
		{
			return (SelectedAvailableCheckout != null) && (!IsPastDate() && !IsAfterValidUntilDate());
		}

		public bool IsPastDate()
		{
			return AcquireCheckoutUntil < DateTime.UtcNow;
		}

		public bool IsAfterValidUntilDate()
		{
			return AcquireCheckoutUntil > SelectedAvailableCheckout.AvailableUntil;
		}

		public string this[ string columnName ]
		{
			get
			{
				if ( columnName == "AcquireCheckoutUntil" && SelectedAvailableCheckout != null )
				{
					if ( IsPastDate() )
						return "Selected checkout date cannot be in the past";
					if ( IsAfterValidUntilDate() )
						return "Selected date is greater than the maximum available checkout date for the selected item.";

				}
				return null;
			}
		}

		public string Error
		{
			get { return this[ "AquireCheckoutUntil" ]; }
		}
	
		static ICheckoutContext CheckoutContext
		{
			get { return SpAgent.Distributed.Checkout; }
		}
	}
}