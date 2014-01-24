using System;
using System.Windows;
using System.Windows.Input;

namespace DemoApp
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
			if ( this.CanExecute( parameter ) )
			{
				((Window)parameter).Close();
			}
		}

		#endregion

		CloseWindowCommand()
		{
		}

		public static readonly ICommand Instance = new CloseWindowCommand();
	}
}
