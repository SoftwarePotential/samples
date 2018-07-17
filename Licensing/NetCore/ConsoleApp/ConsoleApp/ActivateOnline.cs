using ManyConsole.CommandLineUtils;
using System;
using Sp.Agent;
using System.Threading.Tasks;

namespace ConsoleApp
{
	class ActivateOnline : ConsoleCommand
	{
		public ActivateOnline()
		{
			IsCommand( "Activate", "Activates a license using the supplied Activation Key." );
			HasRequiredOption( "k|key=", "E.g. -k <yourActivationKey>", s => _activationKey = s );
		}

		string _activationKey = string.Empty;

		public override int Run( string[] remainingArguments )
		{
			Activate();
			return 0;
		}

		void Activate()
		{
			if ( !ActivationKey.IsWellFormed(_activationKey) )
			{
				Console.WriteLine( $"Error: Activation key is not in the correct format: {_activationKey}." );
				return;
			}

			Console.WriteLine( $"Attempting to activate license with activation key {_activationKey}..." );
			SpAgent.Product.Activation.OnlineActivateAsync( _activationKey )
				.ContinueWith( task => OnActivationComplete( task, _activationKey ) )
				.Wait();
		}

		void OnActivationComplete( Task task, string activationKey )
		{
			if ( task.IsFaulted ) Console.Error.WriteLine(task.Exception.Flatten().InnerException.Message);
			else
			{
				Console.WriteLine( $"Success: Activated license with activation key {_activationKey}." );
				_activationKey = string.Empty;
			}
		}
	}
}
