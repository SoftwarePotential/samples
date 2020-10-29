/*
 * Copyright 2013-2021 (c) Inish Technology Ventures Limited.  All rights reserved.
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

namespace Sp.Samples.Consume.CustomerSync
{
	using RestSharp;
	using System;
	using System.Collections.Generic;

	public class SpCustomerApi : SpApi
	{
		public SpCustomerApi( )
			: base( )
		{
		}

		internal IRestResponse<CustomerSummaryPage> GetCustomerList()
		{
			var request = new RestRequest( "consumeapi" );
			return Execute<CustomerSummaryPage>( request );
		}

		internal IRestResponse<CustomerSummaryPage> GetCustomerList( string query )
		{
			var request = new RestRequest( "consumeapi/customer?" + query );
			return Execute<CustomerSummaryPage>( request );
		}

		internal IRestResponse CreateCustomer( CustomerSummary customer )
		{
			var request = new RestRequest( "consumeapi/customer", Method.POST );
			request.RequestFormat = DataFormat.Json;
			request.AddJsonBody( customer );
			return Execute( request );
		}

		internal IRestResponse<CustomerSummary> GetCustomer( string href )
		{
			var request = new RestRequest( href );
			return Execute<CustomerSummary>( request );
		}

		public IRestResponse PutCustomer( string href, CustomerSummary customer )
		{
			var request = new RestRequest( href, Method.PUT );
			request.RequestFormat = DataFormat.Json;
			request.AddJsonBody( customer );
			return Execute( request );
		}

		public IRestResponse DeleteCustomer( string href )
		{
			var request = new RestRequest( href, Method.DELETE );
			request.RequestFormat = DataFormat.Json;
			return Execute( request );
		}

		public class CustomerSummaryPage
		{
			public List<CustomerSummary> results { get; set; }
		}

		public class CustomerSummary
		{
			public Guid RequestId { get; set; }
			public string Name { get; set; }
			public string ExternalId { get; set; }

			public int _version { get; set; }
			public Links _links { get; set; }


			public _Embedded _embedded { get; set; }

			public class _Embedded
			{
				public Guid Id { get; set; }
				public Guid VendorId { get; set; }
			}
						
			public class Links
			{
				public Link self { get; set; }
			}
		}

		public class Link
		{
			public string href { get; set; }
		}
	}
}
