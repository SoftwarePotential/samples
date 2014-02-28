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

namespace DemoApp.Common
{
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
			if ( CanExecute( parameter ) )
			{
				((Window)parameter).Close();
			}
		}
		#endregion

		public static readonly ICommand Instance = new CloseWindowCommand();
	}
}