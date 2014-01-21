/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System.Windows;
using System.Windows.Input;

namespace DemoApp.Licenses
{
	public partial class LicenseListDialog : Window
	{
		public LicenseListDialog()
		{
			InitializeComponent();
		}

		public static RoutedCommand RemoveLicenseCommand = new RoutedCommand();

		void RemoveLicenseCommand_Executed( object sender, ExecutedRoutedEventArgs e )
		{
			if ( MessageBox.Show( "Are you sure you want to remove this license?", "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes )
			{
				object[] parameters = (object[])e.Parameter;
				var licenseListModel = (LicenseListModel)parameters[ 0 ];
				var licenseToRemove = (LicenseItemModel)parameters[ 1 ];

				licenseListModel.DeleteLicense( licenseToRemove );
			}
		}

		void RemoveLicenseCommand_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
	}
}