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
			IsCommand( "Generate", "Generates a Manual Activation Request using the supplied Activation Key and saves it on the current user's Desktop" );
			HasRequiredOption( "k|key=", "The Activation Key for the desired license.", s => _activationKey = s );
		}

		string _activationKey = string.Empty;
		readonly string _requestDir = Environment.GetFolderPath( Environment.SpecialFolder.Desktop );

		public override int Run( string[] remainingArguments )
		{
			Generate();
			return 0;
		}

		void Generate()
		{
			if ( !ActivationKey.IsWellFormed( _activationKey ) ) {
				Console.WriteLine( $"Error: Activation Key is not in the correct format: {_activationKey}." );
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
				Console.WriteLine( $"Success: The Manual Activation Request has been saved to disk at {path}.");
			}
			catch ( IOException )
			{
				Console.WriteLine( "Error: The Manual Activation Request couldn't be saved to disk." );
			}
		}
	}
}
