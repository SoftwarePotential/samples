using Sp.Agent.Configuration;
using Sp.Agent.Distributor;

namespace Sp.Agent
{
	partial class SpAgent
	{
		// NB this extension method is only needed as this is a standalone diagnostic application that does not rely on the SoftwarePotential.Configuration.Distributor package
		// If you are using the SoftwarePotential.Configuration.Distributor package, this method should be removed
		public static IDistributorsContext Distributors
		{
			get
			{
				// This code depends on the SoftwarePotential.Configuration-xxxxx package to compile
				return SpAgent.Configuration.AgentContext.CreateDistributorsContext();
			}
		}
	}
}
