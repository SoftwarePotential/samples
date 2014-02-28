/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using Sp.Agent;
using Sp.Agent.Distributor;
using System.Windows;
using System.Windows.Input;

namespace DemoApp.Checkout
{
	public partial class CheckoutDialog : Window, IDisplayCheckoutState
	{
		public CheckoutDialog()
		{
			InitializeComponent();		
			Load();
		}

		public static RoutedCommand CheckinCommand = new RoutedCommand();

		public void Load()
		{
			ICheckout checkout;
			if ( Checkout.TryGetCurrent( out checkout ) )
				ShowCurrentCheckout();
			else
				ShowAvailableCheckouts();
		}

		public void ShowCurrentCheckout()
		{
			var currentCheckoutPage = new CurrentCheckoutPage();
			((CurrentCheckoutModel)currentCheckoutPage.DataContext).State = this;

			CheckoutFrame.Navigate( currentCheckoutPage );
		}

		public void ShowAvailableCheckouts()
		{
			var availableCheckoutPage = new AvailableCheckoutPage();
			((AvailableCheckoutsModel)availableCheckoutPage.DataContext).State = this;
			CheckoutFrame.Navigate( availableCheckoutPage );
		}
				
		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString() );
		}

		ICheckoutContext Checkout
		{
			get { return SpAgent.Distributed.Checkout; }
		}
	}
}