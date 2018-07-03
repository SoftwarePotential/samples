' NB This file is auto-generated via the SoftwarePotential-62340 NuGet package.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports Sp.Agent.Configuration
Imports System

Partial Class SpAgent
    ''' <summary>
    ''' <para>Provides access to APIs relevant to the configuration of Software Potential integration.</para>
    ''' <para>Typically should not be used outside of your application's configuration code.</para>
    '''</summary>
    Partial Public Class Configuration
        Shared ReadOnly _lazyAgentContext As Lazy(Of IAgentContext) = New Lazy(Of IAgentContext)(AddressOf GenerateAgentContext)
        Shared Function GenerateAgentContext() As IAgentContext
            Return Global.Sp.Agent.Configuration.AgentContext.For(Permutation62340.Identifiers.ShortCode)
        End Function
        ''' <summary>
        ''' Low-level access to the IAgentContext. In normal usage it should never be necessary to access this directly; each relevant configurable item is exposed as a Configure* partial method on <see cref="SpAgent"/>.
        ''' </summary>
        Public Shared ReadOnly Property AgentContext() As IAgentContext
            Get
                Return _lazyAgentContext.Value
            End Get
        End Property
    End Class
#Region "Backward compatibility shim"
    ' TOCONSIDER when we next version the licensing code snippets and can thus guarantee that all versions of SpAgent.Product.vb will be refreshed to go direct to SpAgent.Configuration.AgentContext (rather than using SpAgent.AgentContext as early versions did), this backcompat shim can be removed
    Shared ReadOnly Property AgentContext() As IAgentContext
        Get
            Return Configuration.AgentContext
        End Get
    End Property
#End Region
End Class
Namespace Permutation62340
    ''' <summary>Identifiers associated with the permutationDescription Software Potential Permutation.</summary>
    Module Identifiers
        '''<summary>5 digit short code as used by <c>Sp.Agent.Configuration.AgentContext.For( string )</c>.</summary>
        Public Const ShortCode As String = "62340"
    End Module
End Namespace