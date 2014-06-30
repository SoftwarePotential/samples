' NB This file is auto-generated via the SoftwarePotential.Configuration.Local NuGet packages.
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Linq

Partial Class SpAgent
''' <summary>
    '''
    ''' <para>TODO: YOUR CUSTOMIZATIONS OR DELETE THIS METHOD</para>
    '''
    ''' <para>Implementing a ConfigureLocalStorePath method is optional (i.e. if you 
    ''' delete the code below, the default processing in the calling code will 
    ''' perform equivalent processing, minus the clearer exception messages emitted by the code below).</para>
    ''' 
    ''' <para>The implementation below is intentionally conservative in nature 
    ''' as we believe it is worth considering the License Storage path to use for your software carefully.</para>
    '''
    ''' <para>The key aspect is that the implementation needs to consistently produce a stable result on a 
    ''' given machine in order for the system to correctly pick up installed licenses on subsequent executions.</para>
    ''' </summary>
    ''' <param name="vendor">Your vendor name as used in the Software Potential Service.</param>
    ''' <param name="product">Your Product Definition name.</param>
    ''' <param name="version">Your Product Definition version.</param>
    <Conditional("DEBUG")>
    Private Shared Sub SanityCheckRelativePathIfDebugBuild(vendor As String, product As String, version As String)
        '=======================================================================================================
        '' TODO: Implement an appropriate algorithm to decide what path structure to use for your license storage
        '' (OR DELETE THIS METHOD TO HAVE THE DEFAULT CONFIGURATION BE APPLIED INSTEAD IF YOU DONT HAVE A COMPANY / PRODUCT NAME COMBINATION THAT IS LONG OR HAS CHARACTERS THAT CANNOT BE USED IN A PATH) 
        '=======================================================================================================

        '' Sanity check the inputs 
        ''   (if any fail, you should consider using an alternate value of your choosing and/or remove any invalid chars)
        WarnAboutInvalidPathChars(vendor)
        WarnAboutInvalidPathChars(product)
        WarnAboutInvalidPathChars(version)

        '' Standard algorithm produces the relative portion of the path just like this 
        ''   (this will cause a problem if any element has characters that are not valid in a path or the combined result is too long)
        Dim storeLocationRelativePath = Path.Combine(vendor, product, version)

        '' Sanity check relative path is not likely to trip the OS 260 char limits 
        ''   (if this fails, consider deriving the location on a different basis)
        WarnAboutLongPaths(storeLocationRelativePath)
    End Sub
    <Conditional("DEBUG")>
    Shared Sub WarnAboutInvalidPathChars(pathElement As String)
        If pathElement.Intersect(Path.GetInvalidPathChars()).Any() Then
            Throw New InvalidOperationException("The string" + pathElement + "contains some characters that are invalid in directory names")
        End If
    End Sub
    <Conditional("DEBUG")>
    Shared Sub WarnAboutLongPaths(relativePath As String)
        '' MaxAppDataRootPathLength should be the longest expected application data path length that may be encountered across any environments this application is intended to be deployed to.
        '' The actual application data path may depend on OS version, OS locale, environment settings, user account name, etc.
        '' (e.g. for single user store it might be C:\Users\<Username> on some machines, but it could be "C:\Documents and Settings\<Username>" on others )
        ''  for multi user store is might be "C:\ProgramData" on some machines, and "C:\Anwendungsdaten" on others)
        '' The value below is supposed to be a conservative value
        Const MaxAppDataRootPathLength As Integer = 60

        Const LicenseFileWithSubdirLength As Integer = 85
        If MaxAppDataRootPathLength + relativePath.Length + LicenseFileWithSubdirLength >= 260 Then
            Throw New InvalidOperationException( _
                "The local store relative path is too long (" + relativePath + "). " _
                & "Please adjust your ConfigureLocalStorePath implementation to yield a stable result across all environments you intend to deploy to.")
        End If
    End Sub
End Class
