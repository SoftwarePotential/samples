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
namespace Sp.Samples.Consume.CustomerSync.Wrappers
{
	using RestSharp;

	public class SpApi
	{
		readonly IRestClient _client;

		public SpApi()
		{
			_client = new RelativePathAwareCustomRestClient( SpApiConfiguration.BaseUrl )
			{
				Authenticator = new WSFederationAuthenticator()
			};
		}

		public IRestResponse<T> Execute<T>( RestRequest request ) where T : new()
		{
			return _client.Execute<T>( request );
		}

		public IRestResponse Execute( RestRequest request )
		{
			return _client.Execute( request );
		}

		public IRestResponse SignOff()
		{
			var signOffRequest = new RestRequest( "Home/Authentication/LogOff", Method.GET );
			return _client.Execute( signOffRequest );
		}
	}
}
