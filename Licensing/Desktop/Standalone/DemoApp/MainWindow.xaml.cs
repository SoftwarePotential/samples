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
			MessageBox.Show( "Feature 1 accessed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}

		void RunFeature2_Click( object sender, RoutedEventArgs e )
		{
			MyAlgorithms.AccessFeature2();
			MessageBox.Show( "Feature 2 accessed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}
		 
		void ShowInstalledLicensesList_Click( object sender, RoutedEventArgs e )
		{
			
			new LicenseListDialog { Owner = this }.ShowDialog();
		}

		void ShowActivationDialog_Click( object sender, RoutedEventArgs e )
		{
			//	((ViewModelBase)activationDialog.DataContext).DisplayState = this;
			new ActivationDialog { Owner = this }.ShowDialog();
		}

		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString() );
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