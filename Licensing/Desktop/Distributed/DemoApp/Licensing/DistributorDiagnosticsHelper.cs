/*
 * Copyright (c) Inish Technology Ventures Limited.  All rights reserved.
 * 
 * This code is licensed under the BSD 3-Clause License included with this source
 * 
 * ALSO SEE: https://github.com/SoftwarePotential/samples/blob/master/License.txt
 * 
 */

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.ServiceModel;
using Sp.Agent;

namespace DemoApp.Licensing
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
			return string.Join(Environment.NewLine, Messages);
		}
	}
}
