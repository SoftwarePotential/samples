// NB This file is auto-generated via the SoftwarePotential-62340 NuGet package.
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

namespace Sp.Agent
{
	using System;
	using Sp.Agent.Configuration;

	static partial class SpAgent
	{
		static readonly Lazy<IAgentContext> _lazyAgentContext = new Lazy<IAgentContext>( GenerateAgentContext );

		static IAgentContext GenerateAgentContext()
		{
			return Sp.Agent.Configuration.AgentContext.For( Permutation62340.Identifiers.ShortCode );
		}

		public static IAgentContext AgentContext
		{
			get { return _lazyAgentContext.Value; }
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