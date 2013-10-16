' NB This file is auto-generated via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports Sp.Agent
Imports Sp.Agent.Configuration
Partial Class SpAgent
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
        ' If this triggers a compiler error, it's likely because there is not a SoftwarePotential.Configuration package reference in place
        ' Typically, one would Install-Package SoftwarePotential.Configuration to remediate this
        ConfigureAgent(SpAgent.AgentContext)

        ' If this triggers a compiler error, it's likely because there is not a SoftwarePotential\SpAgent.Product.cs file in place 
        ' Typically, one would Install-Package SoftwarePotential.Licensing-<ProductName_ProductVersion> to remediate this
        ConfigureProduct(SpAgent.Product)
    End Sub
    ''' <summary>
    ''' <para>Placeholder for method generated in SoftwarePotential\SpAgent&lt;Configuration Extension Name&gt;.cs.</para>
    ''' <para>Alternately, one may choose to provide your own implementation.</para>
    ''' </summary>
    ''' <param name="agentContext">The context, emanating from your Sp.Agent-&lt;ShortCode&gt; package.</param>
    Partial Private Shared Sub ConfigureAgent(agentContext As IAgentContext)
    End Sub
    ''' <summary>
    ''' Placeholder for method typically emitted into SoftwarePotential\Customizations.cs by the SoftwarePotential.Configuration package
    ''' </summary>
    ''' <param name="productContext">The context, emanating from your SoftwarePotential.Licensing-&lt;Product_Version&gt; package.</param>
    Partial Private Shared Sub ConfigureProduct(productContext As IProductContext)
    End Sub
End Class