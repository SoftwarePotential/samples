// NB This file is auto-generated via the SoftwarePotential.Configuration.Distributor-<ShortCode> NuGet package.
// 
// TODO: IF YOU MODIFY THIS FILE, CONSIDER MOVING ANY MODIFIED METHODS (AND/OR 
// RENAMING THIS FILE) SO NUGET PACKAGE UPDATES CANNOT RESULT IN YOU INADVERTENTLY 
// UNDOING CHANGES YOU HAVE MADE

using Sp.Agent.Distributor.AppSettingsHelpers;
using Sp.Agent.Configuration; // Used by some #ifdef'd sections of this file
using System;
using System.Diagnostics;

namespace Sp.Agent
{

#if !SP_AGENT_NO_LOCAL_STORAGE && !SP_AGENT_DISTRIBUTOR_DISABLE_WRITABLE_CONFIGURATION_FILE
    /// <summary>
    /// This portion of the partial class uses the Storage configured via the
    /// SoftwarePotential.Configuration.(Multi|Single)User Package's ConfigureStorage()
    /// implementation to maintain a setting within an area that the Application 
    /// will be able to write to regardless of which user is active.
    /// </summary>
    /// <remarks>
    /// see https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README
    /// or https://support.softwarepotential.com/hc/en-us/articles/115001380105-Multi-User-Configuration-README
    /// for further information about Sp.Agent's storage mechanisms
    /// </remarks>
    static partial class SpAgent
    {
        static partial void ConfigureStaticDistributorDiscovery( Action<Func<Uri>> configure )
        {
            // Set the Discovery callback to yield the value we're maintaining in our Config File
            configure( () =>
            {
                // TODO If you want the application to use Distributed licenses, something will need to set DistributorBaseUri.
                // OPTION 1: Add a ProcessArgsIncludingDistributor() call to your Main()
                // OPTION 2: Add an option to configure DistributorBaseUri to an options Dialog elsewhere in your application 
                // -- see https://support.softwarepotential.com/hc/en-us/articles/115001382105-Distributor-Client-Configuration-README for further details
                Debug.WriteLineIf( Configuration.DistributorBaseUri == null,
                    @"No Distributor BaseUri is currently Configured. For a Distributor to be used, something needs to set SpAgent.Configuration.DistributorBaseUri." );

                return Configuration.DistributorBaseUri;
            } );

            // NB we are reliant on Application Installation having been completed to be able to touch the configuration file.
            //   This code can get triggered as part of installation flow when using the MultiUser configuration (or anything else 
            //   that uses the SpAgent class), so we add a Lazy layer to ensure the initialization doesn't happen too early.
            var lazyRuntimeConfigFile = new Lazy<SpAgentWritableDistributorAppSettings>( () =>
                WritableConfigurationFile.OpenOrCreateEmpty( // Create an empty config file when the application is first run [for a given user if in SingleUser mode]
                    SpAgent.Product.GetConfigurationFolder( "Sp.Agent.Distributor.Configuration" ) ) ); // Store alongside Licenses

            Configuration.InitializeDistributorBaseUriCallbacks(
                value => lazyRuntimeConfigFile.Value.UpdateBaseUri( value ),
                () => lazyRuntimeConfigFile.Value.ReadBaseUriOrDefault() );
        }

        // Remove this ifdef wrapper if you wish to configure named users and replace the "user@domain.com" with the appropriate user name calculation.
#if USING_NAMED_USERS
        /// <summary>
        /// <para>Configure the calculation of a named user e.g. your application's logged in user</para>
        /// <para>This configuration is only necessary when utilizing NamedUser Licenses.</para>
        /// <para>In order to consume a named user license the configured named user must match a user defined within the distributor server </para>
        /// <para>see https://support.softwarepotential.com/hc/en-us/articles/115001382105-Distributor-Client-Configuration-README for further details on Named User licensing</para>
        /// </summary>
        /// <param name="configure"></param>
        static partial void ConfigureNamedUserDiscovery( Action<Func<string>> configure )
        {
            configure( () => { return "user@domain.com"; } );
        }
#endif

