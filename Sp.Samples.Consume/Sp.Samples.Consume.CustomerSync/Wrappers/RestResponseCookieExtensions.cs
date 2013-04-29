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
	using System.Net;
	using RestSharp;

	public static class RestResponseCookieExtensions
	{
		public static Cookie ToHttpCookie( this RestResponseCookie restResponseCookie )
		{
			return new Cookie( restResponseCookie.Name, restResponseCookie.Value, RemoveTrailingSlash( restResponseCookie.Path ), restResponseCookie.Domain ) { Expires = restResponseCookie.Expires, Expired = restResponseCookie.Expired, HttpOnly = restResponseCookie.HttpOnly };
		}

		static string RemoveTrailingSlash( string path )
		{
			if ( string.IsNullOrEmpty( path ) )
				return path;

			if ( path[ path.Length - 1 ] == '/' )
				return path.Substring( 0, path.Length - 1 );
			return path;
		}
	}
}