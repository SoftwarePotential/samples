// NB This file is auto-generated via the SoftwarePotential.Licensing-Demo_10 NuGet package.
//
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

namespace Sp.Agent
{
	// NB Relies on a Reference to Sp.Agent.dll, which emanates from the Sp.Agent package (typically via the SoftwarePotential-<ShortCode> package)
	// if compilation fails here, it's typically because you're missing a reference Sp.Agent package 
	using Sp.Agent.Configuration;
	using System;

	static partial class SpAgent
	{
		static Lazy<IProductContext> _lazyProductContext = new Lazy<IProductContext>( GenerateProductContext );

		/// <summary>
		/// <para>Provides access to locally managed Licensing resources associated with the NuGet Product package you have installed.</para>
		/// </summary>
		public static IProductContext Product
		{
			get { return _lazyProductContext.Value; }
		}

		static IProductContext GenerateProductContext()
		{
			return TheAgentContext.CreateDemo_10ProductContext();
		}

		static IAgentContext TheAgentContext
		{
			get
			{
				// NB The 'SpAgent.Configuration.AgentContext' static property emanates from SoftwarePotential\SpAgent.Permutation.cs
				// if compilation fails here, it's typically because you've yet to add a NuGet Reference to a SoftwarePotential-<ShortCode> package
				// In most cases, this means you're missing a reference to one of the SoftwarePotential.Configuration.* packages (although if you're just doing licensing queries in a submodule of your system, just referencing the SoftwarePotential-<ShortCode> package is sufficient)
				return SpAgent.Configuration.AgentContext;
			}
		}
	}

	static partial class SpAgentDemo_10Extensions
	{
		public static IProductContext CreateDemo_10ProductContext( this IAgentContext that )
		{
			// NB The SpProduct static class emanates from SoftwarePotential\SpProduct.cs in a Sp.Product-Product_Version NuGet Package 
			// if compilation fails here, it's likely to be due to a problem with the installation of the SoftwarePotential.Licensing-Demo_10 NuGet Package (i.e., this one) which typically pulls this in as a package dependency
			return that.ProductContextFor( SpProduct.Name, SpProduct.Version );
		}
	}
};