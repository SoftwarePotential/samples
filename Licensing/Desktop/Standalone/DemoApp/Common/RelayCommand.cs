/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.Windows.Input;

namespace DemoApp.Common
{
	public class RelayCommand : ICommand
	{
		readonly Action _TargetExecuteMethod;
		readonly Func<bool> _TargetCanExecuteMethod;

		public RelayCommand( Action executeMethod )
			: this( executeMethod, () => { return true; } ) { }

		public RelayCommand( Action executeMethod, Func<bool> canExecuteMethod )
		{
			_TargetExecuteMethod = executeMethod;
			_TargetCanExecuteMethod = canExecuteMethod;
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged( this, EventArgs.Empty );
		}

		bool ICommand.CanExecute( object parameter )
		{
			if ( _TargetExecuteMethod != null )
				return _TargetCanExecuteMethod();
			return false;
		}

		public event EventHandler CanExecuteChanged = delegate { };

		void ICommand.Execute( object parameter )
		{
			if ( _TargetExecuteMethod != null )
				_TargetExecuteMethod();
		}
	}
}