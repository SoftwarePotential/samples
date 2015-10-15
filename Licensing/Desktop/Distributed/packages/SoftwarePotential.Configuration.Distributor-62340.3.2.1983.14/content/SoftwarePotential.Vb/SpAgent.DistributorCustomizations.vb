' NB This file is auto-generated via the SoftwarePotential.Configuration.Distributor-<ShortCode> NuGet package.
' 
' TODO: IF YOU MODIFY THIS FILE, CONSIDER MOVING ANY MODIFIED METHODS (AND/OR 
' RENAMING THIS FILE) SO NUGET PACKAGE UPDATES CANNOT RESULT IN YOU INADVERTENTLY 
' UNDOING CHANGES YOU HAVE MADE

Imports System
Imports System.Diagnostics
Imports Sp.Agent.Configuration ' Used by some #ifdef'd sections of this file

#If Not DISABLE_SP_AGENT_DISTRIBUTOR_WRITABLE_CONFIGURATION_FILE And Not SP_AGENT_DISTRIBUTOR_DISABLE_WRITABLE_CONFIGURATION_FILE Then

'' <summary>
'' This portion of the partial class uses the Storage configured via the
'' SoftwarePotential.Configuration.(Multi|Single)User Package's ConfigureStorage()
'' implementation to maintain a setting within an area that the Application 
'' will be able to write to regardless of which user is active.
'' </summary>
'' <remarks>
'' see http://docs.softwarepotential.com/Configuration.Local.SingleUser-README.html 
'' or http://docs.softwarepotential.com/Configuration.Local.MultiUser-README.html 
'' for further information about Sp.Agent's storage mechanisms
'' </remarks>
Partial Class SpAgent
    Private Shared Sub ConfigureStaticDistributorDiscovery(configure As Action(Of Func(Of Uri)))
		configure(Function() As Uri
				  Debug.WriteLineIf(
					  SpAgent.Configuration.DistributorBaseUri Is Nothing,
					  "No Distributor BaseUri is currently Configured. For a Distributor to be used, something needs to set SpAgent.DistributorBaseUri.")
				  Return SpAgent.Configuration.DistributorBaseUri
			  End Function)

        Dim lazyRuntimeConfigFile As Lazy(Of SpAgentWritableDistributorAppSettings) = New Lazy(Of SpAgentWritableDistributorAppSettings)(
            Function() WritableConfigurationFile.OpenOrCreateEmpty(SpAgent.Product.GetConfigurationFolder("Sp.Agent.Distributor.Configuration")))
        SpAgent.Configuration.InitializeDistributorBaseUriCallbacks(
            Sub(value As Uri)
                lazyRuntimeConfigFile.Value.UpdateBaseUri(value)
            End Sub,
            Function() lazyRuntimeConfigFile.Value.ReadBaseUriOrDefault())

    End Sub
    Partial Public Class Configuration
        Private Shared _configuredBaseUri As WriteThroughCachedValue(Of Uri)
        Public Shared Property DistributorBaseUri() As Uri
            Get
                Return _configuredBaseUri.Value
            End Get
            Set(value As Uri)
                _configuredBaseUri.Value = value
            End Set
        End Property
        Shared Sub InitializeDistributorBaseUriCallbacks(write As Action(Of Uri), read As Func(Of Uri))
            _configuredBaseUri = New WriteThroughCachedValue(Of Uri)(write, read)
        End Sub
    End Class
End Class
Partial Public Class CommandLineProcessing
    Public Shared Function ProcessDistributorArgs(args As String()) As Boolean
        Dim distributorBaseUri As String = CommandLineParsing.ArgumentOrDefault("distributor", args)
        Dim result As Boolean
        If distributorBaseUri IsNot Nothing Then
            ExecuteCommandLineAction(
                "Setting Distributor Base Uri to: " + distributorBaseUri,
                Sub()
                    SpAgent.Configuration.DistributorBaseUri = New Uri(distributorBaseUri)
                End Sub)
            result = True
        Else
            If CommandLineParsing.HasSwitch("distributor", args) Then
                ReportInstallationAction("Configured Distributor Base Uri: " + SpAgent.Configuration.DistributorBaseUri.ToString)
                result = True
            Else
                If CommandLineParsing.HasSwitch("distributorConfigDir", args) Then
                    ReportInstallationAction("Distributor Config Folder: " + SpAgent.Product.GetConfigurationFolder("Sp.Agent.Distributor.Configuration"))
                    result = True
                Else
                    result = False
                End If
            End If
        End If
        Return result
    End Function
