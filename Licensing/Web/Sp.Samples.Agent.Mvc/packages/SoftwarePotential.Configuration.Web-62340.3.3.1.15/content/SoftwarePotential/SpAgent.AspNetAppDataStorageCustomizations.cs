// NB This file is auto-generated via the SoftwarePotential.Configuration.Web-XXYYY NuGet package.
//
// For more details see the README at https://support.softwarepotential.com/hc/en-us/articles/115001366649-Web-Configuration-README
//
// TODO: IF YOU MODIFY THIS FILE, CONSIDER MOVING ANY MODIFIED METHODS (AND/OR 
// RENAMING THIS FILE) SO NUGET PACKAGE UPDATES CANNOT RESULT IN YOU INADVERTENTLY 
// UNDOING CHANGES YOU HAVE MADE

using System;

namespace Sp.Agent
{
	/// <summary>
	/// This portion of the partial class configures licenses to be stored 
	/// in a directory that is guaranteed to have been created and permissioned 
	/// externally during application deployment (e.g., by the WebDeploy tool / 
	/// Visual Studio Publish process).
	/// 
	/// The default implementation will configures licenses to be stored under 
	/// a path of the following format: 
	/// <c>C:\inetpub\wwwroot\APPLICATION\App_Data\Licenses</c>.
	/// 
	/// NB THIS STRATEGY REQUIRES A DEPLOYMENT PROCESS THAT GUARANTEES THE ASP.NET
	/// SPECIAL FOLDER 'App_Data' IS PRESENT AND CORRECTLY PERMISSIONED FOR THE 
	/// USER ASSOCIATED WITH ALL YOUR APPLICATION APP POOLS TO BE ABLE TO READ AND
	/// WRITE AS NECESSARY TO WORK CORRECTLY (The package has been configured in 
	/// such a manner as to ensure that WebDeploy/Visual Studio Publish fulfill this).
	/// </summary>
	/// <remarks>
	/// It is assumed that all execution of the Application will be under a
	/// user that, via the installation process, will have been granted appropriate 
	/// access to be able to write, update and delete licenses within the 
	/// aforementioned area as necessary.
	/// 
	/// See the documentation for <c>WithExternallyInitializedStore()</c> in 
	/// <c>Sp.Agent.Local</c> and <c>AspNetAppDataStorageStrategy</c> for further detail.
	///
	/// See <c>SoftwarePotential.Configuration.Local.MultiUser</c> if you need 
	/// to address this restriction [e.g., by having an installer create and 
	/// permission a shared area elsewhere on the file system [potentially outside 
	/// App_Data]].
	///
	/// If you change the default settings, your installation strategy will need to 
	/// fulfill the key prerequisites noted into account.
	/// 
	/// TODO: CUSTOMIZE THE LOCATION/STRATEGY AS NECESSARY.
	/// 
	/// NB IF YOU DO SO, PLEASE CONSIDER RENAMING THIS FILE AND/OR EXTRACTING 
	/// YOUR REPLACEMENT METHOD(S) TO A SEPARATE FILE SO A NUGET PACKAGE UPDATE 
	/// CANNOT INADVERTENTLY CAUSE YOUR CHANGES TO BE LOST
	/// </remarks>
	static partial class SpAgent
	{
		/// <summary>
		/// Must be implemented to provide a path to the base directory 
		/// within which the Licensing System is to maintain its licenses.
		/// </summary>
		/// <remarks>
		/// The default implementation uses ASP.NET's App_Data special folder. 
		/// This directory will be correctly permissioned as part of Web Deployment 
		/// to allow file Write/Create access at run time under Application control.
		/// 
		/// The result of this, combined with that from ConfigureStorageRelativePath(),
		/// should result in a that is guaranteed to be present and correctly configured 
		/// at all times as discussed above.
		/// </remarks>
		/// <param name="configure">delegate that must be invoked to accept the configured value.</param>
		static partial void ConfigureStorageBasePath( Action<string> configure )
		{
			configure( AspNetAppDataStorageStrategy.AppDataPath );
		}

		/// <summary>
		/// Must be implemented to provide a subdirectory to be used 
		/// within the area supplied via <c>ConfigureStorageBasePath()</c> above.
		/// </summary>
		/// <remarks>
		/// The default implementation uses the name of a Folder (Licenses) 
		/// within the project to which a placeholder file as been added by 
		/// this NuGet package.
		///
		/// When combined with the result from ConfigureStorageBasePath(), 
		/// this typically results in a combined path such as
		/// <c>C:\inetpub\wwwroot\APPLICATION\App_Data\Licenses</c>.
		///
		/// It should not necessary to use a deeper folder hierarchy as the 
		/// Base Directory is private to your Web Application.</para>
		/// </remarks>
		/// <param name="configure">delegate that must be invoked to accept the configured value.</param>
		static partial void ConfigureStorageRelativePath( string vendor, string product, string version, Action<string> configure )
		{
			string nameOfFolderNuGetPackageHasIntroducedPlaceholderFile = "Licenses";
			configure( nameOfFolderNuGetPackageHasIntroducedPlaceholderFile );
		}
	}
}