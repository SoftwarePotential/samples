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

using IdentityModel.Client;
using System;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Security.Claims;
using System.Xml;
using System.Xml.Linq;

namespace LicenseManagement
{
    public static class JwtTokenHelper
    {
        public static GenericXmlSecurityToken GetWrappedAccessToken(string clientId, string clientSecret, string scope)
        {
            var token = GetAccessToken(clientId, clientSecret, scope);
            return WrapJwt(token);
        }


        private static string GetAccessToken(string clientId, string clientSecret, string scope)
        {
            var client = new HttpClient();
            var disco = client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = SpApiConfiguration.Authority.ToLower(),
                Policy = { RequireHttps = false }
            }).Result;

            if (disco.IsError)
                throw new Exception(disco.Error);

            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = scope
            }).Result;
            if (tokenResponse.IsError)
                throw new Exception(tokenResponse.Error);
            Console.WriteLine($"Access token retrived, expires in {tokenResponse.ExpiresIn} secs");
            return tokenResponse.AccessToken;
        }

        private static GenericXmlSecurityToken WrapJwt(string jwt)
        {
            var subject = new ClaimsIdentity("saml");
            subject.AddClaim(new Claim(nameof(jwt), jwt));

            var descriptor = new SecurityTokenDescriptor
            {
                TokenType = "urn:oasis:names:tc:SAML:2.0:assertion",
                TokenIssuerName = "urn:wrappedjwt",
                Subject = subject
            };

            var handler = new Saml2SecurityTokenHandler();
            var jwttoken = handler.CreateToken(descriptor);

            var xml = jwttoken.ToTokenXmlString();
            var xelement = ToXmlElement(XElement.Parse(xml));

            var xmlToken = new GenericXmlSecurityToken(xelement, null, DateTime.UtcNow, DateTime.Now.AddHours(1), null, null, null);

            return xmlToken;
        }



        public static XmlElement ToXmlElement(XElement el)
        {
            var doc = new XmlDocument();
            doc.Load(el.CreateReader());
            return doc.DocumentElement;
        }
    }
}
