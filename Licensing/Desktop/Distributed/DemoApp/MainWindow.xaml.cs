/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.Windows.Input;
using DemoApp.BusinessLogic;
using System.Windows;
using DemoApp.Licensing;
using DemoApp.Properties;

namespace DemoApp
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			if ( !Settings.Default.FirstRunLicensingConfigurationFinished )
			{
				var dialog = new LicensingConfigurationDialog();
				dialog.ShowDialog();
			}
		}

		public static RoutedCommand RunFeatureCommand = new RoutedCommand();

		void RunFeatureCommand_Executed( object sender, ExecutedRoutedEventArgs e )
		{
			int requestedFeatureNumber = Convert.ToInt32( e.Parameter );
			RunFeature( requestedFeatureNumber );
		}

		void RunFeatureCommand_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}

		static void RunFeature( int featureNumber )
		{
			switch ( featureNumber )
			{
				case 1: MyAlgorithms.AccessFeature1();
					break;
				case 2: MyAlgorithms.AccessFeature2();
					break;
				case 3: MyAlgorithms.AccessFeature3();
					break;
				default:
					throw new ArgumentOutOfRangeException( "featureNumber" );
			}
			MessageBox.Show( string.Format( "Feature {0} accessed successfully", featureNumber) , "Success", MessageBoxButton.OK, MessageBoxImage.Information );
		}

		void Configure_Click( object sender, RoutedEventArgs e )
		{
			var dialog = new LicensingConfigurationDialog { Owner = this };
			dialog.ShowDialog();
		}
	}
}
