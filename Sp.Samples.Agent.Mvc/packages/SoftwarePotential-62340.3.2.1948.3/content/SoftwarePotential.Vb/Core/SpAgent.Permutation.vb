' NB This file is auto-generated via the SoftwarePotential-62340 NuGet package.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports Sp.Agent.Configuration
Imports System

Partial Class SpAgent
    Shared ReadOnly _lazyAgentContext As Lazy(Of IAgentContext) = New Lazy(Of IAgentContext)(AddressOf GenerateAgentContext)
    Shared Function GenerateAgentContext() As IAgentContext
        Return Global.Sp.Agent.Configuration.AgentContext.For(Permutation62340.Identifiers.ShortCode)
    End Function
    Shared ReadOnly Property AgentContext() As IAgentContext
        Get
            Return _lazyAgentContext.Value
        End Get
    End Property
End Class
Namespace Permutation62340
    ''' <summary>Identifiers associated with the permutationDescription Software Potential Permutation.</summary>
    Module Identifiers
        '''<summary>5 digit short code as used by <c>Sp.Agent.Configuration.AgentContext.For( string )</c>.</summary>
        Public Const ShortCode As String = "62340"
    End Module
End Namespace