// NB This file is auto-added via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
//
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;
using System.Diagnostics;
using System.Linq;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		/// <summary>
		/// <para>Provides standardized processing of Licensing-related command-line parameters.</para>
		/// <para>To be invoked from your application's entry point.</para>
		/// </summary>
		public static partial class CommandLineProcessing
		{
			public static bool ProcessInstallationArgs( string[] args )
			{
				var handledSomething = false;

				// Check if the installer is trying to get us to initialize the store
				// Only strictly needed for MultiUser scenario
				if ( CommandLineParsing.HasSwitch( "initialize", args ) )
				{
					ExecuteCommandLineAction(
						"Initializing Shared License Storage...",
						SpAgent.Configuration.StoresInitialization.Initialize );
					handledSomething = true;
				}

				// Verify the assumption that the installer has done its work by having the Agent Verify each of its registered stores
				// NB do not remove this line as it is responsible for ensuring the correct timely Configuration of the Licensing System.
				ReportInstallationActionIfDebug( "Verifying Storage..." );
				SpAgent.Configuration.VerifyStoresInitialized();
				ReportInstallationActionIfDebug( "... verified." );

				// Check if the installer is trying to get us to activate a license
				var activationKey = CommandLineParsing.ArgumentOrDefault( "activate", args );
				if ( activationKey != null )
				{
					ExecuteCommandLineAction(
						"Activating License: " + activationKey,
						() => SpAgent.Product.Activation.OnlineActivateAsync( activationKey ).Wait() );
					handledSomething = true;
				}

				return handledSomething;
			}

			public static bool ProcessUninitializationArgs( string[] args )
			{
				if ( CommandLineParsing.HasSwitch( "uninitialize", args ) )
				{
					ExecuteCommandLineAction(
						"Removing Shared License Storage...",
						SpAgent.Configuration.StoresInitialization.Uninitialize );
					return true;
				}

				return false;
			}

			static void ExecuteCommandLineAction( string description, Action execute )
			{
				ReportInstallationAction( description + "..." );
				execute();
				ReportInstallationAction( "... succeeded." );
			}

			[Conditional( "DEBUG" )]
			static void ReportInstallationActionIfDebug( string action )
			{
				ReportInstallationAction( action );
			}

			static partial void ReportInstallationAction( string action );

			internal static class CommandLineParsing
			{
				public static bool HasSwitch( string name, string[] args )
				{
					return args.Any( x => String.Equals( "-" + name, x, StringComparison.OrdinalIgnoreCase ) );
				}

				public static string ArgumentOrDefault( string name, string[] args )
				{
					return args
						.Select( x =>
						{
							var prefix = "-" + name + ":";
							if ( !x.StartsWith( prefix, StringComparison.OrdinalIgnoreCase ) )
								return null;
							return x.Substring( prefix.Length );
						} )
						.Where( x => x != null )
						.FirstOrDefault();
				}
			}
		}
	}
}