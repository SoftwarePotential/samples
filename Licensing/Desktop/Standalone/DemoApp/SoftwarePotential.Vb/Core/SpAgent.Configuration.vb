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
    ''' <para>Partial method enabling specification of an appropriate License Storage Policy by a partnering partial class.</para>
    ''' <para>Typically an implementation of this is provided via a SoftwarePotential.Configuration.* package.</para>
    ''' <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
    ''' callback will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
    ''' </summary>
    ''' <param name="configure">
    ''' <para>delegate that accepts the Store Configuration segment of a <c>IAgentContext.Configure()</c> Fluent Configuration sequence.</para>
    ''' <para><example>See the code emitted by any of the following <c>SoftwarePotential.Configuration.</c> packages: <c>Local.*</c>, <c>Web</c> or <c>Distributor</c>.</example></para>
    ''' </param>
    Partial Private Shared Sub ConfigureStorage(configure As Action(Of Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)))
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
    ''' Storage Configuration aspect (or throws if a correctly implemented <c>ConfigureStorage()</c> partial method is not in place).
    ''' </summary>
    ''' <exception cref="T:System.InvalidOperationException">Thrown if no <c>ConfigureStorage()</c> method (that correctly invokes its argument) is present.</exception>
    Private Shared Function FetchStorageConfigurationSequenceOrThrow() As Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase)
        Dim configureStorage As Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase) = Nothing
        SpAgent.ConfigureStorage(Sub(configure As Func(Of IAgentCommenceConfigurationPhase, IAgentDistributorsConfigurationPhase))
                                     configureStorage = configure
                                 End Sub)
        If configureStorage Is Nothing Then
            Throw SpAgent.CreateMissingPartialMethodException("No Storage Configuration provided", "ConfigureStorage")
        End If
        Return configureStorage
    End Function

    Private Shared Function CreateMissingPartialMethodException(preamble As String, methodName As String) As Exception
        Return New InvalidOperationException(String.Format(CultureInfo.InvariantCulture, preamble + " via {0}(); Licensing Storage cannot be successfully initialized." & Environment.NewLine & "Please ensure there is a valid implementation of {0}() that invokes its callback correctly in place." & Environment.NewLine & "See the documentation for the {0}() partial method or further information.", methodName))
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
End Class

''' <summary>
''' Offers a set of extension methods allowing one to decompose an <c>IAgentContext.Configure()</c> Fluent Configuration 
''' Expression Sequence into individual elements yet retain the familiar fluent expression nature.
''' </summary>
''' <remarks>Equivalent to F#'s built-in |> operator.</remarks>
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
