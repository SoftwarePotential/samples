using ManyConsole.CommandLineUtils;
using Sp.Agent;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp
{
	class ListLicenses : ConsoleCommand
	{
		public ListLicenses()
		{
			IsCommand( "List", "Displays a list of currently installed licenses." );
		}

		public override int Run( string[] remainingArguments )
		{
			DisplayLicenses();
			return 0;
		}

		void DisplayLicenses()
		{
			Console.WriteLine( "Installed licenses:" );

			var productContext = SpAgent.Product;
			var licenses = RetrieveAllLicenses( productContext );
			if ( licenses.Count() < 1 ) Console.WriteLine("None");
			else licenses
				.ToList()
				.ForEach( x => Console.WriteLine( $"{x.ActivationKey} valid until {x.ValidUntil.ToShortDateString()}. Features: {JoinFeatures( x.Features.ToArray() )} " ) );
		}

		string JoinFeatures( string[] Features ) => ( Features.Count() > 0 ) ? string.Join( ", ", Features ) : "None";

		public static IEnumerable<LicenseItemModel> RetrieveAllLicenses( IProductContext productContext ) =>
			from license in productContext.Licenses.All()
			select new LicenseItemModel()
			{
				ActivationKey = license.ActivationKey,
				ValidUntil = license.ValidUntil,
				Features = license.Advanced.AllFeatures().Select( f => f.Key )
			};
	}

	public class LicenseItemModel
	{
		public string ActivationKey { get; set; }
		public DateTime ValidUntil { get; set; }
		public IEnumerable<string> Features { get; set; }
	}
}
