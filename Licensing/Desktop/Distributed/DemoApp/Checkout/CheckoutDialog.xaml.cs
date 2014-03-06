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
using System.Windows.Input;

namespace DemoApp.Checkout
{
	public partial class CheckoutDialog : Window, IDisplayState
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
				Navigate(new CurrentCheckoutPage());
			else
				Navigate(new AvailableCheckoutPage());
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

		ICheckoutContext Checkout
		{
			get { return SpAgent.Distributed.Checkout; }
		}
	}
}