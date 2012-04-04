namespace Sp.Samples.Agent.WpfApplication
{
	using System;
	using System.Windows;
	using System.Windows.Threading;
	using Sp.Agent.Execution;
	using Sp.Agent.Storage;

	//using Sp.Agent.Execution;

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
			{
				// Initialize Sp Agent
				SpAgent.Initialize();
			}
			catch ( WritingStorageInaccessibleException exc )
			{
				ShowErrorMessageBox( "Please run the sample installer first.\n\n" + exc.Message );
				Shutdown();
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
