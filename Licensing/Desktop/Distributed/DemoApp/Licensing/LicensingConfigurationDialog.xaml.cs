/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.Windows;
using System.Windows.Input;
using DemoApp.Activation;
using DemoApp.Licenses;
using DemoApp.Properties;

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
			model.ReloadListFrom( () => new LicenseListModelFactory().CreateLicenseList() );
		}

		void Save_Click( object sender, RoutedEventArgs e )
		{
			var model = (DistributorConfigurationModel)GetDataContext( e );
			if ( model.HasValidDistributorUrl )
			{
				var diagnosticsResult = DistributorDiagnosticsHelper.GetDiagnosticsInformation( new Uri( model.DistributorUrl ) );
				if ( !diagnosticsResult.AllVerificationsPassed )
				{
					var messages = diagnosticsResult.GetAllMessagesAsString() + "\nDo you want to save this configuration anyway?";

					var warningMessageBoxResult = MessageBox.Show( messages, "Warning", MessageBoxButton.YesNo,
						MessageBoxImage.Warning );
					if ( warningMessageBoxResult == MessageBoxResult.No )
						return;
				}
			}

			DistributorConfigurationModelRepository.Save( model );
			SetFistRunLicensingConfigurationFinishedIfApplies();
			Close();
		}

		void TestConnection_Click( object sender, RoutedEventArgs e )
		{
			var model = (DistributorConfigurationModel)GetDataContext( e );
			var diagnosticsResult = DistributorDiagnosticsHelper.GetDiagnosticsInformation( new Uri( model.DistributorUrl ) );
			MessageBox.Show( diagnosticsResult.GetAllMessagesAsString(), "Connectivity test", MessageBoxButton.OK,
				diagnosticsResult.AllVerificationsPassed ? MessageBoxImage.Information : MessageBoxImage.Warning );
		}

		static void SetFistRunLicensingConfigurationFinishedIfApplies()
		{
			if ( !Settings.Default.FirstRunLicensingConfigurationFinished )
			{
				Settings.Default.FirstRunLicensingConfigurationFinished = true;
				Settings.Default.Save();
			}
		}

		static object GetDataContext( RoutedEventArgs e )
		{
			return ((FrameworkElement)e.Source).DataContext;
		}
	}

	public class CloseWindowCommand : ICommand
	{
		#region ICommand Members

		public bool CanExecute( object parameter )
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute( object parameter )
		{
			if ( this.CanExecute( parameter ) )
			{
				((Window)parameter).Close();
			}
		}

		#endregion

		CloseWindowCommand()
		{
		}

		public static readonly ICommand Instance = new CloseWindowCommand();
	}
}