        /// <summary>
        /// <para>Provides access to APIs relevant to the configuration of Software Potential integration.</para>
        /// <para>Typically should not be used outside of your application's configuration code.</para>
        /// </summary>
        /// <remarks>This section manages Distributor-specific configuration information.</remarks>
        static partial class Configuration
        {
            static WriteThroughCachedValue<Uri> _configuredBaseUri;

            /// <summary>
            /// Gets or sets the Base Uri for the Distributor from which Resources are to be obtained.
            /// </summary>
            public static Uri DistributorBaseUri
            {
                get { return _configuredBaseUri.Value; }
                set { _configuredBaseUri.Value = value; }
            }

            internal static void InitializeDistributorBaseUriCallbacks( Action<Uri> update, Func<Uri> retrieve )
            {
                _configuredBaseUri = new WriteThroughCachedValue<Uri>( update, retrieve );
            }
        }

        /// <summary>
        /// <para>Provides standardized processing of Licensing-related command-line parameters.</para>
        /// <para>To be invoked from your application's entry point.</para>
        /// </summary>
        /// <remarks>This portion offers facilities relevant to Distributor-aware applications.</remarks>
        static partial class CommandLineProcessing
        {
            public static bool ProcessDistributorArgs( string[] args )
            {
                // Check if the invoker is trying to configure the Distributor Base Uri
                var distributorBaseUri = CommandLineParsing.ArgumentOrDefault( "distributor", args );
                if ( distributorBaseUri != null )
                {
                    ExecuteCommandLineAction(
                        "Setting Distributor Base Uri to: " + distributorBaseUri,
                        () => SpAgent.Configuration.DistributorBaseUri = new Uri( distributorBaseUri ) );
                    return true;
                }

                // If the invoker wants to read the config
                if ( CommandLineParsing.HasSwitch( "distributor", args ) )
                {
                    ReportInstallationAction( "Configured Distributor Base Uri: " + SpAgent.Configuration.DistributorBaseUri );
                    return true;
                }

                // If the invoker wants to know where the config directory is held
                if ( CommandLineParsing.HasSwitch( "distributorConfigDir", args ) )
                {
                    ReportInstallationAction( "Distributor Config Folder: " + SpAgent.Product.GetConfigurationFolder( "Sp.Agent.Distributor.Configuration" ) );
                    return true;
                }

                return false;
            }
        }
    }
#elif SP_AGENT_DISTRIBUTOR_READONLY_CONFIG_IN_APP_CONFIG
	/// <summary>
	/// This portion of the partial class stubs out the Storage aspect of 
	/// the Agent Context Configuration configures the Distributor Discovery to be completely
	/// driven from the application's Configuration file (app.config/web.config/App.exe.config etc.)
	/// </summary>
	/// <remarks>
	/// When using this approach, it is implicit that the installation process of 
	/// your application needs to completely manage the presence/absence of a 
	/// Distributor Base Uri setting in the app.config (which you won't be able to 
	/// update under application control).
	/// This approach combines well with SP_AGENT_NO_LOCAL_STORAGE
	/// </remarks>
	static partial class SpAgent
	{
		// Obtain the Distributor base Uri from the app.config :-
		//	<configuration>
		//		<appSettings>
		//			<add key="Sp.Agent.Distributor.BaseUri" value=""/>
		//		</appSettings>
		//	</configuration>
		static partial void ConfigureStaticDistributorDiscovery( Action<Func<Uri>> configure )
		{
			configure( () =>
			{
				var result = SpAgentDistributorConfiguration.FromAppConfig().ReadBaseUriOrDefault();
				// TODO If you want the application to use Distributed licenses, your installer will need to ensure a value is present in the app.config
				// -- see https://support.softwarepotential.com/hc/en-us/articles/115001382105-Distributor-Client-Configuration-README for further details
				Debug.WriteLineIf( result == null, @"No Distributor BaseUri is currently Configured. For a Distributor to be used, the app.config needs an appSetting named Sp.Agent.Distributor.BaseUri" );
				return result;
			} );
		}
	}
#elif SP_AGENT_DISTRIBUTOR_CONFIG_EXTERNALLY_MANAGED
	/// <summary>
	/// This portion of the partial class shows a skeleton implementation that 
	/// you can complete to have the Distributor Base Uri be retrieved from 
	/// elsewhere in your Application.
	/// </summary>
	/// <remarks>
	/// This approach makes sense if you already have an existing settings Storage 
	/// system and/or Options Dialog in your application and hence have no need 
	/// for (or benefit from) the Distributor Base Uri being maintained elsewhere.
	/// </remarks>
	static partial class SpAgent
	{
		static partial void ConfigureStaticDistributorDiscovery( Action<Func<Uri>> configure )
		{
			configure( () =>
			{
				var result = new Uri("TODO"); // TODO impl, e.g. MyApplicationSettings.DistributorBaseUri;
					// TODO If you want the application to use Distributed licenses, the above Application-provided setting will need to return non-null
					// -- see https://support.softwarepotential.com/hc/en-us/articles/115001382105-Distributor-Client-Configuration-README for further details
				Debug.WriteLineIf( result == null, @"No Distributor BaseUri is currently Configured. For a Distributor to be used, your Externally Managed Settings will need to yield a non-null BaseUri" );
				return result;
			} );
		}
	}
#endif

#if SP_AGENT_DISTRIBUTOR_PROMPT_FOR_ENDPOINT_EXAMPLE
	/// <summary>
	/// This portion of the partial class shows a skeleton implementation that 
	/// illustrates how one might have an Options Dialog be triggered to allow 
	/// selection of a Distributor Endpoint under Application Control.
	/// 
	/// The only essential bit is that the lambda passed to configure() returns either:
	/// - a Valid Uri if a Distributor is to be used 
	/// - null if the user has opted not connect to a Distributor
	/// </summary>
	static partial class SpAgent
	{
		static partial void ConfigureStaticDistributorDiscovery( Action<Func<Uri>> configure )
		{
			// Cache the selected endpoint here so multiple calls to the callback can save the value
			var selectedEndpoint = default( Uri );

			// TODO load saved value from config file or similar

			configure( () =>
			{
				// If user has selected one from dialog, or we got one from the 
				// config, we return it directly rather than re-prompting every 
				// time the Distributor Service needs to be contacted for any reason
				if ( selectedEndpoint != null )
					return selectedEndpoint;

				var ok = RunDialog( out selectedEndpoint );
				if ( !ok )
				{
					System.Diagnostics.Trace.WriteLine( "User Cancelled Distributor Service Endpoint selection" );
					return null; // a NotLicensedException will be raised by the default behavior
				}

				// TODO save selectedEndpoint into storage somewhere so next app startup can pick up the value

				return selectedEndpoint;
			} );
		}

