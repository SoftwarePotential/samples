using ManyConsole.CommandLineUtils;
using Sp.Agent;
using System;
using System.Linq;

namespace ConsoleApp
{
	class ExecuteFeatures : ConsoleCommand
	{
		public ExecuteFeatures()
		{
			IsCommand( "Execute", "Execute licensed feature(s)." );
			AllowsAnyAdditionalArguments( "[Feature1] [Feature2]" );
		}

		public override int Run( string[] remainingArguments )
		{
			var invalidFeatures = remainingArguments.Except( SpAgent.Product.LocalFeatures.Valid() );
			if ( invalidFeatures.Count() > 0 )
			{
				Console.WriteLine( $"Not licensed: {string.Join( ", ", invalidFeatures )}" );
				return 1;
			}

			if ( remainingArguments.Contains( "Feature1" ) ) ExecuteFeature1();
			if ( remainingArguments.Contains( "Feature2" ) ) ExecuteFeature2();

			return 0;
		}

		[Demo_10.Features.Feature1]
		public void ExecuteFeature1()
		{
			Console.WriteLine( "Feature1 executing..." );
		}

		[Demo_10.Features.Feature2]
		public void ExecuteFeature2()
		{
			Console.WriteLine( "Feature2 executing..." );
		}
	}
}
