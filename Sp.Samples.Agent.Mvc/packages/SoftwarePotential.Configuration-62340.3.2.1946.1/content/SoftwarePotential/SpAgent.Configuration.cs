// NB This file is auto-generated via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using Sp.Agent;
using Sp.Agent.Configuration;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		static internal partial class Configuration
		{
			/// <summary>
			/// Should be called once from Main/your entrypoint before running any Licensed 
			/// or Protected code or calling Licensing APIs (e.g. Activation)
			/// <para>Ideally the work involved in this should take place on a background thread 
			/// in parallel with other initialization activities.</para>
			/// <para>NB depending on the store type involved, it may be necessary to call <c>SpAgent.Configuration.InitializeSharedLicenseStorage()</c> or similar as part of one's installation sequence.</para>
			/// </summary>
			/// <remarks>NB Calling the method has the important side effect of Initializing the configurations (see static constructor below)</remarks>
			public static void VerifyStoresInitialized()
			{
				StoresInitialization.Verify();
			}

			/// <remarks>NB Calling the method has the critical side effect of Initializing the Agent Context and Product Context configurations [via the static constructor of SpAgent]</remarks>
			internal static IInitializeStores StoresInitialization
			{
				get { return SpAgent.Product.Stores.Initialization(); }
			}
		}

		/// <summary>
		/// The Sp.Agent Configuration calls should execute once and only once, which is 
		/// exactly what a static constructor does
		/// </summary>
		/// <remarks>
		/// Note that VerifyStoresInitialized() (and SpAgentInstallation.InitializeSharedLicenseStorage)
		/// is housed in this class to benefit from the implicit thread safe call to this on first hit
		/// In other words, be careful if moving stuff out of here
		/// </remarks>
		static SpAgent()
		{
			// If this triggers a compiler error, it's likely because there is not a SoftwarePotential.Configuration package reference in place
			// Typically, one would Install-Package SoftwarePotential.Configuration to remediate this
			ConfigureAgent( SpAgent.AgentContext );

			// If this triggers a compiler error, it's likely because there is not a SoftwarePotential\SpAgent.Product.cs file in place 
			// Typically, one would Install-Package SoftwarePotential.Licensing-<ProductName_ProductVersion> to remediate this
			ConfigureProduct( SpAgent.Product );
		}

		/// <summary>
		/// <para>Placeholder for method generated in SoftwarePotential\SpAgent&lt;Configuration Extension Name&gt;.cs.</para>
		/// <para>Alternately, one may choose to provide your own implementation.</para>
		/// </summary>
		/// <param name="agentContext">The context, emanating from your Sp.Agent-&lt;ShortCode&gt; package.</param>
		static partial void ConfigureAgent( IAgentContext agentContext );

		/// <summary>
		/// Placeholder for method typically emitted into SoftwarePotential\Customizations.cs by the SoftwarePotential.Configuration package
		/// </summary>
		/// <param name="productContext">The context, emanating from your SoftwarePotential.Licensing-&lt;Product_Version&gt; package.</param>
		static partial void ConfigureProduct( IProductContext productContext );
	}
}