using DemoApp.Activation;
using Sp.Agent;
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

			if ( e.Exception is NotLicensedException )
			{
				ShowErrorMessageBox( "You don't have a license for this feature" );
				e.Handled = true;

				// Redirect them to the activation dialog so they can install a valid license
				var activationDialog = new ActivationDialog { Owner = MainWindow };
				activationDialog.ShowDialog();
			}
			else
			{
				//All other exceptions
				string exceptionMessage = e.Exception != null ? e.Exception.Message : e.ToString();
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