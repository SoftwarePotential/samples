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
    using System.Net;

    public class SpApi : RestClient
    {
        public SpApi()
        {
            BaseUrl = new Uri(SpApiConfiguration.BaseUrl);
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }
        public IRestResponse<T> Execute<T>( RestRequest request ) where T : new()
		{
            request.AddAuthorizationHeader();
            return base.Execute<T>( request );
		}

		public IRestResponse Execute( RestRequest request )
		{
            request.AddAuthorizationHeader();
            return base.Execute( request );
		}
	}
}
