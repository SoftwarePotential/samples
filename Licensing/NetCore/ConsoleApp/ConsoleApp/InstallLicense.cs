using ManyConsole.CommandLineUtils;
using Sp.Agent;
using Sp.Agent.Licensing;
using Sp.Agent.Storage;
using System;
using System.IO;

namespace ConsoleApp
{
	class InstallLicense : ConsoleCommand
	{
		public InstallLicense()
		{
			IsCommand( "Install", "Installs a Software Potential license." );
			HasRequiredOption( "f|filename=", "The filename of the license.", s => _licenseFilename = s );
			HasOption( "d|directory=", "The path to the directory conainting the license. If not specified, the default is the current user's Desktop.", x => _licenseDir = x );
		}

		string _licenseFilename = string.Empty;
		string _licenseDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop );

		public override int Run( string[] remainingArguments )
		{
			Install();
			return 0;
		}

		void Install()
		{
			var path = Path.Combine( _licenseDir, _licenseFilename);
			try
			{
				SpAgent.Product.Stores.Install( File.ReadAllBytes( path ) );
				Console.WriteLine( "Success: The license has been successfully installed." );
			}
			catch ( LicenseRevisionException )
			{
				Console.WriteLine( "Error: There is a newer version of the license already installed." );
			}
			catch ( NonmatchingProductIdException )
			{
				Console.WriteLine( $"Error: The given license is not for the product: {SpAgent.Product.ProductName} v{SpAgent.Product.ProductVersion}." );
			}
			catch ( FileNotFoundException )
			{
				Console.WriteLine( $"Error: Could not find license at {path}." );
			}
		}
	}
}
