/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Common;
using System.Windows;

namespace DemoApp.Licenses
{
	public partial class SubscriptionLicenseDialog : Window, IDisplayState
	{
		public SubscriptionLicenseDialog()
		{
			InitializeComponent();
			((ViewModelBase)DataContext).DisplayState = this;
		}

		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString(), "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}

		public bool Warn( object message )
		{
			return MessageBox.Show( message.ToString(), "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes;
		}

		public void Exit()
		{
			((Window)this).Close();
		}
	}
}