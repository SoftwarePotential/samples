' NB This file is auto-generated via the SoftwarePotential.Configuration.Web-XXYYY NuGet package.
' For more details see the README at https://support.softwarepotential.com/hc/en-us/articles/115001366649-Web-Configuration-README
' THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

Imports System
Imports System.IO

''' <summary>
''' Computes storage locations required for the default license storage strategy employed in ASP.NET-based applications.
''' </summary>
Module AspNetAppDataStorageStrategy
		''' <summary>Default Base folder for used for ASP.NET applications.</summary>
		''' <value>Yields an absolute path for the ASP.NET App_Data Special folder for the Current Application.</value>
		ReadOnly Property AppDataPath As String
			Get
				Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data")
			End Get
		End Property
End Module

