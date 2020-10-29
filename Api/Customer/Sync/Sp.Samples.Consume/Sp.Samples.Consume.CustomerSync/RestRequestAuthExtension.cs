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

using System;
using IdentityModel.Client;
using RestSharp;
using System.Net.Http;

namespace Sp.Samples.Consume.CustomerSync
{
    public static class RestRequestAuthExtension
    {
        public static void AddAuthorizationHeader(this IRestRequest that)
        {
            that.AddHeader("Authorization", "bearer " + GetAccessToken());
        }

        static string GetAccessToken()
        {
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = SpApiConfiguration.Authority.ToLower(),
                Policy =
                    {
                        RequireHttps =  false
                    }
            }).Result;

            if (disco.IsError)
                throw new System.Exception(disco.Error);
            var clientId = SpApiConfiguration.ClientId;
            var secret = SpApiConfiguration.ClientSecret;
            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = clientId,
                ClientSecret = secret,
                Scope = SpApiConfiguration.Scope
            }).Result;
            if (tokenResponse.IsError)
                throw new System.Exception(tokenResponse.Error);
            Console.WriteLine($"Got Access Token {DateTime.UtcNow}");
            return tokenResponse.AccessToken;
        }
    }
}
