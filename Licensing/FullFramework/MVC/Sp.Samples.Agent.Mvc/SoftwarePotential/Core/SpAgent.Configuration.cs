// NB This file is auto-added via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
//
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using Sp.Agent;
using Sp.Agent.Configuration;
using Sp.Agent.Configuration.Internal; // For bool IsConfigured( this IAgentContext that)  which is normally not recommended.
using System;
using System.ComponentModel;
using System.Globalization;

namespace Sp.Agent
{
	/// <summary>
	/// This portion of the partial class implements core functionality to check the correct Initialization of Stores in 
	/// order to verify that the environmental prerequisites of the Software Potential Components are present and correct.
	/// </summary>
	/// <remarks>
	/// One should always add a call to <c>SpAgent.Configuration.VerifyStoresInitialized()</c> to your Application 
	/// environment's Entrypoint.
	/// </remarks>
	static partial class SpAgent
	{
		/// <summary>
		/// <para>Provides access to APIs relevant to the configuration of Software Potential integration.</para>
		/// <para>Typically should not be used outside of your application's configuration code.</para>
		/// </summary>
		static partial class Configuration
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

			[EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Not applicable for normal API usage as SpAgent is effectively a Singleton" )]
			internal new static bool Equals( object objA, object objB )
			{
				throw new NotImplementedException();
			}

			[EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Not applicable for normal API usage as SpAgent is effectively a Singleton" )]
			internal new static bool ReferenceEquals( object objA, object objB )
			{
				throw new NotImplementedException();
			}
		}
	}

	/// <summary>
	/// This portion of the partial class implements a static constructor that will ensure the correct initialization of 
	/// the Agent and Product Contexts [in the correct sequence] implicitly as a side effect of calling methods such as 
	/// <c>SpAgent.Configuration.VerifyStoresInitialized()</c>
	/// </summary>
	static partial class SpAgent
	{
		/// <summary>
		/// The Sp.Agent Configuration calls should execute once and only once; exactly what a static constructor does.
		/// </summary>
		/// <remarks>
		/// Note that VerifyStoresInitialized() (and SpAgentInstallation.InitializeSharedLicenseStorage)
		/// is housed in this class to benefit from the implicit thread safe call to this on first hit.
		/// In other words, be careful if moving stuff out of here.
		/// </remarks>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline" )]
		static SpAgent()
		{
			if ( !ShouldSkipConfiguration( SpAgent.Configuration.AgentContext ) )
			{
				// If this triggers a compiler error, it's likely because there is not a SoftwarePotential.Configuration package reference in place
				// Typically, one would Install-Package SoftwarePotential.Configuration (or a dependent package) to remediate this
				ConfigureAgent( SpAgent.Configuration.AgentContext );

				// If this triggers a compiler error, it's likely because there is not a SoftwarePotential\SpAgent.Product.cs file in place 
				// Typically, one would Install-Package SoftwarePotential.Licensing-<ProductName_ProductVersion> to remediate this
				ConfigureProduct( SpAgent.Product );
			}
		}

		/// <summary>
		/// Implements policy detailed in comments on ConfigureMultipleSpAgentsPerPermutation().
		/// </summary>
		/// <param name="agentContext"></param>
		/// <returns></returns>
		static bool ShouldSkipConfiguration( IAgentContext agentContext )
		{
			var sharedModeRequested = false; // By default we want to know if there are competing attempts to Configure the Agent
			ConfigureMultipleSpAgentsPerPermutation( value => sharedModeRequested = value );
			return sharedModeRequested && agentContext.IsConfigured();
		}

		/// <summary>
		/// <para>Partial method enabling one to request an 'already initialized by a co-operating Assembly' guard in a partnering partial.</para>
		/// <para>This allows a set of Assemblies with equivalent configurations to let the first one loaded manage the Configuration and others to assume that if configuration has already taken place that it is an equivalent setup.</para>
		/// <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
		/// callback will result in the default behavior:- each SpAgent class will attempt to Configure the Agent and an exception will result if more than  one does so per Permutation.</para>
		/// <para>NB this will inhibit the normal behavior of trapping inadvertent use of protected code prior to controlled initialization via <c>SpAgent</c> and should be applied with care.</para>
		/// </summary>
		/// <remarks>Often a cleaner solution is to assign separate Permutations to each Application/component in order to both provide isolation and avoid this issue.</remarks>
		/// <param name="configure">delegate that can be passed a flag indicating whether one wants to operate in 'first configuration wins' mode.</param>
		/// <example><code>static partial void ConfigureMultipleSpAgentsPerPermutation( Action&lt;bool> configure )
		/// {
		///     configure( true); // Allow first SpAgent to be touched to manage the configurations and others to assume that any preceding initialization is correct.
		/// }
		/// </code></example>
		static partial void ConfigureMultipleSpAgentsPerPermutation( Action<bool> configure );

