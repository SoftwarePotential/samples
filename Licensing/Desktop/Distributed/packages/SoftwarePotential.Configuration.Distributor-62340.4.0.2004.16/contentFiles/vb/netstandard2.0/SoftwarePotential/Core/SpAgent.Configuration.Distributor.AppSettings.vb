' NB This file is auto-added via the SoftwarePotential.Configuration.Distributor-<ShortCode> NuGet package.
'
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports System
Imports System.Configuration

''' <summary>
''' Encapsulates opening (and/or creating and seeding) of a <c>Sp.Agent.Distributor.dll.config</c> 
''' file that will be maintained outside of an application's <c>app.config</c> file.
''' </summary>
Class WritableConfigurationFile
	Private Const RuntimeConfigFilename As String = "Sp.Agent.Distributor.dll.config"

	''' <summary>
	''' The name of the config file. As all programmatic access is to be gated 
	''' through this class, this should not need to be externally known.
	''' </summary>
	Public Shared Function OpenOrCreateEmpty(configDirectory As String) As SpAgentWritableDistributorAppSettings
		Return WritableConfigurationFile.OpenOrInitializeFromTemplateContent("<?xml version=""1.0"" encoding=""utf-8"" ?>" & Environment.NewLine & "<configuration/>", configDirectory)
	End Function

	Private Shared Function OpenOrInitializeFromTemplateContent(configFileTemplateContent As String, configDirectory As String) As SpAgentWritableDistributorAppSettings
		Return WritableConfigurationFile.OpenOrSeed(Sub(runtimeConfigFile As String)
			System.IO.File.WriteAllText(runtimeConfigFile, configFileTemplateContent, System.Text.Encoding.UTF8)
																End Sub, configDirectory)
	End Function

	Private Shared Function OpenOrSeed(seedFile As System.Action(Of String), configDirectory As String) As SpAgentWritableDistributorAppSettings
		Dim runtimeConfigFile As System.IO.FileInfo = New System.IO.FileInfo(System.IO.Path.Combine(configDirectory, "Sp.Agent.Distributor.dll.config"))
		If Not runtimeConfigFile.Exists Then
			seedFile(runtimeConfigFile.FullName)
		End If
		Return WritableConfigurationFile.Open(runtimeConfigFile)
	End Function

	''' <summary>
	''' Opens the file. Will fail if the file is not present.
	''' </summary>
	''' <param name="runtimeConfigFile">The folder within which the file is stored.</param>
	Public Shared Function Open(runtimeConfigFile As System.IO.FileInfo) As SpAgentWritableDistributorAppSettings
		Return SpAgentDistributorConfiguration.OpenStandaloneFile(runtimeConfigFile)
	End Function
End Class

Class SpAgentDistributorAppSettings
	Protected _configuration As System.Configuration.Configuration

	Protected Shared ReadOnly Property ConfigSettingName() As String
		Get
			Return "Sp.Agent.Distributor.BaseUri"
		End Get
	End Property

	Private ReadOnly Property Filepath() As String
		Get
			Return Me._configuration.FilePath
		End Get
	End Property

	Public Sub New(configuration As System.Configuration.Configuration)
		Me._configuration = configuration
	End Sub

	Public Function ReadBaseUriOrDefault() As Uri
		Dim setting As KeyValueConfigurationElement = Me._configuration.AppSettings.Settings(SpAgentDistributorAppSettings.ConfigSettingName)
		Dim result As Uri
		If setting Is Nothing Then
			result = Nothing
		Else
			Dim settingValue As String = setting.Value
			Dim uri As Uri = Nothing
			If Not uri.TryCreate(settingValue, UriKind.Absolute, uri) Then
				Throw New System.InvalidOperationException(String.Concat( "Malformed Uri in Configuration Setting. Has there been an external modification? (", SpAgentDistributorAppSettings.ConfigSettingName, " = ", settingValue, " in ", Me.Filepath, ")" ))
			End If
			result = uri
		End If
		Return result
	End Function
End Class

''' <summary>
''' Manages opening a Distributor Configuration file from either app.config or a standalone file.
''' </summary>
Module SpAgentDistributorConfiguration
	''' <summary>
	''' Accesses [in read/write mode] the BaseUri setting maintained the specified <paramref name="file" />.
	''' </summary>
	Public Function OpenStandaloneFile(file As System.IO.FileInfo) As SpAgentWritableDistributorAppSettings
		Return New SpAgentWritableDistributorAppSettings(ConfigurationManager.OpenMappedExeConfiguration(New ExeConfigurationFileMap() With {.ExeConfigFilename = file.FullName}, ConfigurationUserLevel.None))
	End Function

	''' <summary>
	''' Accesses [in read-only mode] the BaseUri setting maintained externally in the application's <c>app.config</c> file.
	''' </summary>
	Public Function FromAppConfig() As SpAgentDistributorAppSettings
		Return New SpAgentDistributorAppSettings(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None))
	End Function
End Module

''' <summary>
''' Manages access to an <c>configuration/appSetting/Sp.Agent.Distributor.BaseUri</c> entry in an Application Configuration file.
''' </summary>
''' <remarks>
''' This class handles the reading aspect only. For writing support, see <see cref="SpAgentWritableDistributorAppSettings"/>.
''' </remarks>
Class SpAgentWritableDistributorAppSettings
	Inherits SpAgentDistributorAppSettings

	Public Sub New(configuration As System.Configuration.Configuration)
		MyBase.New(configuration)
	End Sub

	Public Sub UpdateBaseUri(value As Uri)
		Me._configuration.AppSettings.Settings.Remove(SpAgentDistributorAppSettings.ConfigSettingName)

		If value IsNot Nothing Then
			Me._configuration.AppSettings.Settings.Add(SpAgentDistributorAppSettings.ConfigSettingName, value.ToString())
		End If
		
		Me._configuration.Save()
	End Sub
End Class

''' <summary>
''' Caches the (non-<c>null</c> value of writable property. All <c>set</c> operations are expected to be 
''' passed through the cache too in order to ensure the <c>get</c> will yield a consistent result.
''' </summary>
''' <typeparam name="T">The type of value to be cached.</typeparam>
Class WriteThroughCachedValue(Of T)
	Private _set As System.Action(Of T)

	Private _get As Func(Of T)

	Private _cachedValue As T

	Public Property Value() As T
		Get
			If Me._cachedValue Is Nothing Then
				Me._cachedValue = Me._get()
			End If
			Return Me._cachedValue
		End Get
		Set(value As T)
			Me._set(value)
			Me._cachedValue = value
		End Set
	End Property

	Public Sub New(update As System.Action(Of T), retrieve As Func(Of T))
		If update Is Nothing Then
			Throw New System.ArgumentNullException("update")
		End If
		If retrieve Is Nothing Then
			Throw New System.ArgumentNullException("retrieve")
		End If
		Me._set = update
		Me._get = retrieve
	End Sub
End Class
