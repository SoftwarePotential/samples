// NB This file is auto-generated via the SoftwarePotential.Configuration.Local NuGet packages.
// 
// CONSIDER RENAMING OR MOVING THIS FILE SO A PACKAGE UPDATE CANNOT UNDO ANY CHANGES YOU MAKE

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Sp.Agent
{
	static partial class SpAgent
	{
		/// <summary>
		///
		/// <para>TODO: YOUR CUSTOMIZATIONS OR DELETE THIS METHOD</para>
		///
		/// <para>Implementing a ConfigureLocalStorePath method is optional (i.e. if you 
		/// delete the code below, the default processing in the calling code will 
		/// perform equivalent processing, minus the clearer exception messages emitted by the code below).</para>
		/// 
		/// <para>The implementation below is intentionally conservative in nature 
		/// as we believe it is worth considering the License Storage path to use for your software carefully.</para>
		///
		/// <para>The key aspect is that the implementation needs to consistently produce a stable result on a 
		/// given machine in order for the system to correctly pick up installed licenses on subsequent executions.</para>
		/// </summary>
		/// <param name="rootPath">The absolute path to base license storage folder (e.g. "C:\ProgramData" if MultiUser 
		/// or "C:\Users\USERNAME\AppData\Local" for single user).</param>
		/// <param name="vendor">Your vendor name as used in the Software Potential Service.</param>
		/// <param name="product">Your Product Definition name.</param>
		/// <param name="version">Your Product Definition version.</param>
		/// <param name="overridePath">Action that needs to be called to supply your 
		/// desired customized store path in order to override the default behavior.</param>
		static partial void ConfigureLocalStorePath( string rootPath, string vendor, string product, string version, Action<string, string> overridePath )
		{
			//=======================================================================================================
			// TODO: Implement an appropriate algorithm to decide what path structure to use for your license storage
			// (OR DELETE THIS METHOD TO HAVE THE DEFAULT CONFIGURATION BE APPLIED INSTEAD IF YOU DONT HAVE A COMPANY / PRODUCT NAME COMBINATION THAT IS LONG OR HAS CHARACTERS THAT CANNOT BE USED IN A PATH) 
			//=======================================================================================================

			// Sanity check the inputs 
			//   (if any fail, you should consider using an alternate value of your choosing and/or remove any invalid chars)
			WarnAboutInvalidPathChars( vendor );
			WarnAboutInvalidPathChars( product );
			WarnAboutInvalidPathChars( version );

			// Standard algorithm produces the relative portion of the path just like this 
			//   (this will cause a problem if any element has characters that are not valid in a path or the combined result is too long)
			var storeLocationRelativePath = Path.Combine( vendor, product, version );
			
			// Sanity check relative path is not likely to trip the OS 260 char limits 
			//   (if this fails, consider deriving the location on a different basis)
			WarnAboutLongPaths( storeLocationRelativePath );

			// (REQUIRED) Pass the computed overrides back to the caller.
			overridePath( rootPath, storeLocationRelativePath );
		}

		[Conditional( "DEBUG" )]
		static void WarnAboutInvalidPathChars( string pathElement )
		{
			if ( pathElement.Intersect( Path.GetInvalidPathChars() ).Any() )
				throw new InvalidOperationException( "The string" + pathElement + "contains some characters that are invalid in directory names" );
		}

		[Conditional( "DEBUG" )]
		static void WarnAboutLongPaths( string relativePath )
		{
			// MaxAppDataRootPathLength should be the longest expected application data path length that may be encountered across any environments this application is intended to be deployed to.
			// The actual application data path may depend on OS version, OS locale, environment settings, user account name, etc.
			// (e.g. for single user store it might be C:\Users\<Username> on some machines, but it could be "C:\Documents and Settings\<Username>" on others )
			//  for multi user store is might be "C:\ProgramData" on some machines, and "C:\Anwendungsdaten" on others)
			// The value below is supposed to be a conservative value
			const int MaxAppDataRootPathLength = 60;

			const int LicenseFileWithSubdirLength = 85;
			if ( MaxAppDataRootPathLength + relativePath.Length + LicenseFileWithSubdirLength >= 260 )
				throw new InvalidOperationException(
					"The local store relative path is too long (" + relativePath + "). "
					+ "Please adjust your ConfigureLocalStorePath implementation to yield a stable result across all environments you intend to deploy to." );
		}
	}
}