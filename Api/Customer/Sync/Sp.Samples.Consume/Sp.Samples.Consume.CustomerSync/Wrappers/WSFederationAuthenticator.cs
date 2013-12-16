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
using System.Text;

namespace Sp.Samples.Consume.CustomerSync.Wrappers
{
	using System;
	using System.Linq;
	using System.Net;
	using HtmlAgilityPack;
	using RestSharp;
	using RestSharp.Contrib;

	class WSFederationAuthenticator : IAuthenticator
	{

		public WSFederationAuthenticator(  )
		{
		}

		public void Authenticate( IRestClient client, IRestRequest request )
		{
			if ( HasFedAuthCookie( client ) )
				return;

			client.CookieContainer = client.CookieContainer ?? new CookieContainer();
			var auth = new InnerAuthenticator(  );
			// An unauthenticated request attempt to the resource should redirect us to the STS
			var stsLoginUri = auth.AttemptRequestAndGetStsLoginUri( request );
			var authCookies = auth.SignInAndRetrieveFedAuthCookies( stsLoginUri );
			client.CookieContainer.Add( new Uri( client.BaseUrl ), authCookies );
		}

		static bool HasFedAuthCookie( IRestClient client )
		{
			return client.CookieContainer != null &&
				   client.CookieContainer.GetCookies( new Uri( client.BaseUrl ) ).OfType<Cookie>().Any( c => c.Name == "FedAuth" );
		}

		class InnerAuthenticator
		{
			readonly string _username;
			readonly string _password;
			readonly RestClient _authClient;

			public InnerAuthenticator(  )
			{
				_username = SpApiConfiguration.Username;
				_password = SpApiConfiguration.Password;
				_authClient = new RestClient( SpApiConfiguration.BaseUrl);
			}

			public Uri AttemptRequestAndGetStsLoginUri( IRestRequest request )
			{
				// An unauthenticated request attempt to resource should redirect us to the STS
				var loginPage = _authClient.Execute( request );

				if ( loginPage.StatusCode != HttpStatusCode.OK )
				{
					throw new InvalidOperationException( "Request for " + _authClient.BaseUrl + " failed; " + loginPage.ToDiagnosticString() );
				}
				var stsLoginUri = loginPage.ResponseUri;
				return stsLoginUri;
			}

			public CookieCollection SignInAndRetrieveFedAuthCookies( Uri stsLoginUri )
			{
				var wresultRequest = AuthenticateAndSignInWithIpSts( stsLoginUri );
				// Now we've got WS-Federation wsignin message from IP-STS that we should POST back to the FP-STS
				var rpAuthResponse = _authClient.Execute( wresultRequest );
				if ( rpAuthResponse.StatusCode != HttpStatusCode.OK )
					throw new InvalidOperationException( "Login wasn't successful; " + rpAuthResponse.ToDiagnosticString() );

				// Proceed with FP-STS - post wsignin message to the RP
				var wresultRequest2 = PrepareFederationWSignInRequest( rpAuthResponse.ResponseUri.ToString(), rpAuthResponse.Content );
				// NB - Restharp won't save cookies from the first response if there is a redirection, so we need to turn off FollowRedirects
				_authClient.FollowRedirects = false;
				rpAuthResponse = _authClient.Execute( wresultRequest2 );
				if ( rpAuthResponse.StatusCode != HttpStatusCode.Found || !rpAuthResponse.Cookies.Any( c => c.Name == "FedAuth" ) )
					throw new InvalidOperationException( "Login wasn't successful; " + rpAuthResponse.ToDiagnosticString() );

				//Now we have FedAuth and FedAuth1 cookies stored in the cookie container
				var restResponseCookies = rpAuthResponse.Cookies.Where( c => c.Name.StartsWith( "FedAuth" ) ).ToList();
				CookieCollection cookieCollection = new CookieCollection();
				restResponseCookies.ForEach( c => cookieCollection.Add( c.ToHttpCookie() ) );
				return cookieCollection;
			}

			/// <summary>
			/// Authenticates with STS.
			/// Returns RestRequest that should be POSTed to the RP.
			/// Throws InvalidOperationException if the provided credentials are incorrect.
			/// </summary>
			/// <returns></returns>
			RestRequest AuthenticateAndSignInWithIpSts( Uri stsLoginUri )
			{
				// Passive STS authentication
				RestRequest request = new RestRequest( stsLoginUri, Method.POST );
				// NB - we need to temporarily replace existing base URL, as the authentication STS has a different base URL
				var baseUriCopy = _authClient.BaseUrl;
				_authClient.BaseUrl = stsLoginUri.GetLeftPart( UriPartial.Authority );
				request.AddParameter( "Username", _username );
				request.AddParameter( "Password", _password );

				var authenticationResult = _authClient.Execute( request );
				if ( authenticationResult.StatusCode != HttpStatusCode.OK || !authenticationResult.Content.Contains( "wresult" ) )
					throw new InvalidOperationException( "Authentication with STS wasn't successful; " + authenticationResult.ToDiagnosticString() );
				_authClient.BaseUrl = baseUriCopy;

				return PrepareFederationWSignInRequest( _authClient.BaseUrl, authenticationResult.Content );
			}

			/// <summary>
			/// Reads fields from a hidden form that we got as authentication result (including wa, wresult, wctx), and prepares a WS-Federation wsignin message
			/// Browsers are capable of submitting this form automatically, as it contains a piece of JavaScript, but here we must build the request manually.
			/// </summary>
			static RestRequest PrepareFederationWSignInRequest( string baseUri, string authenticationResultContent )
			{
				HtmlDocument doc = new HtmlDocument();
				doc.LoadHtml( authenticationResultContent );
				var rpUri = doc.DocumentNode.SelectSingleNode( "//form" ).GetAttributeValue( "action", "" );
				RestRequest request = new RestRequest( UriHelper.MakeUriRelativeToBase( baseUri, new Uri( rpUri ).PathAndQuery ), Method.POST );
				foreach ( var input in doc.DocumentNode.SelectNodes( "//input[@type='hidden']" ) )
				{
					string name = input.GetAttributeValue( "name", string.Empty );
					string value = HttpUtility.HtmlDecode( input.GetAttributeValue( "value", string.Empty ) );
					//Console.WriteLine( "{0}:{1}", name, value );
					request.AddParameter( name, value );
				}
				request.AddParameter( "Accept", "text/html" );
				return request;
			}
		}
	}

	static class RestResponseExtensions
	{
		public static string ToDiagnosticString( this IRestResponse that )
		{
			return string.Format( "Response url: {0}; Response status: {1}; HTTP status code: {2} ({3}); Error message: {4}; Content: {5}",
				that.ResponseUri, that.ResponseStatus, that.StatusCode, that.StatusDescription, that.ErrorMessage, that.ContentWithEscapedCurlies() );
		}

		public static string ContentWithEscapedCurlies( this IRestResponse that )
		{
			//NB - Xunit runner may choke if REST content is included in an error message without curlies being escaped properly
			var builder = new StringBuilder( that.Content );
			builder.Replace( "{", "{{" );
			builder.Replace( "}", "}}" );
			return builder.ToString();
		}
	}
}