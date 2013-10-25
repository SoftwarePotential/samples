// NB This file is auto-generated via the SoftwarePotential.Configuration.Web-XXYYY NuGet package.
// For more details see the README at http://docs.softwarepotential.com/Configuration.Web-README.html
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;
using System.IO;
using Sp.Agent.Configuration;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		// TODO doc
		static partial void ConfigureStorage( Action<Func<IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase>> configure )
		{
			var externallyInitializedStorageArea = ConfiguredExternallyManagedRootPath();
			configure( agent => agent
				.WithExternallyInitializedStore( externallyInitializedStorageArea ) );
		}
	}
}