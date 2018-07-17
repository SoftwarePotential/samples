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
			var productContext = SpAgent.Product;
			Console.WriteLine( "Installed licenses:" );
			RetrieveAllLicenses( productContext )
				.ToList()
				.ForEach( x => Console.WriteLine( $"{x.ActivationKey} valid until {x.ValidUntil.ToShortDateString()}. Features: {JoinFeaturesOrNone( x.Features.ToArray() )} " ) );
		}

		string JoinFeaturesOrNone( string[] Features )
		{
			if ( Features.Count() < 1 ) return "None";
			return string.Join( ", ", Features );
		}

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
