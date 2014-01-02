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
using DemoApp.Licenses;
using System.Windows;

namespace DemoApp
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void RunFeature1_Click( object sender, RoutedEventArgs e )
		{
			MyAlgorithms.AccessFeature1();
			MessageBox.Show( "Feature 1 accessed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}

		private void RunFeature2_Click( object sender, RoutedEventArgs e )
		{
			MyAlgorithms.AccessFeature2();
			MessageBox.Show( "Feature 2 accessed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}


		private void ShowInstalledLicensesList_Click( object sender, RoutedEventArgs e )
		{
			var licenseListDialog = new LicenseListDialog { Owner = this };
			licenseListDialog.ShowDialog();
		}

		void ShowActivationDialog_Click( object sender, RoutedEventArgs e )
		{
			var activationDialog = new ActivationDialog { Owner = this };
			activationDialog.ShowDialog();
		}
	}
}