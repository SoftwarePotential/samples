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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Sp.Samples.Consume.CustomerSync
{
	class Program
	{
		static SpCustomerApi _customerApi;
		static string _externalIdFilter = "$filter=ExternalId eq '{0}'";
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		static int Main( string[] args )
		{
			if ( args.Length != 2 )
			{
				ShowUsage();
				return 1;
			}

			_customerApi = new SpCustomerApi();

			var customers = GetCustomerData( args[ 1 ] ).ToArray();
			switch ( args[ 0 ] )
			{
				case "-i":
					CreateBatch( customers );
					break;
				case "-u":
					UpdateBatchByExternalId(customers );
					break;
				case "-d":
					DeleteBatchByExternalId( customers );
					break;
			}

			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customers"></param>
		static async void CreateBatch( IEnumerable<SpCustomerApi.CustomerSummary> customers )
		{
			foreach ( var customer in customers )
				await Retry( () => _customerApi.CreateCustomer( customer ) );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="customers"></param>
		static async void UpdateBatchByExternalId( IEnumerable<SpCustomerApi.CustomerSummary> customers )
		{
			foreach ( var customer in customers )
			{
				var existingCustomer = GetCustomerByExternalId( customer.ExternalId );
				existingCustomer.Name = customer.Name;
				await Retry( () => _customerApi.PutCustomer( existingCustomer._links.self.href, existingCustomer ) );
			}
		}

		/// <summary>
		/// Given a batch of customerSumarries imported from a csv 
		/// Retrieve the existing customer from the server using its externalId as a unique identifier
		/// Send a delete httpRequest to the self link contained within the retieved customer
		/// </summary>
		/// <param name="customers"></param>
		static async void DeleteBatchByExternalId( IEnumerable<SpCustomerApi.CustomerSummary> customers )
		{
			foreach ( var customer in customers )
			{
				var existingCustomer = GetCustomerByExternalId( customer.ExternalId );
				await Retry( () => _customerApi.DeleteCustomer( existingCustomer._links.self.href ) );
			}
		}
		
		/// <summary>
		/// Retrieve a customer by externalId using the oData filter: $filter=ExternalId eq '{0}'
		/// Assumes externalId's are a unique identifier of an individual customer
		/// </summary>
		/// <param name="externalId"></param>
		/// <returns></returns>
		static SpCustomerApi.CustomerSummary GetCustomerByExternalId( string externalId )
		{
			return _customerApi.GetCustomerList( string.Format( _externalIdFilter, externalId ) ).Data.results.Single();
		}

		/// <summary>
		/// Parses inputted csv data assumed to be in the format Name,ExternalId and returns an enumerable of CustomerSummaries
		/// </summary>
		/// <param name="path"></param>
		/// <returns>IEnumerable<SpCustomerApi.CustomerSummary></returns>
		static IEnumerable<SpCustomerApi.CustomerSummary> GetCustomerData( string path )
		{
			return from line in File.ReadAllLines( path )
				   let customer = line.Split( ',' )
				   select new SpCustomerApi.CustomerSummary
				   {
					   Name = customer[ 0 ],
					   ExternalId = customer[ 1 ],
					   RequestId = Guid.NewGuid()
				   };
		}

		static void ShowUsage()
		{
			var message = string.Format(
						@"USAGE: {0} 
						/Type:<batchType> {-i: import, -u: update, -d: delete}
						/Path:<filePath>",
					System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName );
			Console.WriteLine( message );
		}

		/// <summary>
		/// Retry wrapper for calls to apis. If a response returns a status code which may be retryable e.g. internal server error attempt a retry
		/// </summary>
		/// <param name="func"></param>
		/// <param name="retryCount"></param>
		/// <returns></returns>
		static async Task<RestSharp.IRestResponse> Retry( Func<RestSharp.IRestResponse> func, int retryCount = 3 )
		{
			while ( true )
			{
				var result = await Task.Run( func );
				if ( result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Accepted || retryCount == 0 )
					return result;
				retryCount--;
				Thread.Sleep( 500 );
			}
		}
	}
}