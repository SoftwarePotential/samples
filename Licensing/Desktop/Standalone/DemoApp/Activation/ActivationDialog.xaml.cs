/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace DemoApp.Activation
{
	public partial class ActivationDialog : Window
	{
		public ActivationDialog()
		{
			InitializeComponent();
		}

		public static RoutedCommand ActivationCommand = new RoutedCommand();

		void ActivationCommand_Executed( object sender, ExecutedRoutedEventArgs e )
		{
			var activationModel = (ActivationModel)e.Parameter;
			SetActivationInProgress( activationModel, true );

			var uiContext = TaskScheduler.FromCurrentSynchronizationContext();
			activationModel.ActivateOnlineAsync()
				.ContinueWith( task =>
				{
					SetActivationInProgress( activationModel, false );

					if ( task.IsFaulted )
					{
						string errorMessage = task.Exception.Flatten().InnerException.Message;
						MessageBox.Show( "Error: " + errorMessage );
					}
					else
					{
						MessageBox.Show( "Successfully activated license with activation key " + activationModel.ActivationKey );
						Close();
					}

				}, CancellationToken.None, TaskContinuationOptions.None, uiContext );
		}

		static void SetActivationInProgress( ActivationModel activationModel, bool isInProgress )
		{
			activationModel.IsActivationInProgress = isInProgress;
			// Notify the CommandManager that ActivationCommand CanExecute computed value could have changed (Activation button visibility depends on that)
			CommandManager.InvalidateRequerySuggested();
		}

		void ActivationCommand_CanExecute( object sender, CanExecuteRoutedEventArgs e )
		{
			var activationModel = (ActivationModel)e.Parameter;
			e.CanExecute = e.Parameter != null && !activationModel.IsActivationInProgress;
		}

		void ActivationDialog_OnClosing( object sender, CancelEventArgs e )
		{
			var activationModel = (ActivationModel)activationPanel.DataContext;
			e.Cancel = activationModel.IsActivationInProgress;
		}
	}
	#region Converters
	[ValueConversion( typeof( bool ), typeof( bool ) )]
	public class InverseBooleanConverter : IValueConverter
	{
		public object Convert( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			if ( targetType != typeof( bool ) )
				throw new InvalidOperationException( "The target must be a boolean" );

			return !(bool)value;
		}

		public object ConvertBack( object value, Type targetType, object parameter,
			System.Globalization.CultureInfo culture )
		{
			throw new NotSupportedException();
		}

	}
	#endregion
}
