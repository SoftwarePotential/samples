/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using Sp.Agent;
using Sp.Agent.Distributor;
using Sp.Agent.Execution;
using Sp.Agent.Storage;
using System.Windows;
using System.Windows.Threading;

namespace DemoApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		// Called once during application startup
		// (event handler registered in App.xaml)
		void Application_Startup( object sender, StartupEventArgs e )
		{
			try
			{	// Initialize Sp Agent
				SpAgent.Configuration.VerifyStoresInitialized();
			}
			catch ( WritingStorageInaccessibleException exc )
			{
				ShowErrorMessageBox( "Unable to initialize the store in local user storage.\n\n" + exc.Message );
			}
		}

		// Global exception handling  
		// (event handler registered in App.xaml)
		void Application_DispatcherUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e )
		{
			if ( e.Exception is DistributorIntegrityNotLicensedException )
			{
				ShowErrorMessageBox( "There's an integrity issue with your distributor. Please contact your Distributor Administrator." );
				e.Handled = true;
			}
			else if ( e.Exception is DistributorNotLicensedException )
			{
				ShowErrorMessageBox( "There's a problem with your distributor. Please check your Licensing Configuration." );
				e.Handled = true;
			}
			else if ( e.Exception is NotLicensedException )
			{
				ShowErrorMessageBox( "You don't have a license for this feature. Please check your Licensing Status." );
				e.Handled = true;
			}
			else
			{
				//All other exceptions
				string exceptionMessage = e.Exception != null ? e.Exception.ToString() : e.ToString();
				ShowErrorMessageBox( "An error occurred: " + exceptionMessage );
				e.Handled = false;
			}
		}

		void ShowErrorMessageBox( string messageBoxText )
		{
			string popupTitle = MainWindow != null ? MainWindow.Title : "Error";
			MessageBox.Show( messageBoxText, popupTitle, MessageBoxButton.OK, MessageBoxImage.Error );
		}
	}
}