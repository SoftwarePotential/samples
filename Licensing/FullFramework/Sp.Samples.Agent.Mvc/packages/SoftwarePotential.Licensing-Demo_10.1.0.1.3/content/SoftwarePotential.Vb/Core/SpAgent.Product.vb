' NB This file is auto-generated via the SoftwarePotential.Licensing-Demo_10 NuGet package.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

' NB Relies on a Reference to Sp.Agent.dll, which emanates from the Sp.Agent package (typically via the SoftwarePotential-<ShortCode> package)
' if compilation fails here, it's typically because you're missing a reference Sp.Agent package 
Imports Sp.Agent.Configuration
Imports Sp.Agent
Imports System

Partial Class SpAgent
    Shared _lazyProductContext As Lazy(Of IProductContext) = New Lazy(Of IProductContext)(AddressOf GenerateProductContext)
    Public Shared ReadOnly Property Product() As IProductContext
        Get
            Return _lazyProductContext.Value
        End Get
    End Property
    Shared Function GenerateProductContext() As IProductContext
        Return TheAgentContext.CreateDemo_10ProductContext()
    End Function
    Private Shared ReadOnly Property TheAgentContext As IAgentContext
        Get
            ' NB The 'SpAgent.Configuration.AgentContext' static property emanates from SoftwarePotential\SpAgent.Permutation.cs
            ' if compilation fails here, it's typically because you've yet to add a NuGet Reference to a SoftwarePotential-<ShortCode> package
            ' In most cases, this means you're missing a reference to one of the SoftwarePotential.Configuration.* packages (although if you're just doing licensing queries in a submodule of your system, just referencing the SoftwarePotential-<ShortCode> package is sufficient)
            Return SpAgent.Configuration.AgentContext
        End Get
    End Property
End Class
Module SpAgentDemo_10Extensions
    <System.Runtime.CompilerServices.Extension>
    Public Function CreateDemo_10ProductContext(that As IAgentContext) As IProductContext
        ' NB The SpProduct static class emanates from SoftwarePotential\SpProduct.cs in a Sp.Product-Product_Version NuGet Package 
        ' if compilation fails here, it's likely to be due to a problem with the installation of the SoftwarePotential.Licensing-Demo_10 NuGet Package (i.e., this one) which typically pulls this in as a package dependency
        Return that.ProductContextFor(SpProduct.Name, SpProduct.Version)
    End Function
End Module