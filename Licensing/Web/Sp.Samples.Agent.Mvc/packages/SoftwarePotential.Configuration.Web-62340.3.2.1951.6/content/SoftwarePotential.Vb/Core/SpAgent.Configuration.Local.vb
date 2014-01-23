' NB This file is auto-generated via the SoftwarePotential.Configuration.Local NuGet packages.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports System
Imports System.IO
Imports System.Globalization

Partial Class SpAgent

	''' <summary>
	''' <para>Composes a complete path to a license store directory from invocations of ConfigureStorageBasePath() 
	''' and ConfigureStorageRelativePath() (see <c>ConfiguredBaseAndRelativePathElements()</c> for details).</para>
	''' <para>Typically, this is useful when such a location is created and permissioned externally such as during application deployment (i.e. by an installer) or by the Application Hosting Environment (e.g., ASP.NET's App_Data).</para>
	''' </summary>
    Shared Function ConfiguredExternallyManagedRootPath() As String
        Dim paths As Tuple(Of String, String) = ConfiguredBaseAndRelativePathElements()
        Return Path.Combine(paths.Item1, paths.Item2)
    End Function

	''' <summary>
	''' Returns a license store location consisting of 2 parts:
	''' <list type="bullet">
	''' <item><description>base path - license store root path, which can be shared between applications</description></item>
	''' <item><description>relative path - when combined with base path, yields a combined result that is unique per application.</description></item></list>
	''' See <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> for details of the relevant extensibility points.
	''' </summary>
    Shared Function ConfiguredBaseAndRelativePathElements() As Tuple(Of String, String)
        Dim basePath As String = Nothing
        ConfigureStorageBasePath(Sub(value) basePath = value)
        If basePath Is Nothing Then
            Throw CreateMissingPartialMethodException("Storage Location Base path element not correctly configured", "ConfigureStorageBasePath")
        End If
        Dim relativePath As String = Nothing
        ConfigureStorageRelativePath(SpProduct.Vendor, SpProduct.Name, SpProduct.Version, Sub(value) relativePath = value)
        If relativePath Is Nothing Then
            Throw CreateMissingPartialMethodException("Storage Location Relative path suffix not correctly configured", "ConfigureStorageBasePath")
        End If
        Return Tuple.Create(basePath, relativePath)
    End Function

	''' <summary>
	''' <para>Partial method enabling specification of an appropriate license store base path by a partnering partial class.</para>
	''' <para>Typically an implementation of this is provided via a <c>SoftwarePotential.Configuration.*</c> package in a <c>SpAgent.*Customizations.cs</c> file.</para>
	''' <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
	''' callback will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
	''' </summary>
	''' <param name="configure">delegate that accepts a License Store base path to be used.</param>
	Partial Private Shared Sub ConfigureStorageBasePath(configure As Action(Of String))
	End Sub

	''' <summary>
	''' <para>Partial method enabling specification of an appropriate license store relative path by a partnering partial class.</para>
	''' <para>Typically an implementation of this is provided via a <c>SoftwarePotential.Configuration.*</c> package in a <c>SpAgent.*Customizations.cs</c> file.</para>
	''' <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
	''' callback will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
	''' </summary>
	''' <param name="configure">delegate that accepts a License Store relative path to be used.</param>
	''' <param name="vendor">Internal Vendor Name as specified in your Product Definition on the Software Potential Service.</param>
	''' <param name="product">Internal Product Name as specified in your Product Definition on the Software Potential Service.</param>
	''' <param name="version">Internal Product Version as specified in your Product Definition on the Software Potential Service.</param>
	Partial Private Shared Sub ConfigureStorageRelativePath(vendor As String, product As String, version As String, configure As Action(Of String))
	End Sub
End Class