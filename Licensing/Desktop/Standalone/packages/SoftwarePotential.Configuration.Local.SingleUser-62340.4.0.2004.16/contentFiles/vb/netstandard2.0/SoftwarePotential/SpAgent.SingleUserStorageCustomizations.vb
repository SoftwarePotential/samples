' NB This file is auto-generated via the SoftwarePotential.Configuration.Local.SingleUser-XXYYY NuGet package.
'
' For more details see the README at http:'''https://support.softwarepotential.com/hc/en-us/articles/115001365849--SingleUser-Configuration-README
'
' TODO: IF YOU MODIFY THIS FILE, CONSIDER MOVING ANY MODIFIED METHODS (AND/OR 
' RENAMING THIS FILE) SO NUGET PACKAGE UPDATES CANNOT RESULT IN YOU INADVERTENTLY 
' UNDOING CHANGES YOU HAVE MADE

Imports System

''' <summary>
''' This portion of the partial class allows one to control the manner 
''' in which the base and relative paths used for the Single User Configuration 
''' are derived. See SpAgent.Configuration.Local.SingleUser.cs for further detail.
''' 
''' The default implementation will configures licenses to be stored under 
''' a path of the following format: 
''' <c>C:\Users\USERNAME\AppData\Local\MyCompany\MyProduct\MyVersion</c>.
''' 
''' NB THIS LOCATION IS SPECIFIC TO A USER PROFILE AND WILL MEAN YOUR 
''' CUSTOMERS CAN NOT SHARE LICENSES ACROSS PROFILES.
''' 
''' See <c>SoftwarePotential.Configuration.Local.MultiUser</c> if you need 
''' to address this restriction [by having an installer create a shared folder 
''' outside of the User Profiles Area]
''' 
''' </summary>
Partial Class SpAgent

	''' <summary>
	''' 
	''' Must be implemented to provide a path to the base directory 
	''' within which the Licensing System is to maintain its licenses.
	''' 
	''' The value supplied must represent a path that is guaranteed to 
	''' be initialized at time of execution.
	''' 
	''' See the documentation for <c>WithSingleUserStore()</c> in 
	''' <c>Sp.Agent.Local</c> and <c>SingleUserStorageStrategy</c> for further detail.
	''' 
	''' </summary>
	''' <param name="configure">delegate that must be invoked to accept the configured value.</param>
	Private Shared Sub ConfigureStorageBasePath(configure As Action(Of String))
		'Typically has a format similar to: C:\Users\USERNAME\AppData\Local
		Dim basePath = SingleUserStorageStrategy.LocalApplicationDataFolderPath
		'Supply desired value into the Agent Context Configuration sequence
		configure( basePath )
	End Sub

	''' <summary>
	''' Must be implemented to provide a subfolder structure that will 
	''' appropriately isolate the Application's storage area from other 
	''' Applications sharing the area specified via <c>ConfigureStorageBasePath()</c>.
	''' 
	''' The value supplied needs to consist of characters valid within a 
	''' Windows filename and, when combined with the base path needs to fit
	''' within Windows path length restrictions.
	''' 
	''' See the documentation for <c>WithSingleUserStore()</c> in 
	''' <c>Sp.Agent.Local</c> for further detail.
	''' </summary>
	''' <param name="configure">delegate that must be invoked to accept the configured value.</param>
	Private Shared Sub ConfigureStorageRelativePath(vendor As String, product As String, version As String, configure As Action(Of String))

		' Provide early warning and clear messages of likely problems in 
		' the field due to invalid characters or likely breaches of length 
		' restrictions based on the Vendor/Product/Version settings from 
		' your Software Potential Product profile.
        SanityCheckRelativePathIfDebugBuild(vendor, product, version)

		' TODO: Customize the relative path as desired or necessary to yield 
		' a path structure that is appropriate for your application and 
		' adheres to the restrictions of your environment.

		' NB: IF YOU DO SO, PLEASE CONSIDER RENAMING THIS FILE AND/OR 
		' EXTRACTING YOUR REPLACEMENT METHOD TO A SEPARATE FILE SO A NUGET 
		' PACKAGE UPDATE CANNOT INADVERTENTLY CAUSE YOUR CHANGES TO BE LOST
        Dim relativePath As String = System.IO.Path.Combine(vendor, product, version)
		configure(relativePath)
	End Sub

	''' <summary>
	''' Derives the location of the Application Data area within the Windows 
	''' User Profile directory structure.
	''' </summary>
	''' <remarks>This is a useful and easy to manage (no installation step 
	''' requirement) area within which to maintain licenses if one can accept 
	''' the attendant restrictions (other users on the same machine will not 
	''' be able to use the licenses).</remarks>
	Private Class SingleUserStorageStrategy

		''' <summary>Computes the Current User's Local Data storage path.</summary>
		''' <value>Same as <c>%LOCALAPPDATA%</c>, e.g., <c> C:\Users\USERNAME\AppData\Local</c>.</value>
		Public Shared ReadOnly Property LocalApplicationDataFolderPath As String
			Get
				Return Environment.GetFolderPath( _
					Environment.SpecialFolder.LocalApplicationData, _
					Environment.SpecialFolderOption.DoNotVerify)
			End Get
		End Property
	End Class
End Class
