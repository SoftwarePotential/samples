'' NB This file is auto-added via the SoftwarePotential.Configuration.Distributor-<ShortCode> NuGet package.
''
'' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports System
Imports System.Globalization
Imports System.Linq
Imports Sp.Agent.Configuration
Imports Sp.Agent
Imports Sp.Agent.Storage.Internal
Imports Sp.Agent.Distributor

''' <summary>
''' This portion of the partial class plugs into the ConfigureDistributor extension 
''' point of the SoftwarePotential.Configuration Package and in turn provides a 
''' (mandatory) ConfigureStaticDistributorDiscovery extension point.
''' </summary>
Partial Class SpAgent

    ''' <summary>
    ''' <para>Partial method enabling specification of an appropriate Distributor Discovery Algorithm via a partnering partial class.</para>
    ''' <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.Distributor package (or dependent).</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the <c>WithDiscovery</c> Configuration segment of a <c>IAgentDistributorsConfigurationPhase.WithDistributor()</c> Fluent Configuration sequence.</para>
    ''' <para><example>See the code emitted by <c>SoftwarePotential.Configuration.Distributor</c>.</example></para>
    ''' </param>
    Partial Private Shared Sub ConfigureStaticDistributorDiscovery(configure As Action(Of Func(Of Uri)))
    End Sub

    ''' <summary>
    ''' Implements the extension point, delegating to <c>ConfigureStaticDistributorDiscovery</c>.
    ''' </summary>
    ''' <param name="configure"></param>
    Private Shared Sub ConfigureDistributor(configure As Action(Of Func(Of IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase)))
        Dim callback As Func(Of Uri) = Nothing
        SpAgent.ConfigureStaticDistributorDiscovery(Sub(discover As Func(Of Uri))
                                                        callback = discover
                                                    End Sub)
        If callback Is Nothing Then
            Throw SpAgent.CreateMissingDistributorPartialMethodException("No Distributor Discovery Algorithm specified", "ConfigureStaticDistributorDiscovery")
        End If
        configure(Function(agent As IAgentDistributorsConfigurationPhase) agent.WithDistributor(Function(distributor As IDistributorCommenceConfigurationPhase) distributor.WithDiscovery(callback).CompleteWithDefaults()))
    End Sub

    Private Shared Function CreateMissingDistributorPartialMethodException(preamble As String, methodName As String) As Exception
        Return New InvalidOperationException(String.Format(CultureInfo.InvariantCulture, preamble + " via {0}(); Distributor Component cannot be successfully configured." & Environment.NewLine & "Please ensure there is a valid implementation of {0}() that invokes its callback correctly in place." & Environment.NewLine & "See the documentation for the {0}() partial method or further information.", methodName))
    End Function

    ' Want to have the initial spin up of the Context Lazy as it has a non-zero cost (the caching effect of Lazy vs a Func is not as critical)
    Shared ReadOnly _distributorsContext As Lazy(Of IDistributorsContext) =
        New Lazy(Of IDistributorsContext)(Function() Configuration.AgentContext.CreateDistributorsContext())

    ''' <summary>
    ''' <para>Provides access to Distributor connectivity and service health diagnostics facilities.</para>
    ''' <para>To adjust configuration, see <see cref="Configuration"/>. For access to configured resources, use <see cref="Distributed"/>.</para>
    ''' </summary>
    Shared ReadOnly Property Distributors() As IDistributorsContext
        Get
            Return _distributorsContext.Value
        End Get
    End Property

    ' Want to have the initial spin up of the Context Lazy as it has a non-zero cost (the caching effect of Lazy vs a Func is not as critical)
    Shared ReadOnly _distributedContext As Lazy(Of IDistributedContext) = New Lazy(Of IDistributedContext)(Function() SpAgent.Product.CreateDistributedContext())

    ''' <summary>
    ''' <para>Provides APIs necessary for building a Distributor-aware application using the NuGet <see cref="Product"/> package you have installed.</para>
    ''' <para>To adjust configuration, see <see cref="Configuration"/>. To verify service health or connectivity use <see cref="Distributors"/>.</para>
    ''' </summary>
    Shared ReadOnly Property Distributed() As IDistributedContext
        Get
            Return _distributedContext.Value
        End Get
    End Property
End Class

''' <summary>
''' For internal use only.
''' </summary>
''' <remarks>
''' Subject to unlimited change without notice even in minor version changes.
''' </remarks>
Module ConfigurationFolderExtensions
    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function GetConfigurationFolder(that As IProductContext, configurationName As String) As String
        Return that.Stores.Configuration().All().[Single]().GetFolder(configurationName)
    End Function
End Module