using ManyConsole.CommandLineUtils;
using Sp.Agent;
using System;

namespace ConsoleApp
{
	class DeleteLicense : ConsoleCommand
	{
		public DeleteLicense()
		{
			IsCommand( "Delete", "Deletes the license corresponding to the supplied Activation Key." );
			HasRequiredOption( "k|key=", "The Activation Key for the license you wish to delete.", s => _activationKey = s );
		}

		string _activationKey = string.Empty;

		public override int Run( string[] remainingArguments )
		{
			Delete();
			return 0;
		}

		private void Delete()
		{
			SpAgent.Product.Stores.Delete( _activationKey );
			Console.WriteLine( $"Success: Deleted license with Activation Key {_activationKey}." );
		}
	}
}
