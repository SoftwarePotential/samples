' NB This file is auto-added via the SoftwarePotential.Configuration-<ShortCode> NuGet package.
'
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports Sp.Agent
Imports System
Imports System.Diagnostics
Imports System.Linq
''' <summary>
''' <para>Provides standardized processing of Licensing-related command-line parameters.</para>
''' <para>To be invoked from your application's entry point.</para>
''' </summary>
Partial Public Class CommandLineProcessing
    Public Shared Function ProcessInstallationArgs(args As String()) As Boolean
        Dim handledSomething As Boolean = False
        If CommandLineParsing.HasSwitch("initialize", args) Then
            ExecuteCommandLineAction("Initializing Shared License Storage...", New Action(AddressOf SpAgent.Configuration.StoresInitialization.Initialize))
            handledSomething = True
        End If
        ReportInstallationActionIfDebug("Verifying Storage...")
        SpAgent.Configuration.VerifyStoresInitialized()
        ReportInstallationActionIfDebug("... verified.")
        Dim activationKey As String = CommandLineParsing.ArgumentOrDefault("activate", args)
        If activationKey IsNot Nothing Then
            ExecuteCommandLineAction("Activating License: " + activationKey, Sub()
                                                                                 SpAgent.Product.Activation.OnlineActivateAsync(activationKey).Wait()
                                                                             End Sub)
            handledSomething = True
        End If
        Return handledSomething
    End Function
    Public Shared Function ProcessUninitializationArgs(args As String()) As Boolean
        Dim result As Boolean
        If CommandLineParsing.HasSwitch("uninitialize", args) Then
            ExecuteCommandLineAction("Removing Shared License Storage...", New Action(AddressOf SpAgent.Configuration.StoresInitialization.Uninitialize))
            result = True
        Else
            result = False
        End If
        Return result
    End Function
    Private Shared Sub ExecuteCommandLineAction(description As String, execute As Action)
        ReportInstallationAction(description + "...")
        execute()
        ReportInstallationAction("... succeeded.")
    End Sub
    <System.Diagnostics.Conditional("DEBUG")>
    Private Shared Sub ReportInstallationActionIfDebug(action As String)
        ReportInstallationAction(action)
    End Sub
    Partial Private Shared Sub ReportInstallationAction(action As String)
    End Sub
End Class
Module CommandLineParsing
    Public Function HasSwitch(name As String, args As String()) As Boolean
        Return args.Any(Function(x As String) String.Equals("-" + name, x, System.StringComparison.OrdinalIgnoreCase))
    End Function
    Public Function ArgumentOrDefault(name As String, args As String()) As String
        Return args.[Select](Function(x As String)
                                 Dim prefix As String = "-" + name + ":"
                                 Dim result As String
                                 If Not x.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase) Then
                                     result = Nothing
                                 Else
                                     result = x.Substring(prefix.Length)
                                 End If
                                 Return result
                             End Function).Where(Function(x As String) x <> Nothing).FirstOrDefault()
    End Function
End Module