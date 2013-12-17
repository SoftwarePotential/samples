using System;
using System.Windows;
using System.Windows.Input;

namespace Sp.Samples.Agent.WpfApplication.Activation
{
	/// <summary>
	/// Interaction logic for ActivationDialog.xaml
	/// </summary>
	public partial class ActivationDialog : Window
	{
		public ActivationDialog()
		{
			InitializeComponent();
		}

		public static RoutedCommand ActivationCommand = new RoutedCommand();

		void ActivationCommand_Executed( object sender, ExecutedRoutedEventArgs e )
		{
			try
			{
				var activationModel = (ActivationModel)e.Parameter;
				activationModel.ActivateOnline();
				MessageBox.Show( "Successfully activated license with activation key " + activationModel.ActivationKey );
				Close();
			}
			catch ( Exception exc )
			{
				MessageBox.Show( "Error: " + exc.Message );
			}
		}

		void ActivationCommand_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			e.CanExecute = true;
		}
	}
}