End Class
#ElseIf SP_AGENT_DISTRIBUTOR_READONLY_CONFIG_IN_APP_CONFIG Then
'' <summary>
'' This portion of the partial class stubs out the Storage aspect of 
'' the Agent Context Configuration configures the Distributor Discovery to be completely
'' driven from the application's Configuration file (app.config/web.config/App.exe.config etc.)
'' </summary>
'' <remarks>
'' When using this approach, it is implicit that the installation process of 
'' your application needs to completely manage the presence/absence of a 
'' Distributor Base Uri setting in the app.config (which you won't be able to 
'' update under application control).
'' This approach combines well with SP_AGENT_NO_LOCAL_STORAGE
'' </remarks>
Partial Class SpAgent
	Private Shared Sub ConfigureStaticDistributorDiscovery(configure As Action(Of Func(Of Uri)))
		configure(Function()
			Dim result = SpAgentDistributorConfiguration.FromAppConfig().ReadBaseUriOrDefault()
			'TODO If you want the application to use Distributed licenses, your installer will need to ensure a value is present in the app.config
			'-- see http://docs.softwarepotential.com/Configuration.Distributor-README.html for further details
			Debug.WriteLineIf( result Is Nothing, "No Distributor BaseUri is currently Configured. For a Distributor to be used, the app.config needs an appSetting named Sp.Agent.Distributor.BaseUri" )
			Return result
		End Function)
	End Sub
End Class
#End If

#If SP_AGENT_DISTRIBUTOR_CONFIG_EXTERNALLY_MANAGED Then
'' <summary>
'' This portion of the partial class shows a skeleton implementation that 
'' you can complete to have the Distributor Base Uri be retrieved from 
'' elsewhere in your Application.
'' </summary>
'' <remarks>
'' This approach makes sense if you already have an existing settings Storage 
'' system and/or Options Dialog in your application and hence have no need 
'' for (or benefit from) the Distributor Base Uri being maintained elsewhere.
'' </remarks>
Partial Class SpAgent
	Private Shared Sub ConfigureStaticDistributorDiscovery(configure As Action(Of Func(Of Uri)))
		configure(Function()
			Dim result = New Uri("TODO")	'TODO impl, e.g. MyApplicationSettings.DistributorBaseUri;
			'TODO If you want the application to use Distributed licenses, the above Application-provided setting will need to return non-null
			'-- see http://docs.softwarepotential.com/Configuration.Distributor-README.html for further details
			Debug.WriteLineIf(result Is Nothing, "No Distributor BaseUri is currently Configured. For a Distributor to be used, your Externally Managed Settings will need to yield a non-null BaseUri")
			Return result
		End Function)
	End Sub
End Class

#ElseIf SP_AGENT_DISTRIBUTOR_PROMPT_FOR_ENDPOINT_EXAMPLE Then
'' <summary>
'' This portion of the partial class shows a skeleton implementation that 
'' illustrates how one might have an Options Dialog be triggered to allow 
'' selection of a Distributor Endpoint under Application Control.
'' 
'' The only essential bit is that the lambda passed to configure() returns either:
'' - a Valid Uri if a Distributor is to be used 
'' - null if the user has opted not connect to a Distributor
'' </summary>
Partial Class SpAgent
	Private Shared Sub ConfigureStaticDistributorDiscovery(configure As Action(Of Func(Of Uri)))
		' Cache the selected endpoint here so multiple calls to the callback can save the value
		Dim selectedEndpoint As Uri = Nothing

		' TODO load saved value from config file or similar

		configure(Function()

			If  Not selectedEndpoint Is Nothing 
					Return selectedEndpoint
			End If

			Dim ok = RunDialog( selectedEndpoint )
			If Not ok
				System.Diagnostics.Trace.WriteLine( "User Cancelled Distributor Service Endpoint selection" )
				Return Nothing 'a NotLicensedException will be raised by the default behavior
			End If

			' TODO save selectedEndpoint into storage

			Return selectedEndpoint
		End Function)
	End Sub

	Private Shared Function RunDialog( ByRef selectedEndpoint As Uri ) As Boolean
		Dim dialogResult As Boolean
		Do
			' TODO prompt user, put result into dialogResult and output into selectedEndpoint

			If dialogResult = DialogResult.Cancel 
				Return False
			End If

			'Validate the selected Endpoint to give immediate feedback to the user as to 
			'whether the nominated endpoint is a Valid Distributor
		Loop While Not SpAgent.Distributors.CanConnect( selectedEndpoint )
		Return True

	End Function

End Class

#ElseIf SP_AGENT_NO_LOCAL_STORAGE Then
'' <summary>
'' This portion of the partial class stubs out the Storage aspect of 
'' the Agent Context Configuration. This is only appropriate if you are not using a 
''  SoftwarePotential.Configuration.(Multi|Single)User Package.
'' </summary>
Partial Class SpAgent
		''  <summary> 
		''  Stub out any storage for licenses or config files on the local machine;
		''  assume all state will be maintained on a continually available Distributor.
		''  </summary> 
		''  <remarks>
		''  NB using this precludes using the following facilities:
		''  - using WritableApplicationConfigFile
		''  - Checking out Licenses
		''  - Using locally installed Licenses
		'' </remarks>
		Private Shared Sub ConfigureStorage( configure As Action(Of Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)))
			configure(Function(agent)
				Return agent.DisableStorage
			 End Function)
		End Sub
		
		'' Private Shared Sub ConfigureStaticDistributorDiscovery(configure As Action(Of Func(Of Uri)))
		''	'' TODO implement derivation of the Distributor Base Uri. See one of the following sections for details:
		''	''	SP_AGENT_DISTRIBUTOR_READONLY_CONFIG_IN_APP_CONFIG
		''	''	SP_AGENT_DISTRIBUTOR_CONFIG_EXTERNALLY_MANAGED
		'' End Sub
End Class
#End If
