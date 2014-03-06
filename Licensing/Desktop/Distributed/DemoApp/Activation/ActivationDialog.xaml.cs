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

namespace DemoApp.Activation
{
	public partial class ActivationDialog : Window, IDisplayState
	{
		public ActivationDialog()
		{
			InitializeComponent();
		}

		public void Navigate( Page page )
		{
		}

		public void NotifyUser( object message )
		{
			MessageBox.Show( message.ToString() );
		}
		
		public void Exit()
		{
			((Window)this).Close();
		}
	}
}