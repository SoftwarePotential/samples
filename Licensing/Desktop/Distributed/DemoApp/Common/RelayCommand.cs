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
		#region ICommand Members

		bool ICommand.CanExecute( object parameter )
		{
			if ( _TargetExecuteMethod != null )
			{
				return _TargetCanExecuteMethod();
			}
			return false;
		}

		public event EventHandler CanExecuteChanged = delegate { };

		void ICommand.Execute( object parameter )
		{
			if ( _TargetExecuteMethod != null )
			{
				_TargetExecuteMethod();
			}
		}
		#endregion
	}


	public class RelayCommand<T> : ICommand
	{
		Action<T> _TargetExecuteMethod;
		Func<T, bool> _TargetCanExecuteMethod;
		Func<object, T> _CommandParameterConverter;

		public RelayCommand( Action<T> executeMethod, Func<T, bool> canExecuteMethod, Func<object, T> commandParameterConverter )
		{
			_TargetExecuteMethod = executeMethod;
			_TargetCanExecuteMethod = canExecuteMethod;
			_CommandParameterConverter = commandParameterConverter;
		}

		public RelayCommand( Action<T> executeMethod, Func<T, bool> canExecuteMethod )
			: this( executeMethod, canExecuteMethod, x => (T)x )
		{
		}

		public RelayCommand( Action<T> executeMethod )
			: this( executeMethod, x => true )
		{
		}

		public void RaiseCanExecuteChanged()
		{
			CanExecuteChanged( this, EventArgs.Empty );
		}

		T ConvertParameter( object parameter )
		{
			T tparm = _CommandParameterConverter( parameter );
			return tparm;
		}

		#region ICommand Members
		bool ICommand.CanExecute( object parameter )
		{
			if ( _TargetCanExecuteMethod != null )
			{
				return _TargetCanExecuteMethod( ConvertParameter( parameter ) );
			}
			if ( _TargetExecuteMethod != null )
			{
				return true;
			}
			return false;
		}

		// Beware - should use weak references if command instance lifetime is longer than lifetime of UI objects that get hooked up to command
		// Prism commands solve this in their implementation
		public event EventHandler CanExecuteChanged = delegate { };

		void ICommand.Execute( object parameter )
		{
			if ( _TargetExecuteMethod != null )
			{
				_TargetExecuteMethod( ConvertParameter( parameter ) );
			}
		}
		#endregion
	}
}