		/// <summary>
		/// <para>Partial method enabling specification of an appropriate License Storage Policy by a partnering partial class.</para>
		/// <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.* package.</para>
		/// <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
		/// callback will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
		/// </summary>
		/// <param name="configure">
		/// <para>delegate that accepts the Store Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
		/// <para><example>See the code emitted by any of the following <c>SoftwarePotential.Configuration.</c> packages: <c>Local.*</c>, <c>Web</c> or <c>Distributor</c>.</example></para>
		/// </param>
		static partial void ConfigureStorage( Action<Func<IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase>> configure );

		/// <summary>
		/// <para>Partial method enabling specification of an appropriate Distributor Client Policy by a partnering partial class.</para>
		/// <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.Distributor package (or dependent).</para>
		/// <para>It is safe to omit any implementation of this method.</para>
		/// </summary>
		/// <param name="configure">
		/// <para>delegate that accepts the Distributor Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
		/// <para><example>See the code emitted by <c>SoftwarePotential.Configuration.Distributor</c>.</example></para>
		/// </param>
		/// <example>
		/// Default implementation: 
		/// <code>configure( distributor => distributor.DisableDistributor())</code>
		/// </example>
		static partial void ConfigureDistributor( Action<Func<IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase>> configure );

		/// <summary>
		/// <para>Partial method enabling specification of an appropriate Embedded Licenses Policy by a partnering partial class.</para>
		/// <para>Typically the default implementation is appropriate for normal usage as having an explicit <c>DisableEmbeddedLicenses()</c> affords only a minor performance benefit.</para>
		/// <para>It is safe to omit any implementation of this method.</para>
		/// </summary>
		/// <param name="configure">
		/// <para>delegate that accepts the Embedded Licenses Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
		/// <para><example><code>embeddedLicenses => embeddedLicenses.DisableEmbeddedLicenses()</code> </example></para>
		/// </param>
		/// <example>
		/// Default implementation: 
		/// <code>configure( embeddedLicenses => embeddedLicenses.WithEmbeddedLicensesAutoDetected())</code>
		/// </example>
		static partial void ConfigureEmbeddedLicenses( Action<Func<IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase>> configure );

		/// <summary>
		/// Placeholder for method typically emitted into SoftwarePotential\ProductCustomizations.cs by the SoftwarePotential.Configuration package
		/// </summary>
		/// <param name="productContext">The context, emanating from your SoftwarePotential.Licensing-&lt;Product_Version&gt; package.</param>
		static partial void ConfigureProduct( IProductContext productContext );

		/// <summary>
		/// Applies a <c>IAgentContext.Configure()</c> Fluent Configuration Expression gathered via the relevant helpers for each link in the chain.
		/// Each portion is gathered via associated <c>Configure</c>* <c>partial</c> methods, so it should not be necessary to adjust the implementation here.
		/// </summary>
		static void ConfigureAgent( IAgentContext agentContext )
		{
			agentContext.Configure( configure => configure
				.WithStorageConfiguration( FetchStorageConfigurationSequenceOrThrow() )
				.WithDistributorConfiguration( DistributorsConfigurationSequenceOrNullObject )
				.WithEmbeddedLicensesConfiguration( EmbeddedLicensesConfigurationSequenceOrNullObject ) );
		}

