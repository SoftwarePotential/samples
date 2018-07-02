// NB This file is auto-added via the SoftwarePotential.Configuration.Local.SingleUser-XXYYY NuGet package.
// For more details see the README at https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using Sp.Agent.Configuration;
using System;
using System.IO;

namespace Sp.Agent
{
	/// <summary>
	/// This portion of the partial class customizes the Storage aspect of the 
	/// Agent Context Configuration to:
	/// - have license data be maintained within the Windows User Profile Application Data area 
	/// - have a subdirectory structure within that to segregate such files per Vendor and Product 
	/// [within the User Profile, without any installation-time initialization requirements].
	/// </summary>
	/// <remarks>
	/// The <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> 
	/// partial methods must be implemented to provide the specific path to be used.
	/// </remarks>
	static partial class SpAgent
	{
		/// <summary>
		/// <para>Uses <c>WithSingleUserStore</c> from the Sp.Agent.Local extension 
		/// to configure a base path (which is assumed to exist) together with a 
		/// Vendor/Product specific relative path.</para>
		/// <para>see <c>WithSingleUserStore</c> for further information.</para>
		/// </summary>
		/// <remarks>
		/// <para>Slots into the extension point exposed by the SoftwarePotential.Configuration 
		/// package in its SpAgent.Configuration.cs file.</para>
		/// <para>It should not be necessary to modify this code - the bodies of 
		/// <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> 
		/// partial methods can be adjusted as necessary.</para>
		/// </remarks>
		/// <exception cref="Sp.Agent.Storage.StorageInaccessibleException">Thrown at runtime should the supplied <c>basePath</c> not be present and accessible when required.</exception>
		static partial void ConfigureLocalBoundStorage( Action<Func<IAgentCommenceConfigurationPhase, IAgentHardwareBoundStorageConfigurationPhase>> configure )
		{
			var storageArea = ConfiguredBaseAndRelativePathElements();
			var rootPath = storageArea.Item1;
			var relativePath = storageArea.Item2;
			configure( agent => agent
				.WithSingleUserStore( rootPath, relativePath ) );
		}
	}
}