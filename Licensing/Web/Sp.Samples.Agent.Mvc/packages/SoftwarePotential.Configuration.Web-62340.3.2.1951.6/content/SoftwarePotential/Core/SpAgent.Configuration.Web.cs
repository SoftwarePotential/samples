// NB This file is auto-generated via the SoftwarePotential.Configuration.Web-XXYYY NuGet package.
// For more details see the README at http://docs.softwarepotential.com/Configuration.Web-README.html
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;
using System.IO;
using Sp.Agent.Configuration;

namespace Sp.Agent
{
	/// <summary>
	/// This portion of the partial class customizes the Storage aspect of the 
	/// Agent Context Configuration to have license data be maintained within a 
	/// single Directory that is managed (i.e., created and permissioned such that 
	/// all running instances can write or update licenses) completely outside 
	/// the application.
	/// </summary>
	/// <remarks>
	/// The <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> 
	/// partial methods must be implemented to provide the specific path to be used.
	/// </remarks>
	static partial class SpAgent
	{
		/// <summary>
		/// Uses the <c>Sp.Agent.Local</c> extension's <c>WithExternallyInitializedStore()</c>
		/// facility to supply a storage root path which is externally provisioned.
		/// </summary>
		/// <remarks>
		/// <para>Slots into the extension point exposed by the SoftwarePotential.Configuration 
		/// package in its SpAgent.Configuration.cs file.</para>
		/// <para>It should not be necessary to modify this code - the bodies of 
		/// <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> 
		/// partial methods can be adjusted as necessary.</para>
		/// </remarks>
		/// <exception cref="Sp.Agent.Storage.StorageInaccessibleException">Thrown at runtime should the supplied path (composed from the base and relative components) not be present and accessible when access to licensing storage is required.</exception>
		static partial void ConfigureStorage( Action<Func<IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase>> configure )
		{
			var externallyInitializedStorageArea = ConfiguredExternallyManagedRootPath();
			configure( agent => agent
				.WithExternallyInitializedStore( externallyInitializedStorageArea ) );
		}
	}
}