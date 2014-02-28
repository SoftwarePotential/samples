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

namespace DemoApp.Checkout
{
	public class CurrentCheckoutModel : ViewModelBase
	{
		private ICheckout _checkout;
		DateTime _validUntil;

		public RelayCommand CheckinCommand { get; private set; }

		public IDisplayCheckoutState State { get; set; }

		public DateTime ValidUntil
		{
			get { return _validUntil; }
			set
			{
				_validUntil = value;
				OnPropertyChanged( "ValidUntil" );
			}
		}
		
		public ObservableCollection<string> Features { get; set; }
		public CurrentCheckoutModel(  )
		{
			Checkout.TryGetCurrent( out _checkout );
			ValidUntil = _checkout.ValidUntil;
			Features = new ObservableCollection<string>( _checkout.Features );
			CheckinCommand = new RelayCommand( Checkin );
		}
		
		public void Checkin()
		{
			try
			{
				_checkout.Relinquish();
				State.ShowAvailableCheckouts();
			}
			catch ( DistributorRequestException )
			{
				State.NotifyUser( "There has been an issue contacting your distributor server. Please try again. If the problem persists, please contact your system administrator." );
			}
			catch ( Exception exc )
			{
				State.NotifyUser( exc.Message );
			}
		}

		ICheckoutContext Checkout
		{
			get { return SpAgent.Distributed.Checkout; }
		}
	}
}
