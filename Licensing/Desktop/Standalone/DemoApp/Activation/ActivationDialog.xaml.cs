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
