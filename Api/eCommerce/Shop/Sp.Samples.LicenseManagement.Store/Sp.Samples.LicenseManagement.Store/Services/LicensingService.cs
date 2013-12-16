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

using Sp.Samples.LicenseManagement.Store.LicenseManagementWS;
using System.ServiceModel;

namespace Sp.Samples.LicenseManagement.Store.Services
{
	public class LicensingService
	{
		readonly Credentials _clientCredentials;

		public LicensingService( Credentials clientCredentials )
		{
			_clientCredentials = clientCredentials;
		}

		public License CreateLicenseFromSkuId( string skuId )
		{
			LicenseManagementWSClient client = CreateLicenseManagementClient( _clientCredentials );
			try
			{
				LicenseInfo licenseInfo = client.GetSkuById( skuId ).LicenseInfo;

				//Issue License from License info
				License license = client.CreateLicense( licenseInfo );
				client.Close();
				return license;
			}
			catch ( FaultException apiEx )
			{
				client.Abort();
				throw apiEx;
			}
		}

		private static LicenseManagementWSClient CreateLicenseManagementClient( Credentials credentials )
		{
			//System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(Validator);

			LicenseManagementWSClient client = new LicenseManagementWSClient("WSHttpBinding_ILicenseManagementWS");
			client.ClientCredentials.UserName.UserName = credentials.Username;
			client.ClientCredentials.UserName.Password = credentials.Password;

			return client;
		}
	}
}