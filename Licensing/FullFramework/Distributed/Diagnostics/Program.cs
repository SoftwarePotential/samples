﻿using DocoptNet;
using Sp.Agent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceModel;

namespace Diagnostics
{
	public class Program
	{
		const string PORT = ":8731";
		static int Main( string[] args )
		{
			try
			{
				return RunWithBannerAndOrUsage( args, "Software Potential Distributor Diagnostics tool", @"

Commands:
  all           Perform a complete diagnostic for a nominated distributor service.
  
  vendor        Perform a vendor name diagnostic against the given distributor service.
  connectivity  Perform a connectivity diagnostic on the path to a given distributor service.
  integrity     Perform an integrity diagnostic on a given distributor service.

  version       Obtains service version.

Usage: 
  {0} (--help | -h)
  {0} all --serverName=<serverName> [--nologo] [--verbose]
  {0} vendor --serverName=<serverName> [--verbose]
  {0} connectivity --serverName=<serverName> [--verbose]
  {0} integrity --serverName=<serverName> [--verbose]
  {0} version --serverName=<serverName>  [--verbose]

Options:
--help -h                         Show this screen.
--serverName=<serverName>         Server machine name.
"
					, arguments =>
				{
					var verbose = arguments[ "--verbose" ].IsTrue;
					if ( verbose )
						foreach ( var argument in arguments )
							Console.WriteLine( "{0} = {1}", argument.Key, argument.Value );

					if ( arguments[ "all" ].IsTrue )
					{
						Console.WriteLine( "CONNECTIVITY DIAGNOSTIC:" );
						Console.WriteLine();
						Connectivity( arguments[ "--serverName" ].ToString() );
						Console.WriteLine();
						Console.WriteLine( "VENDOR DIAGNOSTIC:" );
						Console.WriteLine();
						Vendor( arguments[ "--serverName" ].ToString() );
						Console.WriteLine();
						Console.WriteLine( "INTEGRITY DIAGNOSTIC:" );
						Console.WriteLine();
						Integrity( arguments[ "--serverName" ].ToString() );
					}
					else if ( arguments[ "connectivity" ].IsTrue )
						Connectivity( arguments[ "--serverName" ].ToString() );
					else if ( arguments[ "vendor" ].IsTrue )
						Vendor( arguments[ "--serverName" ].ToString() );
					else if ( arguments[ "integrity" ].IsTrue )
						Integrity( arguments[ "--serverName" ].ToString() );
					else if ( arguments[ "version" ].IsTrue )
						ServiceVersion( arguments[ "--serverName" ].ToString() );

					if ( verbose )
						Console.WriteLine( "Completed successfully." );
				} );
			}
			catch ( Exception ex )
			{
				Console.Error.WriteLine( "EXCEPTION: " + Environment.NewLine + ex );
				return 1;
			}
		}

		static void Connectivity( string serverName )
		{
			try
			{
				if ( SpAgent.Distributors.CanConnect( ServiceBaseUri( serverName ) ) )
					Console.WriteLine( "Service is contactable." );
				else
					Console.WriteLine( "Service is not contactable." );
			}
			catch ( Exception ex )
			{
				Console.WriteLine( ex );
			}
		}

		static void Vendor( string serverName )
		{
			var serverVendorName = SpAgent.Distributors.VendorName( ServiceBaseUri( serverName ) );
			var clientVendorName = SpAgent.Configuration.AgentContext.VendorName;
			if ( serverVendorName == clientVendorName )
				Console.WriteLine( "Vendor name check passed. Vendor Name: " + serverVendorName );
			else
				Console.WriteLine( "Vendor name check failed. Server Vendor Name: " + serverVendorName + "; Client Vendor Name: " + clientVendorName );
		}

		static void Integrity( string serverName )
		{
			if ( SpAgent.Distributors.IsServiceHealthy( ServiceBaseUri( serverName ) ) )
				Console.WriteLine( "Integrity check passed." );
			else
				Console.WriteLine( "Integrity check failed. Service State is corrupted." );
		}

		static void ServiceVersion( string serverName )
		{
			try
			{
				var serverVersion = SpAgent.Distributors.ServiceVersion( ServiceBaseUri( serverName ) );
				Console.WriteLine( "Service Version: " + serverVersion );
			}
			catch ( EndpointNotFoundException )
			{
				Console.WriteLine( "Service is not contactable." );
			}
			catch ( Exception ex )
			{
				Console.Write( ex );
			}
		}

		static Uri ServiceBaseUri( string serverName )
		{
			return new Uri( "http://" + serverName + PORT );
		}

		static int RunWithBannerAndOrUsage( string[] args, string name, string usage, Action<IDictionary<string, ValueObject>> run )
		{
			try
			{
				var parser = new CommandLineParser( name, usage );

				var arguments = parser.Parse( args );

				run( arguments );

				return 0;
			}
			catch ( DocoptBaseException ex )
			{
				if ( ex.ErrorCode == 0 )
					Console.WriteLine( ex.Message );
				else
					Console.Error.WriteLine( ex.Message );
				return ex.ErrorCode;
			}
		}

		class CommandLineParser
		{
			readonly string _name;
			readonly string _usage;

			public CommandLineParser( string name, string usage )
			{
				_name = name;
				_usage = usage;
			}

			public IDictionary<string, ValueObject> Parse( ICollection<string> args )
			{
				if ( args.Count == 0 )
					args = new[] { "--help" };

				return new Docopt().Apply( string.Format( _usage, ExeName ), args );
			}

			static string ExeName
			{
				get { return Path.GetFileNameWithoutExtension( ExePath ); }
			}

			static string ExePath
			{
				get { return new Uri( Assembly.CodeBase ).LocalPath; }
			}

			static Assembly Assembly
			{
				get { return typeof( Program ).Assembly; }
			}
		}
	}
}