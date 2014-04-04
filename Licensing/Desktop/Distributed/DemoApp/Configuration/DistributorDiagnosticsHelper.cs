/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using Sp.Agent;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.ServiceModel;

namespace DemoApp.Configuration
{
	class DistributorDiagnosticsHelper
	{
		public static DiagnosticsResult GetDiagnosticsInformation( Uri url )
		{
			var diagnosticsMessages = new List<string>();
			bool canConnect = false;
			try
			{
				canConnect = SpAgent.Distributors.CanConnect( url );
			}
			catch ( SocketException )
			{
			}

			if ( !canConnect )
			{
				diagnosticsMessages.Add( string.Format( "Could not connect to {0}.", url ) );
				return new DiagnosticsResult( false, diagnosticsMessages );
			}

			string version = SpAgent.Distributors.ServiceVersion( url );

			string serviceVendor;
			try
			{
				serviceVendor = SpAgent.Distributors.VendorName( url );
			}
			catch
			{
				// It should always be possible to obtain the vendor name - all Distributor services, regardless of version should support this API
				diagnosticsMessages.Add( string.Format( "Could not obtain vendor name from {0}.", url ) );
				return new DiagnosticsResult( false, diagnosticsMessages );
			}

			string agentVendor = SpAgent.Configuration.AgentContext.VendorName;
			var localPermutationIsForSameVendor = serviceVendor == agentVendor;
			if ( !localPermutationIsForSameVendor )
			{
				// Any communication with a server which is not for the correct vendor will yield exceptions due to an inability to complete the security handshake. 
				diagnosticsMessages.Add( string.Format( "The nominated service endpoint is incompatible with this Application; please select an alternative endpoint.\n\nService vendor name: \"{0}\"\nApplication vendor name: \"{1}\"\n\nService endpoint version: {2}", serviceVendor,  agentVendor, version ) );
				// Because IsServiceHealthy will fail, we bail and do not attempt to use it after a mismatch is detected
				return new DiagnosticsResult( false, diagnosticsMessages );
			}

			diagnosticsMessages.Add( string.Format( "Distributor service version {0} detected.", version ) );

			bool isServiceHealthy;
			try
			{
				isServiceHealthy = SpAgent.Distributors.IsServiceHealthy( url );
			}
			catch ( CommunicationException )
			{
				diagnosticsMessages.Add( string.Format( "Could not obtain service health status from  {0}.", url ) );
				return new DiagnosticsResult( false, diagnosticsMessages );
			}

			if ( !isServiceHealthy )
			{
				diagnosticsMessages.Add( string.Format( "Distributor service at {0} reports an integrity issue.", url ) );
				return new DiagnosticsResult( false, diagnosticsMessages );
			}

			return new DiagnosticsResult( true, diagnosticsMessages );
		}
	}

	class DiagnosticsResult
	{
		public bool AllVerificationsPassed { get; private set; }
		public IEnumerable<string> Messages { get; private set; }

		public DiagnosticsResult( bool allVerificationsPassed, IEnumerable<string> messages )
		{
			AllVerificationsPassed = allVerificationsPassed;
			Messages = messages;
		}

		public string GetAllMessagesAsString()
		{
			return string.Join( Environment.NewLine, Messages );
		}
	}
}