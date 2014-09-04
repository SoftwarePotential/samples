' NB This file is auto-generated via the SoftwarePotential.Configuration.Local.SingleUser-XXYYY NuGet package.
' For more details see the README at http://docs.softwarepotential.com/Configuration.Local.SingleUser-README.html
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

imports Sp.Agent.Configuration
imports System
imports System.IO

''' <summary>
''' This portion of the partial class customizes the Storage aspect of the 
''' Agent Context Configuration to:
''' - have license data be maintained within the Windows User Profile Application Data area 
''' - have a subdirectory structure within that to segregate such files per Vendor and Product 
''' [within the User Profile, without any installation-time initialization requirements].
''' </summary>
''' <remarks>
''' The <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> 
''' partial methods must be implemented to provide the specific path to be used.
''' </remarks>
Partial Class SpAgent

	''' <summary>
	''' <para>Uses <c>WithSingleUserStore</c> from the Sp.Agent.Local extension 
	''' to configure a base path (which is assumed to exist) together with a 
	''' Vendor/Product specific relative path.</para>
	''' <para>see <c>WithSingleUserStore</c> for further information.</para>
	''' </summary>
	''' <remarks>
	''' <para>Slots into the extension point exposed by the SoftwarePotential.Configuration 
	''' package in its SpAgent.Configuration.cs file.</para>
	''' <para>It should not be necessary to modify this code - the bodies of 
	''' <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> 
	''' partial methods can be adjusted as necessary.</para>
	''' </remarks>
	''' <exception cref="Sp.Agent.Storage.StorageInaccessibleException">Thrown at runtime should the supplied <c>basePath</c> not be present and accessible when required.</exception>
	Private Shared Sub ConfigureStorage(configure As Action(Of Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)))
		Dim storageArea = ConfiguredBaseAndRelativePathElements()
		Dim rootPath = storageArea.Item1
		Dim relativePath = storageArea.Item2
			configure( Function(agent)
				Return agent.WithSingleUserStore( rootPath, relativePath ) 
			 End Function)
	End Sub
End Class