		static bool RunDialog( out Uri selectedEndpoint )
		{
			do
			{
				var dialogResult;

				// TODO prompt user, put result into dialogResult and output into selectedEndpoint

				if ( dialogResult == DialogResult.Cancel )
					return false;

				// Validate the selected Endpoint to give immediate feedback to the user as to 
				// whether the nominated endpoint is a Valid Distributor
			} while ( !SpAgent.Distributors.CanConnect( selectedEndpoint ) );
			return true;
		}
	}
#endif

#if SP_AGENT_NO_LOCAL_STORAGE
	/// <summary>
	/// This portion of the partial class stubs out the Storage aspect of 
	/// the Agent Context Configuration. This is only appropriate if you are not using a 
	/// SoftwarePotential.Configuration.(Multi|Single)User Package.
	/// </summary>
	static partial class SpAgent
	{
		/// <summary>
		/// Stub out any storage for licenses or config files on the local machine;
		/// assume all state will be maintained on a continually available Distributor.
		/// </summary> 
		/// <remarks>
		/// NB using this precludes using the following facilities:
		/// - using WritableApplicationConfigFile
		/// - Checking out Licenses
		/// - Using locally installed Licenses
		/// </remarks>
		static partial void ConfigureStorage( Action<Func<IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase>> configure )
		{
			configure( agent => agent
				.DisableStorage() );
		}

		//static partial void ConfigureStaticDistributorDiscovery( Action<Func<Uri>> configure )
		//{
		//	// TODO implement derivation of the Distributor Base Uri. See one of the following sections for details:
		//	//	SP_AGENT_DISTRIBUTOR_READONLY_CONFIG_IN_APP_CONFIG
		//	//	SP_AGENT_DISTRIBUTOR_CONFIG_EXTERNALLY_MANAGED
		//}
	}
#endif
}
