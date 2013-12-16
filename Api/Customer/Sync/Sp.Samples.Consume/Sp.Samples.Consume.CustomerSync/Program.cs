/*
 * Copyright 2013 (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/wiki/License
 * 
 */

// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.

using System;

namespace Sp.Samples.Consume.CustomerSync
{
	class Program
	{
		static int Main( string[] args )
		{
			try
			{
				var customers = new ExternalIdBasedCustomerList( new SpCustomerApi() );
				if ( args.Length == 3 && String.Equals( args[ 0 ], "CREATEORUPDATE", StringComparison.OrdinalIgnoreCase ) )
				{
					customers.CreateOrUpdate( args[ 1 ], args[ 2 ] );
					return 0;
				}
				else if ( args.Length == 3 && String.Equals( args[ 0 ], "DELETE", StringComparison.OrdinalIgnoreCase ) )
				{
					customers.Delete( args[ 1 ] );
					return 0;
				}
				else
				{
					ShowUsage();
					return 2;
				}
			}
			catch ( Exception ex )
			{
				Console.Error.WriteLine( "Failed to " + args[ 0 ] + " Customer ExternalId: " + args[ 1 ] + Environment.NewLine + ex );
				return 1;
			}
		}

		static void ShowUsage()
		{
			var message = string.Format( @"USAGE: {0} Action ExternalId Name
Action: CreateOrUpdate|Delete
ExternalId: The identifier you have assigned the customer within your CRM system
Name: The Customer Name to be shown within Software Potential",
					System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName );
			Console.WriteLine( message );
		}
	}
}