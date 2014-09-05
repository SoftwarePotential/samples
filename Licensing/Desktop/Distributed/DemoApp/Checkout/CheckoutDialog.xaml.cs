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
using System.Windows;
using System.Windows.Controls;

namespace DemoApp.Checkout
{
	public partial class CheckoutDialog : Window, IDisplayState
	{
		public CheckoutDialog()
		{
			InitializeComponent();		
			Load();
		}
		
		public void Load()
		{
			try
			{
				ICheckout checkout;
				if ( Checkout.TryGetCurrent( out checkout ) )
					Navigate( new CurrentCheckoutPage() );
				else
				{
					var model = new AvailableCheckoutsModel( this );
					var availableCheckoutPage = new AvailableCheckoutPage { DataContext = model };
					Navigate( availableCheckoutPage );
				}
			}
			catch ( DistributorRequestException )
			{
				NotifyUser( "There has been an issue contacting your distributor server. Please try again. If the problem persists, please contact your system administrator." );
			}
			catch ( NoDistributorException )
			{
				NotifyUser( "There is no distributor server configured. Please configure a server in the configuration dialog." );
			}
		}

		public void Navigate( Page page )
		{			
			((ViewModelBase)page.DataContext).DisplayState = this;
			CheckoutFrame.Navigate( page );
		}

		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString() );
		}

		public void Exit()
		{
			((Window)this).Close();
		}
		
		public bool Warn(object message)
		{
			return MessageBox.Show( message.ToString(), "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes;
		}

		ICheckoutContext Checkout
		{
			get { return SpAgent.Distributed.Checkout; }
		}
	}
}
