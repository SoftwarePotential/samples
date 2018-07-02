// NB This file is auto-added via the SoftwarePotential.Configuration.Distributor-<ShortCode> NuGet package.
//
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using Sp.Agent.Configuration;
using Sp.Agent.Distributor;
using Sp.Agent.Storage.Internal;
using System;
using System.Globalization;
using System.Linq;

namespace Sp.Agent
{
    /// <summary>
    /// This portion of the partial class plugs into the ConfigureDistributor extension 
    /// point of the SoftwarePotential.Configuration Package and in turn provides a 
    /// (mandatory) ConfigureStaticDistributorDiscovery extension point.
    /// </summary>
    static partial class SpAgent
    {
        /// <summary>
        /// <para>Partial method enabling specification of an appropriate Distributor Discovery Algorithm via a partnering partial class.</para>
        /// <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.Distributor package (or dependent).</para>
        /// </summary>
        /// <param name="configure">
        /// <para>delegate that accepts the <c>WithDiscovery</c> Configuration segment of a <c>IAgentDistributorsConfigurationPhase.WithDistributor()</c> Fluent Configuration sequence.</para>
        /// <para><example>See the code emitted by <c>SoftwarePotential.Configuration.Distributor</c>.</example></para>
        /// </param>
        static partial void ConfigureStaticDistributorDiscovery( Action<Func<Uri>> configure );

        /// <summary>
        /// <para>Partial method enabling specification of an appropriate Distributor Discovery Algorithm via a partnering partial class.</para>
        /// <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.Distributor package (or dependent).</para>
        /// </summary>
        /// <param name="configure">
        /// <para>delegate that accepts the <c>WithDiscovery</c> Configuration segment of a <c>IAgentDistributorsConfigurationPhase.WithDistributor()</c> Fluent Configuration sequence.</para>
        /// <para><example>See the code emitted by <c>SoftwarePotential.Configuration.Distributor</c>.</example></para>
        /// </param>
        static partial void ConfigureNamedUserDiscovery( Action<Func<string>> configure );

        /// <summary>
        /// Implements the extension point, delegating to <c>ConfigureStaticDistributorDiscovery</c>.
        /// </summary>
        /// <param name="configure"></param>
        static partial void ConfigureDistributor( Action<Func<IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase>> configure )
        {
            var callback = default( Func<Uri> );
            ConfigureStaticDistributorDiscovery( discover => callback = discover );
            if ( callback == null )
                throw CreateMissingDistributorPartialMethodException( "No Distributor Discovery Algorithm specified", "ConfigureStaticDistributorDiscovery" );

            var namedUserCallback = default( Func<string> );
            ConfigureNamedUserDiscovery( resolveNamedUser => namedUserCallback = resolveNamedUser );
            if ( namedUserCallback == null )

                configure( agent => agent.WithDistributor( distributor => distributor
                    .WithDiscovery( callback )                    
                    .CompleteWithDefaults() ) );
            else
                configure( agent => agent.WithDistributor( distributor => distributor
                    .WithDiscovery( callback )
                    .WithNamedUser( namedUserCallback )
                    .CompleteWithDefaults() ) );
        }

        static Exception CreateMissingDistributorPartialMethodException( string preamble, string methodName )
        {
            return new InvalidOperationException( String.Format( CultureInfo.InvariantCulture,
                preamble + @" via {0}(); Distributor Component cannot be successfully configured.
Please ensure there is a valid implementation of {0}() that invokes its callback correctly in place.
See the documentation for the {0}() partial method or further information.", methodName ) );
        }

        // Want to have the initial spin up of the Context Lazy as it has a non-zero cost (the caching effect of Lazy vs a Func is not as critical)
        static readonly Lazy<IDistributorsContext> _distributorsContext = new Lazy<IDistributorsContext>( () => Configuration.AgentContext.CreateDistributorsContext() );

        /// <summary>
        /// <para>Provides access to Distributor connectivity and service health diagnostics facilities.</para>
        /// <para>To adjust configuration, see <see cref="Configuration"/>. For access to configured resources, use <see cref="Distributed"/>.</para>
        /// </summary>
        public static IDistributorsContext Distributors
        {
            get { return _distributorsContext.Value; }
        }

        // Want to have the initial spin up of the Context Lazy as it has a non-zero cost (the caching effect of Lazy vs a Func is not as critical)
        static readonly Lazy<IDistributedContext> _distributedContext = new Lazy<IDistributedContext>( () => SpAgent.Product.CreateDistributedContext() );

        /// <summary>
        /// <para>Provides APIs necessary for building a Distributor-aware application using the NuGet <see cref="Product"/> package you have installed.</para>
        /// <para>To adjust configuration, see <see cref="Configuration"/>. To verify service health or connectivity use <see cref="Distributors"/>.</para>
        /// </summary>
        public static IDistributedContext Distributed
        {
            get { return _distributedContext.Value; }
        }
    }

    /// <summary>
    /// For internal use only.
    /// </summary>
    /// <remarks>
    /// Subject to unlimited change without notice even in minor version changes.
    /// </remarks>
    static class ConfigurationFolderExtensions
    {
        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <remarks>
        /// Subject to unlimited change without notice even in minor version changes.
        /// </remarks>
        public static string GetConfigurationFolder( this IProductContext that, string configurationName )
        {
            return that.Stores.Configuration().All().Single().GetFolder( configurationName );
        }
    }
}