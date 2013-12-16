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

using System;

namespace Sp.Samples.LicenseManagement.Store
{
	public class CredentialsNotConfiguredException : Exception
	{
		public CredentialsNotConfiguredException()
			: base( FormatMessage() )
		{
		}

		static string FormatMessage()
		{
			return String.Format( @"The Software Potential API credentials (appSettings values for {0} and/or {1}) have not been initialized).
Please ensure the credentials are provisioned correctly", SoftwarePotentialConfigurationFile.SettingKey( "username" ), SoftwarePotentialConfigurationFile.SettingKey( "password" ) );
		}
	}
}

