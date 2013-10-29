// NB This file is auto-generated via the SoftwarePotential.Configuration.Web-XXYYY NuGet package.
// For more details see the README at http://docs.softwarepotential.com/Configuration.Web-README.html
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		/// <summary>
		/// Use ASP.NET's App_Data special folder to store licenses - this 
		/// directory will be correctly permissioned as part of Web Deployment 
		/// to allow file Write/Create access at run time under Application control
		/// </summary>
		/// <remarks>
		/// If you change this strategy, your installation strategy will need to 
		/// take this precondition into account
		/// </remarks>
		static partial void ConfigureStorageBasePath( Action<string> configure )
		{
			configure( AspNetAppDataStorageStrategy.AppDataPath );
		}

		/// <summary>
		/// Use a hardcoded subdirectory matching the location of 
		/// App_Data\Licenses\placeholder.txt
		/// </summary>
		/// <remarks>
		/// <para>The configured subdirectory of App_Data needs to be guaranteed 
		/// to be present and correctly permissioned for the application to run.</para>
		/// <para>It is not necessary to use a deeper folder hierarchy as the 
		/// Base Directory is private to your Application.</para>
		/// </remarks>
		static partial void ConfigureStorageRelativePath( string vendor, string product, string version, Action<string> configure )
		{
			configure( "Licenses" );
		}
	}
}