		/// <summary>
		/// Yields an <c>IAgentConfiguration.Configure()</c> Fluent Configuration Sequence Segment for the 
		/// Storage Configuration aspect (or throws if a correctly implemented <c>ConfigureStorage()</c> partial method is not in place).
		/// </summary>
		/// <exception cref="InvalidOperationException">Thrown if no <c>ConfigureStorage()</c> method (that correctly invokes its argument) is present.</exception>
		static Func<IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase> FetchStorageConfigurationSequenceOrThrow()
		{
			var configureStorage = default( Func<IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase> );
			ConfigureStorage( configure => configureStorage = configure );
			if ( configureStorage == null )
				CreateMissingPartialMethodException( "No Storage Configuration provided", "ConfigureStorage" );
			return configureStorage;
		}

		static Exception CreateMissingPartialMethodException( string preamble, string methodName )
		{
			return new InvalidOperationException( String.Format( CultureInfo.InvariantCulture,
				preamble + @" via {0}(); Licensing Storage cannot be successfully initialized.
Please ensure there is a valid implementation of {0}() that invokes its callback correctly in place.
See the documentation for the {0}() partial method or further information.", methodName ) );
		}

		/// <summary>
		/// Yields an <c>IAgentConfiguration.Configure()</c> Fluent Configuration Sequence Segment for the 
		/// Distributors Configuration aspect (or provides a default that disables Distributor access if no correctly 
		/// implemented <c>ConfigureDistributor()</c> partial method is in place).
		/// </summary>
		static Func<IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase> DistributorsConfigurationSequenceOrNullObject
		{
			get
			{
				var configureDistributors = default( Func<IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase> );
				ConfigureDistributor( configure => configureDistributors = configure );
				if ( configureDistributors == null )
					return distributor => distributor.DisableDistributor();
				return configureDistributors;
			}
		}

		/// <summary>
		/// Yields an <c>IAgentConfiguration.Configure()</c> Fluent Configuration Sequence Segment for the 
		/// Embedded Licenses Configuration aspect (or provides a default that auto-detects the presence of Embedded Licenses 
		/// if no correctly implemented <c>ConfigureEmbeddedLicenses()</c> partial method is in place).
		/// </summary>
		static Func<IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase> EmbeddedLicensesConfigurationSequenceOrNullObject
		{
			get
			{
				var configureEmbedded = default( Func<IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase> );
				ConfigureEmbeddedLicenses( configure => configureEmbedded = configure );
				if ( configureEmbedded == null )
					return embeddedLicenses => embeddedLicenses.WithEmbeddedLicensesAutoDetected();
				return configureEmbedded;
			}
		}
	}

	/// <summary>
	/// Offers a set of extension methods allowing one to decompose an <c>IAgentContext.Configure()</c> Fluent Configuration 
	/// Expression Sequence into individual elements yet retain the familiar fluent expression nature.
	/// </summary>
	/// <remarks>Equivalent to F#'s built-in |> operator.</remarks>
	static class AgentConfigurationExtensions
	{
		/// <summary>
		/// Applies a delegate in place of having a statically bound expression in a <c>IAgentContext.Configure()</c> 
		/// Fluent Configuration Expression Sequence for the Storage Configuration aspect of the Agent Configuration.
		/// </summary>
		public static IAgentDistributorsConfigurationPhase WithStorageConfiguration( this IAgentCommenceConfigurationPhase that, Func<IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase> configureStorage )
		{
			return configureStorage( that );
		}

		/// <summary>
		/// Applies a delegate in place of having a statically bound expression in a <c>IAgentContext.Configure()</c> 
		/// Fluent Configuration Expression Sequence for the Distributor Configuration aspect of the Agent Configuration.
		/// </summary>
		public static IAgentEmbeddedLicensesConfigurationPhase WithDistributorConfiguration( this IAgentDistributorsConfigurationPhase that, Func<IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase> configureDistributor )
		{
			return configureDistributor( that );
		}

		/// <summary>
		/// Applies a delegate in place of having a statically bound expression in a <c>IAgentContext.Configure()</c> 
		/// Fluent Configuration Expression Sequence for the Embedded Licenses Configuration aspect of the Agent Configuration.
		/// </summary>
		public static IAgentCompletedConfigurationPhase WithEmbeddedLicensesConfiguration( this IAgentEmbeddedLicensesConfigurationPhase that, Func<IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase> configureEmbeddedLicenses )
		{
			return configureEmbeddedLicenses( that );
		}
	}
}