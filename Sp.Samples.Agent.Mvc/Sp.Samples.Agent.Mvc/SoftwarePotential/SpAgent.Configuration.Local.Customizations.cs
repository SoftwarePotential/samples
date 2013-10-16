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