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
			IsCommand( "Install", "Installs a license using the supplied filepath." );
			HasRequiredOption( "l|license=", "E.g. -l <licenseFilename>", s => _licenseFilename = s );
		}

		string _licenseFilename = string.Empty;
		readonly string _licenseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile ), "Downloads" ); // make cross platform?

		public override int Run( string[] remainingArguments )
		{
			Install();
			return 0;
		}

		void Install()
		{
			try
			{
				SpAgent.Product.Stores.Install( File.ReadAllBytes( Path.Combine( _licenseDir, _licenseFilename ) ) );
				Console.WriteLine( "Success: The license has been successfully installed." );
			}
			catch ( LicenseRevisionException )
			{
				Console.Error.WriteLine( "Error: There is a newer version of the license already installed." );
			}
			catch ( NonmatchingProductIdException )
			{
				Console.Error.WriteLine( $"Error: The given license is not for the product: {SpAgent.Product.ProductName} v{SpAgent.Product.ProductVersion}." );
			}
			catch ( FileNotFoundException )
			{
				Console.Error.WriteLine( $"Error: Could not find license file at {_licenseFilename}." );
			}
		}
	}
}
