using System;
using System.Linq;
using System.Net;
using System.Threading;
using RestSharp;

namespace Sp.Samples.Consume.CustomerSync
{
	public class ExternalIdBasedCustomerList
	{
		readonly SpCustomerApi _customerApi;

		public ExternalIdBasedCustomerList( SpCustomerApi customerApi )
		{
			_customerApi = customerApi;
		}

		/// <summary>
		/// Tries to get a customer record for the given externalId
		/// If the externalId does not exist it does a create 
		/// If the externalId already exists it updates the name of the customer
		/// </summary>
		public void CreateOrUpdate( string externalId, string updatedName )
		{
			var existingCustomer = TryGetCustomer( externalId );
			if ( existingCustomer == null )
				Create( externalId, updatedName );
			else
				Update( updatedName, existingCustomer );
		}

		void Create( string externalId, string name )
		{
			var newCustomer = new SpCustomerApi.CustomerSummary { ExternalId = externalId, Name = name, RequestId = Guid.NewGuid() };
			IRestResponse response = Retry( () => _customerApi.CreateCustomer( newCustomer ) );
			if ( response.StatusCode != HttpStatusCode.OK )
				throw new Exception( string.Format( "Customer: {0} failed to create with status code {1}", externalId, response.StatusCode ) );
		}

		void Update( string updatedName, SpCustomerApi.CustomerSummary existingCustomer )
		{
			existingCustomer.Name = updatedName;
			IRestResponse response = Retry( () => _customerApi.PutCustomer( existingCustomer._links.self.href, existingCustomer ) );
			if ( response.StatusCode != HttpStatusCode.Accepted )
				throw new Exception( string.Format( "Customer: {0} failed to update with status code {1}", existingCustomer.ExternalId, response.StatusCode ) );
		}

		/// <summary>
		/// Given a batch of customerSumarries imported from a csv 
		/// Retrieve the existing customer from the server using its externalId as a unique identifier
		/// Send a delete httpRequest to the self link contained within the retieved customer
		/// </summary>
		public void Delete( string externalId )
		{
			var existingCustomer = TryGetCustomer( externalId );
			if ( existingCustomer == null )
				return;
			IRestResponse response = Retry( () => _customerApi.DeleteCustomer( existingCustomer._links.self.href ) );
			if ( response.StatusCode != HttpStatusCode.Accepted )
				throw new Exception( string.Format( "Customer: {0} failed to delete with status code {1}", externalId, response.StatusCode ) );
		}

		/// <summary>
		/// Retrieve a customer by externalId using the oData filter: $filter=ExternalId eq '{0}'
		/// Assumes externalId's are a unique identifier of an individual customer
		/// </summary>
		SpCustomerApi.CustomerSummary TryGetCustomer( string externalId )
		{
			var customer = Retry( () => _customerApi.GetCustomerList( string.Format( "$filter=ExternalId eq '{0}'", externalId ) ) );
			return customer.Data.results.SingleOrDefault();
		}

		/// <summary>
		/// Retry wrapper for calls to apis.
		/// The Software Potential Customer Management APIs return 200 OK or 202 Accepted 
		/// </summary>
		static T Retry<T>( Func<T> func, int retryCount = 3 ) where T : IRestResponse
		{
			while ( true )
			{
				var result = func();
				if ( result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Accepted || retryCount == 0 )
					return result;
				retryCount--;
				Thread.Sleep( 500 );
			}
		}
	}
}