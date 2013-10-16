' NB This file is auto-generated via the SoftwarePotential.Configuration.Local.SingleUser-XXYYY NuGet package.
' For more details see the README at http://docs.softwarepotential.com/Configuration.Local.SingleUser-README.html
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

imports Sp.Agent.Configuration
imports System
imports System.IO

Partial Class SpAgent
    ''' <summary>
    ''' <para>Configures licenses to be stored under <c>C:\Users\USERNAME\AppData\Local\MyCompany\MyProduct\MyVersion</c>.</para>
    ''' <para>NB THIS LOCATION IS SPECIFIC TO A USER PROFILE AND WILL MEAN YOUR CUSTOMERS CAN NOT SHARE LICENSES ACROSS PROFILES.</para>
    ''' <para>See <c>SoftwarePotential.Configuration.Local.MultiUser</c> if you need to address this restriction [by having an installer create a shared area]</para>
    ''' </summary>
    ''' ''' <param name="agentContext">The context, emanating from your Sp.Agent package.</param>
    ''' <remarks>
    ''' <para>Slots into the extension point exposed by the SoftwarePotential.Configuration package in its SpAgent.cs file.</para>
    ''' </remarks>
    Private Shared Sub ConfigureAgent(agentContext As IAgentContext)
        Dim location = LocalSingleUserStorePath
        Dim rootPath = location.Item1
        Dim relativePath = location.Item2
        agentContext.Configure(Function(configure) configure _
            .WithSingleUserStore(rootPath, relativePath) _
                .CompleteWithDefaults())
    End Sub
    ''' <summary>
    ''' <para>Computes the (absolute and relative) path to use for license storage.</para>
    ''' <para>See ConfigureLocalSingleUserStorePath() for a mechanism to override this cleanly.</para>
    ''' </summary>
    Shared ReadOnly Property LocalSingleUserStorePath As Tuple(Of String, String)
        Get
            Dim baseDir = LocalApplicationDataFolderPath
            Dim vendor = SpProduct.Vendor
            Dim product = SpProduct.Name
            Dim version = SpProduct.Version
            Dim overridePath As Tuple(Of String, String)

            ' Call the partial method. If it is implemented, it will call us back via the lambda and the result will be stashed in overridePath
            ConfigureLocalStorePath( _
                baseDir, vendor, product, version, _
                Sub(rootPath, relativePath) overridePath = Tuple.Create(rootPath, relativePath))
            Return If(overridePath, Tuple.Create(baseDir, Path.Combine(SpProduct.Vendor, SpProduct.Name, SpProduct.Version)))
        End Get
    End Property
    ''' <summary>
    ''' <para>Placeholder to enable overriding the store base path (without requiring the editing of this file if desired).</para>
    ''' <para>See SpAgent.Configuration.Local.Customizations.cs for a mechanism to override this cleanly.</para>
    ''' </summary>
    ''' <param name="rootPath">The absolute path to application data folder (e.g. C:\Users\USERNAME\AppData\Local).</param>
    ''' <param name="vendor">Vendor name</param>
    ''' <param name="product">Product name</param>
    ''' <param name="version">Product version</param>
    ''' <param name="overridePath">Action that needs to be called to supply the customized store path</param>
    Partial Private Shared Sub ConfigureLocalStorePath(rootPath As String, vendor As String, product As String, version As String, overridePath As Action(Of String, String))
    End Sub
    ''' <summary>
    ''' <para>Default License Storage Root folder.</para>
    ''' <para>See ConfigureLocalSingleUserStorePath() for a mechanism to override this cleanly.</para>
    ''' </summary>
    ''' <value>Yields the ProgramData path for this machine (even if that folder does not exist).</value>
    Shared ReadOnly Property LocalApplicationDataFolderPath As String
        Get
            ' %LOCALAPPDATA%, typically expands to C:\Users\USERNAME\AppData\Local _
            Return Environment.GetFolderPath( _
                Environment.SpecialFolder.LocalApplicationData,
                Environment.SpecialFolderOption.DoNotVerify) ' If it can't be located, we still want a path (.None by default substitutes string.Empty if the folder is not found)
        End Get
    End Property
End Class