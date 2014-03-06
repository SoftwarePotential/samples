/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Activation;
using DemoApp.Licenses;
using System.Windows;

namespace DemoApp.Licensing
{
	/// <summary>
	/// Interaction logic for LicensingConfigurationDialog.xaml
	/// </summary>
	public partial class LicensingConfigurationDialog : Window
	{
		public LicensingConfigurationDialog()
		{
			InitializeComponent();
		}

		void Activate_Click( object sender, RoutedEventArgs e )
		{
			ShowDialogAndRefreshLicenseDataSource( new ActivationDialog() );
		}

		void ViewLicenses_Click( object sender, RoutedEventArgs e )
		{
			ShowDialogAndRefreshLicenseDataSource( new LicenseListDialog() );
		}

		void ShowDialogAndRefreshLicenseDataSource( Window dialogWindow )
		{
			dialogWindow.Owner = this;
			dialogWindow.ShowDialog();

			var model = (LicenseListModel)localLicensesConfigurationPanel.DataContext;
			model.Reload();
		}

	}
}