// NB This file is auto-added via the SoftwarePotential.Configuration.Local.* and/or SoftwarePotential.Configuration.Web NuGet packages.
// 
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Sp.Agent
{
	/// <summary>
	/// This portion of the partial class has some helper methods that are used to enable the packages to give earlier 
	/// warning and/or disambiguated explanations of issues that are likely to crop up at runtime due to the specific 
	/// values emanating from your Application-specific implementations of ConfigureStorageBasePath() and/or ConfigureStorageRelativePath().
	/// </summary>
	/// <remarks>The sanity check methods are intentionally rendered inert in Release builds via a <c>ConditionalAttribute</c> 
	/// on the basis that you the developer should already have encountered any potential issues during the development process.
	/// </remarks>
	partial class SpAgent
	{
		/// <summary>
		/// <para>The implementation below is intentionally conservative in nature 
		/// as we believe it is worth considering the License Storage path to use for your software carefully.</para>
		/// <para>The key concern is that the implementation needs to consistently produce a stable result on a 
		/// given machine in order for the system to correctly pick up installed licenses on subsequent executions.</para>
		/// </summary>
		/// <param name="vendor">Your Internal vendor name as used in the Software Potential Service.</param>
		/// <param name="product">Your Internal Product Definition name.</param>
		/// <param name="version">Your Internal Product Definition version.</param>
		[Conditional( "DEBUG" )]
		static void SanityCheckRelativePathIfDebugBuild( string vendor, string product, string version )
		{
			// Sanity check the inputs 
			//   (if any of the following fail, you should consider using an alternate 
			//   value of your choosing and/or remove any invalid chars)
			WarnAboutInvalidPathChars( vendor );
			WarnAboutInvalidPathChars( product );
			WarnAboutInvalidPathChars( version );

			// The default algorithm combines the relative portion of the path just like this 
			//   (this will cause a problem if any element has characters that are not valid in a path or the combined result is too long)
			var relativePath = Path.Combine( vendor, product, version );

			// Sanity check relative path is not likely to trip the OS 260 char limits 
			//   (if this fails, consider deriving the Relative portion on a different
			//   basis by customizing your implementation of ConfigureStorageRelativePath())
			WarnAboutLongPaths( relativePath );
		}

		[Conditional( "DEBUG" )]
		static void WarnAboutInvalidPathChars( string pathElement )
		{
			if ( pathElement.Intersect( Path.GetInvalidPathChars() ).Any() )
				throw new InvalidOperationException( "The string" + pathElement + "contains some characters that are invalid in directory names" );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ConfigureStorageRelativePath" )]
		[Conditional( "DEBUG" )]
		static void WarnAboutLongPaths( string relativePath )
		{
			// MaxAppDataRootPathLength should be the longest expected application 
			// data path length that can be encountered across any environments your
			// application is intended to be deployed to.

			// The actual application data path may depend on OS version, OS locale, 
			// environment settings, user account name, etc. e.g.:
			// - for a single user store it might be C:\Users\<Username> on some machines, 
			//   but it could be "C:\Documents and Settings\<Username>" on others 
			// - for a multi user store it might be "C:\ProgramData" on some machines, 
			//   and "C:\Anwendungsdaten" on others
			// While the value below is intended to be a conservative value, we don't recommend exceeding it
			const int MaxAppDataRootPathLength = 60;

			const int LicenseFileWithSubdirLength = 85;
			const int WindowsAndDotNetTogetherConspireToHaveIssuesForPathsLongerThan = 260;
			if ( MaxAppDataRootPathLength + relativePath.Length + LicenseFileWithSubdirLength >= WindowsAndDotNetTogetherConspireToHaveIssuesForPathsLongerThan )
				throw new InvalidOperationException(
					"The local store relative path is too long (" + relativePath + "). "
					+ "Please adjust your ConfigureStorageRelativePath implementation to yield a stable result across all environments you intend to deploy to." );
		}
	}
}