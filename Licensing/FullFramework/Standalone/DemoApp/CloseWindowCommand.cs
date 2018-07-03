using System;
using System.Windows;
using System.Windows.Input;

namespace DemoApp
{
	public class CloseWindowCommand : ICommand
	{
		#region ICommand Members

		void ICommand.Execute( object parameter )
		{
			if ( ((ICommand)this).CanExecute( parameter ) )
			{
				((Window)parameter).Close();
			}
		}

		bool ICommand.CanExecute( object parameter )
		{
			return true;
		}

		event EventHandler ICommand.CanExecuteChanged
		{
			add { }
			remove { }
		}

		#endregion

		CloseWindowCommand()
		{
		}

		public static readonly ICommand Instance = new CloseWindowCommand();
	}
}
