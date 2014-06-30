using DemoApp.Activation;
using Sp.Agent;
using Sp.Agent.Execution;
using Sp.Agent.Storage;
using System.Windows;
using System.Windows.Threading;
using System.Linq;
using System;
namespace DemoApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		const string RENEWAL_MESSAGE = "Some licenses need to be renewed. This can be done via the Licenses->Manage Subscriptions menu option.";
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

			// Check for subscription licenses that need to be renewed
			if ( HasLicensesDueForRenewal() )
				MessageBox.Show( RENEWAL_MESSAGE );
		}

		// Global exception handling  
		// (event handler registered in App.xaml)
		void Application_DispatcherUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e )
		{

			if ( e.Exception is NotLicensedException )
			{
				if ( HasLicensesDueForRenewal() )
					MessageBox.Show( "You don't have a license for this feature. " + RENEWAL_MESSAGE );
				else
					ShowErrorMessageBox( "You don't have a license for this feature." );
				e.Handled = true;		
			}
			else
			{
				//All other exceptions
				string exceptionMessage = e.Exception != null ? e.Exception.Message : e.ToString();
				ShowErrorMessageBox( "An error occurred: " + exceptionMessage );
				e.Handled = false;
			}
		}

		static bool HasLicensesDueForRenewal()
		{		
			return SpAgent.Product.Licenses.DueForRenewalNow().Any();
		}

		void ShowErrorMessageBox( string messageBoxText )
		{
			string popupTitle = MainWindow != null ? MainWindow.Title : "Error";
			MessageBox.Show( messageBoxText, popupTitle, MessageBoxButton.OK, MessageBoxImage.Error );
		}
	}
}