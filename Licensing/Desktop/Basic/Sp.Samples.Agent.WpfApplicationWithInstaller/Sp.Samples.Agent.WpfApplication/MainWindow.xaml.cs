/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */
namespace Sp.Samples.Agent.WpfApplication
{
	using System.Windows;
	using Sp.Samples.Agent.WpfApplication.Activation;
	using Sp.Samples.Agent.WpfApplication.BusinessLogic;
	using Sp.Samples.Agent.WpfApplication.Licenses;

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void RunFeatureA_Click( object sender, RoutedEventArgs e )
		{
			MyAlgorithms.AccessFeatureA();
			MessageBox.Show( "Feature A accessed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}

		private void RunFeatureB_Click( object sender, RoutedEventArgs e )
		{
			MyAlgorithms.AccessFeatureB();
			MessageBox.Show( "Feature B accessed successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}


		private void ShowInstalledLicensesList_Click( object sender, RoutedEventArgs e )
		{
			var licenseListView = new LicenseListView { Owner = this };
			licenseListView.ShowDialog();
		}

		void ShowActivationDialog_Click( object sender, RoutedEventArgs e )
		{
			var licenseListView = new ActivationDialog { Owner = this };
			licenseListView.ShowDialog();
		}
	}
}
