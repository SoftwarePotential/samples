// NB This file is auto-added via the SoftwarePotential.Configuration.Local.* and/or SoftwarePotential.Configuration.Web NuGet packages.
// 
// THE CODE SHOULD BE OVERWRITTEN BY PACKAGE UPDATES - IT IS RECOMMENDED TO DEFINE ANY EXTENSIONS YOU MAY DESIRE ELSEWHERE

using System;
using System.IO;

namespace Sp.Agent
{
	/// <summary>
	/// This portion of the partial class:
	/// - exposes two partial methods which are typically implemented in a dependent package (see above for examples).
	/// - provides helpers that invoke the extensions (or throw if no such implementation is present)
	/// </summary>
	static partial class SpAgent
	{
		/// <summary>
		/// <para>Composes a complete path to a license store directory from invocations of ConfigureStorageBasePath() 
		/// and ConfigureStorageRelativePath() (see <c>ConfiguredBaseAndRelativePathElements()</c> for details).</para>
		/// <para>Typically, this is useful when such a location is created and permissioned externally such as during application deployment (i.e. by an installer) or by the Application Hosting Environment (e.g., ASP.NET's App_Data).</para>
		/// </summary>
		static string ConfiguredExternallyManagedRootPath()
		{
			var baseAndRelative = ConfiguredBaseAndRelativePathElements();
			return Path.Combine( baseAndRelative.Item1, baseAndRelative.Item2 );
		}

		/// <summary>
		/// Returns a license store location consisting of 2 parts:
		/// <list type="bullet">
		/// <item><description>base path - license store root path, which can be shared between applications</description></item>
		/// <item><description>relative path - when combined with base path, yields a combined result that is unique per application.</description></item></list>
		/// See <c>ConfigureStorageBasePath()</c> and <c>ConfigureStorageRelativePath()</c> for details of the relevant extensibility points.
		/// </summary>
		static Tuple<string, string> ConfiguredBaseAndRelativePathElements()
		{
			var basePath = default( string );
			ConfigureStorageBasePath( value => basePath = value );
			if ( basePath == null )
				throw CreateMissingPartialMethodException( "Storage Location Base path element not correctly configured", "ConfigureStorageBasePath" );
			var relativePath = default( string );
			ConfigureStorageRelativePath( SpProduct.Vendor, SpProduct.Name, SpProduct.Version, value => relativePath = value );
			if ( relativePath == null )
				throw CreateMissingPartialMethodException( "Storage Location Relative path suffix not correctly configured", "ConfigureStorageRelativePath" );
			return Tuple.Create( basePath, relativePath );
		}

		/// <summary>
		/// <para>Partial method enabling specification of an appropriate license store base path by a partnering partial class.</para>
		/// <para>Typically an implementation of this is provided via a <c>SoftwarePotential.Configuration.*</c> package in a <c>SpAgent.*Customizations.cs</c> file.</para>
		/// <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
		/// callback will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
		/// </summary>
		/// <param name="configure">delegate that accepts a License Store base path to be used.</param>
		static partial void ConfigureStorageBasePath( Action<string> configure );

		/// <summary>
		/// <para>Partial method enabling specification of an appropriate license store relative path by a partnering partial class.</para>
		/// <para>Typically an implementation of this is provided via a <c>SoftwarePotential.Configuration.*</c> package in a <c>SpAgent.*Customizations.cs</c> file.</para>
		/// <para>Failure to provide an implementation of this partial method that correctly invokes the <paramref name="configure"/> 
		/// callback will result in an <see cref="InvalidOperationException"/> at Application startup.</para>
		/// </summary>
		/// <param name="configure">delegate that accepts a License Store relative path to be used.</param>
		/// <param name="vendor">Internal Vendor Name as specified in your Product Definition on the Software Potential Service.</param>
		/// <param name="product">Internal Product Name as specified in your Product Definition on the Software Potential Service.</param>
		/// <param name="version">Internal Product Version as specified in your Product Definition on the Software Potential Service.</param>
		static partial void ConfigureStorageRelativePath( string vendor, string product, string version, Action<string> configure );
	}
}