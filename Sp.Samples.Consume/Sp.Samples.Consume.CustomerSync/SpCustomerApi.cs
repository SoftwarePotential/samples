

namespace Sp.Samples.Consume.CustomerSync
{
	using RestSharp;
	using Sp.Samples.Consume.CustomerSync.Wrappers;
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
			var request = new RestRequest( "/Consume" );
			return Execute<CustomerSummaryPage>( request );
		}

		internal IRestResponse<CustomerSummaryPage> GetCustomerList( string query )
		{
			var request = new RestRequest( "/Consume/customer?" + query );
			return Execute<CustomerSummaryPage>( request );
		}

		internal IRestResponse CreateCustomer( CustomerSummary customer )
		{
			var request = new RestRequest( "/Consume/customer", Method.POST );
			request.RequestFormat = DataFormat.Json;
			request.AddBody( customer );
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
			request.AddBody( customer );
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
