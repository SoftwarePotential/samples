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
	using System.Configuration;
	public static class SpApiConfiguration
	{
		public static string Username
		{
			get { return ConfigurationManager.AppSettings[ "Username" ]; }
		}

		public static string Password
		{
			get { return ConfigurationManager.AppSettings[ "Password" ]; }
		}

		public static string BaseUrl
		{
			get { return "https://srv.softwarepotential.com"; }
		}
	}
}
