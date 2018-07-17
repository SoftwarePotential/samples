using ManyConsole.CommandLineUtils;
using Sp.Agent;
using System;
using System.IO;

namespace ConsoleApp
{
	class GenerateActivationRequest : ConsoleCommand
	{
		public GenerateActivationRequest()
		{
			IsCommand( "Generate", "Generates an activation request using the supplied Activation Key." );
			HasRequiredOption( "k|key=", "E.g. -k <yourActivationKey>", s => _activationKey = s );
		}

		string _activationKey = string.Empty;
		readonly string _requestDir = Environment.GetFolderPath( Environment.SpecialFolder.Desktop ); // make cross platform?

		public override int Run( string[] remainingArguments )
		{
			Generate();
			return 0;
		}

		void Generate()
		{
			if ( !ActivationKey.IsWellFormed( _activationKey ) ) {
				Console.WriteLine( $"Error: Activation key is not in the correct format: {_activationKey}." );
				return;
			}

			SaveActivationRequest( SpAgent.Product.Activation.Advanced().CreateManualActivationRequest( _activationKey, null ) );
		}

		void SaveActivationRequest( string request )
		{
			var path = Path.Combine( _requestDir, $"{_activationKey}.txt" );
			try
			{
				File.WriteAllText(  path, request );
				Console.WriteLine( $"Success: The Activation Request has been saved to disk at {path}.");
			}
			catch ( IOException )
			{
				Console.Error.WriteLine("Error: The Activation Request couldn't be saved to disk.");
			}
		}
	}
}
