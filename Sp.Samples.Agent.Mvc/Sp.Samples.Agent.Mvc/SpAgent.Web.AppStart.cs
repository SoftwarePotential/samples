using Sp.Agent;
using Sp.Samples.Agent.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod( typeof( AppStart ), "Start" )]

namespace Sp.Samples.Agent.Mvc
{
	static class AppStart
	{
		static void Start()
		{
			//Verify Sp Agent License Store is initialized and accessible
			SpAgent.Configuration.VerifyStoresInitialized();
		}
	}
}