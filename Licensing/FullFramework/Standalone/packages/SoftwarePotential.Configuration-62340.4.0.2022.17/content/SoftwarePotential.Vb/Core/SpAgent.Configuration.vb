' NB This file is auto-generated via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports Sp.Agent
Imports Sp.Agent.Configuration
Imports Sp.Agent.Configuration.Internal ' For IsConfigured( this IAgentContext that) as Boolean (which is normally not recommended)
Imports System
Imports System.Globalization

' sic - this is not the primary location of this partial class, so no xmldocs here
Partial Class SpAgent
    ' sic - this is not the primary location of this partial class, so no xmldocs here
    Partial Class Configuration
        ''' <summary>
        ''' Should be called once from Main/your entrypoint before running any Licensed 
        ''' or Protected code or calling Licensing APIs (e.g. Activation)
        ''' <para>Ideally the work involved in this should take place on a background thread 
        ''' in parallel with other initialization activities.</para>
        ''' <para>NB depending on the store type involved, it may be necessary to call <c>SpAgent.Configuration.InitializeSharedLicenseStorage()</c> or similar as part of one's installation sequence.</para>
        ''' </summary>
        ''' <remarks>NB Calling the method has the important side effect of Initializing the configurations (see static constructor below)</remarks>
        Shared Sub VerifyStoresInitialized()
            StoresInitialization.Verify()
        End Sub
        ''' <remarks>NB Calling the method has the critical side effect of Initializing the Agent Context and Product Context configurations [via the static constructor of SpAgent]</remarks>
        Shared ReadOnly Property StoresInitialization() As IInitializeStores
            Get
                Return SpAgent.Product.Stores.Initialization()
            End Get
        End Property
    End Class
    ''' <summary>
    ''' The Sp.Agent Configuration calls should execute once and only once, which is 
    ''' exactly what a static constructor does
    ''' </summary>
    ''' <remarks>
    ''' Note that VerifyStoresInitialized() (and SpAgentInstallation.InitializeSharedLicenseStorage)
    ''' is housed in this class to benefit from the implicit thread safe call to this on first hit
    ''' In other words, be careful if moving stuff out of here
    ''' </remarks>
    Shared Sub New()
        If Not ShouldSkipConfiguration(SpAgent.Configuration.AgentContext) Then
            ' If this triggers a compiler error, it's likely because there is not a SoftwarePotential.Configuration package reference in place
            ' Typically, one would Install-Package SoftwarePotential.Configuration to remediate this
            ConfigureAgent(SpAgent.Configuration.AgentContext)

            ' If this triggers a compiler error, it's likely because there is not a SoftwarePotential\SpAgent.Product.cs file in place 
            ' Typically, one would Install-Package SoftwarePotential.Licensing-<ProductName_ProductVersion> to remediate this
            ConfigureProduct(SpAgent.Product)
        End If
    End Sub

    ''' <summary>
    ''' Implements policy detailed in comments on ConfigureMultipleSpAgentsPerPermutation().
    ''' </summary>
    ''' <param name="agentContext"></param>
    ''' <returns></returns>
    Shared Function ShouldSkipConfiguration(agentContext As IAgentContext) As Boolean
        Dim sharedModeRequested As Boolean = False ' By default we want to know if there are competing attempts to Configure the Agent
        ConfigureMultipleSpAgentsPerPermutation(
            Sub(value)
                sharedModeRequested = value
            End Sub)
        Return sharedModeRequested And agentContext.IsConfigured()
    End Function

    ''' <summary>
    ''' <para>Partial method enabling one to request an 'already initialized by a co-operating Assembly' guard in a partnering partial.</para>
    ''' <para>This allows a set of Assemblies with equivalent configurations to let the first one loaded manage the Configuration and others to assume that if configuration has already taken place that it is an equivalent setup.</para>
    ''' <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
    ''' callback will result in the default behavior:- each SpAgent class will attempt to Configure the Agent and an exception will result if more than  one does so per Permutation.</para>
    ''' <para>NB this will inhibit the normal behavior of trapping inadvertent use of protected code prior to controlled initialization via <c>SpAgent</c> and should be applied with care.</para>
    ''' </summary>
    ''' <remarks>Often a cleaner solution is to assign separate Permutations to each Application/component in order to both provide isolation and avoid this issue.</remarks>
    ''' <param name="configure">delegate that can be passed a flag indicating whether one wants to operate in 'first configuration wins' mode.</param>
    ''' <example><code>Partial Private Shared Sub ConfigureMultipleSpAgentsPerPermutation(configure As Action(Of Boolean))
    '''     configure( True) // Allow first SpAgent to be touched to manage the configurations and others to assume that any preceding initialization is correct.
    ''' End Sub
    ''' </code></example>
    Partial Private Shared Sub ConfigureMultipleSpAgentsPerPermutation(configure As Action(Of Boolean))
    End Sub

    ''' <summary>
    ''' <para>Partial method enabling specification of the policy for all Client side License Storage (both Local-bound and Removable stores) as a whole by a partnering partial class.</para>
    ''' <para>NB using this precludes the use of <code>ConfigureLocalBoundStorage()</code> and/or <code>ConfigureRemovableStorage()</code>. Where possible it is recommended to provide individual overrides of those.</para>
    ''' <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/>
    ''' callback (or one of <code>ConfigureLocalBoundStorage()</code> and/or <code>ConfigureRemovableStorage()</code>) will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the Store Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para>delegate that accepts the Local-bound and Removable Store Configuration segments of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para><example>See the code emitted by any of the following <c>SoftwarePotential.Configuration.</c> packages: <c>Local.*</c>, <c>Web</c> or <c>Distributor</c>.</example></para>
    ''' </param>
    Partial Private Shared Sub ConfigureStorage(configure As Action(Of Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)))
    End Sub

    ''' <summary>
    ''' <para>Partial method enabling specification of an appropriate License Storage Policy for Local-bound licenses (excluding Removable stores) by a partnering partial class.</para>
    ''' <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.* package.</para>
    ''' <para>Failure to provide an implementation of this partial method and/or <code>ConfigureRemovableStorage()</code> that correctly invokes the <paramref name="configure"/>
    ''' callback (or a <code>ConfigureLocalStorage()</code> implementation) will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the Local-bound Store Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para><example>See the code emitted by any of the following <c>SoftwarePotential.Configuration.</c> packages: <c>Local.*</c>, <c>Web</c> or <c>Distributor</c>.</example></para>
    ''' </param>
    Partial Private Shared Sub ConfigureLocalBoundStorage(configure As Action(Of Func(Of IAgentCommenceConfigurationPhase, IAgentHardwareBoundStorageConfigurationPhase)))
    End Sub

    ''' <summary>
    ''' <para>Partial method enabling specification of an appropriate License Storage Policy for Local-bound licenses (excluding Removable stores) by a partnering partial class.</para>
    ''' <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.* package.</para>
    ''' <para>Failure to provide an implementation of this partial method and/or <code>ConfigureRemovableStorage()</code> that correctly invokes the <paramref name="configure"/>
    ''' callback (or a <code>ConfigureLocalStorage()</code> implementation) will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the Local-bound Store Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para><example>See the code emitted by any of the following <c>SoftwarePotential.Configuration.</c> packages: <c>Local.*</c>, <c>Web</c> or <c>Distributor</c>.</example></para>
    ''' </param>
    Partial Private Shared Sub ConfigureHardwareBoundStorage(configure As Action(Of Func(Of IAgentHardwareBoundStorageConfigurationPhase, IAgentExternalStorageConfigurationPhase)))
    End Sub

    ''' <summary>
    ''' <para>Partial method enabling specification of an appropriate License Storage Policy for licenses to be maintained on Removable stores by a partnering partial class.</para>
    ''' <para>Typically this is tailored by the ISV to reflect the particular discovery policies/hardware integration dictated by one's licensing scheme/technological constraints.</para>
    ''' <para>Implementation is optional (assuming you have a <code>ConfigureLocalBoundStorage()</code> implementation). Can NOT be combined with <code>ConfigureLocalStorage()</code>.</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the Removable Store Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para><example>See the code emitted by any of the following <c>SoftwarePotential.Configuration.</c> packages: <c>Local.*</c>, <c>Web</c> or <c>Distributor</c>.</example></para>
    ''' </param>
    Partial Private Shared Sub ConfigureRemovableStorage(configure As Action(Of Func(Of IAgentExternalStorageConfigurationPhase, IAgentDistributorsConfigurationPhase)))
    End Sub

    ''' <summary>
    ''' <para>Partial method enabling specification of an appropriate Distributor Client Policy by a partnering partial class.</para>
    ''' <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.Distributor package (or dependent).</para>
    ''' <para>It is safe to omit any implementation of this method.</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the Distributor Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para><example>See the code emitted by <c>SoftwarePotential.Configuration.Distributor</c>.</example></para>
    ''' </param>
    ''' <example>
    ''' Default implementation: 
    ''' <code>configure( distributor => distributor.DisableDistributor())</code>
    ''' </example>
    Partial Private Shared Sub ConfigureDistributor(configure As Action(Of Func(Of IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase)))
    End Sub

    ''' <summary>
    ''' <para>Partial method enabling specification of an appropriate Embedded Licenses Policy by a partnering partial class.</para>
    ''' <para>Typically the default implementation is appropriate for normal usage as having an explicit <c>DisableEmbeddedLicenses()</c> affords only a minor performance benefit.</para>
    ''' <para>It is safe to omit any implementation of this method.</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the Embedded Licenses Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para><example><code>embeddedLicenses => embeddedLicenses.DisableEmbeddedLicenses()</code> </example></para>
    ''' </param>
    ''' <example>
    ''' Default implementation: 
    ''' <code>configure( embeddedLicenses => embeddedLicenses.WithEmbeddedLicensesAutoDetected())</code>
    ''' </example>
    Partial Private Shared Sub ConfigureEmbeddedLicenses(configure As Action(Of Func(Of IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase)))
    End Sub

    ''' <summary>
    ''' Placeholder for method typically emitted into SoftwarePotential\ProductCustomizations.cs by the SoftwarePotential.Configuration package
    ''' </summary>
    ''' <param name="productContext">The context, emanating from your SoftwarePotential.Licensing-&lt;Product_Version&gt; package.</param>
    Partial Private Shared Sub ConfigureProduct(productContext As IProductContext)
    End Sub

    ''' <summary>
    ''' Applies a <c>IAgentContext.Configure()</c> Fluent Configuration Expression gathered via the relevant helpers for each link in the chain.
    ''' Each portion is gathered via associated <c>Configure</c>* <c>partial</c> methods, so it should not be necessary to adjust the implementation here.
    ''' </summary>
    Private Shared Sub ConfigureAgent(agentContext As IAgentContext)
        agentContext.Configure(Function(configure As IAgentCommenceConfigurationPhase)
                                   Return configure _
                                       .WithStorageConfiguration(SpAgent.FetchStorageConfigurationSequenceOrThrow()) _
                                       .WithDistributorConfiguration(SpAgent.DistributorsConfigurationSequenceOrNullObject) _
                                       .WithEmbeddedLicensesConfiguration(SpAgent.EmbeddedLicensesConfigurationSequenceOrNullObject)
                               End Function)
    End Sub

    ''' <summary>
    ''' Yields an <c>IAgentConfiguration.Configure()</c> Fluent Configuration Sequence Segment for the
    ''' Storage Configuration aspect (or throws if and incomplete or ambiguous set of <c>ConfigureStorage()</c> / <c>ConfigureLocalBoundStorage()</c> / <c>ConfigureRemovableStorage()</c>c> partial methods are in place).
    ''' </summary>
    ''' <exception cref="InvalidOperationException">Thrown if none of: <c>ConfigureStorage()</c>, <c>ConfigureLocalBoundStorage()</c> or <c>ConfigureRemovableStorage()</c> (that correctly invoke their argument) are in place.</exception>
    ''' <exception cref="InvalidOperationException">Thrown if both a <c>ConfigureStorage()</c> method and one or more of <c>ConfigureLocalBoundStorage()</c> / <c>ConfigureRemovableStorage()</c> have rendered the configuration ambiguous.</exception>
    Private Shared Function FetchStorageConfigurationSequenceOrThrow() As Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)
        Dim configureLocalBoundStorage As Func(Of IAgentCommenceConfigurationPhase, IAgentHardwareBoundStorageConfigurationPhase) = Nothing
        SpAgent.ConfigureLocalBoundStorage(Sub(configure As Func(Of IAgentCommenceConfigurationPhase, IAgentHardwareBoundStorageConfigurationPhase))
                                               configureLocalBoundStorage = configure
                                           End Sub)
        Dim configureRemovableStorage As Func(Of IAgentExternalStorageConfigurationPhase, IAgentCloudStorageConfigurationPhase) = Nothing
        Dim configureCloudStorage As Func(Of IAgentCloudStorageConfigurationPhase, IAgentDistributorsConfigurationPhase) = Nothing
        Dim configureStorage As Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase) = Nothing
        Dim configureHardwareBoundStorage As Func(Of IAgentHardwareBoundStorageConfigurationPhase, IAgentExternalStorageConfigurationPhase) = Nothing
        If configureLocalBoundStorage IsNot Nothing OrElse configureRemovableStorage IsNot Nothing OrElse configureHardwareBoundStorage IsNot Nothing Then
            If configureStorage IsNot Nothing Then
                Throw SpAgent.CreateAmbiguousPartialMethodException()
            End If
            If configureLocalBoundStorage Is Nothing Then
                configureLocalBoundStorage = (Function(x As IAgentCommenceConfigurationPhase) x.DisableLocalStore())
            End If
            If configureHardwareBoundStorage Is Nothing Then
                configureHardwareBoundStorage = (Function(x As IAgentHardwareBoundStorageConfigurationPhase) x.DisableHardwareBoundStorage())
            End If
            If configureRemovableStorage Is Nothing Then
                configureRemovableStorage = (Function(x As IAgentExternalStorageConfigurationPhase) x.DisableRemovableStorage())
            End If
            If configureCloudStorage Is Nothing Then
                configureCloudStorage = (Function(x As IAgentCloudStorageConfigurationPhase) x.DisableCloudStorage())
            End If
            configureStorage = (Function(commence As IAgentCommenceConfigurationPhase) configureCloudStorage(configureRemovableStorage(configureHardwareBoundStorage(configureLocalBoundStorage(commence)))))
        Else
            If configureStorage Is Nothing Then
                Throw SpAgent.CreateMissingPartialMethodException("No Storage Configuration provided", "ConfigureLocalBoundStorage()(with optional ConfigureRemovableStorage()) or ConfigureStorage")
            End If
        End If
        Return configureStorage
    End Function

    Private Shared Function CreateAmbiguousPartialMethodException() As Exception
        Return New InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "Ambiguous Storage Configuration provided; Licensing Storage cannot be successfully initialized." & Environment.NewLine & "Please ensure that exactly one of the following is in place:" & Environment.NewLine & "A) an implementation of ConfigureStorage() that invokes its callback correctly" & Environment.NewLine & "OR B) implementation(s) of ConfigureLocalBoundStorage() and/or ConfigureRemovableStorage() that invoke their callbacks correctly." & Environment.NewLine & Environment.NewLine & "See the documentation for the the relevant partial methods for further information."))
    End Function

    Private Shared Function CreateMissingPartialMethodException(preamble As String, methodName As String) As Exception
        Return New InvalidOperationException(String.Format(CultureInfo.InvariantCulture, preamble + " via {0}(); Licensing Storage cannot be successfully initialized." & Environment.NewLine & "Please ensure there is a valid implementation of {0}() that invokes its callback correctly in place." & Environment.NewLine & "See the documentation for the {0}() partial method for further information.", methodName))
    End Function

    ''' <summary>
    ''' Yields an <c>IAgentConfiguration.Configure()</c> Fluent Configuration Sequence Segment for the 
    ''' Distributors Configuration aspect (or provides a default that disables Distributor access if no correctly 
    ''' implemented <c>ConfigureDistributor()</c> partial method is in place).
    ''' </summary>
    Private Shared ReadOnly Property DistributorsConfigurationSequenceOrNullObject() As Func(Of IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase)
        Get
            Dim configureDistributors As Func(Of IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase) = Nothing
            SpAgent.ConfigureDistributor(Sub(configure As Func(Of IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase))
                                             configureDistributors = configure
                                         End Sub)
            Dim result As Func(Of IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase)
            If configureDistributors Is Nothing Then
                result = (Function(distributor As IAgentDistributorsConfigurationPhase) distributor.DisableDistributor())
            Else
                result = configureDistributors
            End If
            Return result
        End Get
    End Property

    ''' <summary>
    ''' Yields an <c>IAgentConfiguration.Configure()</c> Fluent Configuration Sequence Segment for the 
    ''' Embedded Licenses Configuration aspect (or provides a default that auto-detects the presence of Embedded Licenses 
    ''' if no correctly implemented <c>ConfigureEmbeddedLicenses()</c> partial method is in place).
    ''' </summary>
    Private Shared ReadOnly Property EmbeddedLicensesConfigurationSequenceOrNullObject() As Func(Of IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase)
        Get
            Dim configureEmbedded As Func(Of IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase) = Nothing
            Dim result As Func(Of IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase)
            If configureEmbedded Is Nothing Then
                result = (Function(embeddedLicenses As IAgentEmbeddedLicensesConfigurationPhase) embeddedLicenses.WithEmbeddedLicensesAutoDetected())
            Else
                result = configureEmbedded
            End If
            Return result
        End Get
    End Property

    Private Shared Sub ConfigureStorage(configure As Action(Of Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)))

    End Sub
End Class

''' <summary>
''' Offers a set of extension methods allowing one to decompose an <c>IAgentContext.Configure()</c> Fluent Configuration 
''' Expression Sequence into individual elements yet retain the familiar fluent expression nature.
''' </summary>
Module AgentConfigurationExtensions

    ''' <summary>
    ''' Applies a delegate in place of having a statically bound expression in a <c>IAgentContext.Configure()</c> 
    ''' Fluent Configuration Expression Sequence for the Storage Configuration aspect of the Agent Configuration.
    ''' </summary>
    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function WithStorageConfiguration(that As IAgentCommenceConfigurationPhase, configureStorage As Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)) As IAgentDistributorsConfigurationPhase
        Return configureStorage(that)
    End Function

    ''' <summary>
    ''' Applies a delegate in place of having a statically bound expression in a <c>IAgentContext.Configure()</c> 
    ''' Fluent Configuration Expression Sequence for the Distributor Configuration aspect of the Agent Configuration.
    ''' </summary>
    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function WithDistributorConfiguration(that As IAgentDistributorsConfigurationPhase, configureDistributor As Func(Of IAgentDistributorsConfigurationPhase, IAgentEmbeddedLicensesConfigurationPhase)) As IAgentEmbeddedLicensesConfigurationPhase
        Return configureDistributor(that)
    End Function


    ''' <summary>
    ''' Applies a delegate in place of having a statically bound expression in a <c>IAgentContext.Configure()</c> 
    ''' Fluent Configuration Expression Sequence for the Storage Configuration aspect of the Agent Configuration.
    ''' </summary>
    <System.Runtime.CompilerServices.ExtensionAttribute()>
    Public Function WithEmbeddedLicensesConfiguration(that As IAgentEmbeddedLicensesConfigurationPhase, configureEmbeddedLicenses As Func(Of IAgentEmbeddedLicensesConfigurationPhase, IAgentCompletedConfigurationPhase)) As IAgentCompletedConfigurationPhase
        Return configureEmbeddedLicenses(that)
    End Function
End Module
