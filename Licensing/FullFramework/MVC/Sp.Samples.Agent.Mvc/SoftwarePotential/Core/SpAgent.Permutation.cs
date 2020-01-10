// NB This file is auto-generated via the SoftwarePotential-62340 NuGet package.
//
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

namespace Sp.Agent
{
	using Sp.Agent.Configuration;
	using System;
	using System.ComponentModel;

    /// <summary>
    /// This portion of the partial class provides a PermutationId-neutral way of 
    /// being able to access SpAgent.AgentContext from anywhere within the Application.
    /// This is used by other code across the NuGet-based Configuration packages as necessary.
    /// </summary>
    static partial class SpAgent
	{
		/// <summary>
		/// <para>Provides access to APIs relevant to the configuration of Software Potential integration.</para>
		/// <para>Typically should not be used outside of your application's configuration code.</para>
		/// </summary>
		static internal partial class Configuration
		{
			static readonly Lazy<IAgentContext> _lazyAgentContext = new Lazy<IAgentContext>( GenerateAgentContext );

			static IAgentContext GenerateAgentContext()
			{
				var currentAssembly = typeof( SpAgent ).Assembly;
				// Normal search locations for DLLs includes: 1) AppDomain Base dir 2) beside Sp.Agent.dll -- that's sufficient for the bulk of common applications
				// Hence the following is only strictly necessary if a) permuted DLLs are maintained outside of the AppDomain base directory AND b) there are potentially >1 permutations in a given AppDomain - 
				var associatedDllProvidingAdditionalPermutedDllSearchLocation = currentAssembly.GlobalAssemblyCache ? null : currentAssembly;
				return Sp.Agent.Configuration.AgentContext.For( Permutation62340.Identifiers.ShortCode, associatedDllProvidingAdditionalPermutedDllSearchLocation );
			}

			/// <summary>
			/// Low-level access to the IAgentContext. In normal usage it should never be necessary to access this directly; each relevant configurable item is exposed as a Configure* partial method on <see cref="SpAgent"/>.
			/// </summary>
			public static IAgentContext AgentContext
			{
				get { return _lazyAgentContext.Value; }
			}
		}

		#region Backward compatibility shim
		// TOCONSIDER when we next version the licensing code snippets and can thus guarantee that all versions of SpAgent.Product.cs will be refreshed to go direct to SpAgent.Configuration.AgentContext (rather than using SpAgent.AgentContext as early versions did), this backcompat shim can be removed
		static IAgentContext AgentContext { get { return Configuration.AgentContext; } }
		#endregion
	}

	/// <summary>
	/// This portion of the partial offers as many hints as possible to users and tooling that <c>Equals</c> and <c>ReferenceEquals</c> have no part to play in normal usage of the APIs.
	/// </summary>
	static partial class SpAgent
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "objA" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "objB" ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Not applicable for normal API usage as SpAgent is effectively a Singleton" )]
		internal new static bool Equals( object objA, object objB )
		{
			throw new NotImplementedException();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "objA" ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "objB" ), EditorBrowsable( EditorBrowsableState.Never ), Obsolete( "Not applicable for normal API usage as SpAgent is effectively a Singleton" )]
		internal new static bool ReferenceEquals( object objA, object objB )
		{
			throw new NotImplementedException();
		}
	}

	namespace Permutation62340
	{
		/// <summary>
		/// Identifiers associated with the 'Permutation1' Software Potential Permutation
		/// </summary>
		static class Identifiers
		{
			/// <summary>
			/// 5 digit short code as used by <c>Sp.Agent.Configuration.AgentContext.For( string )</c>
			/// </summary>
			public const string ShortCode = @"62340";
		}
	}
}