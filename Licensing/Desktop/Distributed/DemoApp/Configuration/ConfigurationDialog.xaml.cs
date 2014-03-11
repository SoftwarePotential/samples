/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using DemoApp.Common;
using System.Windows;
using System.Windows.Controls;

namespace DemoApp.Configuration
{
	public partial class ConfigurationDialog : Window, IDisplayState
	{
		public ConfigurationDialog()
		{
			InitializeComponent();
			Navigate( new MainPage() );
		}
		
		public void Navigate( Page page )
		{
			((ViewModelBase)page.DataContext).DisplayState = this;
			ConfigurationFrame.Navigate( page );
		}

		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString() );
		}
		
		public bool Warn( object message )
		{
			return MessageBox.Show( message.ToString(), "Please confirm", MessageBoxButton.YesNo ) == MessageBoxResult.Yes;
		}

		public void Exit()
		{
			Close();
		}
	}
}