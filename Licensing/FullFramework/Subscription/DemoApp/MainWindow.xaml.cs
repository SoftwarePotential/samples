/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Activation;
using DemoApp.BusinessLogic;
using DemoApp.Common;
using DemoApp.Licenses;
using System.Windows;

namespace DemoApp
{
	public partial class MainWindow : Window, IDisplayState
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		void RunFeature1_Click( object sender, RoutedEventArgs e )
		{
			MyAlgorithms.AccessFeature1();
			NotifyUser( "Feature 1 accessed successfully" );
		}

		void RunFeature2_Click( object sender, RoutedEventArgs e )
		{
			MyAlgorithms.AccessFeature2();
			NotifyUser( "Feature 2 accessed successfully" );
		}

		void ShowInstalledLicensesList_Click( object sender, RoutedEventArgs e )
		{
			var licenseListDialog = new SubscriptionLicenseDialog { Owner = this };
			licenseListDialog.ShowDialog();
		}

		void ShowActivationDialog_Click( object sender, RoutedEventArgs e )
		{
			var activationDialog = new ActivationDialog { Owner = this };
			//	((ViewModelBase)activationDialog.DataContext).DisplayState = this;
			activationDialog.ShowDialog();
		}

		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString(), "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}

		public void Exit()
		{
			((Window)this).Close();
		}

		public bool Warn( object message )
		{
			return MessageBox.Show( message.ToString(), "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes;
		}
	}
}