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
	using System;
	using RestSharp;

	public class RelativePathAwareCustomRestClient : RestClient
	{
		public RelativePathAwareCustomRestClient( string baseUrl )
		{
			BaseUrl = baseUrl;
		}

		public override IRestResponse<T> Execute<T>( IRestRequest request )
		{
			request.Resource = MakeUriRelativeToRestSharpClientBaseUri( request.Resource ).ToString();
			return base.Execute<T>( request );
		}

		public override IRestResponse Execute( IRestRequest request )
		{
			request.Resource = MakeUriRelativeToRestSharpClientBaseUri( request.Resource ).ToString();
			return base.Execute( request );
		}

		// Required if your BaseUri includes a path (e.g., within InishTech test environments, instances are not always at / on a machine)
		Uri MakeUriRelativeToRestSharpClientBaseUri( string resource )
		{
			return UriHelper.MakeUriRelativeToBase( BaseUrl, resource );
		}
	}
}
